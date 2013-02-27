/*******************************/
/****CONFIGURABLE PARAMETERS****/
/*******************************/

/* Set the minimum throttle command sent to the ESC (Electronic Speed Controller)
   This is the minimum value that allow motors to run at a idle speed  */
//#define MINTHROTTLE 1300 // for Turnigy Plush ESCs 10A
//#define MINTHROTTLE 1120 // for Super Simple ESCs 10A
//#define MINTHROTTLE 1220
#define MINTHROTTLE 1150 

/* The type of multicopter */
//#define GIMBAL
//#define BI
//#define TRI
//#define QUADP
#define QUADX
//#define Y4
//#define Y6
//#define HEX6
//#define HEX6X
//#define OCTOX8
//#define OCTOFLATP
//#define OCTOFLATX
//#define FLYING_WING //experimental

#define YAW_DIRECTION 1 // if you want to reverse the yaw correction direction
//#define YAW_DIRECTION -1

//#define I2C_SPEED 100000L     //100kHz normal mode, this value must be used for a genuine WMP
#define I2C_SPEED 400000L   //400kHz fast mode, it works only with some WMP clones

//enable internal I2C pull ups
//#define INTERNAL_I2C_PULLUPS


//****** advanced users settings   *************

/* This option should be uncommented if ACC Z is accurate enough when motors are running*/
//#define TRUSTED_ACCZ

/* PIN A0 and A1 instead of PIN D5 & D6 for 6 motors config and promini config
   This mod allow the use of a standard receiver on a pro mini
   (no need to use a PPM sum receiver)
*/
//#define A0_A1_PIN_HEX

/* possibility to use PIN8 or PIN12 as the AUX2 RC input
   it deactivates in this case the POWER PIN (pin 12) or the BUZZER PIN (pin 8)
*/
//#define RCAUXPIN8
//#define RCAUXPIN12

/* This option is here if you want to use the old level code from the verison 1.7
   It's just to have some feedback. This will be removed in the future */
//#define STAB_OLD_17

/* Pseudo-derivative conrtroller for level mode (experimental)
   Additional information: http://www.multiwii.com/forum/viewtopic.php?f=8&t=503 */
//#define LEVEL_PDF

/* introduce a deadband around the stick center
   Must be greater than zero, comment if you dont want a deadband on roll, pitch and yaw */
//#define DEADBAND 6

//if you use independent sensors
//leave it commented it you already checked a specific board above
/* I2C gyroscope */
#define MPU60x0
//#define ITG3200
//#define L3G4200D

/* I2C accelerometer */
//#define ADXL345
//#define BMA020
//#define BMA180
//#define NUNCHACK  // if you want to use the nunckuk as a standalone I2C ACC without WMP
//#define LIS3LV02
//#define LSM303DLx_ACC

/* ADC accelerometer */ // for 5DOF from sparkfun, uses analog PIN A1/A2/A3
//#define ADCACC

/* ITG3200 & ITG3205 Low pass filter setting. In case you cannot eliminate all vibrations to the Gyro, you can try
   to decrease the LPF frequency, only one step per try. As soon as twitching gone, stick with that setting.
   It will not help on feedback wobbles, so change only when copter is randomly twiching and all dampening and
   balancing options ran out. Uncomment only one option!
   IMPORTANT! Change low pass filter setting changes PID behaviour, so retune your PID's after changing LPF.*/
//#define ITG3200_LPF_256HZ     // This is the default setting, no need to uncomment, just for reference
//#define ITG3200_LPF_188HZ
//#define ITG3200_LPF_98HZ
//#define ITG3200_LPF_42HZ
//#define ITG3200_LPF_20HZ
//#define ITG3200_LPF_10HZ      // Use this only in extreme cases, rather change motors and/or props

/* Failsave settings - added by MIS
   Failsafe check pulse on THROTTLE channel. If the pulse is OFF (on only THROTTLE or on all channels) the failsafe procedure is initiated.
   After FAILSAVE_DELAY time of pulse absence, the level mode is on (if ACC or nunchuk is avaliable), PITCH, ROLL and YAW is centered
   and THROTTLE is set to FAILSAVE_THR0TTLE value. You must set this value to descending about 1m/s or so for best results. 
   This value is depended from your configuration, AUW and some other params. 
   Next, afrer FAILSAVE_OFF_DELAY the copter is disarmed, and motors is stopped.
   If RC pulse coming back before reached FAILSAVE_OFF_DELAY time, after the small quard time the RC control is returned to normal.
   If you use serial sum PPM, the sum converter must completly turn off the PPM SUM pusles for this FailSafe functionality.*/
#define FAILSAFE                                  // Alex: comment this line if you want to deactivate the failsafe function
#define FAILSAVE_DELAY     10                     // Guard time for failsafe activation after signal lost. 1 step = 0.1sec - 1sec in example
#define FAILSAVE_OFF_DELAY 200                    // Time for Landing before motors stop in 0.1sec. 1 step = 0.1sec - 20sec in example
#define FAILSAVE_THR0TTLE  (MINTHROTTLE + 200)    // Throttle level used for landing - may be relative to MINTHROTTLE - as in this case

/* EXPERIMENTAL !!
  contribution from Luis Correia
  see http://www.multiwii.com/forum/viewtopic.php?f=18&t=828
  It uses a Bluetooth Serial module as the input for controlling the device via an Android application
  As with the SPEKTRUM option, is not possible to use the configuration tool on a mini or promini. */
//#define BTSERIAL

/* interleaving delay in micro seconds between 2 readings WMP/NK in a WMP+NK config
   if the ACC calibration time is very long (20 or 30s), try to increase this delay up to 4000
   it is relevent only for a conf with NK */
#define INTERLEAVING_DELAY 3000

/* when there is an error on I2C bus, we neutralize the values during a short time. expressed in microseconds
   it is relevent only for a conf with at least a WMP */
#define NEUTRALIZE_DELAY 100000

/* this is the value for the ESCs when they are not armed
   in some cases, this value must be lowered down to 900 for some specific ESCs */
#define MINCOMMAND 1000

/* this is the maximum value for the ESCs at full power
   this value can be increased up to 2000 */
#define MAXTHROTTLE 2020

/* This is the speed of the serial interface. 115200 kbit/s is the best option for a USB connection.*/
#define SERIAL_COM_SPEED 115200

/* In order to save space, it's possibile to desactivate the LCD configuration functions
   comment this line only if you don't plan to used a LCD */
//#define LCD_CONF

/* To use an Eagle Tree Power Panel LCD for configuration, uncomment this line
 White wire  to Ground
 Red wire    to +5V VCC (or to the WMP power pin, if you prefer to reset everything on the bus when WMP resets)
 Yellow wire to SDA - Pin A4 Mini Pro - Pin 20 Mega
 Brown wire  to SCL - Pin A5 Mini Pro - Pin 21 Mega 
 (Contribution by Danal) */
//#define LCD_ETPP

/* to use Cat's whisker TEXTSTAR LCD, uncomment following line.
   Pleae note this display needs a full 4 wire connection to (+5V, Gnd, RXD, TXD )
   Configure display as follows: 115K baud, and TTL levels for RXD and TXD, terminal mode
   NO rx / tx line reconfiguration, use natural pins */
//#define LCD_TEXTSTAR

/* motors will not spin when the throttle command is in low position
   this is an alternative method to stop immediately the motors */
//#define MOTOR_STOP

/* some radios have not a neutral point centered on 1500. can be changed here */
#define MIDRC 1500
 
//****** end of advanced users settings *************

//if you want to change to orientation of individual sensor
//#define ACC_ORIENTATION(X, Y, Z)  {accADC[ROLL]  =  Y; accADC[PITCH]  = -X; accADC[YAW]  = Z;}
//#define GYRO_ORIENTATION(X, Y, Z) {gyroADC[ROLL] = -Y; gyroADC[PITCH] =  X; gyroADC[YAW] = Z;}
//#define MAG_ORIENTATION(X, Y, Z)  {magADC[ROLL]  = X; magADC[PITCH]  = Y; magADC[YAW]  = Z;}

/**************************************/
/****END OF CONFIGURABLE PARAMETERS****/
/**************************************/
