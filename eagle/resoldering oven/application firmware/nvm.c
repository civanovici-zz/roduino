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
 * This file handles
 * Non Volatile Memory stuff
 * functions to load/save from/to EEPROM
 * things like profiles and settings
 *
 */

#include "reflowtoasteroven.h"
#include "nvm.h"
#include <avr/eeprom.h>

void profile_load(profile_t* profile)
{
	// load the data block from EEPROM
	uint8_t* ptr = (uint8_t*)profile;
	uint8_t i = 0, checksum = 0;
	for (i = 0; i < sizeof(profile_t); i++)
	{
		uint8_t j = eeprom_read_byte(EEPROM_PROFILE_ADDR + i);
		ptr[i] = j;
		checksum ^= j; // track checksum to verify later
	}
	
	// verify data validity
	uint8_t k = eeprom_read_byte(EEPROM_PROFILE_ADDR + i);
	if (checksum != k || !profile_valid(profile))
	{
		// data not valid, reset to defaults
		profile_setdefault(profile);
		profile_save(profile);
	}
}

void profile_save(profile_t* profile)
{
	// save the data block into EEPROM
	uint8_t* ptr = (uint8_t*)profile;
	uint8_t i = 0, checksum = 0;
	for (i = 0; i < sizeof(profile_t); i++)
	{
		uint8_t j = ptr[i];
		eeprom_update_byte(EEPROM_PROFILE_ADDR + i, j);
		checksum ^= j; // track checksum to save
	}
	eeprom_update_byte(EEPROM_PROFILE_ADDR + i, checksum); // save the checksum for validation during load
}

void settings_load(settings_t* s)
{
	// load the data block from EEPROM
	uint8_t* ptr = (uint8_t*)s;
	uint8_t i = 0, checksum = 0;
	for (i = 0; i < sizeof(settings_t); i++)
	{
		uint8_t j = eeprom_read_byte(EEPROM_SETTINGS_ADDR + i);
		ptr[i] = j;
		checksum ^= j;
	}
	
	// verify data validity
	uint8_t k = eeprom_read_byte(EEPROM_SETTINGS_ADDR + i);
	if (checksum != k || !settings_valid(s))
	{
		// data not valid, reset to defaults
		settings_setdefault(s);
		settings_save(s);
	}
}

void settings_save(settings_t* s)
{
	// save the data block into EEPROM
	uint8_t* ptr = (uint8_t*)s;
	uint8_t i = 0, checksum = 0;
	for (i = 0; i < sizeof(settings_t); i++)
	{
		uint8_t j = ptr[i];
		eeprom_update_byte(EEPROM_SETTINGS_ADDR + i, j);
		checksum ^= j; // track checksum to save
	}
	eeprom_update_byte(EEPROM_SETTINGS_ADDR + i, checksum); // save the checksum for validation during load
}