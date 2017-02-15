using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Arduino_Temperature
{
    static class HTML
    {
        public static void writeHTMLFile(string PathHTML, bool tischAktiv, string tempDataTisch, List<DataObject> dataTisch, bool bodenAktiv, string tempDataBoden, List<DataObject> dataBoden)
        {
            using (StreamWriter sw = new StreamWriter(PathHTML))
            {
                string ret = Properties.Resources.html_temperature_main_template;
                if (tischAktiv)
                {
                    ret = ret.Replace("&TEMP1", tempDataTisch);
                    ret = ret.Replace("&TABLE_TISCH", createHTMLTableString(dataTisch, "Daten Tisch:"));
                }
                else
                {
                    ret = ret.Replace("&TEMP1", "");
                    ret = ret.Replace("&TABLE_TISCH", "");
                }
                if (bodenAktiv)
                {
                    ret = ret.Replace("&TEMP2", tempDataBoden);
                    ret = ret.Replace("&TABLE_BODEN", createHTMLTableString(dataBoden, "Daten Boden:"));
                }
                else
                {
                    ret = ret.Replace("&TEMP2", "Sensor 2 nicht aktiv");
                    ret = ret.Replace("&TABLE_BODEN", "Keine Daten fuer Sensor 2, nicht aktiv");
                }

                ret = ret.Replace("&LASTUPDATE", Common.getCurrentDateTimeFormatted());
                ret = ret.Replace("&HTML_HEAD", HttpUtility.HtmlEncode(XML.HTMLHead));
                //ret = ret.Replace("°", "&deg;");
                sw.WriteLine(ret);
            }
        }


        private static string createHTMLTableString(List<DataObject> lDojb, string title)
        {
            if (lDojb.Count < 1)
                return "";



            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine("</br><h3>" + title + "</h3>");
            sb.AppendLine(@"<table style=""width:100%"">");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Datum und Uhrzeit") + "</th>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Temperatur (°C)") + "</th>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Luftfeuchtigkeit (%)") + "</th>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Heat Index (°C)") + "</th>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Luftdruck (mb)") + "</th>");
            sb.AppendLine("    <th>" + HttpUtility.HtmlEncode("Zusatz-Info") + "</th>");
            sb.AppendLine("  </tr>");

            foreach (DataObject dobj in lDojb)
            {
                sb.AppendLine("  <tr>");
                sb.AppendLine("    <td>" + dobj.Timepoint.ToShortDateString() + " " + dobj.Timepoint.ToLongTimeString() + "</td>");

                if (dobj.DataAvailable)
                {
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.Temperature)) ? "No data" : HttpUtility.HtmlEncode(dobj.Temperature)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.Humidity)) ? "No data" : HttpUtility.HtmlEncode(dobj.Humidity)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.HeatIndex)) ? "No data" : HttpUtility.HtmlEncode(dobj.HeatIndex)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.AirPressure)) ? "No data" : HttpUtility.HtmlEncode(dobj.AirPressure)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.AdditionalInformation)) ? "No data" : HttpUtility.HtmlEncode(dobj.AdditionalInformation)) + "</td>");
                }
                else
                {
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td></td>");
                }

                sb.AppendLine("  </tr>");
            }

            sb.AppendLine("  </table> ");

            sb.AppendLine("</br>");
            return sb.ToString();
        }
    }
}
