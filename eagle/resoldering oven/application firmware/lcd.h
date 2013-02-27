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
 * This file is a header file
 *
 */

#ifndef lcd_h
#define lcd_h

#include <stdio.h>

#define LCD_WIDTH 160
#define LCD_HEIGHT 100
#define LCD_ROWS 13 // 100 pix high, 8 bits, 100/8=12.5, thus 13 rows
#define FONT_WIDTH 6
#define FONT_TAB_SIZE 4

void lcd_init();
void lcd_set_row(uint8_t r);
void lcd_set_column(uint8_t c);
void lcd_set_row_column(uint8_t r, uint8_t c);
void lcd_draw_start();
void lcd_draw_end();
void lcd_draw_unit(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3);
void lcd_clear_row(char r, char c_start);
void lcd_clear_restofrow();
void lcd_clear_screen();
void lcd_draw_char(char c);

extern FILE lcd_stream;

#endif