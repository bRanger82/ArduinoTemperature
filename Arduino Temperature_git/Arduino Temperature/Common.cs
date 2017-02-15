using System;
using System.Collections.Generic;
using System.Globalization;
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

        static public string replaceDecPoint(string input)
        {
            string temp = input;
            string decPoint = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            temp = temp.Replace(".", decPoint);
            temp = temp.Replace(",", decPoint);
            return temp;
        }

    }
}
