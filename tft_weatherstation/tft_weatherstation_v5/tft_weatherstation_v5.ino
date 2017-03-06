#include <Adafruit_GFX.h>    // Core graphics library
#include <Adafruit_TFTLCD.h> // Hardware-specific library
#include "TSL2561.h"
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
#define LEDblau 44 // Farbe blau an Pin 3
#define LEDrot 46 // Farbe rot an Pin 5
#define LEDgruen 45 // Farbe gruen an Pin 6
#define delayRGB 500 // p ist eine Pause mit 1000ms also 1 Sekunde
#define brightness1a 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define brightness1b 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define brightness1c 150 // Zahlenwert zwischen 0 und 255 – gibt die Leuchtstärke der einzelnen Farbe an
#define dunkel 0 // Zahlenwert 0 bedeutet Spannung 0V – also LED aus.

//cycle will be increased in the loop(), if cycle == CYCLE_UPDATE_TFT --> update TFT and reset cycle to 0
//this will avoid updating the TFT each time when a serial data is sent (no need to update the TFT that often)
#define CYCLE_UPDATE_TFT 6 
int cycle = CYCLE_UPDATE_TFT;

#define DELAY_TIME 10000 //Delay Time between each processData() call
TSL2561 tsl(TSL2561_ADDR_FLOAT); 
MCUFRIEND_kbv tft;
DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;
volatile bool ledTestRun = false;
volatile bool UPDATE_TFT_DATA = false;
volatile bool dataAvailable = false;

float h = NAN;     //Luftfeuchte auslesen
float t = NAN;  //Temperatur auslesen
float r = NAN; //Heat-Index berechnen
double p0 = 0; //Wert fuer Luftdruck
uint32_t lux = 0;
  
/*
#define MAXARRAY 5
float tempArray [MAXARRAY] = {0};
int lenTempArray = 0;

float calculateTrendY(float y [], int len)
{
    float sumMultXY = 0;
    float sumX = 0;
    float sumY = 0;
    float resBOne = 0;
    float resBTwo = 0;

    for(size_t i=0; i<len; i++)
    {
       sumMultXY += i * y[i];
       sumX += i;
       sumY += y[i];
       resBOne += i * i;
    }

    resBOne *= len;
    resBTwo = sumX * sumX;
    float oben = (len * sumMultXY) - (sumX * sumY);
    float unten = resBOne - resBTwo;

    return (oben / unten);
}

void addValueToArray(float value)
{
  if (lenTempArray >= MAXARRAY)
  {
    for ( int i = 0; i<MAXARRAY-1; i++)
    {
        tempArray[i] = tempArray[i+1];
    }
    tempArray [MAXARRAY-1] = value;
  } else
  {
      tempArray [lenTempArray++] = value;
  }
}*/

void initSerial(void)
{
  Serial.begin(9600);
  Serial.flush();   
}

void initSensors(void)
{
  dht.begin();
  pressure.begin();  
  tsl.begin();
  tft.reset();
  tsl.setGain(TSL2561_GAIN_16X);
  tsl.setTiming(TSL2561_INTEGRATIONTIME_13MS);  
}

void initTFT(void)
{
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
}

void initIOs(void)
{
  pinMode(PIN_LED, OUTPUT);
  digitalWrite(PIN_LED, LOW);  
  pinMode(LEDblau, OUTPUT);
  pinMode(LEDgruen, OUTPUT);
  pinMode(LEDrot, OUTPUT);
  attachInterrupt(digitalPinToInterrupt(interruptPin), interruptCall, RISING);
}


void setup(void) 
{
  initSerial();
  initSensors();
  initTFT();
  initIOs();
  ledTest();
}

void loop(void) 
{
  bool sendSerialData = false;
  if (Serial.available() > 0)
  {
    String input = Serial.readString();
    if (input == "GetInformation")
      sendSerialData = true;
  }
  processData(sendSerialData);
  delay(DELAY_TIME);
  cycle++;
}

unsigned long writeTextToTFT(double temp, double humanity, double heatindex, double airpressure, uint32_t lightvalue) 
{
  tft.fillScreen(BLACK);
  unsigned long start = micros();
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
  if (Serial)
    tft.println("Serial connected");
  else
    tft.println("Serial _not_ connected");
  return micros() - start;
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

void processData(bool sendSerialData)
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
    digitalWrite(LEDrot, HIGH);
    if (sendSerialData)
    {
      Serial.println("Fehler: Daten konnten gelesen werden!");
      Serial.flush();
    }
    
    
  } 
  else
  {    
    digitalWrite(LEDrot, LOW);
    //update TFT only the CYCLE_UPDATE_TFT times when a processData is called
    //to avoid each time update of the TFT
    //or if an error occured, update the TFT the next OK time
    if (cycle >= CYCLE_UPDATE_TFT || errorOccured || UPDATE_TFT_DATA) 
    {
      writeTextToTFT(t, h, r, p0, lux);
      errorOccured = false;
      UPDATE_TFT_DATA = false;
    }
    
    if (sendSerialData)
    {
      writeSerialProtocolV3(h, t, r, p0, lux);  
    }
  }
  
  if (cycle >= CYCLE_UPDATE_TFT)  //otherwise, if it this is done within the else block above AND
  {                               //in case an error occurs and persists a longer time, the cycle will increase more and more 
    cycle = 1;                    //which could then cause an overflow of the cycle value
  }
}

void printIPInfoOnTFT(void)
{
  tft.fillScreen(BLACK);
  tft.setCursor(0, 0);
  tft.setTextSize(1);
  tft.println(" ");
  tft.setTextSize(2);
  tft.setTextColor(WHITE);
  tft.println("IP:");
  tft.println("127.0.0.1");
  tft.println("Signal Staerke:");
  tft.println("-71 dBm");
}

void ledTest(void)
{
  analogWrite(LEDblau, brightness1a); // blau einschalten
  delay(delayRGB);
  analogWrite(LEDblau, dunkel); // blau ausschalten
  analogWrite(LEDrot, brightness1b); // rot einschalten
  delay(delayRGB);
  analogWrite(LEDrot, dunkel); // rotausschalten
  analogWrite(LEDgruen, brightness1c); // gruen einschalten
  delay(delayRGB);
  analogWrite(LEDgruen, dunkel); // gruenausschalten
}

void interruptCall(void)
{
  printIPInfoOnTFT();
  UPDATE_TFT_DATA = true;
}


