
#include "WProgram.h"
#include <avr/pgmspace.h>
#include <Wire.h>
#include <SPI.h>

#define SAMPLE_RATE 32

#define ADCHPD 1
#define ADCS 2
#define HYST 32
#define LINVOL 23
#define RINVOL 23
#define LHPVOL 121
#define RHPVOL 121
#define MICBOOST 0
#define MUTEMIC 0
#define INSEL 0
#define BYPASS 1
#define DACSEL 1
#define SIDETONE 0
#define SIDEATT 0
#define OVERSAMPLE 32
#define AUDIO_FORMAT 2 //
#define DATA_BIT_LENGHTH 2
#define DATA_PHASE 0
#define DATA_CLOCK_SWAP 0
#define MASTER_MODE 1
#define BIT_CLOCK_INVERT 0


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
  Wire.send(0x0e); // analog audio pathway configuration
  Wire.send(0x42);
  //Wire.send(( BIT_CLOCK_INVERT << 7)| MASTER_MODE << 6)|(DATA_CLOCK_SWAP << 5)|(DATA_PHASE << 4)|(1 << 3)|(0 << 2)|(1 << 1)|(0 << 0));
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
  
}



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

// create data variables for audio transfer
int left_in = 0x0000;
int left_out = 0x0000;
int right_in = 0x0000;
int right_out = 0x0000;
void setup() {
  AudioCodec_init(); // setup codec registers
  // call this last if setting up other parts
}
void loop() {
  //while (1); // reduces clock jitter
}



