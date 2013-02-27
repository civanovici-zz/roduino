static uint8_t point;
static uint8_t serDataFrom;
static uint8_t serChkSum;
static uint8_t s[128];
void serialize16(int16_t a) {s[point++]  = a; s[point++]  = a>>8&0xff;}
void serialize8(uint8_t a)  {s[point++]  = a;}

// ***********************************
// Interrupt driven UART transmitter for MIS_OSD
// ***********************************
static uint8_t tx_ptr;
static uint8_t tx_busy = 0;

ISR_UART {
  UDR0 = s[tx_ptr++];           /* Transmit next byte */
  if ( tx_ptr == point ) {      /* Check if all data is transmitted */
    UCSR0B &= ~(1<<UDRIE0);     /* Disable transmitter UDRE interrupt */
    tx_busy = 0;
  }
}

void UartSendData() {          // start of the data block transmission
  cli();
  tx_ptr = 0;
  UCSR0A |= (1<<UDRE0);        /* Clear UDRE interrupt flag */
  UCSR0B |= (1<<UDRIE0);       /* Enable transmitter UDRE interrupt */
  UDR0 = s[tx_ptr++];          /* Start transmission */
  tx_busy = 1;
  sei();
}

void serialCom() {
  int16_t a;
  uint8_t i;

  if ((!tx_busy) && (Serial.available() || PicopterRadio.available())) {
    uint8_t cmd;
    if (Serial.available()) {
      cmd = Serial.read();
      serDataFrom = 1;
    }
    else {
      cmd = PicopterRadio.read();
      serDataFrom = 2;
    }
    Stream* dataSource = (serDataFrom == 2) ? ((Stream*)(&PicopterRadio)) : ((Stream*)(&Serial));
    switch (cmd) {
    case 'M': // Multiwii @ arduino to GUI all data
      point=0;
      serialize8('M');
      serialize8(VERSION);  // MultiWii Firmware version
      for(i=0;i<3;i++) serialize16(accSmooth[i]);
      for(i=0;i<3;i++) serialize16(gyroData[i]/8);
      //for(i=0;i<3;i++) serialize16(magADC[i]/3);
      //serialize16(EstAlt/10);
      //serialize16(heading); // compass
      //for(i=0;i<4;i++) serialize16(servo[i]);
      for(i=0;i<8;i++) serialize16(motor[i]);
      for(i=0;i<8;i++) serialize16(rcData[i]);
      serialize8(nunchuk|ACC<<1|BARO<<2|MAG<<3|GPSPRESENT<<4);
      serialize8(accMode|baroMode<<1|magMode<<2|(GPSModeHome|GPSModeHold)<<3);
      serialize16(cycleTime);
      for(i=0;i<2;i++) serialize16(angle[i]/10);
      serialize8(MULTITYPE);
      for(i=0;i<5;i++) {serialize8(P8[i]);serialize8(I8[i]);serialize8(D8[i]);}
      serialize8(P8[PIDLEVEL]);
      serialize8(I8[PIDLEVEL]);
      //serialize8(P8[PIDMAG]);
      serialize8(rcRate8);
      serialize8(rcExpo8);
      serialize8(rollPitchRate);
      serialize8(yawRate);
      serialize8(dynThrPID);
      //for(i=0;i<8;i++) serialize8(activate[i]);
      //serialize16(0);
      //serialize16(0);
      //serialize8(0);
      //serialize8(0);
      //serialize8(0);
      //serialize16(0);serialize16(0);
      //serialize8(vbat);
      serialize16(0);        // 4 variables are here for general monitoring purpose
      serialize16(i2c_errors_count);  // debug2
      serialize16(0);                 // debug3
      serialize16(point);                 // debug4
      serialize8('M');
      if (serDataFrom == 1) {
        UartSendData(); // Serial.write(s,point);
      }
      else if (serDataFrom == 2) {
        PicopterRadio.write(s, point);
      }
      break;
    case 'O':  // arduino to OSD data - contribution from MIS
      point=0;
      serialize8('O');
      for(i=0;i<3;i++) serialize16(accSmooth[i]);
      for(i=0;i<3;i++) serialize16(gyroData[i]);
      //serialize16(EstAlt*10.0f);
      //serialize16(heading); // compass - 16 bytes
      for(i=0;i<2;i++) serialize16(angle[i]); //20
      for(i=0;i<6;i++) serialize16(motor[i]); //32
      for(i=0;i<6;i++) {serialize16(rcData[i]);} //44
      serialize8(nunchuk|ACC<<1|BARO<<2|MAG<<3);
      serialize8(accMode|baroMode<<1|magMode<<2);
      //serialize8(vbat);     // Vbatt 47
      serialize8(VERSION);  // MultiWii Firmware version
      serialize8('O'); //49
      if (serDataFrom == 1) {
        UartSendData(); // Serial.write(s,point);
      }
      else if (serDataFrom == 2) {
        PicopterRadio.write(s, point);
      }
      break;
    case 'W': //GUI write params to eeprom @ arduino
      while (dataSource->available()<1) {}
      serChkSum = 0;
      point = dataSource->read();
      for(i = 0; i<point; i++) {
        while (dataSource->available()<1) {}
        s[i] = dataSource->read();
        serChkSum += s[i];
      }
      while (dataSource->available()<1) {}
      point = dataSource->read();
      if (point == serChkSum) {
        point = 0;
        for(i=0;i<5;i++) {P8[i]= s[point++]; I8[i]= s[point++]; D8[i]= s[point++];} //15
        P8[PIDLEVEL] = s[point++]; I8[PIDLEVEL] = s[point++]; //17
        //P8[PIDMAG] = dataSource->read(); //18
        rcRate8 = s[point++]; rcExpo8 = s[point++]; //20
        rollPitchRate = s[point++];
        yawRate = s[point++]; //22
        dynThrPID = s[point++]; //23
        //for(i=0;i<8;i++) activate[i] = dataSource->read(); //31
        //dataSource->read();dataSource->read(); // so we unload the two bytes
        writeParams();
      }
      break;
    case 'S': //GUI to arduino ACC calibration request
      calibratingA=400;
      calibratingG=400;
      break;
    case 'E': //GUI to arduino MAG calibration request
      calibratingM=1;
      break;
    case 'C':
      getNewRadioFreq();
      break;
    }
    
    if (serDataFrom == 2) {
      PicopterRadio.setFlushOnNext();
    }
  }
}

void getNewRadioFreq()
{
  long startTime = millis();
  char s[8];
  uint8_t i = 0, c = 0;
  
  // read in a string until timeout, or null termination, or CR, or NL
  do {
    if (Serial.available()) {
      c = Serial.read();
      if (c == '\0' || c == '\r' || c == '\n') {
        s[i] = '\0';
        break;
      }
      s[i] = c;
      s[i+1] = '0';
      i++;
      startTime = millis();
    }
  }
  while (millis() - startTime > 50);
  
  s[i] = '\0'; // force end the string
  
  // translate and set if possible
  if (strlen(s) > 0) {
    int chan = atoi(s);
    PicopterRadio.setChannel((chan % PCR_RFCHAN_MAX) + PCR_RFCHAN_START);
    Serial.print("C"); // send reply
    for (chan = 0; chan < 5; chan++) {
      LEDPIN_ON;
      delay(100);
      LEDPIN_OFF;
      delay(100);
    }
  }
}
