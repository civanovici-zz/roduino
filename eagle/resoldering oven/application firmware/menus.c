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
 * This file handles the behaviours of the main menu and all submenues
 *
 */

#include "reflowtoasteroven.h"
#include "lcd.h"
#include "heatingelement.h"
#include "temperaturemeasurement.h"
#include "buttons.h"
#include <string.h>
#include <stdlib.h>
#include <avr/pgmspace.h>

// this string is allocated for temporary use
char strbuf[(LCD_WIDTH/FONT_WIDTH) + 2];

// this counts the number of loop iterations that a button has been held for
uint8_t button_held;

// changes a value based on the up and down buttons
double button_change_double(double oldvalue, double increment, double limit1, double limit2)
{
	double maxlimit = limit1 >= limit2 ? limit1 : limit2;
	double minlimit = limit1 < limit2 ? limit1 : limit2;
	
	if (button_up())
	{
		button_debounce();

		// the amount of change occurs increases as the button is held down for longer
		button_held = (button_held < 200) ? (button_held + 1) : button_held;
		increment += increment*(button_held/3);

		// return the changed value
		return ((oldvalue + increment) > maxlimit) ? maxlimit : (oldvalue + increment);
		// the range of this value is also limited
	}
	else if (button_down())
	{
		button_debounce();

		// the amount of change occurs increases as the button is held down for longer
		button_held = (button_held < 200) ? (button_held + 1) : button_held;
		increment += increment*(button_held/3);

		// return the changed value
		return ((oldvalue - increment) < minlimit) ? minlimit : (oldvalue - increment);
		// the range of this value is also limited
	}
	else
	{
		// no button pressed, meaning button is not held down
		button_held = 0;
		// and the value does not change, but keep it in range
		return (oldvalue > maxlimit) ? maxlimit : ((oldvalue < minlimit) ? minlimit : oldvalue);
	}
}

// same as above but for integers
int32_t button_change_int(int32_t oldvalue, int32_t increment, int32_t limit1, int32_t limit2)
{
	int32_t maxlimit = limit1 >= limit2 ? limit1 : limit2;
	int32_t minlimit = limit1 < limit2 ? limit1 : limit2;
	
	if (button_up())
	{
		button_debounce();

		// the amount of change occurs increases as the button is held down for longer
		button_held = (button_held < 100) ? (button_held + 1) : button_held;
		increment += increment*(button_held/3);

		// return the changed value
		return ((oldvalue + increment) > maxlimit) ? maxlimit : (oldvalue + increment);
		// the range of this value is also limited
	}
	else if (button_down())
	{
		button_debounce();

		// the amount of change occurs increases as the button is held down for longer
		button_held = (button_held < 100) ? (button_held + 1) : button_held;
		increment += increment*(button_held/3);

		// return the changed value
		return ((oldvalue - increment) < minlimit) ? minlimit : (oldvalue - increment);
		// the range of this value is also limited
	}
	else
	{
		// no button pressed, meaning button is not held down
		button_held = 0;
		// and the value does not change, but keep it in range
		return (oldvalue > maxlimit) ? maxlimit : ((oldvalue < minlimit) ? minlimit : oldvalue);
	}
}

char* str_from_int(signed long value)
{
	return ltoa(value, strbuf, 10);
}

char* str_from_double(double value, int decimalplaces)
{
	char* result = dtostrf(value, -(LCD_WIDTH/FONT_WIDTH), decimalplaces, strbuf);
	
	// trim trailing spaces
	int8_t len = strlen(result);
	len--;
	while (result[len] == ' ' && len >= 0)
	{
		result[len] = 0;
		len--;
	}
	return result;
}

void menu_manual_pwm_ctrl()
{
	sensor_filter_reset();
	
	fprintf_P(&log_stream, PSTR("manual PWM control mode,\n"));
	uint16_t iteration = 0;

	uint16_t cur_pwm = 0;
	uint16_t cur_sensor = sensor_read();
	heat_set(cur_pwm);
	while(1)
	{
		heat_set(cur_pwm);
		cur_sensor = sensor_read();
		
		// draw the LCD
		for (int r = 0; r < LCD_ROWS; r++)
		{
			lcd_set_row_column(r, 0);

			switch (r)
			{
				case 0:
					// screen title
					fprintf_P(&lcd_stream, PSTR("Manual PWM Control\n"));
					break;
				case 1:
					// this creates a horizontal divider line
					for (int c = 0; c < LCD_WIDTH; c++)
					{
						lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
					}
					break;
					
				// print info/submenu items
				case 2:
					fprintf_P(&lcd_stream, PSTR("PWM= %s / 65535\n"), str_from_int(cur_pwm));
					break;
				case 3:
					fprintf_P(&lcd_stream, PSTR("Sensor: %s / 1023\n"), str_from_int(cur_sensor));
					break;
				case 4:
					fprintf_P(&lcd_stream, PSTR("Temp: %s `C\n"), str_from_int(lround(cur_sensor * THERMOCOUPLE_CONSTANT)));
					break;
				default:
					lcd_clear_restofrow();
			}
		}

		// change this value according to which button is pressed
		cur_pwm = button_change_int(cur_pwm, 1024, 0, 65535);

		if (button_mid())
		{
			button_held = 0;
			button_debounce();
			while (button_mid());
			button_debounce();

			// exit this mode, back to home menu
			return;
		}

		if (tmr_writelog_flag)
		{
			tmr_writelog_flag = 0;

			iteration++; // used so CSV entries can be sorted by time

			// print log in CSV format
			fprintf_P(&log_stream, PSTR("%s, "), str_from_double(iteration * TMR_OVF_TIMESPAN * 512, 1));
			fprintf_P(&log_stream, PSTR("%s, "), str_from_int(cur_sensor));
			fprintf_P(&log_stream, PSTR("%s,\n"), str_from_int(cur_pwm));
		}
	}
}

void menu_manual_temp_ctrl()
{
	sensor_filter_reset();
	
	settings_load(&settings); // load from eeprom
	
	// signal start of mode in log
	fprintf_P(&log_stream, PSTR("manual temperature control mode,\n"));

	uint16_t iteration = 0;
	uint16_t tgt_temp = 0;
	uint16_t cur_pwm = 0;
	double integral = 0.0, last_error = 0.0;
	uint16_t tgt_sensor = temperature_to_sensor((double)tgt_temp);
	uint16_t cur_sensor = sensor_read();
	
	while(1)
	{
		if (tmr_drawlcd_flag || (tmr_checktemp_flag == 0 && tmr_writelog_flag == 0))
		{
			tmr_drawlcd_flag = 0;
			// draw the LCD
			for (int r = 0; r < LCD_ROWS; r++)
			{
				lcd_set_row_column(r, 0);

				switch (r)
				{
					case 0:
						// screen title
						fprintf_P(&lcd_stream, PSTR("Manual Temp Control\n"));
						break;
					case 1:
						// this creates a horizontal divider line
						for (int c = 0; c < LCD_WIDTH; c++)
						{
							lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
						}
						break;
						
					// print info/submenu items
					case 2:
						fprintf_P(&lcd_stream, PSTR("Target= %s `C\n"), str_from_int(tgt_temp));
						break;
					case 3:
						fprintf_P(&lcd_stream, PSTR("Current: %s `C\n"), str_from_int(lround(sensor_to_temperature(cur_sensor))));
						break;
					case 4:
						fprintf_P(&lcd_stream, PSTR("Sensor: %s / 1023\n"), str_from_int(cur_sensor));
						break;
					case 5:
						fprintf_P(&lcd_stream, PSTR("PWM: %s / 65535\n"), str_from_int(cur_pwm));
						break;
					default:
						lcd_clear_restofrow();
				}
			}
			
			// change this value according to which button is pressed
			tgt_temp = button_change_int(tgt_temp, 10, 0, 350);
		}

		// reset these so the PID controller starts fresh when settings change
		if (button_up() || button_down())
		{
			integral = 0;
			last_error = 0;
		}

		if (button_mid())
		{
			button_held = 0;

			button_debounce();
			while (button_mid());
			button_debounce();

			// exit this mode, back to home menu
			return;
		}

		if (tmr_checktemp_flag)
		{
			tmr_checktemp_flag = 0;
			
			tgt_sensor = temperature_to_sensor((double)tgt_temp);
			cur_sensor = sensor_read();
			cur_pwm = pid((double)temperature_to_sensor((double)tgt_temp), (double)cur_sensor, &integral, &last_error);
		}

		if (tmr_writelog_flag)
		{
			tmr_writelog_flag = 0;
			
			iteration++; // used so CSV entries can be sorted by time

			// print log in CSV format
			fprintf_P(&log_stream, PSTR("%s, "), str_from_double(iteration * TMR_OVF_TIMESPAN * 512, 1));
			fprintf_P(&log_stream, PSTR("%s, "), str_from_int(cur_sensor));
			fprintf_P(&log_stream, PSTR("%s, "), str_from_int(tgt_temp));
			fprintf_P(&log_stream, PSTR("%s,\n"), str_from_int(cur_pwm));
		}
	}
}

void menu_edit_profile(profile_t* profile)
{
	char selection = 0;

	while(1)
	{
		heat_set(0); // keep off for safety

		// draw on LCD
		for (int r = 0; r < LCD_ROWS; r++)
		{
			lcd_set_row_column(r, 0);

			// draw a indicator beside the selected menu item
			if ((r - 2) == selection)
			{
				lcd_draw_char('>');
			}
			else
			{
				if (r != 1)
				{
					lcd_draw_char(' ');
				}
			}

			switch (r)
			{
				case 0:
					// menu title
					fprintf_P(&lcd_stream, PSTR("Edit Profile\n"));
					break;
				case 1:
					// this draws a horizontal divider line across the screen
					for (int c = 0; c < LCD_WIDTH; c++)
					{
						lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
					}
					break;
				
				// display info/submenu items
				case 2:
					fprintf_P(&lcd_stream, PSTR("Start Rate= %s `C/s\n"), str_from_double(profile->start_rate, 1));
					break;
				case 3:
					fprintf_P(&lcd_stream, PSTR("Soak Temp 1= %d `C\n"), (uint16_t)lround(profile->soak_temp1));
					break;
				case 4:
					fprintf_P(&lcd_stream, PSTR("Soak Temp 2= %d `C\n"), (uint16_t)lround(profile->soak_temp2));
					break;
				case 5:
					fprintf_P(&lcd_stream, PSTR("Soak Length= %d s\n"), profile->soak_length);
					break;
				case 6:
					fprintf_P(&lcd_stream, PSTR("Peak Temp= %d `C\n"), (uint16_t)lround(profile->peak_temp));
					break;
				case 7:			
					fprintf_P(&lcd_stream, PSTR("Time to Peak= %d s\n"), profile->time_to_peak);
					break;
				case 8:
					fprintf_P(&lcd_stream, PSTR("Cool Rate= %s `C/s\n"), str_from_double(profile->cool_rate, 1));
					break;
				case 9:
					fprintf_P(&lcd_stream, PSTR("Return to Auto Mode\n"));
					break;
				default:
					lcd_clear_restofrow();
					break;
			}
		}
	
		// change values according to the selected item and buttons being pressed
		switch (selection)
		{
			case 0:
				profile->start_rate = button_change_double(profile->start_rate, 0.1, 0.1, 5.0);
				break;
			case 1:
				profile->soak_temp1 = button_change_double(profile->soak_temp1, 1, 50, 300);
				break;
			case 2:
				profile->soak_temp2 = button_change_double(profile->soak_temp2, 1, 50, 300);
				break;
			case 3:
				profile->soak_length = button_change_int(profile->soak_length, 0.1, 60, 60*5);
				break;
			case 4:
				profile->peak_temp = button_change_double(profile->peak_temp, 1, 150, 350);
				break;
			case 5:
				profile->time_to_peak = button_change_int(profile->time_to_peak, 1, 0, 60*5);
				break;
			case 6:
				profile->cool_rate = button_change_double(profile->cool_rate, 0.1, 0.1, 5.0);
				break;
			default:
				// there's no need for button holding if it's in a non-value-changing menu item
				button_held = 0;
				break;
		}
		
		if (selection == 7) // the "return" option
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();
				
				if (profile_valid(profile))
				{
					return;
				}
				else
				{
					lcd_clear_screen();
					lcd_set_row_column(0, 0);
					fprintf_P(&lcd_stream, PSTR("Error in Profile\n"));
					_delay_ms(1000);
				}
			}
		}
		
		if (button_mid())
		{
			button_held = 0;
			
			button_debounce();
			while (button_mid());
			button_debounce();

			// change selected menu item to the next one
			selection = (selection + 1) % 8;
		}
	}
}

void menu_auto_mode()
{
	static profile_t profile;
	profile_load(&profile); // load from eeprom

	char selection = 0;

	while(1)
	{
		heat_set(0); // keep off for safety

		// draw on LCD
		for (int r = 0; r < LCD_ROWS; r++)
		{
			lcd_set_row_column(r, 0);

			// draw a indicator beside the selected menu item
			if ((r - 2) == selection)
			{
				lcd_draw_char('>');
			}
			else
			{
				if (r != 1)
				{
					lcd_draw_char(' ');
				}
			}

			switch (r)
			{
				case 0:
					// menu title
					fprintf_P(&lcd_stream, PSTR("Auto Mode\n"));
					break;
				case 1:
					// this draws a horizontal divider line across the screen
					for (int c = 0; c < LCD_WIDTH; c++)
					{
						lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
					}
					break;
				
				// display info/submenu items
				case 2:
					fprintf_P(&lcd_stream, PSTR("Edit Profile\n"));
					break;
				case 3:
					fprintf_P(&lcd_stream, PSTR("Reset to Defaults\n"));
					break;
				case 4:
					fprintf_P(&lcd_stream, PSTR("Start\n"));
					break;
				case 5:
					fprintf_P(&lcd_stream, PSTR("Back to Home Menu\n"));
					break;
				default:
					lcd_clear_restofrow();
			}
		}

		if (selection == 0) // the "edit profile" menu item
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				menu_edit_profile(&profile);
				profile_save(&profile); // save to eeprom
			}
		}
		else if (selection == 1) // the "reset to default" menu item
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				profile_setdefault(&profile);
				profile_save(&profile); // save to eeprom
				
				lcd_clear_screen();
				lcd_set_row_column(0, 0);
				fprintf_P(&lcd_stream, PSTR("Reset to Defaults... Done\n"));
				_delay_ms(1000);
			}
		}
		else if (selection == 2) // the "start" menu item
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				lcd_clear_screen();
				
				auto_go(&profile);

				// go back to home menu when finished
				return;
			}
		}
		else if (selection == 3) // the "back to home menu" option
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				// go back to home menu
				return;
			}
		}

		if (button_mid())
		{
			button_debounce();
			while (button_mid());
			button_debounce();

			// change selected menu item to the next one
			selection = (selection + 1) % 4;
		}
	}
}

void menu_edit_settings()
{
	settings_load(&settings); // load from eeprom
	
	char selection = 0;
	while(1)
	{
		heat_set(0); // keep off for safety

		// draw LCD
		for (int r = 0; r < LCD_ROWS; r++)
		{
			lcd_set_row_column(r, 0);

			// draw indicator beside the selected menu item
			if (r - 2 == selection)
			{
				lcd_draw_char('>');
			}
			else
			{
				if (r != 1)
				{
					lcd_draw_char(' ');
				}
			}

			switch (r)
			{
				case 0:
					// menu title
					fprintf_P(&lcd_stream, PSTR("Edit Settings\n"));
					break;
				case 1:
					// draw a horizontal divider line across the screen
					for (int c = 0; c < LCD_WIDTH; c++)
					{
						lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
					}
					break;
					
				// display info/submenu items
				case 2:
					fprintf_P(&lcd_stream, PSTR("PID P= %s\n"), str_from_double(settings.pid_p, 2));
					break;
				case 3:
					fprintf_P(&lcd_stream, PSTR("PID I= %s\n"), str_from_double(settings.pid_i, 2));
					break;
				case 4:
					fprintf_P(&lcd_stream, PSTR("PID D= %s\n"), str_from_double(settings.pid_d, 2));
					break;
				case 5:
					fprintf_P(&lcd_stream, PSTR("Max Temp= %d `C\n"), (uint16_t)lround(settings.max_temp));
					break;
				case 6:
					fprintf_P(&lcd_stream, PSTR("Time to Max= %d s\n"), (uint16_t)lround(settings.time_to_max));
					break;
				case 7:
					fprintf_P(&lcd_stream, PSTR("Reset to Defaults\n"));
					break;
				case 8:
					fprintf_P(&lcd_stream, PSTR("Back to Home Menu\n"));
				default:
					lcd_clear_restofrow();
			}
		}

		// change value according to which value is selected and which button is pressed
		switch(selection)
		{
			case 0:
				settings.pid_p = button_change_double(settings.pid_p, 0.1, 0.0, 10000.0);
				break;
			case 1:
				settings.pid_i = button_change_double(settings.pid_i, 0.01, 0.0, 10000.0);
				break;
			case 2:
				settings.pid_d = button_change_double(settings.pid_d, 0.01, -10000.0, 10000.0);
				break;
			case 3:
				settings.max_temp = button_change_double(settings.max_temp, 1.0, 200.0, 350.0);
				break;
			case 4:
				settings.time_to_max = button_change_double(settings.time_to_max, 1.0, 0.0, 60*20);
				break;
			default:
				// there's no need for button holding if it's in a non-value-changing menu item
				button_held = 0;
				break;
		}

		if (selection == 5) // the "reset to default" menu item
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				settings_setdefault(&settings);
			}
		}
		else if (selection == 6) // the "back to home menu" option
		{
			if (button_up())
			{
				button_debounce();
				while (button_up());
				button_debounce();

				if (settings_valid(&settings))
				{
					settings_save(&settings); // save to eeprom

					// back to main menu
					return;
				}
				else
				{
					lcd_clear_screen();
					lcd_set_row_column(0, 0);
					fprintf_P(&lcd_stream, PSTR("Error in Settings\n"));
					_delay_ms(1000);
				}
			}
		}

		if (button_mid())
		{
			button_held = 0;
			
			button_debounce();
			while (button_mid());
			button_debounce();

			// change selected menu item to the next one
			selection = (selection + 1) % 7;
		}
	}
}

void main_menu()
{
	char selection = 0;
	char screen_dirty = 1;

	while(1)
	{
		heat_set(0); // turn off for safety

		if (screen_dirty != 0) // only draw if required
		{
			for (int r = 0; r < LCD_ROWS; r++)
			{
				lcd_set_row_column(r, 0);

				// draw a indicator beside the selected menu item
				if (r - 2 == selection)
				{
					lcd_draw_char('>');
				}
				else
				{
					if (r != 1)
					{
						lcd_draw_char(' ');
					}
				}

				switch (r)
				{
					case 0:
						// menu title
						fprintf_P(&lcd_stream, PSTR("Home Menu\n"));
						break;
					case 1:
						// this draws a horizontal divider line across the screen
						for (int c = 0; c < LCD_WIDTH; c++)
						{
							lcd_draw_unit(0x3C, 0x5A, 0x3C, 0x5A);
						}
						break;
					
					// display submenu items
					case 2:
						fprintf_P(&lcd_stream, PSTR("Auto Mode\n"));
						break;
					case 3:
						fprintf_P(&lcd_stream, PSTR("Manual Temp Control\n"));
						break;
					case 4:
						fprintf_P(&lcd_stream, PSTR("Manual PWM Control\n"));
						break;
					case 5:
						fprintf_P(&lcd_stream, PSTR("Edit Settings\n"));
						break;
					default:
						lcd_clear_restofrow();
				}

				screen_dirty = 0; // the screen is fresh
			}
		}

		if (button_up())
		{
			button_debounce();
			while (button_up());
			button_debounce();

			// enter the submenu that is selected

			if (selection == 0)
			{
				menu_auto_mode();
			}
			else if (selection == 1)
			{
				menu_manual_temp_ctrl();
			}
			else if (selection == 2)
			{
				menu_manual_pwm_ctrl();
			}
			else if (selection == 3)
			{
				menu_edit_settings();
			}

			screen_dirty = 1;
		}
		else if (button_mid())
		{
			button_debounce();
			while (button_mid());
			button_debounce();

			selection = (selection + 1) % 4; // change selected menu item

			screen_dirty = 1;
		}
	}
}