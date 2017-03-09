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
#define LEDblau 44    // RBG LED Blue
#define LEDrot 46     // RBG LED Red
#define LEDgruen 45   // RBG LED Green
#define delayRGB 500  // RGB LED, Delay between Test Light switches
#define brightness1a 125 // RGB LED, Brightnesslevel 1
#define brightness1b 125 // RGB LED, Brightnesslevel 2
#define brightness1c 125 // RGB LED, Brightnesslevel 3
#define dunkel 0 // RGB LED, Brightnesslevel 0 = dark

//cycle will be increased in the loop(), if cycle == CYCLE_UPDATE_TFT --> update TFT and reset cycle to 0
//this will avoid updating the TFT each time when a serial data is sent (no need to update the TFT that often)
#define CYCLE_UPDATE_TFT 6 
int cycle = CYCLE_UPDATE_TFT;

#define DELAY_TIME 10000 //Delay Time between each processData() call

//Declare sensors and create instances
TSL2561 tsl(TSL2561_ADDR_FLOAT); 
MCUFRIEND_kbv tft;
DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;

//Send-Data is called more often than update display
//if UPDATE_TFT_DATA is set to true, the next Send-Data event also triggers an update of the TFT display
//after the trigger fired one time, it is set to false again when the TFT display update was done
volatile bool UPDATE_TFT_DATA = false;

void initSerial(void)
{
  Serial.begin(112000);
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

//check if serial data is available and process the data
void checkSerialData()
{
  //if DELAY_TIME is set to a few seconds or more
  //the serial read has to wait. this was split up here
  int numSubSteps = 100;
  for (int i = 1; i <=numSubSteps; i++)
  {
    if ((Serial.available() > 0))
    {
      processIncomingData(Serial.readString());
      Serial.flush();
    }
    delay(DELAY_TIME / numSubSteps);    
  }  
}

void loop(void) 
{
  processData();
  checkSerialData();
  cycle++;
}

void processIncomingData(String data)
{
  if (data == "LED")
  {
    digitalWrite(LEDblau, !digitalRead(LEDblau));
    Serial.println("REPLY|BLAU-OK|EOF");
    return;
  } else if (data == "STATUSLED")
  {
    if (digitalRead(LEDblau) == HIGH)
      Serial.println("REPLY|BLAU-IST-AN|EOF");
    else 
      Serial.println("REPLY|BLAU-IST-AUS|EOF");
    return;    
  } else if (data == "PROTOCOL_VERSION")
  {
    Serial.println("REPLY|V3|EOF");
    return;
  } else if (data == "GET_TEMP_ARDUINO")
  {
    Serial.println("REPLY|FOUND_ARDUINO|EOF");
    return;
  }

  Serial.println("REPLY|FEHLER: Nicht Implementiert!|EOF");
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
  analogWrite(LEDgruen, brightness1c);
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
  analogWrite(LEDgruen, dunkel); 
}

void writeSerialProtocolV2(float humanity, float temperature, float heatIndex, double airPressure)
{
  analogWrite(LEDgruen, brightness1c);
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
  analogWrite(LEDgruen, dunkel);
}

void writeSerialProtocolV3(float humanity, float temperature, float heatIndex, double airPressure, int lux)
{
  analogWrite(LEDgruen, brightness1c);
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
  analogWrite(LEDgruen, dunkel);
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
  double p0 = 0; //Wert fuer Luftdruck
  bool pressAvailable = getPressure(&p0); //Luftdruck lesen
  static bool errorOccured = false;
  uint16_t x = tsl.getLuminosity(TSL2561_VISIBLE);
  uint32_t lum = tsl.getFullLuminosity();
  uint16_t ir, full;
  ir = lum >> 16;
  full = lum & 0xFFFF;
  uint32_t lux = tsl.calculateLux(full, ir);
  
  if (isnan(t) || isnan(h) || isnan(r) || !pressAvailable) //Fehler beim Lesen eines der Daten
  {
    showError();
    errorOccured = true;
    Serial.println("Fehler: Daten konnten gelesen werden!");
    Serial.flush();
  } 
  else
  {
    //update TFT only the CYCLE_UPDATE_TFT times when a processData is called
    //to avoid each time update of the TFT
    //or if an error occured, update the TFT the next OK time
    if (cycle >= CYCLE_UPDATE_TFT || errorOccured || UPDATE_TFT_DATA) 
    {
      writeTextToTFT(t, h, r, p0, lux);
      errorOccured = false;
      UPDATE_TFT_DATA = false;
    }
    
    writeSerialProtocolV3(h, t, r, p0, lux); 
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
