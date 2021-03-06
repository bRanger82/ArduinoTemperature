﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    
    public static class HTML
    {
        public static async Task<string> DownloadPage(string url)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(2000);
                //return await client.GetStringAsync(url);
                
                using (var r = await client.GetAsync(new Uri(url), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    return result;
                }
                
            }
        }

        public static void WriteHTMLFile(string filename, Dictionary<string, DataObject> lDobj, string HeadText)
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
                    sb.Append(CreateTopOfHtml((DataObject)kvp.Value));
                    sb.AppendLine("</br>");
                    dataAvailable = true;
                }
            }



            foreach (KeyValuePair<string, DataObject> kvp in lDobj)
            {
                if (((DataObject)kvp.Value).DataAvailable)
                {
                    sb.Append(CreateHTMLTableString((DataObject)kvp.Value));
                    sb.Append(((DataObject)kvp.Value).GetLastUpdatedFormatted());
                }
            }
            
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

        private static string GetHtmlData(DataObject dobj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            foreach (DataObjectCategory cat in DataObjectCategory.GetAvailableProtocols(dobj))
            {
                double value = dobj.GetItem(cat);
                string name = cat.Value.ToString();

            }

            return sb.ToString();
        }

        private static string GetData(DataObject dobjExt, string dobjCat)
        {
            double temp = dobjExt.GetItem(dobjCat);
            if (temp == double.MinValue)
                return "Keine Daten";

            return HttpUtility.HtmlEncode(temp.ToString("F") + " " + DataObjectCategory.GetSensorValueUnit(dobjCat));
        }

        private static string CreateTopOfHtml(DataObject dobj)
        {
            if (!XML.HtmlEnabled || !dobj.DataAvailable)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Clear();

            List<string> capabaleItems = DataObjectCategory.GetCapableItems(dobj.Protocol);

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
                sb.AppendLine("<td>" + HttpUtility.HtmlEncode(s) + "</td><td>" + GetData(dobj, s) + "</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine(HttpUtility.HtmlEncode("Zuletzt aktualisiert: "
                          + dobj.GetLastUpdatedFormatted())
                          + "</br>");
            return sb.ToString();
        }

        private static string CreateHTMLTableString(DataObject dobj)
        {
            if (!XML.HtmlEnabled || !dobj.DataAvailable)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Clear();



            List<string> capabaleItems = DataObjectCategory.GetCapableItems(dobj.Protocol);

            sb.AppendLine("</br><h3>" + HttpUtility.HtmlEncode(dobj.Name) + "</h3>");
            sb.AppendLine("  <table> ");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Datum und Uhrzeit") + "</th>");
            foreach (string s in capabaleItems)
            {
                sb.AppendLine("    <th>" + HttpUtility.HtmlEncode(s) + "</th>");
            }
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Zusatz-Info") + "</th>");
            sb.AppendLine("  </tr>");

            foreach (string dt in dobj.GetLogTimings())
            {
                sb.AppendLine("  <tr>");
                sb.AppendLine("    <td>" + dt + "</td>");

                foreach (string s in capabaleItems)
                {
                    sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.GetLogItem(dt, s) + DataObjectCategory.GetSensorValueUnit(s)) + "</td>");
                }

                sb.AppendLine("    <td>" + HttpUtility.HtmlEncode(dobj.AdditionalInformation) + "</td>");
                sb.AppendLine("  </tr>");
            }

            sb.AppendLine("  </table> ");
            sb.AppendLine("</br>");

            return sb.ToString();
        }
    }

}
