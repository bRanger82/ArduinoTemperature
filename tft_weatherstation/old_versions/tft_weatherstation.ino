// IMPORTANT: Adafruit_TFTLCD LIBRARY MUST BE SPECIFICALLY
// CONFIGURED FOR EITHER THE TFT SHIELD OR THE BREAKOUT BOARD.
// SEE RELEVANT COMMENTS IN Adafruit_TFTLCD.h FOR SETUP.
//Technical support:goodtft@163.com

#include <Adafruit_GFX.h>    // Core graphics library
#include <Adafruit_TFTLCD.h> // Hardware-specific library

#include <SFE_BMP180.h>
#include <Wire.h>
#include "DHT.h"

#define LCD_CS A3 // Chip Select goes to Analog 3
#define LCD_CD A2 // Command/Data goes to Analog 2
#define LCD_WR A1 // LCD Write goes to Analog 1
#define LCD_RD A0 // LCD Read goes to Analog 0

#define LCD_RESET A4 // Can alternately just connect to Arduino's reset pin
#define LDR A10
#define DHTPIN 22     
#define DHTTYPE DHT22
#define PIN_LED 24

// Assign human-readable names to some common 16-bit color values:
#define	BLACK   0x0000
#define	BLUE    0x001F
#define	RED     0xF800
#define	GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF

#include <MCUFRIEND_kbv.h>
MCUFRIEND_kbv tft;

DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;

unsigned long testText(double temp, double humanity, double heatindex, double airpressure, byte lightvalue) {
  tft.fillScreen(BLACK);
  unsigned long start = micros();
  tft.setCursor(0, 0);
  tft.setTextSize(1);
  tft.println(" ");
  tft.setTextSize(3);
  tft.setTextColor(YELLOW);
  tft.println("Sensor INNEN:");
  tft.println(" ");
  tft.setTextSize(2);
  tft.print("Temperatur: ");
  tft.print(temp);
  tft.println(" C");
  tft.println(" ");
  tft.print("Luftfeuchtigkeit: ");
  tft.print(humanity);
  tft.println(" %");
  tft.println(" ");
  tft.print("Heat-Index: ");
  tft.print(heatindex);
  tft.println(" C");
  tft.println(" ");
  tft.print("Luftdruck: ");
  tft.print(airpressure);
  tft.println(" mb");
  tft.println(" ");
  tft.print("Lichtwert: ");
  tft.print(lightvalue);
  tft.println(" Lux");
  tft.println(" ");
  return micros() - start;
}


void setup(void) {
  Serial.begin(9600);
  Serial.flush();
  
  dht.begin();
  pressure.begin();

  tft.reset();

  uint16_t identifier = tft.readID();
  if(identifier == 0x9325) {
  } else if(identifier == 0x9328) {
  } else if(identifier == 0x4535) {
  }else if(identifier == 0x7575) {
  } else if(identifier == 0x9341) {
  }else if(identifier == 0x7783) {
  }else if(identifier == 0x8230) {
  }else if(identifier == 0x8357) {
  }else if(identifier==0x0101)
  {     
    identifier=0x9341;
  }else {
    identifier=0x9341;
   
  }
  tft.begin(identifier);
  tft.setRotation(1);
  
  pinMode(PIN_LED, OUTPUT);
  digitalWrite(PIN_LED, LOW);
}

void showError(void)
{
  tft.fillScreen(BLACK);
  tft.setCursor(0, 0);
  tft.setTextSize(1);
  tft.println(" ");
  tft.setTextSize(3);
  tft.setTextColor(RED);
  tft.println("Sensor INNEN:");
  tft.println(" ");
  tft.println("Daten konnten");
  tft.println("nicht");
  tft.println("gelesen werden!");
}

void loop(void) {
  float h = dht.readHumidity();     //Luftfeuchte auslesen
  float t = dht.readTemperature();  //Temperatur auslesen
  float r = dht.computeHeatIndex(t, h, false);
  int sensorWert = analogRead(LDR);
  char st;
  double T,P,p0;
  bool pressAvailable = false;
  
  p0 = 0;
  
  st = pressure.startTemperature();
  if (st != 0)
  {
    delay(st);
    st = pressure.getTemperature(T);
  }
  
  st = pressure.startPressure(3);

  if (st != 0)
  {
    delay(st);
    
    st = pressure.getPressure(P,T);
    
    if (st != 0)
    {
      pressAvailable = true;
      p0 = P;
    }
  }
  
  if (Serial)
  {
    if (isnan(t) || isnan(h) || isnan(r) || !pressAvailable) 
    {
      Serial.println("Fehler: Daten konnten gelesen werden!");
      Serial.flush();
      showError();
    } 
    else
    {
      digitalWrite(PIN_LED, HIGH);
      Serial.print("START");
      Serial.print("|"); // delim
      Serial.print("LEN:4"); //4 Werte werden uebertragen
      Serial.print("|"); // delim
      Serial.print(h); // Luftfeuchte
      Serial.print("|"); // delim
      Serial.print(t); // Temperatur
      Serial.print("|"); // delim
      Serial.print(r); // HeatIndex
      Serial.print("|");
      Serial.print(p0); //Luftdruck millibar
      Serial.print("|"); // delim
      Serial.println("EOF");
      delay(250);
      digitalWrite(PIN_LED, LOW);
      testText(t, h, r, p0, sensorWert);
      Serial.flush();
    }
  }
  
  delay(10000);
}
