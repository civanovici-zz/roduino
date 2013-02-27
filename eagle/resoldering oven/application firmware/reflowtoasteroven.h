/* Reflow Toaster Oven
 * http://frank.circleofcurrent.com/reflowtoasteroven/
 * Copyright (c) 2011 Frank Zhao
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 *
 * This file is a header file containing a lot of globally used configuration
 *
 */

#ifndef reflowtoasteroven_h
#define reflowtoasteroven_h

#include <stdio.h>

#define TMR_PRESCALER 64
#define TMR_OVF_TIMESPAN 0.002048// timespan (in seconds) between consecutive timer overflow events
#define THERMOCOUPLE_CONSTANT 0.32 // this is derived from the AD595AQ datasheet
#define ROOM_TEMP 20.0
#define TEMP_MEASURE_CHAN 7 // the ADC pin connected to the AD595AQ
#define DEMO_MODE 0 // 1 means the current temperature reading will always be overwritten to match the target temperature, note that PWM output is still active even if in DEMO mode
#define USE_USB // define this to use PJRC's USB-serial to communicate to the computer, if this is not defined, nothing is really implemented to handle the logging stream
#define BOOTLOADER_ADDR 0x7000

typedef struct
{
	double start_rate;
	double soak_temp1;
	double soak_temp2;
	uint16_t soak_length;
	double peak_temp;
	uint16_t time_to_peak;
	double cool_rate;
} profile_t;

typedef struct
{
	double pid_p;
	double pid_i;
	double pid_d;
	double max_temp;
	double time_to_max;
} settings_t;

extern settings_t settings;
extern FILE log_stream;
extern volatile char tmr_writelog_flag;
extern volatile char tmr_checktemp_flag;
extern volatile char tmr_drawlcd_flag;

void profile_setdefault(profile_t* profile);
void settings_setdefault(settings_t* s);
char profile_valid(profile_t* profile);
char settings_valid(settings_t* s);
void auto_go(profile_t* profile);
char* str_from_int(signed long value);
char* str_from_double(double value, int decimalplaces);

#define min(x,y) (((x) < (y)) ? (x) : (y))
#define max(x,y) (((x) > (y)) ? (x) : (y))
#define sensor_to_temperature(x) ((x)*THERMOCOUPLE_CONSTANT)

#endif