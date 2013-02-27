#define PICOPTER

  #define LEDPIN_PINMODE             DDRF |= 1<<2; PORTF &= ~(1<<2);
  #define LEDPIN_TOGGLE              PORTF ^= 1<<2;     //switch LEDPIN state (digital PIN 13)
  #define LEDPIN_OFF                 PORTF &= ~(1<<2);
  #define LEDPIN_ON                  PORTF |= (1<<2);
  #define BUZZERPIN_PINMODE          ;
  #define BUZZERPIN_ON               ;
  #define BUZZERPIN_OFF              ;
  #define POWERPIN_PINMODE           ;
  #define POWERPIN_ON                ;
  #define POWERPIN_OFF               ; //switch OFF WMP, digital PIN 12
  #define I2C_PULLUPS_ENABLE         PORTD |= 1<<0; PORTD |= 1<<1;   // PIN A4&A5 (SDA&SCL)
  #define I2C_PULLUPS_DISABLE        PORTD &= ~(1<<0); PORTD &= ~(1<<1);
  #define STABLEPIN_PINMODE          DDRE |= 1<<2;
  #define STABLEPIN_ON               PORTE |= 1<<2;
  #define STABLEPIN_OFF              PORTE &= ~(1<<2);
  #define STABLEPIN_TOGGLE           PORTE ^= 1<<2;
  #define ISR_UART                   ISR(USART0_UDRE_vect)
  #define MPU60x0
  #define ACC_ORIENTATION(X, Y, Z)  {accADC[ROLL]  =  -Y; accADC[PITCH]  = X; accADC[YAW]  = Z;}
  #define GYRO_ORIENTATION(X, Y, Z) {gyroADC[ROLL] =  -Y; gyroADC[PITCH] = X; gyroADC[YAW] = Z;}
  #define BMA180_ADDRESS 0x80
  #define ITG3200_ADDRESS 0XD0

#if defined(ADXL345) || defined(BMA020) || defined(BMA180) || defined(NUNCHACK) || defined(ADCACC) || defined(LSM303DLx_ACC) || defined(MPU60x0)
  #define ACC 1
#else
  #define ACC 0
#endif

#if defined(HMC5883) || defined(HMC5843) || defined(AK8975)
  #define MAG 1
#else
  #define MAG 0
#endif

#if defined(ITG3200) || defined(L3G4200D) || defined(MPU60x0)
  #define GYRO 1
#else
  #define GYRO 0
#endif

#if defined(BMP085) || defined(MS561101BA)
  #define BARO 1
#else
  #define BARO 0
#endif

#if defined(GPS)
  #define GPSPRESENT 1
#else
  #define GPSPRESENT 0
#endif


#if defined(RCAUXPIN8)
  #define BUZZERPIN_PINMODE          ;
  #define BUZZERPIN_ON               ;
  #define BUZZERPIN_OFF              ;
  #define RCAUXPIN
#endif
#if defined(RCAUXPIN12)
  #define POWERPIN_PINMODE           ;
  #define POWERPIN_ON                ;
  #define POWERPIN_OFF               ;
  #define RCAUXPIN
#endif


#if defined(POWERMETER)
  #ifndef VBAT
	#error "to use powermeter, you must also define and configure VBAT"
  #endif
#endif
#ifdef LCD_TELEMETRY_AUTO
  #ifndef LCD_TELEMETRY
     #error "to use automatic telemetry, you MUST also define and configure LCD_TELEMETRY"
  #endif
#endif

#if defined(TRI)
  #define MULTITYPE 1
#elif defined(QUADP)
  #define MULTITYPE 2
#elif defined(QUADX)
  #define MULTITYPE 3
#elif defined(BI)
  #define MULTITYPE 4
#elif defined(GIMBAL)
  #define MULTITYPE 5
#elif defined(Y6)
  #define MULTITYPE 6
#elif defined(HEX6)
  #define MULTITYPE 7
#elif defined(FLYING_WING)
  #define MULTITYPE 8
#elif defined(Y4)
  #define MULTITYPE 9
#elif defined(HEX6X)
  #define MULTITYPE 10
#elif defined(OCTOX8)
  #define MULTITYPE 11
#elif defined(OCTOFLATP)
  #define MULTITYPE 11      //the GUI is the same for all 8 motor configs
#elif defined(OCTOFLATX)
  #define MULTITYPE 11      //the GUI is the same for all 8 motor configs
#endif
