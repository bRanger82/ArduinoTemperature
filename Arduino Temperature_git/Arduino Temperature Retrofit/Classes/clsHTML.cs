using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Arduino_Temperature_Retrofit.Classes
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

            foreach (DataObjectCategory cat in DataObjectCategory.GetAvailableProtocols(dobj))
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
            foreach (string s in capabaleItems)
            {
                sb.AppendLine("    <th>" + HttpUtility.HtmlEncode(s) + "</th>");
            }
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Zusatz-Info") + "</th>");
            sb.AppendLine("  </tr>");

            foreach (string dt in dobj.getLogTimings())
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

}
