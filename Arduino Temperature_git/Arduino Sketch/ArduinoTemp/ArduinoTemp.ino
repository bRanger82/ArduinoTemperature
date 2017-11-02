#include "DHT.h"
#include "BMP180.h"

#define DHTPIN 2     
#define DHTTYPE DHT22

DHT dht(DHTPIN, DHTTYPE);
BMP180 pressure;

void setup() {
  
  Serial.begin(9600); 
  
  dht.begin();
  pressure = BMP180();
  if(pressure.EnsureConnected())
  {    
    pressure.SoftReset();
    pressure.Initialize();
  }
}

void loop() {
  float h = dht.readHumidity();     //Luftfeuchte auslesen
  float t = dht.readTemperature();  //Temperatur auslesen
  float r = dht.computeHeatIndex(t, h, false);
  long currentPressureP = 0;
  float currentPressuremb = 0;
  float currentPressureinHg = 0;  
    
  if (pressure.IsConnected)
  {
    currentPressureP = pressure.GetPressure();
    currentPressuremb = currentPressureP/100.0;
    currentPressureinHg = currentPressuremb*0.02953;  
  }
  
  if (Serial)
  {
    // Prüfen ob eine gültige Zahl zurückgegeben wird. Wenn NaN (not a number) zurückgegeben wird, dann Fehler ausgeben.
    if (isnan(t) || isnan(h) || isnan(r) || !pressure.IsConnected) 
    {
      Serial.println("Fehler: Daten konnten gelesen werden!");
    } 
    else
    {
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
      Serial.print(currentPressuremb); //Luftdruck millibar
      Serial.print("|"); // delim
      Serial.println("EOF");
    }
    delay(10000);    
  }
  
}
