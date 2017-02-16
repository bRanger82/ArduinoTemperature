#include <Adafruit_GFX.h>    // Core graphics library
#include <Adafruit_TFTLCD.h> // Hardware-specific library

#include <SFE_BMP180.h>
#include <Wire.h>
#include "DHT.h"
#include <MCUFRIEND_kbv.h>

#define LCD_CS A3 // Chip Select goes to Analog 3
#define LCD_CD A2 // Command/Data goes to Analog 2
#define LCD_WR A1 // LCD Write goes to Analog 1
#define LCD_RD A0 // LCD Read goes to Analog 0

#define LCD_RESET A4 // Can alternately just connect to Arduino's reset pin
#define LDR A10
#define DHTPIN 22     
#define DHTTYPE DHT22
#define PIN_LED 24 //Serial Data LED
#define SERIALWAIT 200

// Assign human-readable names to some common 16-bit color values:
#define BLACK   0x0000
#define BLUE    0x001F
#define RED     0xF800
#define GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF


MCUFRIEND_kbv tft;
DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;

unsigned long writeTextToTFT(double temp, double humanity, double heatindex, double airpressure, byte lightvalue) 
{
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


void setup(void) 
{
  Serial.begin(9600);
  Serial.flush();
  
  dht.begin();
  pressure.begin();

  tft.reset();

  uint16_t identifier = tft.readID();
  if(identifier == 0x9325) {
  } else if(identifier == 0x9328) {
  } else if(identifier == 0x4535) {
  } else if(identifier == 0x7575) {
  } else if(identifier == 0x9341) {
  } else if(identifier == 0x7783) {
  } else if(identifier == 0x8230) {
  } else if(identifier == 0x8357) {
  } else if(identifier==0x0101)
  {     
    identifier=0x9341;
  } else {
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

void writeSerialProtocolV1(float humanity, float temperature, float heatIndex)
{
  digitalWrite(PIN_LED, HIGH);
  Serial.print("START");
  Serial.print("|"); // delim
  Serial.print(humanity); // Luftfeuchte
  Serial.print("|"); // delim
  Serial.print(temperature); // Temperatur
  Serial.print("|"); // delim
  Serial.print(heatIndex); // HeatIndex
  Serial.print("|"); // delim
  Serial.println("EOF");
  Serial.flush();
  delay(SERIALWAIT);
  digitalWrite(PIN_LED, LOW);  
}

void writeSerialProtocolV2(float humanity, float temperature, float heatIndex, double airPressure)
{
  digitalWrite(PIN_LED, HIGH);
  Serial.print("START");
  Serial.print("|"); // delim
  Serial.print("LEN:4"); //4 Werte werden uebertragen
  Serial.print("|"); // delim
  Serial.print(humanity); // Luftfeuchte
  Serial.print("|"); // delim
  Serial.print(temperature); // Temperatur
  Serial.print("|"); // delim
  Serial.print(heatIndex); // HeatIndex
  Serial.print("|");
  Serial.print(airPressure); //Luftdruck millibar
  Serial.print("|"); // delim
  Serial.println("EOF");
  Serial.flush();
  delay(SERIALWAIT);
  digitalWrite(PIN_LED, LOW);
}

void writeSerialProtocolV3(float humanity, float temperature, float heatIndex, double airPressure, int lux)
{
  digitalWrite(PIN_LED, HIGH);
  Serial.print("START");
  Serial.print("|"); // delim
  Serial.print("LEN:5"); //5 Werte werden uebertragen
  Serial.print("|"); // delim
  Serial.print(humanity); // Luftfeuchte
  Serial.print("|"); // delim
  Serial.print(temperature); // Temperatur
  Serial.print("|"); // delim
  Serial.print(heatIndex); // HeatIndex
  Serial.print("|"); // delim
  Serial.print(airPressure); //Luftdruck millibar
  Serial.print("|"); // delim
  Serial.print(lux); // Lichtstaerke
  Serial.print("|"); // delim
  Serial.println("EOF");
  Serial.flush();
  delay(SERIALWAIT);
  digitalWrite(PIN_LED, LOW);
}

bool getPressure(double * value)
{
  double T,P;
  char st;

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
      *value = P;
      return true;
    }
  }
  return false;
}

void processData()
{
  float h = dht.readHumidity();     //Luftfeuchte auslesen
  float t = dht.readTemperature();  //Temperatur auslesen
  float r = dht.computeHeatIndex(t, h, false); //Heat-Index berechnen
  int sensorWert = analogRead(LDR); //Lichtwert auslesen --> TODO: LUX Modul bestellen und verwenden statt LDR
  double p0 = 0; //Wert fuer Luftdruck
  bool pressAvailable = getPressure(&p0); //Luftdruck lesen
  
  if (isnan(t) || isnan(h) || isnan(r) || !pressAvailable) //Fehler beim Lesen eines der Daten
  {
    showError();
    if (Serial)
    {
      Serial.println("Fehler: Daten konnten gelesen werden!");
      Serial.flush();
    }
  } 
  else
  {
    writeTextToTFT(t, h, r, p0, sensorWert);
    if (Serial)
    {
      writeSerialProtocolV2(h, t, r, p0);  
    }
  }
}

void loop(void) 
{
  processData();
  delay(10000);
}

