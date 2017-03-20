﻿using System;
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
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }

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
            bool dataAvailable = false;

            sb.AppendLine("<html>");
            sb.AppendLine("<head><h2>" + HttpUtility.HtmlEncode(XML.HtmlHeadText) + "</h2></head>");
            sb.AppendLine("<style>");
            sb.AppendLine("table, th, td {");
            sb.AppendLine("   border: 1px solid black;");
            sb.AppendLine("}");
            sb.AppendLine("table {");
            sb.AppendLine("    border-collapse: collapse;");
            sb.AppendLine("    width: 95%;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("th, td {");
            sb.AppendLine("    text-align: left;");
            sb.AppendLine("    vertical-align: middle;");
            sb.AppendLine("    padding: 5px;");
            sb.AppendLine("}");
            sb.AppendLine("th {");
            sb.AppendLine("    background-color: #4CAF50;");
            sb.AppendLine("    color: white;");
            sb.AppendLine("}");
            sb.AppendLine("tr:nth-child(even){background-color: #f5f5f5}");
            sb.AppendLine("</style>");

            foreach (KeyValuePair<string, DataObject> kvp in lDobj)
            {
                if (((DataObject)kvp.Value).DataAvailable)
                {
                    sb.Append(createTopOfHtml((DataObject)kvp.Value));
                    sb.AppendLine("</br>");
                    dataAvailable = true;
                }
            }

            

            foreach (KeyValuePair<string, DataObject> kvp in lDobj)
            {
                if (((DataObject)kvp.Value).DataAvailable)
                    sb.Append(createHTMLTableString((DataObject)kvp.Value));
            }
            sb.AppendLine("Zuletzt aktualisiert: " + Common.getCurrentDateTimeFormatted());
            sb.AppendLine("</html>");

            // if no data is available to write in html set missing data text
            if (!dataAvailable)
            {
                sb.Clear();
                sb.AppendLine("<html>");
                sb.AppendLine("<head><h2>" + HttpUtility.HtmlEncode(XML.HtmlHeadText) + "</h2></head>");
                sb.AppendLine("<h3>" + HttpUtility.HtmlEncode("Es sind noch keine Daten verfügbar!") + "</h3>");
                sb.AppendLine("<h3>" + HttpUtility.HtmlEncode("Bitte versuchen Sie es später noch einmal.") + "</h3>");
                sb.AppendLine("</html>");
            }

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

        private static string getData(DataObject dobjExt, string dobjCat)
        {
            double temp = dobjExt.getItem(dobjCat);
            if (temp == double.MinValue)
                return "Keine Daten";

            return HttpUtility.HtmlEncode(temp.ToString("F") + " " + DataObjectCategory.getSensorValueUnit(dobjCat));
        }

        private static string createTopOfHtml(DataObject dobj)
        {
            if (!XML.HtmlEnabled || !dobj.DataAvailable)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            
            List<string> capabaleItems = DataObjectCategory.getCapableItems(dobj.Protocol);
            
            sb.AppendLine("</br><h3>" 
                          + HttpUtility.HtmlEncode("Sensor: " + dobj.Name) 
                          + "</h3>");

            sb.AppendLine("<table>");
            sb.AppendLine("  <tr>");
            sb.AppendLine("		<th>Bezeichnung</th>");
            sb.AppendLine("		<th>Wert</th>");
            sb.AppendLine("  </tr>");

            foreach (string s in capabaleItems)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + HttpUtility.HtmlEncode(s) + "</td><td>" + getData(dobj, s) + "</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine(HttpUtility.HtmlEncode("Zuletzt aktualisiert: "
                          + Common.getCurrentDateTimeFormatted())
                          + "</br>");
            return sb.ToString();
        }

        private static string createHTMLTableString(DataObject dobj)
        {
            if (!XML.HtmlEnabled || !dobj.DataAvailable)
                return string.Empty;
            
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            


            List<string> capabaleItems = DataObjectCategory.getCapableItems(dobj.Protocol);

            sb.AppendLine("</br><h3>" + HttpUtility.HtmlEncode(dobj.Name) + "</h3>");
            sb.AppendLine("  <table> ");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Datum und Uhrzeit") + "</th>");
            foreach(string s in capabaleItems)
            {
                sb.AppendLine("    <th>" + HttpUtility.HtmlEncode(s) + "</th>");
            }
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Zusatz-Info") + "</th>");
            sb.AppendLine("  </tr>");
            
            foreach(string dt in dobj.getLogTimings())
            {
                sb.AppendLine("  <tr>");
                sb.AppendLine("    <td>" + dt + "</td>");

                foreach (string s in capabaleItems)
                {
                        sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.getLogItem(dt, s) + DataObjectCategory.getSensorValueUnit(s)) + "</td>");
                }

                sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.AdditionalInformation) + "</td>");
                sb.AppendLine("  </tr>");
            }
            
            sb.AppendLine("  </table> ");
            sb.AppendLine("</br>");

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
