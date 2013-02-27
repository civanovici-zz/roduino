// ************************************************************************************************************
// board orientation and setup
// ************************************************************************************************************
//default board orientation
#if !defined(ACC_ORIENTATION) 
  #define ACC_ORIENTATION(X, Y, Z)  {accADC[ROLL]  = X; accADC[PITCH]  = Y; accADC[YAW]  = Z;}
#endif
#if !defined(GYRO_ORIENTATION) 
  #define GYRO_ORIENTATION(X, Y, Z) {gyroADC[ROLL] = X; gyroADC[PITCH] = Y; gyroADC[YAW] = Z;}
#endif

/*** I2C address ***/
#if !defined(BMA180_ADDRESS) 
  #define BMA180_ADDRESS 0x80
  //#define BMA180_ADDRESS 0x82
#endif

#if !defined(ITG3200_ADDRESS) 
  #define ITG3200_ADDRESS 0XD0
  //#define ITG3200_ADDRESS 0XD2
#endif

//ITG3200 and ITG3205 Gyro LPF setting
#if defined(ITG3200_LPF_256HZ) || defined(ITG3200_LPF_188HZ) || defined(ITG3200_LPF_98HZ) || defined(ITG3200_LPF_42HZ) || defined(ITG3200_LPF_20HZ) || defined(ITG3200_LPF_10HZ)
  #if defined(ITG3200_LPF_256HZ)
    #define ITG3200_SMPLRT_DIV 0  //8000Hz
    #define ITG3200_DLPF_CFG   0
  #endif
  #if defined(ITG3200_LPF_188HZ)
    #define ITG3200_SMPLRT_DIV 0  //1000Hz
    #define ITG3200_DLPF_CFG   1
  #endif
  #if defined(ITG3200_LPF_98HZ)
    #define ITG3200_SMPLRT_DIV 0
    #define ITG3200_DLPF_CFG   2
  #endif
  #if defined(ITG3200_LPF_42HZ)
    #define ITG3200_SMPLRT_DIV 0
    #define ITG3200_DLPF_CFG   3
  #endif
  #if defined(ITG3200_LPF_20HZ)
    #define ITG3200_SMPLRT_DIV 0
    #define ITG3200_DLPF_CFG   4
  #endif
  #if defined(ITG3200_LPF_10HZ)
    #define ITG3200_SMPLRT_DIV 0
    #define ITG3200_DLPF_CFG   5
  #endif
#else
    //Default settings LPF 256Hz/8000Hz sample
    #define ITG3200_SMPLRT_DIV 0  //8000Hz
    #define ITG3200_DLPF_CFG   0
#endif

uint8_t rawADC[6];
static uint32_t neutralizeTime = 0;
  
// ************************************************************************************************************
// I2C general functions
// ************************************************************************************************************

void i2c_init(void) {
  #if defined(INTERNAL_I2C_PULLUPS)
    I2C_PULLUPS_ENABLE
  #else
    I2C_PULLUPS_DISABLE
  #endif
  TWSR = 0;                                    // no prescaler => prescaler = 1
  TWBR = ((16000000L / I2C_SPEED) - 16) / 2;   // change the I2C clock rate
  TWCR = 1<<TWEN;                              // enable twi module, no interrupt
}

void i2c_rep_start(uint8_t address) {
  TWCR = (1<<TWINT) | (1<<TWSTA) | (1<<TWEN) ; // send REPEAT START condition
  waitTransmissionI2C();                       // wait until transmission completed
  TWDR = address;                              // send device address
  TWCR = (1<<TWINT) | (1<<TWEN);
  waitTransmissionI2C();                       // wail until transmission completed
}

void i2c_stop(void) {
  TWCR = (1 << TWINT) | (1 << TWEN) | (1 << TWSTO);
  //  while(TWCR & (1<<TWSTO));                // <- can produce a blocking state with some WMP clones
}

void i2c_write(uint8_t data ) {	
  TWDR = data;                                 // send data to the previously addressed device
  TWCR = (1<<TWINT) | (1<<TWEN);
  waitTransmissionI2C();
}

uint8_t i2c_readAck() {
  TWCR = (1<<TWINT) | (1<<TWEN) | (1<<TWEA);
  waitTransmissionI2C();
  return TWDR;
}

uint8_t i2c_readNak(void) {
  TWCR = (1<<TWINT) | (1<<TWEN);
  waitTransmissionI2C();
  uint8_t r = TWDR;
  i2c_stop();
  return r;
}

void waitTransmissionI2C() {
  uint16_t count = 255;
  while (!(TWCR & (1<<TWINT))) {
    count--;
    if (count==0) {              //we are in a blocking state => we don't insist
      TWCR = 0;                  //and we force a reset on TWINT register
      neutralizeTime = micros(); //we take a timestamp here to neutralize the value during a short delay
      i2c_errors_count++;
      break;
    }
  }
}

void i2c_getSixRawADC(uint8_t add, uint8_t reg) {
  i2c_rep_start(add);
  i2c_write(reg);         // Start multiple read at the reg register
  i2c_rep_start(add +1);  // I2C read direction => I2C address + 1
  for(uint8_t i = 0; i < 5; i++)
    rawADC[i]=i2c_readAck();
  rawADC[5]= i2c_readNak();
}

void i2c_writeReg(uint8_t add, uint8_t reg, uint8_t val) {
  i2c_rep_start(add+0);  // I2C write direction
  i2c_write(reg);        // register selection
  i2c_write(val);        // value to write in register
  i2c_stop();
}

uint8_t i2c_readReg(uint8_t add, uint8_t reg) {
  i2c_rep_start(add+0);  // I2C write direction
  i2c_write(reg);        // register selection
  i2c_rep_start(add+1);  // I2C read direction
  return i2c_readNak();  // Read single register and return value
}

// ****************
// GYRO common part
// ****************
void GYRO_Common() {
  static int16_t previousGyroADC[3] = {0,0,0};
  static int32_t g[3];
  uint8_t axis;
  
  if (calibratingG>0) {
    for (axis = 0; axis < 3; axis++) {
      // Reset g[axis] at start of calibration
      if (calibratingG == 400) g[axis]=0;
      // Sum up 400 readings
      g[axis] +=gyroADC[axis];
      // Clear global variables for next reading
      gyroADC[axis]=0;
      gyroZero[axis]=0;
      if (calibratingG == 1) {
        gyroZero[axis]=g[axis]/400;
        blinkLED(10,15,1+3*nunchuk);
      }
    }
    calibratingG--;
  }
  for (axis = 0; axis < 3; axis++) {
    gyroADC[axis]  -= gyroZero[axis];
    //anti gyro glitch, limit the variation between two consecutive readings
    gyroADC[axis] = constrain(gyroADC[axis],previousGyroADC[axis]-800,previousGyroADC[axis]+800);
    previousGyroADC[axis] = gyroADC[axis];
  }
}

// ****************
// ACC common part
// ****************
void ACC_Common() {
  static int32_t a[3];
  
  if (calibratingA>0) {
    for (uint8_t axis = 0; axis < 3; axis++) {
      // Reset a[axis] at start of calibration
      if (calibratingA == 400) a[axis]=0;
      // Sum up 400 readings
      a[axis] +=accADC[axis];
      // Clear global variables for next reading
      accADC[axis]=0;
      accZero[axis]=0;
    }
    // Calculate average, shift Z down by acc_1G and store values in EEPROM at end of calibration
    if (calibratingA == 1) {
      accZero[ROLL]  = a[ROLL]/400;
      accZero[PITCH] = a[PITCH]/400;
      accZero[YAW]   = a[YAW]/400-acc_1G; // for nunchuk 200=1G
      accTrim[ROLL]   = 0;
      accTrim[PITCH]  = 0;
      writeParams(); // write accZero in EEPROM
    }
    calibratingA--;
  }
  accADC[ROLL]  -=  accZero[ROLL] ;
  accADC[PITCH] -=  accZero[PITCH];
  accADC[YAW]   -=  accZero[YAW] ;
}

#if defined(MPU60x0)

#define MPU60x0_I2CADDR 0xD0

void MPU60x0_init() {
  i2c_writeReg(MPU60x0_I2CADDR, 0x6B, 0x80); // reset
  delay(100);
  i2c_writeReg(MPU60x0_I2CADDR, 0x6B, 0x03); // clk sel PLL gyro Z
  i2c_writeReg(MPU60x0_I2CADDR, 0x19, 0x04); // sample rate 200Hz
  i2c_writeReg(MPU60x0_I2CADDR, 0x1A, 0x03); // DLPF 42Hz
  i2c_writeReg(MPU60x0_I2CADDR, 0x1B, 0x18); // gyro scale 2000 deg/s
  i2c_writeReg(MPU60x0_I2CADDR, 0x1C, 0x00); // accel scale 2g
}

void ACC_getADC () {
  i2c_getSixRawADC(MPU60x0_I2CADDR, 0x3B);
  //usefull info is on the 14 bits  [2-15] bits  /4 => [0-13] bits  /8 => 11 bit resolution
  ACC_ORIENTATION(  - ((rawADC[0]<<8) | rawADC[1])/16 ,
                    - ((rawADC[2]<<8) | rawADC[3])/16 ,
                      ((rawADC[4]<<8) | rawADC[5])/16 );
  ACC_Common();
}

void Gyro_getADC () {
  i2c_getSixRawADC(MPU60x0_I2CADDR, 0x43);
  GYRO_ORIENTATION(  + ( ((rawADC[2]<<8) | rawADC[3])/4) , // range: +/- 8192; +/- 2000 deg/sec
                     - ( ((rawADC[0]<<8) | rawADC[1])/4 ) ,
                     - ( ((rawADC[4]<<8) | rawADC[5])/4 ) );
  GYRO_Common();
}

#endif

// ************************************************************************************************************
// contribution initially from opie11 (rc-groups)
// adaptation from C2po (may 2011)
// contribution from ziss_dm (June 2011)
// contribution from ToLuSe (Jully 2011)
// I2C Accelerometer BMA180
// ************************************************************************************************************
// I2C adress: 0x80 (8bit)    0x40 (7bit) (SDO connection to VCC) 
// I2C adress: 0x82 (8bit)    0x41 (7bit) (SDO connection to VDDIO)
// Resolution: 14bit
//
// Control registers:
//
// 0x20    bw_tcs:   |                                           bw<3:0> |                        tcs<3:0> |
//                   |                                             150Hz |                 !!Calibration!! |
// ************************************************************************************************************
#if defined(BMA180)
void ACC_init () {
  delay(10);
  //default range 2G: 1G = 4096 unit.
  i2c_writeReg(BMA180_ADDRESS,0x0D,1<<4); // register: ctrl_reg0  -- value: set bit ee_w to 1 to enable writing
  delay(5);
  uint8_t control = i2c_readReg(BMA180_ADDRESS, 0x20);
  control = control & 0x0F; // register: bw_tcs reg: bits 4-7 to set bw -- value: set low pass filter to 10Hz (bits value = 0000xxxx)
  control = control | 0x00; 
  i2c_writeReg(BMA180_ADDRESS, 0x20, control);
  delay(5); 
  control = i2c_readReg(BMA180_ADDRESS, 0x30);
  control = control & 0xFC; 
  control = control | 0x02; 
  i2c_writeReg(BMA180_ADDRESS, 0x30, control);
  delay(5); 
  acc_1G = 512;
}

void ACC_getADC () {
  TWBR = ((16000000L / 400000L) - 16) / 2;  // Optional line.  Sensor is good for it in the spec.
  i2c_getSixRawADC(BMA180_ADDRESS,0x02);
  //usefull info is on the 14 bits  [2-15] bits  /4 => [0-13] bits  /8 => 11 bit resolution
  ACC_ORIENTATION(  - ((rawADC[1]<<8) | rawADC[0])/32 ,
                    - ((rawADC[3]<<8) | rawADC[2])/32 ,
                      ((rawADC[5]<<8) | rawADC[4])/32 );
  ACC_Common();
}
#endif

// ************************************************************************************************************
// I2C Gyroscope ITG3200 
// ************************************************************************************************************
// I2C adress: 0xD2 (8bit)   0x69 (7bit)
// I2C adress: 0xD0 (8bit)   0x68 (7bit)
// principle:
// 1) VIO is connected to VDD
// 2) I2C adress is set to 0x69 (AD0 PIN connected to VDD)
// or 2) I2C adress is set to 0x68 (AD0 PIN connected to GND)
// 3) sample rate = 1000Hz ( 1kHz/(div+1) )
// ************************************************************************************************************
#if defined(ITG3200)
void Gyro_init() {
  delay(100);
  i2c_writeReg(ITG3200_ADDRESS, 0x3E, 0x80); //register: Power Management  --  value: reset device
//  delay(5);
//  i2c_writeReg(ITG3200_ADDRESS, 0x15, ITG3200_SMPLRT_DIV); //register: Sample Rate Divider  -- default value = 0: OK
  delay(5);
  i2c_writeReg(ITG3200_ADDRESS, 0x16, 0x18 + ITG3200_DLPF_CFG); //register: DLPF_CFG - low pass filter configuration
  delay(5);
  i2c_writeReg(ITG3200_ADDRESS, 0x3E, 0x03); //register: Power Management  --  value: PLL with Z Gyro reference
  delay(100);
}

void Gyro_getADC () {
  TWBR = ((16000000L / 400000L) - 16) / 2; // change the I2C clock rate to 400kHz
  i2c_getSixRawADC(ITG3200_ADDRESS,0X1D);
  GYRO_ORIENTATION(  + ( ((rawADC[2]<<8) | rawADC[3])/4) , // range: +/- 8192; +/- 2000 deg/sec
                     - ( ((rawADC[0]<<8) | rawADC[1])/4 ) ,
                     - ( ((rawADC[4]<<8) | rawADC[5])/4 ) );
  GYRO_Common();
}
#endif

void initSensors() {
  delay(200);
  POWERPIN_ON;
  delay(100);
  i2c_init();
  delay(100);
  #if !defined(MPU60x0)
  if (GYRO) Gyro_init();
  if (ACC) {ACC_init();acc_25deg = acc_1G * 0.423;}
  #else
  MPU60x0_init();
  acc_1G = 512;
  acc_25deg = acc_1G * 0.423;
  #endif
}
