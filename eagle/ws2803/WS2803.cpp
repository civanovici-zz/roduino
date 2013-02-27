#include "SPI.h"
#include "WS2803.h"

// Example to control WS2803-based RGB LED Modules in a strand or strip
// Written by Adafruit - MIT license
// Adapted by RoDuino
/*****************************************************************************/

// Constructor for use with hardware SPI (specific clock/data pins):
WS2803::WS2803(uint16_t n) {
  alloc(n);
  updatePins();
}

// Constructor for use with arbitrary clock/data pins:
WS2803::WS2803(uint16_t n, uint8_t dpin, uint8_t cpin) {
  alloc(n);
  updatePins(dpin, cpin);
}

// Allocate 3 bytes per pixel, init to RGB 'off' state:
void WS2803::alloc(uint16_t n) {
  begun   = false;
  numLEDs = n;
  pixels  = (uint8_t *)calloc(n, 3);//ar trebui nx3x6, numarul de drivere, 3 byte/pixel, 6 pixel/driver
}

// Activate hard/soft SPI as appropriate:
void WS2803::begin(void) {
  if(hardwareSPI == true) {
    startSPI();
  } else {
    pinMode(datapin, OUTPUT);
    pinMode(clkpin , OUTPUT);
  }
  begun = true;
}

// Change pin assignments post-constructor, switching to hardware SPI:
void WS2803::updatePins(void) {
  hardwareSPI = true;
  datapin     = clkpin = 0;
  // If begin() was previously invoked, init the SPI hardware now:
  if(begun == true) startSPI();
  // Otherwise, SPI is NOT initted until begin() is explicitly called.

  // Note: any prior clock/data pin directions are left as-is and are
  // NOT restored as inputs!
}

// Change pin assignments post-constructor, using arbitrary pins:
void WS2803::updatePins(uint8_t dpin, uint8_t cpin) {

  if(begun == true) { // If begin() was previously invoked...
    // If previously using hardware SPI, turn that off:
    if(hardwareSPI == true) SPI.end();
    // Regardless, now enable output on 'soft' SPI pins:
    pinMode(dpin, OUTPUT);
    pinMode(cpin, OUTPUT);
  } // Otherwise, pins are not set to outputs until begin() is called.

  // Note: any prior clock/data pin directions are left as-is and are
  // NOT restored as inputs!

  hardwareSPI = false;
  datapin     = dpin;
  clkpin      = cpin;
  clkport     = portOutputRegister(digitalPinToPort(cpin));
  clkpinmask  = digitalPinToBitMask(cpin);
  dataport    = portOutputRegister(digitalPinToPort(dpin));
  datapinmask = digitalPinToBitMask(dpin);
}

// Enable SPI hardware and set up protocol details:
void WS2803::startSPI(void) {
    SPI.begin();
    SPI.setBitOrder(MSBFIRST);
    SPI.setDataMode(SPI_MODE0);
    SPI.setClockDivider(SPI_CLOCK_DIV8);  // 2 MHz
    // WS2803 datasheet recommends max SPI clock of 2 MHz, and 50 Ohm
    // resistors on SPI lines for impedance matching.  In practice and
    // at short distances, 2 MHz seemed to work reliably enough without
    // resistors, and 4 MHz was possible with a 220 Ohm resistor on the
    // SPI clock line only.  Your mileage may vary.  Experiment!
    // SPI.setClockDivider(SPI_CLOCK_DIV4);  // 4 MHz
}

uint16_t WS2803::numPixels(void) {
  return numLEDs;
}

void WS2803::show(void) {
  uint16_t i, nl3 = numLEDs * 3; // 3 bytes per LED
  uint8_t  bit;

  // Write 24 bits per pixel:
  if(hardwareSPI) {
    for(i=0; i<nl3; i++) {
      SPDR = pixels[i];
      while(!(SPSR & (1<<SPIF)));
    }
  } else {
    for(i=0; i<nl3; i++ ) {
      for(bit=0x80; bit; bit >>= 1) {
        if(pixels[i] & bit) *dataport |=  datapinmask;
        else                *dataport &= ~datapinmask;
        *clkport |=  clkpinmask;
        *clkport &= ~clkpinmask;
      }
    }
  }

  delay(1); // Data is latched by holding clock pin low for 1 millisecond
}

// Set pixel color from separate 8-bit R, G, B components:
void WS2803::setPixelColor(uint16_t n, uint8_t r, uint8_t g, uint8_t b) {
  if(n < numLEDs) { // Arrays are 0-indexed, thus NOT '<='
    uint8_t *p = &pixels[n * 3];
    *p++ = r;
    *p++ = g;
    *p++ = b;
  }
}

// Set pixel color from 'packed' 32-bit RGB value:
void WS2803::setPixelColor(uint16_t n, uint32_t c) {
  if(n < numLEDs) { // Arrays are 0-indexed, thus NOT '<='
    uint8_t *p = &pixels[n * 3];
    *p++ = c >> 16;
    *p++ = c >>  8;
    *p++ = c;
  }
}

