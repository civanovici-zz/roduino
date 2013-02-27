
#if defined(BI) || defined(TRI) || defined(SERVO_TILT) || defined(GIMBAL) || defined(FLYING_WING) || defined(CAMTRIG)
  #define SERVO
#endif

#if defined(GIMBAL)
  #define NUMBER_MOTOR 0
#elif defined(FLYING_WING)
  #define NUMBER_MOTOR 1
#elif defined(BI)
  #define NUMBER_MOTOR 2
#elif defined(TRI)
  #define NUMBER_MOTOR 3
#elif defined(QUADP) || defined(QUADX) || defined(Y4)
  #define NUMBER_MOTOR 4
#elif defined(Y6) || defined(HEX6) || defined(HEX6X)
  #define NUMBER_MOTOR 6
#elif defined(OCTOX8) || defined(OCTOFLATP) || defined(OCTOFLATX)
  #define NUMBER_MOTOR 8
#endif

void writeServos() {
  #if defined(SERVO)
    atomicServo[0] = (servo[0]-1000)/4;
    atomicServo[1] = (servo[1]-1000)/4;
    atomicServo[2] = (servo[2]-1000)/4;
    atomicServo[3] = (servo[3]-1000)/4;
  #endif
}

void writeMotors() { // [1000;2000] => [125;250]
  #if defined(MEGA)
    for(uint8_t i=0;i<NUMBER_MOTOR;i++)
      analogWrite(PWM_PIN[i], motor[i]>>3);
  #elif defined(PICOPTER)
  
    #define _pqSetPwm(_x, _s) (((((_x) > (MINCOMMAND + (0xFF * _s))) ? (MINCOMMAND + (0xFF * _s)) : (((_x) < (MINCOMMAND)) ? (MINCOMMAND) : (_x))) - MINCOMMAND) / _s)
    // this corresponds to Picopter's motor mapping, see schematic
    OCR1A = _pqSetPwm(motor[0], 4); // REAR_R
    OCR1B = _pqSetPwm(motor[1], 4); // FRONT_R
    OCR3A = _pqSetPwm(motor[2], 4); // REAR_L
    OCR1C = _pqSetPwm(motor[3], 4); // FRONT_L
  #else
    for(uint8_t i=0;i<min(NUMBER_MOTOR,4);i++)
      analogWrite(PWM_PIN[i], motor[i]>>3);
    #if (NUMBER_MOTOR == 6)
      atomicPWM_PIN5_highState = motor[5]/8;
      atomicPWM_PIN5_lowState = 255-atomicPWM_PIN5_highState;
      atomicPWM_PIN6_highState = motor[4]/8;
      atomicPWM_PIN6_lowState = 255-atomicPWM_PIN6_highState;
    #endif
  #endif
}

void writeAllMotors(int16_t mc) {   // Sends commands to all motors
  for (uint8_t i =0;i<NUMBER_MOTOR;i++)
    motor[i]=mc;
  writeMotors();
}

void initOutput() {
  #ifdef PICOPTER
  // initialize timer compare match outputs
  // some of this initialization is done in the "core" wiring.c
  DDRB |= _BV(5) | _BV(6) | _BV(7);
  PORTB &= ~(_BV(5) | _BV(6) | _BV(7));
  DDRE |= _BV(3);
  PORTE &= ~_BV(3);
  TCCR1A |= _BV(COM1A1) | _BV(COM1B1) | _BV(COM1C1);
  TCCR3A |= _BV(COM1A1);
  writeAllMotors(0);
  #else
  writeAllMotors(1000);
  #endif
}

void mixTable() {
  int16_t maxMotor;
  uint8_t i,axis;
  
  #define PIDMIX(X,Y,Z) rcCommand[THROTTLE] + axisPID[ROLL]*X + axisPID[PITCH]*Y + YAW_DIRECTION * axisPID[YAW]*Z

  #if NUMBER_MOTOR > 3
    //prevent "yaw jump" during yaw correction
    axisPID[YAW] = constrain(axisPID[YAW],-100-abs(rcCommand[YAW]),+100+abs(rcCommand[YAW]));
  #endif
  #ifdef QUADP
    motor[0] = PIDMIX( 0,+1,-1); //REAR
    motor[1] = PIDMIX(-1, 0,+1); //RIGHT
    motor[2] = PIDMIX(+1, 0,+1); //LEFT
    motor[3] = PIDMIX( 0,-1,-1); //FRONT
  #endif
  #ifdef QUADX
    motor[0] = PIDMIX(-1,+1,-1); //REAR_R  OCR1A
    motor[1] = PIDMIX(-1,-1,+1); //FRONT_R OCR1B
    motor[2] = PIDMIX(+1,+1,+1); //REAR_L  OCR3A
    motor[3] = PIDMIX(+1,-1,-1); //FRONT_L OCR1C
  #endif

  maxMotor=motor[0];
  for(i=1;i< NUMBER_MOTOR;i++)
    if (motor[i]>maxMotor) maxMotor=motor[i];
  for (i = 0; i < NUMBER_MOTOR; i++) {
    if (maxMotor > MAXTHROTTLE) // this is a way to still have good gyro corrections if at least one motor reaches its max.
      motor[i] -= maxMotor - MAXTHROTTLE;
    motor[i] = constrain(motor[i], MINTHROTTLE, MAXTHROTTLE);    
    if ((rcData[THROTTLE]) < MINCHECK)
      #ifndef MOTOR_STOP
        motor[i] = MINTHROTTLE;
      #else
        motor[i] = MINCOMMAND;
      #endif
    #ifndef PICOPTER
    if (armed == 0)
      motor[i] = MINCOMMAND;
    #endif
  }
}

