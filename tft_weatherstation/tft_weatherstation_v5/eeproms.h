#ifndef  EEPROM_ROUTINES
#define  EEPROM_ROUTINES

#if defined(ARDUINO) && ARDUINO >= 100
#include "Arduino.h"
#else
#include "WProgram.h"
#endif
#include <Wire.h>

template <class T>
uint16_t writeObjectSimple(uint8_t i2cAddr, uint16_t addr, const T& value) {

	const uint8_t* p = (const uint8_t*)(const void*)&value;
	uint16_t i;
	for (i = 0; i < sizeof(value); i++) {
		Wire.beginTransmission(i2cAddr);
		Wire.write((uint16_t)(addr >> 8));  // MSB
		Wire.write((uint16_t)(addr & 0xFF));// LSB
		Wire.write(*p++);
		Wire.endTransmission();
		addr++;
		delay(5);  //max time for writing in 24LC256
	}
	return i;
}



template <class T>
uint16_t readObjectSimple(uint8_t i2cAddr, uint16_t addr, T& value) {

	uint8_t* p = (uint8_t*)(void*)&value;
	uint8_t objSize = sizeof(value);
	uint16_t i;
	for (i = 0; i < objSize; i++) {
		Wire.beginTransmission(i2cAddr);
		Wire.write((uint16_t)(addr >> 8));  // MSB
		Wire.write((uint16_t)(addr & 0xFF));// LSB
		Wire.endTransmission();
		Wire.requestFrom(i2cAddr, (uint8_t)1);
		if (Wire.available()) {
			*p++ = Wire.read();
		}
		addr++;
	}
	return i;
}

#endif