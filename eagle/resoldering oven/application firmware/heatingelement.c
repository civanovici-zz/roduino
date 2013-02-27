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
 * This file contains the controls for the heating element using PWM
 *
 */
 
#include <avr/io.h>
#include <avr/interrupt.h>
#include "heatingelement.h"

void heat_init()
{
	// initialize the SSR control pin as output
	PWM_DDRx |= _BV(PWM_PIN);
	PWM_PORTx &= ~_BV(PWM_PIN);
}

volatile uint16_t pwm_ocr = 0;
volatile uint16_t pwm_ocr_temp = 0;
volatile uint16_t heat_isr_cnt = 0;

// this function needs to be called during the timer overflow interrupt of a 8-bit timer running at 8MHz/64
inline void heat_isr()
{
	if (heat_isr_cnt == 511)
	{
		heat_isr_cnt = 0;
		pwm_ocr = pwm_ocr_temp;
		if (pwm_ocr > 0)
		{
			PWM_PORTx |= _BV(PWM_PIN);
		}
		else
		{
			PWM_PORTx &= ~_BV(PWM_PIN);
		}
	}
	else
	{
		if (pwm_ocr <= heat_isr_cnt)
		{
			PWM_PORTx &= ~_BV(PWM_PIN);
		}
		
		heat_isr_cnt++;
	}
}

void heat_set(uint16_t ocr)
{
	pwm_ocr_temp = ocr >> 7;
}