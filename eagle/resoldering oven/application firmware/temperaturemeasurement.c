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
 * This file contains routines to take temperature measurements from the AD595AQ thermocouple IC using the AVR's ADC
 *
 */

#include <avr/io.h>
#include <avr/interrupt.h>
#include <math.h>
#include <stdlib.h>

#include "reflowtoasteroven.h"
#include "temperaturemeasurement.h"

#define ADC_SAMPLE_SIZE 128
#define ADC_AVERAGE_SIZE (ADC_SAMPLE_SIZE/4)
volatile uint16_t adc_samples[ADC_SAMPLE_SIZE];
volatile uint8_t adc_sample_idx;
uint16_t temp_last_read = 0;

/*
 * This file contains code that is specific for measuring the temperature using the AD595AQ
 * I discovered that the AD595AQ sometimes outputs a square wave instead of a steady voltage
 * 
 * I'm not sure if this is intentional and a part of the AD595AQ's design, or if it has something to do with my own circuit
 *
 * Whatever the reason is, this code is written to take the peak readings of the square wave, while ignoring the troughs
 *
 * 
*/

uint16_t sensor_read()
{
	int i, j;
	uint16_t working_sample;
	uint16_t minimum = 0x7FFF;
	uint16_t maximum = 0x0000;
	uint16_t middle;
	uint32_t sum = 0;
	uint8_t sum_cnt = 0;
	
	// determine the crest and troughs of the wave
	for (i = 0; i < ADC_SAMPLE_SIZE; i++)
	{
		working_sample = adc_samples[i];
		
		if (working_sample > maximum)
		{
			maximum = working_sample;
		}
		
		if (working_sample < minimum)
		{
			minimum = working_sample;
		}
	}
	
	// calculate the value in the middle of the wave
	middle = ((maximum - minimum) / 2) + minimum;
	// now we can take the peak values since we know the middle value
	
	// average recent peak values while ignoring troughs
	for (i = 0, j = adc_sample_idx; sum_cnt < ADC_AVERAGE_SIZE; i++)
	{
		working_sample = adc_samples[j];
		if (working_sample >= middle)
		{
			sum += working_sample;
			sum_cnt++;
		}
		
		j = (j + ADC_SAMPLE_SIZE - 1) % ADC_SAMPLE_SIZE;
	}
	
	uint16_t result = (uint16_t)lround((double)sum / (double)sum_cnt);
	if (result < temp_last_read - 5 && temp_last_read > 5)
	{
		result = temp_last_read - 5;
	}
	//else
	{
		temp_last_read = result;
	}
	
	return result;
}

void sensor_filter_reset()
{
	temp_last_read = 0;
}

// conversion functions
inline uint16_t temperature_to_sensor(double temp)
{
	return (uint16_t)lround(temp / THERMOCOUPLE_CONSTANT);
}

// new sample has arrived
ISR(ADC_vect)
{
	// read in sample
	adc_samples[adc_sample_idx] = ADCL;
	adc_samples[adc_sample_idx] |= (ADCH << 8);
	adc_sample_idx = (adc_sample_idx + 1) % ADC_SAMPLE_SIZE;
	
	// initiate next reading
	ADCSRA = _BV(ADEN) | _BV(ADSC) | _BV(ADIE) | _BV(ADPS2) | _BV(ADPS1) | _BV(ADPS0);
}

void adc_init()
{
	adc_sample_idx = 0;
	ADMUX = _BV(REFS0) | TEMP_MEASURE_CHAN; // set channel and reference
	ADCSRA = _BV(ADEN) | _BV(ADSC) | _BV(ADIE) | _BV(ADPS2) | _BV(ADPS1) | _BV(ADPS0); // initialize first reading, with slowest prescaler, and enable ADC interrupt
}