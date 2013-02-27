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

#include "PicopterWiiClassicCtrler.h"
#include "twi.h"
#include <util/delay.h>
#include <avr/io.h>
#include <stdlib.h>
#include <math.h>

#define WIIEXT_TWI_ADDR 0x52
#define USE_LIVE_CALIBRATION // disable this if you want more speed and take less memory

typedef struct
{
    double lx;
    double ly;
    double rx;
    double ry;
} dual_stick_calibration_t;

uint8_t twiBuffer[16];
dual_stick_calibration_t init_cali;
#ifdef USE_LIVE_CALIBRATION
dual_stick_calibration_t
#else
dual_stick_info_t
#endif
live_cali;
uint8_t wcc_hasInitialized = 0;

uint8_t wcc_initCalibrate()
{
	  int rawLeftX = 0;
    int rawLeftY = 0;
    int rawRightX = 0;
    int rawRightY = 0;
    
    const int average_length = 10;
    int i;
    for (i = 0; i < average_length; i++)
    {
        twiBuffer[0] = 0x00;
        if (twi_writeTo(WIIEXT_TWI_ADDR, twiBuffer, 1, 1) != 0)
        {
            wcc_hasInitialized = 0;
            return 0;
        }
        
        twi_readFrom(WIIEXT_TWI_ADDR, twiBuffer, 6);
        //wiiext_decrypt(twiBuffer, 6);
        
        rawLeftX += (int)((twiBuffer[0] & 0x3F) << 2);
        rawLeftY += (int)((twiBuffer[1] & 0x3F) << 2);
        rawRightX += (int)((twiBuffer[0] & 0xC0) | ((twiBuffer[1] & 0xC0) >> 2) | ((twiBuffer[2] & 0x80) >> 4));
        rawRightY += (int)((twiBuffer[2] & 0x1F) << 3);
    }
    
    init_cali.lx = (double)rawLeftX / (double)average_length;
    init_cali.ly = (double)rawLeftY / (double)average_length;
    init_cali.rx = (double)rawRightX / (double)average_length;
    init_cali.ry = (double)rawRightY / (double)average_length;
    
    #ifdef USE_LIVE_CALIBRATION
    live_cali.lx = init_cali.lx;    
    live_cali.ly = init_cali.ly;
    live_cali.rx = init_cali.rx;
    live_cali.ry = init_cali.ry;
    #else
    live_cali.lx = lround(init_cali.lx);
    live_cali.ly = lround(init_cali.ly);
    live_cali.rx = lround(init_cali.rx);
    live_cali.ry = lround(init_cali.ry);
    #endif
    
    return 1;
}

uint8_t wcc_initSub()
{
    _delay_ms(25); // power up delay
    
    // initialize the Wii Classic Controller
    // make decryption predictable

    twiBuffer[0] = 0x40; twiBuffer[1] = 0x00; twiBuffer[2] = 0x00; twiBuffer[3] = 0x00; twiBuffer[4] = 0x00; twiBuffer[5] = 0x00; twiBuffer[6] = 0x00;
    if (twi_writeTo(WIIEXT_TWI_ADDR, twiBuffer, 7, 1) != 0)
    {
        wcc_hasInitialized = 0;
        return 0;
    }
        
    _delay_ms(1); // the nunchuk needs some time to process

    twiBuffer[0] = 0x46; twiBuffer[1] = 0x00; twiBuffer[2] = 0x00; twiBuffer[3] = 0x00; twiBuffer[4] = 0x00; twiBuffer[5] = 0x00; twiBuffer[6] = 0x00;
    if (twi_writeTo(WIIEXT_TWI_ADDR, twiBuffer, 7, 1) != 0)
    {
        wcc_hasInitialized = 0;
        return 0;
    }
    _delay_ms(1); // the nunchuk needs some time to process

    twiBuffer[0] = 0x4C; twiBuffer[1] = 0x00; twiBuffer[2] = 0x00; twiBuffer[3] = 0x00; twiBuffer[4] = 0x00; twiBuffer[5] = 0x00; twiBuffer[6] = 0x00;
    if (twi_writeTo(WIIEXT_TWI_ADDR, twiBuffer, 5, 1) != 0)
    {
        wcc_hasInitialized = 0;
        return 0;
    }
    _delay_ms(1); // the nunchuk needs some time to process

    if (wcc_initCalibrate() == 0)
    {
				wcc_hasInitialized = 0;
				return 0;
    }
    
    wcc_hasInitialized = 1;
    return 1;
}

uint8_t wcc_init()
{
    twi_init();
    DDRD &= ~(_BV(0) | _BV(1));
    PORTD &= ~(_BV(0) | _BV(1));

    wcc_hasInitialized = 0;
    
    return wcc_initSub();
}

uint8_t wcc_read(dual_stick_info_t* data)
{
    // auto-reinitialize if disconnected
    if (wcc_hasInitialized == 0)
    {
        if (wcc_initSub() == 0)
        {
            return 0;
        }
    }
    
    int rawLeftX;
    int rawLeftY;
    int rawRightX;
    int rawRightY;
    
    twiBuffer[0] = 0x00;
    if (twi_writeTo(WIIEXT_TWI_ADDR, twiBuffer, 1, 1) != 0)
    {
        wcc_hasInitialized = 0;
        return 0;
    }
    
    twi_readFrom(WIIEXT_TWI_ADDR, twiBuffer, 6);
    //wiiext_decrypt(twiBuffer, 6);
    
    rawLeftX = (int)((twiBuffer[0] & 0x3F) << 2);
    rawLeftY = (int)((twiBuffer[1] & 0x3F) << 2);
    rawRightX = (int)((twiBuffer[0] & 0xC0) | ((twiBuffer[1] & 0xC0) >> 2) | ((twiBuffer[2] & 0x80) >> 4));
    rawRightY = (int)((twiBuffer[2] & 0x1F) << 3);
 
    #ifdef USE_LIVE_CALIBRATION
    const int centerThreshold = 1;
    const double filterConst = 0.05;
    if (rawLeftX >= lround(init_cali.lx) - centerThreshold && rawLeftX <= lround(init_cali.lx) + centerThreshold &&
        rawLeftY >= lround(init_cali.ly) - centerThreshold && rawLeftY <= lround(init_cali.ly) + centerThreshold)
    {
        live_cali.lx = live_cali.lx * (1.0 - filterConst) + (double)rawLeftX * filterConst;
        live_cali.ly = live_cali.ly * (1.0 - filterConst) + (double)rawLeftY * filterConst;
    }
    
    if (rawRightX >= lround(init_cali.rx) - centerThreshold && rawRightX <= lround(init_cali.rx) + centerThreshold &&
        rawRightY >= lround(init_cali.ry) - centerThreshold && rawRightY <= lround(init_cali.ry) + centerThreshold)
    {
        live_cali.rx = live_cali.rx * (1.0 - filterConst) + (double)rawRightX * filterConst;
        live_cali.ry = live_cali.ry * (1.0 - filterConst) + (double)rawRightY * filterConst;
    }
    
    data->lx = rawLeftX - lround(live_cali.lx);
    data->ly = rawLeftY - lround(live_cali.ly);
    data->rx = rawRightX - lround(live_cali.rx);
    data->ry = rawRightY - lround(live_cali.ry);
    #else
    data->lx = rawLeftX - live_cali.lx;
    data->ly = rawLeftY - live_cali.ly;
    data->rx = rawRightX - live_cali.rx;
    data->ry = rawRightY - live_cali.ry;
    #endif
    
    data->buttons = ~((twiBuffer[4] << 8) | twiBuffer[5]);
    
    return 1;
}