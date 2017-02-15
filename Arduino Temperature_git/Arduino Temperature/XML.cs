using System;
using System.Windows.Forms;
using System.Xml;

namespace Arduino_Temperature
{
    public class XML
    {
        private static XmlDocument xDoc = new XmlDocument();
        private static bool XMLLoaded = false;
        public static string XMLFileName { get { return System.IO.Path.Combine(Application.StartupPath, "Temperature.xml"); } }
        public static bool XMLExists { get { return System.IO.File.Exists(XMLFileName); } }

        public static string Title { get { return getValue("/root/titel"); } }
        public static string FileHTML { get { return getValue("/root/FileHTML"); } }
        public static string HTMLHead { get { return getValue("/root/HTMLHEAD"); } }
        public static string TischPort { get { return getValue("/root/Tisch/Port"); } }
        public static string TischBezeichnung { get { return getValue("/root/Tisch/Bezeichnung"); } }
        public static string TischLogfile { get { return getValue("/root/Tisch/LogFile"); } }
        public static string BodenPort { get { return getValue("/root/Boden/Port"); } }
        public static string BodenBezeichnung { get { return getValue("/root/Boden/Bezeichnung"); } }
        public static string BodenLogfile { get { return getValue("/root/Boden/LogFile"); } }
        public static bool TischAktiv { get { return getValue("/root/Tisch/Aktiv").ToUpper() == "Y"; } }
        public static bool BodenAktiv { get { System.Diagnostics.Debug.Print("getValue('/root/Boden/Aktiv') = " + getValue("/root/Boden/Aktiv")); return getValue("/root/Boden/Aktiv").ToUpper() == "Y"; } }

        public static long maxLogFileSize
        {
            get
            {
                long fSize = 0;
                if (long.TryParse(getValue("/root/Boden/maxLogFileSize"), out fSize))
                    return fSize;
                else
                    return 1024 * 1024;
            }
        }

        public static int maxTimeDifferenceReadData
        {
            get
            {
                int iDiff = 0;
                if (int.TryParse(getValue("/root/Boden/maxTimeDifferenceReadData"), out iDiff))
                    return iDiff;
                else
                    return 60;
            }
        }

        private static string getValue(string nodePath)
        {
            try
            {
                if (!XMLLoaded) loadXML();
                XMLLoaded = true;
                XmlNode t = xDoc.SelectSingleNode(nodePath);
                if (t == null)
                    return String.Empty;
                else
                    return t.InnerText.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return String.Empty;
            }
        }

        private static bool loadXML()
        {
            if (!XMLExists)
                return false;

            xDoc.Load(@"Temperature.xml");

            return true;
        }
    }
}
