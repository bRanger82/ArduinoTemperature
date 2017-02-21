using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
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

            switch(typ)
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
    }
}
