/* Copyright (c) 2011 Frank Zhao
   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

   * Redistributions of source code must retain the above copyright
     notice, this list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright
     notice, this list of conditions and the following disclaimer in the
     documentation and/or other materials provided with the distribution.
   * Neither the name of the authors nor the names of its contributors
     may be used to endorse or promote products derived from this software
     without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
   AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
   ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
   LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
   CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
   SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
   CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
   ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
   POSSIBILITY OF SUCH DAMAGE. */

#ifndef PicopterRadio_h

#include "ZigduinoRadioCfg.h"

#ifdef __cplusplus
extern "C" {
#endif

#include "radio.h"
#include "board.h"
#include "transceiver.h"

#ifdef __cplusplus
} // extern "C"
#endif

// device address definitions
#define PCR_FLIER_DEVADDR 0x12
#define PCR_CTRLER_DEVADDR 0x34
#define PCR_RCCHAN_CNT 6 // number of RC channels
#define PCR_RFCHAN_MAX 3 // should be 16 in theory
#define PCR_RFCHAN_START 11

// these button maps correspond with the bits of the Wii Classic Controller button bytes
//  |  7  |  6  |  5  |  4  |  3  |  2  |  1  |  0  |
// 4| BDR | BDD | BLT | B-  | BH  | B+  | BRT |  1  |
// 5| BZL | BY  | BY  | BA  | BX  | BZR | BDL | BDU |
#define PCC_ARM 10
#define PCC_DISARM 12
#define PCC_ZEROSENSORS 3
#define PCC_ACROMODE 9
#define PCC_STABLEMODE 2
#define PCC_SYNCCHAN 4
#define PCC_WIICALI 5
#define PCC_ACCTRIM 7
#define PCC_ACCTRIMUP 0
#define PCC_ACCTRIMDOWN 14
#define PCC_ACCTRIMLEFT 1
#define PCC_ACCTRIMRIGHT 15

#define PCF_LED1_ON()  PORTF |= _BV(2)
#define PCF_LED1_TOG() PORTF ^= _BV(2)
#define PCF_LED1_OFF() PORTF &= ~_BV(2)
#define PCF_LED2_ON()  PORTE |= _BV(2)
#define PCF_LED2_TOG() PORTE ^= _BV(2)
#define PCF_LED2_OFF() PORTE &= ~_BV(2)

#define PCS_LED1_ON()  PORTF |= _BV(2)
#define PCS_LED1_TOG() PORTF ^= _BV(2)
#define PCS_LED1_OFF() PORTF &= ~_BV(2)
#define PCS_LED2_ON()  PORTF &= ~_BV(3)
#define PCS_LED2_TOG() PORTF ^= _BV(3)
#define PCS_LED2_OFF() PORTF |= _BV(3)

#ifdef __cplusplus
#include <Stream.h>
#include <Print.h>

// just a class definition, for usage and comments, see the c/cpp file
class cPicopterRadio : public Stream
{
    private:
    public:
        cPicopterRadio();
        void begin(uint8_t);
        void requestDiscovery();
        uint8_t hasDiscovered();
        void setChannel(channel_t chan);
        void sendFlightCommands();
        void setRxMode();
        uint8_t rand();
        uint8_t isPresent();
        void setFlushOnNext();
        virtual int available(void);
        virtual int peek(void);
        virtual int read(void);
        virtual void flush(void);
        void flushTx(void);
        virtual void write(uint8_t);
        using Print::write; // pull in write(str) and write(buf, size) from Print
};

extern cPicopterRadio PicopterRadio; // make single instance accessible

#else

// function prototypes if compiled as C
void pcr_begin(uint8_t);
void pcr_requestDiscovery();
uint8_t pcr_hasDiscovered();
int pcr_available(void);
int pcr_peek(void);
int pcr_read(void);
void pcr_flush(void);
uint8_t pcr_rand();
uint8_t pcr_isPresent();
void pcr_setFlushOnNext();

#endif

// function prototypes for both C and C++
void pcr_flushTx(void);
void pcr_write(uint8_t);
void pcr_setRxMode(void);

extern volatile uint16_t pcr_rcChan[PCR_RCCHAN_CNT]; // load or read RC channels from here

#define PicopterRadio_h
#endif