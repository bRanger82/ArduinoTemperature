#include <Adafruit_GFX.h>    // Core graphics library
#include <Adafruit_TFTLCD.h> // Hardware-specific library
#include <SFE_BMP180.h>
#include "TSL2561.h"
#include "DHT.h"

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
#define interruptPin 19

// Assign human-readable names to some common 16-bit color values:
#define BLACK   0x0000
#define BLUE    0x001F
#define RED     0xF800
#define GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF

//RGB LED
#define LEDgruen 45 // Farbe gruen an Pin 6
#define delayRGB 500 // p ist eine Pause mit 1000ms also 1 Sekunde
#define brightness1a 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define brightness1b 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define brightness1c 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define dunkel 0 // Zahlenwert 0 bedeutet Spannung 0V – also LED aus.

TSL2561 tsl(TSL2561_ADDR_FLOAT); 
Adafruit_TFTLCD tft(LCD_CS, LCD_CD, LCD_WR, LCD_RD, LCD_RESET);
DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;

float h = NAN;     //Luftfeuchte auslesen
float t = NAN;  //Temperatur auslesen
float r = NAN; //Heat-Index berechnen
double p0 = 0; //Wert fuer Luftdruck
uint32_t lux = 0;
  
void initSerial(void)
{
  Serial.begin(9600);
  Serial.flush();   
}

void initSensors(void)
{
  dht.begin();
  delay(50);
  pressure.begin();  
  delay(50);
  tsl.begin();
  tsl.setGain(TSL2561_GAIN_16X);
  tsl.setTiming(TSL2561_INTEGRATIONTIME_13MS);  
  delay(50);
}

void initTFT(void)
{
  tft.reset();
  delay(50);
  uint16_t identifier = tft.readID();
  Serial.print("Identifier: ");
  Serial.println(identifier);
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
  tft.reset();
  tft.begin(0x9325);
  
  delay(50);
  tft.setRotation(1);    
  tft.fillScreen(BLACK);
  delay(50);
  tft.setCursor(20, 20);
  tft.println("Hallo");
  delay(5000);
}

void initIOs(void)
{
  pinMode(LEDgruen, OUTPUT);
}


void setup(void) 
{
  initSerial();
  initSensors();
  initTFT();
  initIOs();
}

void loop(void) 
{
  processData();
}

void writeTextToTFT(double temp, double humanity, double heatindex, double airpressure, uint32_t lightvalue) 
{
  tft.fillScreen(BLACK);
  tft.setCursor(0, 0);
  tft.setTextSize(1);
  tft.println(" ");
  tft.setTextSize(3);
  tft.setTextColor(GREEN);
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
  tft.setTextSize(1);
  tft.println(" ");
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
  digitalWrite(LEDgruen, HIGH);
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
  digitalWrite(LEDgruen, LOW);  
}

void writeSerialProtocolV2(float humanity, float temperature, float heatIndex, double airPressure)
{
  digitalWrite(LEDgruen, HIGH);
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
  digitalWrite(LEDgruen, LOW);
}

void writeSerialProtocolV3(float humanity, float temperature, float heatIndex, double airPressure, int lux)
{
  digitalWrite(LEDgruen, HIGH);
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
  digitalWrite(LEDgruen, LOW);
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
  h = dht.readHumidity();     //Luftfeuchte auslesen
  t = dht.readTemperature();  //Temperatur auslesen
  r = dht.computeHeatIndex(t, h, false); //Heat-Index berechnen
  bool pressAvailable = getPressure(&p0); //Luftdruck lesen
  static bool errorOccured = false;
  uint16_t x = tsl.getLuminosity(TSL2561_VISIBLE);
  uint32_t lum = tsl.getFullLuminosity();
  uint16_t ir, full;
  ir = lum >> 16;
  full = lum & 0xFFFF;
  lux = tsl.calculateLux(full, ir);
   
  if (isnan(t) || isnan(h) || isnan(r) || !pressAvailable) //Fehler beim Lesen eines der Daten
  {
    showError();
    errorOccured = true;    
  } else
  {
    writeTextToTFT(t, h, r, p0, lux);
    writeSerialProtocolV3(h, t, r, p0, lux); 
  }
  delay(10000);
}




