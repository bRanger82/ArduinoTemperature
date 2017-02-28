using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature_Retrofit
{
    static public class Common
    {
        static public string getCurrentDateTimeFormatted()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        static public string getCurrentDateTimeFormattedNoSec(DateTime dtValue)
        {
            return dtValue.ToString("dd.MM.yyyy HH:mm");
        }

        static public string replaceDecPoint(string input)
        {
            string temp = input;
            string decPoint = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            temp = temp.Replace(".", decPoint);
            temp = temp.Replace(",", decPoint);
            return temp;
        }

        static public string getSensorValueUnit(SensorValueType typ, bool leadingSpace = true)
        {
            string ret = (leadingSpace) ? " " : "";

            switch (typ)
            {
                case SensorValueType.AirPressure: ret = ret + "mb"; break;
                case SensorValueType.Temperature: ret = ret + "°C"; break;
                case SensorValueType.Humidity: ret = ret + "%"; break;
                case SensorValueType.LUX: ret = ret + "lux"; break;
                default: ret = "N/A"; break;
            }
            return ret;
        }

        public enum SensorValueType
        {
            Temperature,
            Humidity,
            LUX,
            AirPressure,
            HeatIndex
        }

        static public class COMSettings
        {
            public static StopBits DefaultStopBits { get { return StopBits.One; } }
            public static int DefaultDataBits = 8;
            public static int DefaultBaudRate = 9600;
            public static bool DefaultDtrEnable = true;
        }

        /// <summary>
        /// Berechnet den naechsten wahrscheinlichen Wert einer Reihe
        /// </summary>
        /// <param name="values">Liste von Werten</param>
        /// <returns>Aenderung des naechesten Wertes im Vergleich zum akutuellem Wert</returns>
        static public double calculateTrend(List<double> values)
        {
            double sumMultXY = 0;
            double sumX = 0;
            double sumY = 0;
            double resBOne = 0;
            double resBTwo = 0;

            for (int i = 0; i < values.Count; i++)
            {
                sumMultXY += i * values[i];
                sumX += i;
                sumY += values[i];
                resBOne += i * i;
            }

            resBOne *= values.Count;
            resBTwo = sumX * sumX;
            double oben = (values.Count * sumMultXY) - (sumX * sumY);
            double unten = resBOne - resBTwo;

            return (oben / unten);
        }
    }
}
