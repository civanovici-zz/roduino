volatile uint16_t rcValue[8] = {1502,1502,1502,1502,1502,1502,1502,1502}; // interval [1000;2000]

// Configure each rc pin for PCINT
void configureReceiver() {
  // Nothing to do here for Picopter
}
  
void computeRC() {
  static int16_t rcData4Values[8][4], rcDataMean[8];
  static uint8_t rc4ValuesIndex = 0;
  uint8_t chan,a,ind;

  #if defined(SBUS)
    readSBus();
  #endif
  rc4ValuesIndex++;
  for (chan = 0; chan < 8; chan++) {
    rcData4Values[chan][rc4ValuesIndex%4] =
    #ifndef PICOPTER
    readRawRC(chan);
    #else
    pcr_rcChan[chan];
    #endif
    rcDataMean[chan] = 0;
    if (chan != AUX1 && chan != AUX2) {
      for (a=0;a<4;a++) rcDataMean[chan] += rcData4Values[chan][a];
      rcDataMean[chan]= (rcDataMean[chan]+2)/4;
      if ( rcDataMean[chan] < rcData[chan] -3)  rcData[chan] = rcDataMean[chan]+2;
      if ( rcDataMean[chan] > rcData[chan] +3)  rcData[chan] = rcDataMean[chan]-2;
    }
    else {
      rcData[chan] =
      #ifndef PICOPTER
      readRawRC(chan);
      #else
      pcr_rcChan[chan];
      #endif
    }
  }
}

