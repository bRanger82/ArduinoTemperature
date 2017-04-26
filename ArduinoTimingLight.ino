#include <SFE_BMP180.h>
#include "DHT.h"

#define DHTPIN 2     
#define DHTTYPE DHT22
#define SERIALWAIT 200

int DELAY_TIME = 30000; //Delay Time between each processData() call (ms)

DHT dht(DHTPIN, DHTTYPE);
SFE_BMP180 pressure;

float h = 0;   //Luftfeuchte auslesen
float t = 0;   //Temperatur auslesen
float r = 0;   //Heat-Index berechnen
double p0 = 0; //Wert fuer Luftdruck

void initSerial(void)
{
  Serial.begin(9600);
  Serial.flush();   
}

void initSensors(void)
{
  dht.begin();
  pressure.begin();
}



void setup(void) 
{
  initSerial();
  initSensors();
}

void checkSerialData()
{
  for (int i = 1; i <=100; i++)
  {
    if ((Serial.available() > 0))
    {
      processIncomingData(Serial.readString());
      Serial.flush();
    }
    delay(DELAY_TIME / 100);    
  }  
}

void loop(void) 
{
  processData();
  checkSerialData();
}

void processIncomingData(String data)
{
  if (data == "HELP")
  {
    Serial.println("REPLY|PROTOCOL_VERSION|GETTIMING|GET_TEMP_ARDUINO|DATA|SETTIMING|EOF");
    return;
  } else if (data == "PROTOCOL_VERSION")
  {
    Serial.println("REPLY|V2|EOF");
    return;
  } else if (data == "GETTIMING")
  {
    Serial.print("REPLY|TIMING|");
    Serial.print(DELAY_TIME);
    Serial.println("|ms|EOF");
    return;
  } else if (data == "GET_TEMP_ARDUINO")
  {
    Serial.println("REPLY|FOUND_ARDUINO|EOF");
    return;
  } else if (data == "DATA")
  {
    writeSerialProtocolV2(h, t, r, p0);
    return;
  } else if (data.startsWith("SETTIMING|"))
  {
    String tmp = data.substring(10);
    int newTiming = tmp.toInt();

    if (newTiming < 10000 || newTiming > 60000)
    {
      Serial.println("REPLY|ERROR_SET_TIMING_NOT_BETWEEN_10000_AND_60000_MS|EOF");
    } else
    {
      DELAY_TIME = newTiming;
      Serial.print("REPLY|OK_SET_TO_NEW_TIMING_");
      Serial.print(DELAY_TIME);
      Serial.println("_MS|EOF");
    }
    return;
  }

  Serial.println("REPLY|FEHLER: Nicht Implementiert!|EOF");
}


void writeSerialProtocolV2(float humanity, float temperature, float heatIndex, double airPressure)
{
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
  p0 = 0; //Wert fuer Luftdruck
  bool pressAvailable = getPressure(&p0); //Luftdruck lesen
  
  if (isnan(t) || isnan(h) || isnan(r) || !pressAvailable) //Fehler beim Lesen eines der Daten
  {
    if (Serial)
    {
      Serial.println("Fehler: Daten konnten gelesen werden!");
      Serial.flush();
    }
  } 
  else
  {
    writeSerialProtocolV2(h, t, r, p0); 
  }

}
