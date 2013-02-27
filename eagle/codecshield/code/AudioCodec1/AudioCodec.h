// AudioCodec.h
// guest openmusiclabs 7.28.11
// this is the library file for ARDUINO -> there is a different
// file for Maple, make sure you are using the right one.
// place this file in the libraries file of your Arduino sketches folder
// e.g. C:\Documents and Settings\user\My Documents\Arduino\libraries\
// you may have to create the \libraries folder
// modded 4.15.12
// added better pot handling and more sample rates

#ifndef AudioCodec_h // include guard
#define AudioCodec_h

#include "WProgram.h"
#include <avr/pgmspace.h>
#include "mult16x16.h"
#include "mult16x8.h"
#include "mult32x16.h"


#ifndef SAMPLE_RATE
  #define SAMPLE_RATE 44
#elif (SAMPLE_RATE == 88)||(SAMPLE_RATE == 44)||(SAMPLE_RATE == 29)||(SAMPLE_RATE == 22)||(SAMPLE_RATE == 14)||(SAMPLE_RATE == 8)||(SAMPLE_RATE == 7)|| (SAMPLE_RATE == 4)||(SAMPLE_RATE == 2)
#else
  #error SAMPLE_RATE value not defined
#endif

#ifndef ADCHPD
  #define ADCHPD 0
#elif (ADCHPD == 0)||(ADCHPD == 1)
#else
  #error ADCHPD value not defined
#endif

#ifndef ADCS
  #define ADCS 2
#elif (ADCS >=0)&&(ADCS <= 4)
#else
  #error ADCS value not defined
#endif

#ifndef HYST
  #define HYST 32
#elif (HYST >= 0)&&(HYST <= 255)
#else
  #error HYST value not defined
#endif

#ifndef LINVOL
  #define LINVOL 23
#elif (LINVOL >= 0) && (LINVOL <= 0x1f)
#else
  #error LINVOL value not defined
#endif

#ifndef RINVOL
  #define RINVOL 23
#elif (RINVOL >= 0) && (RINVOL <= 0x1f)
#else
  #error RINVOL value not defined
#endif

#ifndef LHPVOL
  #define LHPVOL 121
#elif (LHPVOL == 0) || ((LHPVOL >= 0x30) && (LHPVOL <= 0x7f))
#else
  #error LHPVOL value not defined
#endif

#ifndef RHPVOL
  #define RHPVOL 121
#elif (RHPVOL == 0) || ((RHPVOL >= 0x30) && (RHPVOL <= 0x7f))
#else
  #error RHPVOL value not defined
#endif

#ifndef MICBOOST
  #define MICBOOST 0
#elif (MICBOOST == 0)||(MICBOOST == 1)
#else
  #error MICBOOST value not defined
#endif

#ifndef MUTEMIC
  #define MUTEMIC 1
#elif (MUTEMIC == 0)||(MUTEMIC == 1)
#else
  #error MUTEMIC value not defined
#endif

#ifndef INSEL
  #define INSEL 0
#elif (INSEL == 0)||(INSEL == 1)
#else
  #error INSEL value not defined
#endif

#ifndef BYPASS
  #define BYPASS 0
#elif (BYPASS == 0)||(BYPASS == 1)
#else
  #error BYPASS value not defined
#endif

#ifndef DACSEL
  #define DACSEL 1
#elif (DACSEL == 0)||(DACSEL == 1)
#else
  #error DACSEL value not defined
#endif

#ifndef SIDETONE
  #define SIDETONE 0
#elif (SIDETONE == 0)||(SIDETONE == 1)
#else
  #error SIDETONE value not defined
#endif

#ifndef SIDEATT
  #define SIDEATT 0
#elif (SIDEATT >= 0)&&(SIDEATT <= 3)
#else
  #error SIDEATT value not defined
#endif

#ifndef OVERSAMPLE
  #define OVERSAMPLE 64
#elif (OVERSAMPLE == 1)||(OVERSAMPLE == 2)||(OVERSAMPLE == 4)||(OVERSAMPLE == 8)||(OVERSAMPLE == 16)||(OVERSAMPLE == 32)||(OVERSAMPLE == 64)
#else
  #error OVERSAMPLE value not defined
#endif


// setup variables for ADC
#if ADCS == 0
  // do nothing
#else
  unsigned char _t = OVERSAMPLE; // number of times to oversample
  unsigned char _n = 0x00; // we start with mod0
  unsigned int _modtemp = 0x0000;
#endif


static inline void AudioCodec_init(void) {

  // setup spi peripheral
  digitalWrite(10, LOW); // set ss pin to output low
  pinMode (10, OUTPUT);
  SPI.begin();
  SPI.setBitOrder(MSBFIRST);
  SPI.setClockDivider(SPI_CLOCK_DIV2);
  SPI.setDataMode(SPI_MODE0);
  
  // setup i2c pins and configure codec
  Wire.begin();
  Wire.beginTransmission(0x1a);
  Wire.send(0x0c); // power reduction register
  Wire.send(0x00); // turn everything on
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x0e); // digital data format
  Wire.send(0x03); // 16b SPI mode
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x00); // left in setup register
  Wire.send(LINVOL);
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x02); // right in setup register
  Wire.send(RINVOL);
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x04); // left headphone out register
  Wire.send(LHPVOL);
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x06); // right headphone out register
  Wire.send(RHPVOL);
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x0a); // digital audio path configuration
  Wire.send(ADCHPD);
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x08); // analog audio pathway configuration
  Wire.send((SIDEATT << 6)|(SIDETONE << 5)|(DACSEL << 4)|(BYPASS << 3)|(INSEL << 2)|(MUTEMIC << 1)|(MICBOOST << 0));
  Wire.endTransmission();
  
  Wire.beginTransmission(0x1a);
  Wire.send(0x10); // clock configuration
  #if SAMPLE_RATE == 88
    Wire.send(0xbc);
  #elif SAMPLE_RATE == 44
    Wire.send(0xa0);
  #elif SAMPLE_RATE == 29
    Wire.send(0xa2);
  #elif SAMPLE_RATE == 22
    Wire.send(0xe0);
  #elif SAMPLE_RATE == 14
    Wire.send(0xe2);
  #elif SAMPLE_RATE == 8
    Wire.send(0xac);
  #elif SAMPLE_RATE == 7
    Wire.send(0x8c);
  #elif SAMPLE_RATE == 4
    Wire.send(0xec);
  #elif SAMPLE_RATE == 2
    Wire.send(0xce);
  #endif
  Wire.endTransmission();

  Wire.beginTransmission(0x1a);
  Wire.send(0x12); // codec enable
  Wire.send(0x01);
  Wire.endTransmission();
  
  // setup ADCs
  #if ADCS == 0
    DIDR0 = (1 << ADC1D)|(1 << ADC0D); // turn off digital inputs for ADC0 and ADC1
  #elif (ADCS == 1) || (ADCS == 2)
    ADMUX = 0x40; // start with ADC0 - internal VCC for Vref
    ADCSRA = 0xc7; // ADC enable, single sample, ck/128
    ADCSRB = 0x00; // just in case
    DIDR0 = (1 << ADC1D)|(1 << ADC0D); // turn off digital inputs for ADC0 and ADC1
  #elif (ADCS == 3)
    ADMUX = 0x40; // start with ADC0 - internal VCC for Vref
    ADCSRA = 0xc7; // ADC enable, single sample, ck/128
    ADCSRB = 0x00; // just in case
    DIDR0 = (1<<ADC2D)|(1 << ADC1D)|(1 << ADC0D); // turn off digital inputs for ADC0:2
  #elif (ADCS == 4)
    ADMUX = 0x40; // start with ADC0 - internal VCC for Vref
    ADCSRA = 0xc7; // ADC enable, single sample, ck/128
    ADCSRB = 0x00; // just in case
    DIDR0 = (1<<ADC3D)|(1<<ADC2D)|(1 << ADC1D)|(1 << ADC0D); // turn off digital inputs for ADC0:3
  #endif
  
  // setup timer1 for codec clock division
  TCCR1A = 0x00; // set to CTC mode
  TCCR1B = 0x0f; // set to CTC mode, external clock
  TCCR1C = 0x00; // not used
  TCNT1H = 0x00; // clear the counter
  TCNT1H = 0x00;
  #if SAMPLE_RATE == 88
    OCR1AH = 0x00; // set the counter top
    OCR1AL = 0x3f;
  #elif (SAMPLE_RATE == 44) || (SAMPLE_RATE == 22)
    OCR1AH = 0x00; // set the counter top
    OCR1AL = 0x7f;
  #elif (SAMPLE_RATE == 14) || (SAMPLE_RATE == 29)
    OCR1AH = 0x00; // set the counter top
    OCR1AL = 0xbf;
  #elif (SAMPLE_RATE == 8) || (SAMPLE_RATE == 4)
    OCR1AH = 0x02; // set the counter top
    OCR1AL = 0xbf;
  #elif SAMPLE_RATE == 7
    OCR1AH = 0x02; // set the counter top
    OCR1AL = 0xff;
  #elif SAMPLE_RATE == 2
    OCR1AH = 0x04; // set the counter top
    OCR1AL = 0x7f;
  #endif
  TIMSK1 = 0x02; // turn on compare match interrupt
  
  // turn off all enabled interrupts (delay and wire)
  TIMSK0 = 0x00;
  TWCR = 0x00;

  sei(); // turn on interrupts
}


// adc sample routine
// this creates relatively low noise 16b values from adc samples
#if ADCS == 0
  static inline void AudioCodec_ADC() {
    // do nothing
  }
#elif ADCS == 1
  static inline void AudioCodec_ADC(unsigned int* _mod0value) {
    if (ADCSRA & (1 << ADIF)) { // check if sample ready
      _modtemp += ADCL; // fetch ADCL first to freeze sample
      _modtemp += (ADCH << 8); // add to temp register
      ADCSRA = 0xd7; // reset the interrupt flag
      --_t; // decrement sample counter
      if (_t == 0) { // check if enough samples have been averaged
        // shift value to make a 16b integer
        unsigned char x = 0;
        for (unsigned char y = OVERSAMPLE; y > 1; y >>= 1) {
          x += 1;
        }
        _modtemp <<= 6 - x;
        // add in hysteresis to remove jitter
        if (((_modtemp - *_mod0value) < HYST) || ((*_mod0value - _modtemp) < HYST)) {
        }
        else {
          *_mod0value = _modtemp; // move temp value
	}
        _modtemp = 0x0000; // reset temp value
        _t = OVERSAMPLE; // reset counter
      }
    }
  }
#elif ADCS == 2
  static inline void AudioCodec_ADC(unsigned int* _mod0value, unsigned int* _mod1value) {
    if (ADCSRA & (1 << ADIF)) { // check if sample ready
      _modtemp += ADCL; // fetch ADCL first to freeze sample
      _modtemp += (ADCH << 8); // add to temp register
      --_t; // decrement sample counter
      if (_t == 0) { // check if enough samples have been averaged
        // shift value to make a 16b integer
        unsigned char x = 0;
        for (unsigned char y = OVERSAMPLE; y > 1; y >>= 1) {
          x += 1;
        }
        _modtemp <<= 6 - x;
	if (_n == 0) { // check if just finished with mod0
	  ADMUX = 0x41; // change mux to mod1
          _n += 1; // index counter to mod1      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod0value) < HYST) || ((*_mod0value - _modtemp) < HYST)) {
          }
          else {
            *_mod0value = _modtemp; // move temp value
	  }
        }
        else { // just finished with mod1
	  ADMUX = 0x40; // change mux to mod0
          _n = 0; // set counter to mod0      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod1value) < HYST) || ((*_mod1value - _modtemp) < HYST)) {
          }
          else {
            *_mod1value = _modtemp; // move temp value
	  }
        }
        _modtemp = 0x0000; // reset temp value
        _t = OVERSAMPLE; // reset counter
      }
      ADCSRA = 0xd7; // reset the interrupt flag and start next conversion
    }
  }
#elif ADCS == 3
  static inline void AudioCodec_ADC(unsigned int* _mod0value, unsigned int* _mod1value, unsigned int* _mod2value) {
    if (ADCSRA & (1 << ADIF)) { // check if sample ready
      _modtemp += ADCL; // fetch ADCL first to freeze sample
      _modtemp += (ADCH << 8); // add to temp register
      --_t; // decrement sample counter
      if (_t == 0) { // check if enough samples have been averaged
        // shift value to make a 16b integer
        unsigned char x = 0;
        for (unsigned char y = OVERSAMPLE; y > 1; y >>= 1) {
          x += 1;
        }
        _modtemp <<= 6 - x;
	if (_n == 0) { // check if just finished with mod0
	  ADMUX = 0x41; // change mux to mod1
          _n += 1; // index counter to mod1      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod0value) < HYST) || ((*_mod0value - _modtemp) < HYST)) {
          }
          else {
            *_mod0value = _modtemp; // move temp value
	  }
        }
        else if (_n == 1){ // just finished with mod1
	  ADMUX = 0x42; // change mux to mod2
          _n += 1; // set counter to mod2      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod1value) < HYST) || ((*_mod1value - _modtemp) < HYST)) {
          }
          else {
            *_mod1value = _modtemp; // move temp value
	  }
        }
        else { // just finished with mod2
	  ADMUX = 0x40; // change mux to mod0
          _n = 0; // set counter to mod0 
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod2value) < HYST) || ((*_mod2value - _modtemp) < HYST)) {
          }
          else {
            *_mod2value = _modtemp; // move temp value
	  }
        }
        _modtemp = 0x0000; // reset temp value
        _t = OVERSAMPLE; // reset counter
      }
      ADCSRA = 0xd7; // reset the interrupt flag and start next conversion
    }
  }
#elif ADCS == 4
  static inline void AudioCodec_ADC(unsigned int* _mod0value, unsigned int* _mod1value, unsigned int* _mod2value, unsigned int* _mod3value) {
    if (ADCSRA & (1 << ADIF)) { // check if sample ready
      _modtemp += ADCL; // fetch ADCL first to freeze sample
      _modtemp += (ADCH << 8); // add to temp register
      --_t; // decrement sample counter
      if (_t == 0) { // check if enough samples have been averaged
        // shift value to make a 16b integer
        unsigned char x = 0;
        for (unsigned char y = OVERSAMPLE; y > 1; y >>= 1) {
          x += 1;
        }
        _modtemp <<= 6 - x;
	if (_n == 0) { // check if just finished with mod0
	  ADMUX = 0x41; // change mux to mod1
          _n += 1; // index counter to mod1      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod0value) < HYST) || ((*_mod0value - _modtemp) < HYST)) {
          }
          else {
            *_mod0value = _modtemp; // move temp value
	  }
        }
        else if (_n == 1){ // just finished with mod1
	  ADMUX = 0x42; // change mux to mod2
          _n += 1; // set counter to mod2      
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod1value) < HYST) || ((*_mod1value - _modtemp) < HYST)) {
          }
          else {
            *_mod1value = _modtemp; // move temp value
	  }
        }
        else if (_n == 2) { // just finished with mod2
	  ADMUX = 0x43; // change mux to mod3
          _n += 1; // set counter to mod3
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod2value) < HYST) || ((*_mod2value - _modtemp) < HYST)) {
          }
          else {
            *_mod2value = _modtemp; // move temp value
	  }
        }
        else { // just finished with mod3
	  ADMUX = 0x40; // change mux to mod0
          _n = 0; // set counter to mod0 
          // add in hysteresis to remove jitter
          if (((_modtemp - *_mod3value) < HYST) || ((*_mod3value - _modtemp) < HYST)) {
          }
          else {
            *_mod3value = _modtemp; // move temp value
	  }
        }
        _modtemp = 0x0000; // reset temp value
        _t = OVERSAMPLE; // reset counter
      }
      ADCSRA = 0xd7; // reset the interrupt flag and start next conversion
    }
  }
#endif


// codec data transfer function
static inline void AudioCodec_data(int* _lin, int* _rin, int _lout, int _rout) {

  int _out_temp = _lout;
  PORTB |= (1<<PORTB2);  // toggle ss pina
  asm volatile ("out %0, %B1" : : "I" (_SFR_IO_ADDR(SPDR)), "r" (_out_temp) );
  PORTB &= ~(1<<PORTB2); // toggle ss pin
  while(!(SPSR & (1<<SPIF))){ // wait for data transfer to complete
  }
  asm volatile ("out %0, %A1" : : "I" (_SFR_IO_ADDR(SPDR)), "r" (_out_temp) );
  asm volatile ("in r3, %0" : : "I" (_SFR_IO_ADDR(SPDR)) );
  _out_temp = _rout;
  while(!(SPSR & (1<<SPIF))){ // wait for data transfer to complete
  }
  asm volatile ("out %0, %B1" : : "I" (_SFR_IO_ADDR(SPDR)), "r" (_out_temp) );
  asm volatile ("in r2, %0" : : "I" (_SFR_IO_ADDR(SPDR)) );
  asm volatile ("movw %0, r2" : "=r" (*_lin) : );
  while(!(SPSR & (1<<SPIF))){ // wait for data transfer to complete
  }
  asm volatile ("out %0, %A1" : : "I" (_SFR_IO_ADDR(SPDR)), "r" (_out_temp) );
  asm volatile ("in r3, %0" : : "I" (_SFR_IO_ADDR(SPDR)) );
  while(!(SPSR & (1<<SPIF))){ // wait for data transfer to complete
  }
  asm volatile ("in r2, %0" : : "I" (_SFR_IO_ADDR(SPDR)) );
  asm volatile ("movw %0, r2" : "=r" (*_rin) : );
}


#endif // end include guard

