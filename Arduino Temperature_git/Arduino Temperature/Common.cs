using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{
    static class Common
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

        public static class COMSettings
        {
            public static StopBits DefaultStopBits { get { return StopBits.One; } }
            public static int DefaultDataBits = 8;
            public static int DefaultBaudRate = 9600;
            public static bool DefaultDtrEnable = true;
        }
    }
}
