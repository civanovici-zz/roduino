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
 * This file handles a lot of initialization, low level timer tasks, and automatic temperature control
 *
 */
 
#include <avr/power.h>
#include <avr/wdt.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <util/delay.h>

#include "reflowtoasteroven.h"
#include "lcd.h"
#include "temperaturemeasurement.h"
#include "heatingelement.h"
#include "buttons.h"
#ifdef USE_USB
#include "usb_serial.h"
#endif

settings_t settings; // store this globally so it's easy to access

void main_menu();
void tmr_init();

int main()
{
	// disable stuff enabled by bootloader/fuses
	MCUSR &= ~((1 << WDRF) | (1 << IVCE) | (1 << IVSEL));
	wdt_disable();

	// enable clock division
	clock_prescale_set(clock_div_2);
	
	// initialize stuff here
	
	sei(); // initialize interrupts
	
	#ifdef USE_USB
	usb_init();
	#else
	// if you plan on using another AVR without built-in USB
	// then initialize the serial port here
	#endif
	
	fprintf_P(&log_stream, PSTR("hello world,\n"));
	
	adc_init();
	lcd_init();
	buttons_init();
	tmr_init();
	heat_init();
	
	fprintf_P(&log_stream, PSTR("reflow toaster oven,\n"));
	
	// initialization has finished here
	
	main_menu(); // enter the menu system
	
	return 0;
}

// this estimates the PWM duty cycle needed to reach a certain steady temperature
// if the toaster is capable of a maximum of 300 degrees, then 100% duty cycle is used if the target temperature is 300 degrees, and 0% duty cycle is used if the target temperature is room temperature.
double approx_pwm(double target)
{
	return 65535.0 * ((target * THERMOCOUPLE_CONSTANT) / settings.max_temp);
}

uint16_t pid(double target, double current, double * integral, double * last_error)
{
	double error = target - current;
	if (target == 0)
	{
		// turn off if target temperature is 0
		
		(*integral) = 0;
		(*last_error) = error;
		return 0;
	}
	else
	{
		if (target < 0)
		{
			target = 0;
		}
		
		// calculate PID terms
		
		double p_term = settings.pid_p * error;		
		double new_integral = (*integral) + error;
		double d_term = ((*last_error) - error) * settings.pid_d;
		(*last_error) = error;
		double i_term = new_integral * settings.pid_i;
		
		double result = approx_pwm(target) + p_term + i_term + d_term;
		
		// limit the integral so it doesn't get out of control
		if ((result >= 65535.0 && new_integral < (*integral)) || (result < 0.0 && new_integral > (*integral)) || (result <= 65535.0 && result >= 0))
		{
			(*integral) = new_integral;
		}
		
		// limit the range and return the rounded result for use as the PWM OCR value
		return (uint16_t)lround(result > 65535.0 ? 65535.0 : (result < 0.0 ? 0.0 : result));
	}
}

volatile uint16_t tmr_ovf_cnt = 0;
volatile char tmr_checktemp_flag = 0;
volatile char tmr_drawlcd_flag = 0;
volatile char tmr_writelog_flag = 0;

ISR(TIMER0_OVF_vect)
{
	heat_isr();
	
	tmr_ovf_cnt++;
	if (tmr_ovf_cnt % 1024 == 0) // about 2s
	{
		tmr_ovf_cnt = 0;
		tmr_checktemp_flag = 1;
		tmr_drawlcd_flag = 1;
		tmr_writelog_flag = 1;
	}
	else if (tmr_ovf_cnt % 512 == 0) // about 1s
	{
		tmr_checktemp_flag = 1;
		tmr_writelog_flag = 1;
	}
	else if (tmr_ovf_cnt % 256 == 0) // about 0.5s
	{
		tmr_checktemp_flag = 1;
	}
}

void tmr_init()
{
	TCCR0B = _BV(CS01) | _BV(CS00); // start timer0 (a 8 bit timer) with 64 prescaler	
	TIMSK0 |= _BV(TOV0); // enable interrupts
}

// store the temperature history for graphic purposes
uint8_t temp_history[LCD_WIDTH];
uint16_t temp_history_idx;
uint8_t temp_plan[LCD_WIDTH]; // also store the target temperature for comparison purposes

char graph_text[((LCD_WIDTH/FONT_WIDTH) + 2) * 5];

void draw_graph()
{
	for (int r = 0; r < LCD_ROWS; r++)
	{
		int text_end = 0;
		if (r < 4)
		{
			lcd_set_row_column(r, 0);
			text_end += FONT_WIDTH * fprintf(&lcd_stream, &(graph_text[r*((LCD_WIDTH/FONT_WIDTH) + 2)]));
		}
		
		lcd_set_row_column(r, text_end);
		
		for (int c = text_end; c < LCD_WIDTH; c++)
		{
			// flip the numbers because of y axis
			uint8_t history = LCD_HEIGHT - temp_history[c];
			uint8_t plan = LCD_HEIGHT - temp_plan[c];
			
			int rowtop = r * 8; // calculate actual y location based on row and 8 pixels per row
			
			uint8_t b[4] = { 0, 0, 0, 0, };
			
			// draw the graph by shading the area below the curve
			// actual temperature will be a black area
			for (int bi = 0; bi < 8; bi++)
			{
				if (history <= (rowtop + bi))
				{
					b[0] |= (1 << bi);
					b[1] |= (1 << bi);
					b[2] |= (1 << bi);
					b[3] |= (1 << bi);
				}
				
				/*
				
				// target temperature
				// commented out because it's mostly useless most of the time
				
				if (plan <= (rowtop + bi))
				{
					if (plan < history)
					{
						b[0] |= (1 << bi);
						b[1] |= (1 << bi);
						b[2] |= (1 << bi);
						b[3] |= (1 << bi);
					}
					else if (plan > history && plan == (rowtop + bi))
					{
						b[0] &= ~(1 << bi);
						b[1] &= ~(1 << bi);
						b[2] &= ~(1 << bi);
						b[3] &= ~(1 << bi);
					}
				}
				
				//*/
			}
			
			lcd_draw_unit(b[0], b[1], b[2], b[3]);
		}
	}
}

// this function runs an entire reflow soldering profile
// it works like a state machine
void auto_go(profile_t* profile)
{
	sensor_filter_reset();
	
	settings_load(&settings); // load from eeprom
	
	// validate the profile before continuing
	if (!profile_valid(profile))
	{
		lcd_set_row_column(0, 0);
		fprintf_P(&lcd_stream, PSTR("Error in Profile\n"));
		_delay_ms(1000);
		return;
	}
	
	fprintf_P(&log_stream, PSTR("auto mode session start,\n"));
	
	// this will be used for many things later
	double max_heat_rate = settings.max_temp / settings.time_to_max;
	
	// reset the graph
	for (int i = 0; i < LCD_WIDTH; i++)
	{
		temp_history[i] = 0;
		temp_plan[i] = 0;
	}
	temp_history_idx = 0;
	
	// total duration is calculated so we know how big the graph needs to span
	// note, this calculation is only an worst case estimate
	// it is also aware of whether or not the heating rate can be achieved
	double total_duration = (double)(profile->soak_length + profile->time_to_peak) +
							((profile->soak_temp1 - ROOM_TEMP) / min(profile->start_rate, max_heat_rate)) + 
							((profile->peak_temp - ROOM_TEMP) / profile->cool_rate) +
							10.0; // some extra just in case
	double graph_tick = total_duration / LCD_WIDTH;
	double graph_timer = 0.0;

	// some more variable initialization
	double integral = 0.0, last_error = 0.0;
	char stage = 0; // the state machine state
	uint32_t total_cnt = 0; // counter for the entire process
	uint16_t length_cnt = 0; // counter for a particular stage
	char update_graph = 0; // if there is new stuff to draw
	uint16_t pwm_ocr = 0; // temporary holder for PWM duty cycle
	double tgt_temp = sensor_to_temperature(sensor_read());
	double start_temp = tgt_temp;
	uint16_t cur_sensor = sensor_read();
	while (1)
	{	
		if (tmr_checktemp_flag)
		{
			tmr_checktemp_flag = 0;
			
			total_cnt++;
			
			cur_sensor = sensor_read();
			
			if (DEMO_MODE)
			{
				// in demo mode, we fake the reading
				cur_sensor = temperature_to_sensor(tgt_temp);
			}
			
			if (stage == 0) // preheat to thermal soak temperature
			{
				length_cnt++;
				if (sensor_to_temperature(cur_sensor) >= profile->soak_temp1)
				{
					// reached soak temperature
					stage++;
					integral = 0.0;
					last_error = 0.0;
					length_cnt = 0;
				}
				else
				{
					// calculate next temperature by increasing current temperature
					tgt_temp = max(ROOM_TEMP, start_temp) + (profile->start_rate * TMR_OVF_TIMESPAN * 256 * length_cnt);
					
					if (length_cnt % 8 == 0)
					{
						start_temp = sensor_to_temperature(cur_sensor);
						length_cnt = 0;
					}
					
					tgt_temp = min(tgt_temp, profile->soak_temp1);
					
					// calculate the maximum allowable PWM duty cycle because we already know the maximum heating rate
					//uint32_t upperlimit = lround((1.125 * 65535.0 * profile->start_rate) / max_heat_rate);
					//upperlimit = max(upperlimit, approx_pwm(temperature_to_sensor(tgt_temp)));
					
					// calculate and set duty cycle
					uint16_t pwm = pid((double)temperature_to_sensor(tgt_temp), (double)cur_sensor, &integral, &last_error);
					pwm_ocr = pwm;
					//pwm_ocr = pwm > upperlimit ? upperlimit : pwm;
				}
			}
			
			if (stage == 1) // thermal soak stage, ensures entire PCB is evenly heated
			{
				length_cnt++;
				if (((uint16_t)lround(length_cnt * TMR_OVF_TIMESPAN * 256) > profile->soak_length))
				{
					// has passed time duration, next stage
					length_cnt = 0;
					stage++;
					integral = 0.0;
					last_error = 0.0;
				}
				else
				{
					// keep the temperature steady
					tgt_temp = (((profile->soak_temp2 - profile->soak_temp1) / profile->soak_length) * (length_cnt * TMR_OVF_TIMESPAN * 256)) + profile->soak_temp1;
					tgt_temp = min(tgt_temp, profile->soak_temp2);
					pwm_ocr = pid((double)temperature_to_sensor(tgt_temp), (double)cur_sensor, &integral, &last_error);
				}
			}
			
			if (stage == 2) // reflow stage, try to reach peak temp
			{
				length_cnt++;
				if (((uint16_t)lround(length_cnt * TMR_OVF_TIMESPAN * 256) > profile->time_to_peak))
				{
					// has passed time duration, next stage
					length_cnt = 0;
					stage++;
					integral = 0.0;
					last_error = 0.0;
				}
				else
				{
					// raise the temperature
					tgt_temp = (((profile->peak_temp - profile->soak_temp2) / profile->time_to_peak) * (length_cnt * TMR_OVF_TIMESPAN * 256)) + profile->soak_temp2;
					tgt_temp = min(tgt_temp, profile->peak_temp);
					pwm_ocr = pid((double)temperature_to_sensor(tgt_temp), (double)cur_sensor, &integral, &last_error);
				}
			}
			
			if (stage == 3) // make sure we've reached peak temperature
			{
				if (sensor_to_temperature(cur_sensor) >= profile->peak_temp)
				{
					stage++;
					integral = 0.0;
					last_error = 0.0;
					length_cnt = 0;
				}
				else
				{
					tgt_temp = profile->peak_temp + 5.0;
					pwm_ocr = pid((double)temperature_to_sensor(tgt_temp), (double)cur_sensor, &integral, &last_error);
				}
			}
			
			if (stage == 4) // cool down
			{
				length_cnt++;
				if (cur_sensor < temperature_to_sensor(ROOM_TEMP * 1.25))
				{
					pwm_ocr = 0; // turn off
					tgt_temp = ROOM_TEMP;
					stage++;
				}
				else
				{
					// change the target temperature
					tgt_temp = profile->peak_temp - (profile->cool_rate * TMR_OVF_TIMESPAN * 256 * length_cnt);
					uint16_t pwm = pid((double)temperature_to_sensor(tgt_temp), (double)cur_sensor, &integral, &last_error);
					
					// apply a upper limit to the duty cycle to avoid accidentally heating instead of cooling
					//uint16_t ap = approx_pwm(temperature_to_sensor(tgt_temp));
					//pwm_ocr = pwm > ap ? ap : pwm;
					pwm_ocr = pwm;
				}
			}
			
			heat_set(pwm_ocr); // set the heating element power
			
			graph_timer += TMR_OVF_TIMESPAN * 256;
			
			if (stage != 5 && graph_timer >= graph_tick)
			{
				graph_timer -= graph_tick;
				// it's time for a new entry on the graph
				
				if (temp_history_idx == (LCD_WIDTH - 1))
				{
					// the graph is longer than expected
					// so shift the graph
					for (int i = 0; i < LCD_WIDTH - 1; i++)
					{
						temp_plan[i] = temp_plan[i+1];
						temp_history[i] = temp_history[i+1];
					}
				}
				
				// shift the graph down a bit to get more room
				int32_t shiftdown = lround((ROOM_TEMP * 1.25 / settings.max_temp) * LCD_HEIGHT);
				
				// calculate the graph plot entries
				
				int32_t plan = lround((tgt_temp / settings.max_temp) * LCD_HEIGHT) - shiftdown;
				temp_plan[temp_history_idx] = plan >= LCD_HEIGHT ? LCD_HEIGHT : (plan <= 0 ? 0 : plan);
				
				int32_t history = lround((sensor_to_temperature(cur_sensor) / settings.max_temp) * LCD_HEIGHT) - shiftdown;
				temp_history[temp_history_idx] = history >= LCD_HEIGHT ? LCD_HEIGHT : (history <= 0 ? 0 : history);
				
				if (temp_history_idx < (LCD_WIDTH - 1) && (temp_plan[temp_history_idx] != 0 || temp_plan[temp_history_idx] != 0))
				{
					temp_history_idx++;
				}
				
				update_graph = 1;
			}
		}
		
		if (tmr_drawlcd_flag)
		{
			tmr_drawlcd_flag = 0;
			
			// print some data to top left corner of LCD
			sprintf_P(&(graph_text[0]), PSTR("cur:%d`C "), (uint16_t)lround(sensor_to_temperature(cur_sensor)));
			sprintf_P(&(graph_text[1*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("tgt:%d`C "), (uint16_t)lround(tgt_temp));
			
			// tell the user about the current stage
			switch (stage)
			{
				case 0:
					sprintf_P(&(graph_text[2*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Preheat"));
					break;
				case 1:
					sprintf_P(&(graph_text[2*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Soak   "));
					break;
				case 2:
				case 3:
					sprintf_P(&(graph_text[2*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Reflow"));
					break;
				case 4:
					sprintf_P(&(graph_text[2*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Cool  "));
					break;
				default:
					sprintf_P(&(graph_text[2*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Done  "));
					break;
			}
			
			// indicate whether or not this is running in demo mode
			if (DEMO_MODE)
			{
				sprintf_P(&(graph_text[3*((LCD_WIDTH/FONT_WIDTH) + 2)]), PSTR("Demo  "));
			}
			else
			{
				graph_text[3*((LCD_WIDTH/FONT_WIDTH) + 2)] = 0;
			}
			
			if (update_graph)
			{
				update_graph = 0;
				draw_graph();
			}
			else
			{
				for (int r = 0; r < 4; r++)
				{
					lcd_set_row_column(r, 0);
					fprintf(&lcd_stream, &(graph_text[r*((LCD_WIDTH/FONT_WIDTH) + 2)]));
				}
				lcd_draw_end();
			}
		}
		
		if (tmr_writelog_flag)
		{
			tmr_writelog_flag = 0;
			
			// print to CSV log format
			fprintf_P(&log_stream, PSTR("%d, "), stage);
			fprintf_P(&log_stream, PSTR("%s, "), str_from_double(total_cnt * TMR_OVF_TIMESPAN * 256, 1));
			fprintf_P(&log_stream, PSTR("%d, "), cur_sensor);
			fprintf_P(&log_stream, PSTR("%d, "), temperature_to_sensor(tgt_temp));
			
			fprintf_P(&log_stream, PSTR("%s,\n"), str_from_int(pwm_ocr));
			
			//fprintf_P(&log_stream, PSTR("%s, "), str_from_int(pwm_ocr));
			//fprintf_P(&log_stream, PSTR("%s,\n"), str_from_double(integral, 1));
		}
		
		// hold down mid button to stop
		if (button_mid())
		{
			if (stage != 5)
			{
				lcd_set_row_column(2, 0);
				fprintf_P(&lcd_stream, PSTR("Done    "));
				lcd_draw_end();
			}
			
			button_debounce();
			while (button_mid());
			button_debounce();
			
			if (stage != 5)
			{
				stage = 5;
			}
			else
			{
				// release and hold down again to exit
				return;
			}
		}
	}
}

void profile_setdefault(profile_t* profile)
{
	profile->start_rate = 1;
	profile->soak_temp1 = 150.0;
	profile->soak_temp2 = 185.0;
	profile->soak_length = 70;
	profile->peak_temp = 217.5;
	profile->time_to_peak = 45;
	profile->cool_rate = 2.0;
}

void settings_setdefault(settings_t* s)
{
	s->pid_p = 6000.0;
	s->pid_i = 20.00;
	s->pid_d = -0.00;
	s->max_temp = 225.0;
	s->time_to_max = 300.0;
}

char profile_valid(profile_t* profile)
{
	return
	(profile->start_rate > 0.0 &&
	profile->soak_temp1 > 0.0 &&
	profile->soak_temp2 >= profile->soak_temp1 &&
	profile->peak_temp >= profile->soak_temp2 &&
	profile->cool_rate > 0.0);
}

char settings_valid(settings_t* s)
{
	return
	(s->max_temp > 0.0 && s->time_to_max > 0.0);
}

static int log_putchar_stream(char c, FILE* stream)
{
	if (c == '\n')
	{
		log_putchar_stream('\r', stream);
	}
	
#ifdef USE_USB
	int r = usb_serial_putchar(c);
	if (c == '\n')
	{
		usb_serial_flush_output();
	}
	return r;
#else
	return 0;
#endif

}

FILE log_stream = FDEV_SETUP_STREAM(log_putchar_stream, NULL, _FDEV_SETUP_WRITE);