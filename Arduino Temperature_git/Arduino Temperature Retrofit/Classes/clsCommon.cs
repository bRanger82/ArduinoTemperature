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

        static public string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";

        static public string getCurrentDateTimeFormatted()
        {
            return DateTime.Now.ToString(DateTimeFormat);
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

        static public double CalculateStdDev(List<double> historyEntries, int numHistoryEntriesAsBasis = 50)
        {
            if (historyEntries.Count < 5) //mindestens 5 Werte sind notwendig
                return (double)0;

            List<double> values = getSubset(historyEntries, numHistoryEntriesAsBasis);

            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
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

            List<Point> lstData = new List<Point>();

            for (int i =0; i < values.Count; i++)
            {
                Point p = new Point();
                p.m_x = i;
                p.m_y = values[i];
                lstData.Add(p);
            }

            double slope;
            CalcTrendValues(lstData, out slope);

            return slope;
        }

        private struct Point
        {
            public double m_y;
            public double m_x;
        }

        private static void CalcTrendValues(List<Point> data, out double slope) //, out double intercept, out double rSquared)
        {
            double xSum = 0;
            double ySum = 0;
            double xySum = 0;
            double xSqSum = 0;
            double ySqSum = 0;

            foreach (var point in data)
            {
                var x = point.m_x;
                var y = point.m_y;

                xSum += x;
                ySum += y;
                xySum += (x * y);
                xSqSum += (x * x);
                ySqSum += (y * y);
            }
            
            slope = ((data.Count * xySum) - (xSum * ySum)) / ((data.Count * xSqSum) - (xSum * xSum));
            
            double intercept = ((xSqSum * ySum) - (xSum * xySum)) /
                              ((data.Count * xSqSum) - (xSum * xSum));

            var a = ((data.Count * xySum) - (xSum * ySum));
            var b = (((data.Count * xSqSum) - (xSum * xSum)) *
                         ((data.Count * ySqSum) - (ySum * ySum)));
            double rSquared = (a * a) / b;

            Console.WriteLine("Slope: {0}, intercept: {1}, rSquared: {2} for last x: {3} and y: {4}", slope, intercept, rSquared, data[data.Count - 1].m_x, data[data.Count - 1].m_y);
        }

    }
}
