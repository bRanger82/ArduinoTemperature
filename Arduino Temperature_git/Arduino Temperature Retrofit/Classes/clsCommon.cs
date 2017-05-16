using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace Arduino_Temperature_Retrofit
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }


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
            temp = temp.Replace(".", decPoint); //in case of '.' --> ','
            temp = temp.Replace(",", decPoint); //in case of ',' --> '.'
            return temp;
        }

        static public class COMSettings
        {
            public static StopBits DefaultStopBits { get { return StopBits.One; } }
            public static int DefaultDataBits = 8;
            public static int DefaultBaudRate = 9600;
            public static bool DefaultDtrEnable = false;
        }

        static private List<double> getSubset(List<double> values, int count)
        {
            if (values.Count <= count)
                return values;

            int start = values.Count - count;

            return values.GetRange(start, count);
        }

        /// <summary>
        /// Berechnet den naechsten wahrscheinlichen Wert einer Reihe
        /// </summary>
        /// <param name="values">Liste von Werten</param>
        /// <returns>Aenderung des naechesten Wertes im Vergleich zum akutuellem Wert</returns>
        static public double calculateTrend(List<double> historyEntries, int numHistoryEntriesAsBasis = 50)
        {

            if (historyEntries.Count < 5) //mindestens 5 Werte sind notwendig
                return (double)0;

            //nur die letzten Einträge sollen geprüft werden (ein Datensatz, der u.U. mehrere Stunden zurück liegt hat keine Aussagekraft mehr)
            List<double> values = getSubset(historyEntries, numHistoryEntriesAsBasis);

            double sumMultXY = 0;
            double sumX = 0;
            double sumY = 0;
            double resBOne = 0;
            double resBTwo = 0;


            for (int i =0; i < values.Count; i++)
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
