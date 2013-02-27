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
 * This file handles pin definitions for the buttons on the PCB
 * also the button debounce delay is defined here
 *
 */

#ifndef buttons_h
#define buttons_h

#include <avr/io.h>
#include <util/delay.h>

#define button_debounce_us(x) _delay_us(x)
#define button_debounce_ms(x) _delay_ms(x)
#define button_debounce() _delay_us(100)

#define buttons_init() do{\
PORTD |= _BV(2);\
DDRD &= ~_BV(2);\
PORTD |= _BV(3);\
DDRD &= ~_BV(3);\
PORTD |= _BV(5);\
DDRD &= ~_BV(5);\
}while(0);

#define button_up() bit_is_clear(PIND, 5)
#define button_mid() bit_is_clear(PIND, 3)
#define button_down() bit_is_clear(PIND, 2)

#endif