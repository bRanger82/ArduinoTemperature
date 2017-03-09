using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Arduino_Temperature_Retrofit
{

    public class HTMLSettings
    {
        public bool Enabled { get; set; } = false;
        public string HeadText { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public int UpdateFrequency { get; set; } = 60; //seconds
        public DateTime LastRun { get; set; } = DateTime.Now.AddYears(-1);
    }

    static public class HTML
    {

        public static void writeHTMLFile(string filename, Dictionary<string, DataObject> lDobj, string HeadText)
        {
            StringBuilder sb = new StringBuilder();
            List<DataObject> dobjList = new List<DataObject>();

            foreach(KeyValuePair<string, DataObject> kvp in lDobj)
            {
                DataObject dobj = (DataObject)kvp.Value;
                dobjList.Add(dobj);
                
            }
            sb.Append(createHTMLTableString(dobjList));

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(sb.ToString());
                sw.Flush();
            }

            sb.Clear();
        }

        private static string getHtmlData(DataObject dobj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            foreach(DataObjectCategory cat in DataObjectCategory.GetAvailableProtocols(dobj))
            {
                double value = dobj.getItem(cat);
                string name = cat.Value.ToString();
                
            }

            return sb.ToString();
        }

        private static string getData(DataObject dobjExt, DataObjectCategory dobjCat)
        {
            double temp = dobjExt.getItem(dobjCat);
            if (temp == double.MinValue)
                return "Keine Daten";

            return HttpUtility.HtmlEncode(temp.ToString());
        }

        private static string createHTMLTableString(List<DataObject> lDojb)
        {
            if (lDojb.Count < 1)
                return "";

            if (!XML.HtmlEnabled)
                return string.Empty;
            
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>" + HttpUtility.HtmlEncode(XML.HtmlHeadText) + "</head>");
            foreach (DataObject dobj in lDojb)
            {
                if (!dobj.HTMLEnabled)
                    continue;

                List<string> capabaleItems = DataObjectCategory.getCapableItems(dobj.Protocol);

                sb.AppendLine("</br><h3>" + dobj.Name + "</h3>");
                sb.AppendLine(@"<table style=""width:100%"">");
                sb.AppendLine("  <tr>");
                sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Datum und Uhrzeit") + "</th>");
                foreach(string s in capabaleItems)
                {
                    sb.AppendLine("    <th>" + HttpUtility.HtmlEncode(s) + "</th>");
                }
                sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Zusatz-Info") + "</th>");
                sb.AppendLine("  </tr>");


                sb.AppendLine("  <tr>");
                sb.AppendLine("    <td>" + dobj.LastUpdated.ToShortDateString() + " " + dobj.LastUpdated.ToLongTimeString() + "</td>");

                if (dobj.DataAvailable)
                {
                    foreach(string s in capabaleItems)
                    {
                        DataObjectCategory dobjCat = DataObjectCategory.getObjectCategory(s);
                        sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(getData(dobj, dobjCat)) + "</td>");
                    }

                    sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.AdditionalInformation) + "</td>");
                }
                else
                {
                    foreach (string s in capabaleItems)
                    {
                        DataObjectCategory dobjCat = DataObjectCategory.getObjectCategory(s);
                        sb.AppendLine("    <td>Keine Daten</td>");
                    }

                    sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.AdditionalInformation) + "</td>");
                }

                sb.AppendLine("  </tr>");
                

                sb.AppendLine("  </table> ");

                sb.AppendLine("</br>");
            }

            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
    static public class LOG
    {
        public enum LogDataReturnValue
        {
            OK = 0,
            NO_WRITE_PERMISSION = 1,
            LOG_NOT_ENABLED = 2
        }

        static public LogDataReturnValue LogData(DataObject dobj, string data)
        {
            if (!dobj.LoggingEnabled)
                return LogDataReturnValue.LOG_NOT_ENABLED;

            string path = dobj.LogPath;
            FileInfo fi = new FileInfo(path);

            if (!Permission.HasAccess(fi, FileSystemRights.WriteData))
            {
                //Has no access to directory
                return LogDataReturnValue.NO_WRITE_PERMISSION;
            }

            //if directory not exists, create it
            if (!Directory.Exists(path.Substring(0, path.LastIndexOf("\\"))))
                Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));

            //if file not exists, create an empty one (streamwriter object needs an existing file for appending)
            if (!File.Exists(path))
                File.Create(path).Close();

            //if max filesize is reached, delete the logfile
            if (fi.Length > dobj.maxLogFileSize)
                fi.Delete();

            //append data-string to log
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(data);
            }

            return LogDataReturnValue.OK;

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
