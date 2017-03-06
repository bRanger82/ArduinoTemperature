using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Arduino_Temperature_Retrofit
{
    public class XMLSensorObject
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Port { get; set; }
        public string LogFilePath { get; set; }
        public int numLogEntries { get; set; }
        public long maxLogFileSize { get; set; }
        public bool LogEnabled { get; set; }
        public bool HTMLEnabled { get; set; }
        public int Baudrate { get; set; }
        public bool DtrEnabled { get; set; }
    }

    public static class clsXML
    {
        private static XmlDocument xDoc = new XmlDocument();
        private static string XmlFileName = @"Settings.xml";

        private static string getInnerText(XmlNode node, string name)
        {
            XmlNode eNode = node.SelectSingleNode(name);
            if (eNode != null)
                return eNode.InnerText;
            else
                return string.Empty;
        }

        public static string Title { get { return getValue("/root/titel"); } }
        public static string getHtmlFile { get { return getValue("/root/HTML/FileHTML"); } }
        public static string getHtmlHeadText { get { return getValue("/root/HTML/HTMLHEAD"); } }
        public static bool HttpEnabled { get { return checkBool(getValue("/root/HTML/Enabled")); } }
        public static bool DtrEnabled { get { return checkBool(getValue("/root/HTML/Enabled")); } }
        public static bool getTopMost { get { return checkBool(getValue("/root/ProgrammEinstellungen/Topmost")); } }

        private static string getValue(string nodePath)
        {
            try
            {
                loadXML();

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

            xDoc.Load(XmlFileName);

            return true;
        }

        private static bool checkBool(string value)
        {
            return (value.ToUpper() == "Y");
        }

        public static List<XMLSensorObject> getSensorItemsFromXML()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();

            loadXML();

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    XMLSensorObject tmpSensor = new XMLSensorObject();

                    tmpSensor.Active = checkBool(getInnerText(child, "Aktiv"));

                    tmpSensor.LogEnabled = checkBool(getInnerText(child, "LogEnabled"));

                    tmpSensor.HTMLEnabled = checkBool(getInnerText(child, "WriteHTML"));

                    tmpSensor.numLogEntries = DataObject.LogMinEntries; //default
                    int numEntries;
                    if (int.TryParse(getInnerText(child, "NumLogItems"), out numEntries))
                    {
                        if (numEntries >= DataObject.LogMinEntries && numEntries <= DataObject.LogMaxEntries)
                            tmpSensor.numLogEntries = numEntries;
                        else
                            MessageBox.Show("XML Fehler: Die Anzahl der Num Einträge muss zwischen " + DataObject.LogMinEntries.ToString() + " und " + DataObject.LogMaxEntries.ToString() + " liegen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else
                    {
                        MessageBox.Show("XML Fehler: Die Anzahl der Num Einträge kann nicht verarbeitet werden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    int Baudrate;
                    if (int.TryParse(getInnerText(child, "Baudrate"), out Baudrate))
                    {
                        tmpSensor.Baudrate = Baudrate;
                    } else
                    {
                        tmpSensor.Baudrate = Common.COMSettings.DefaultBaudRate;
                    }

                    long maxLogFileSize = 4194304;
                    if (long.TryParse(getInnerText(child, "maxLogFileSize"), out maxLogFileSize))
                    {
                        if (maxLogFileSize >= 1048576 && maxLogFileSize <= 1073741824)
                            tmpSensor.maxLogFileSize = maxLogFileSize;
                        else
                            MessageBox.Show("XML Fehler: Die maximale Größe des Logfiles muss zwischen 1.048.576 und 1.073.741.824 liegen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("XML Fehler: Die maximale Größe des Logfiles kann nicht verarbeitet werden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    tmpSensor.maxLogFileSize = maxLogFileSize;

                    tmpSensor.Name = getInnerText(child, "Bezeichnung");
                    tmpSensor.Port = getInnerText(child, "Port");
                    tmpSensor.LogFilePath = getInnerText(child, "LogFile");

                    lst.Add(tmpSensor);

                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("tmpSensor.Name          == {0}", tmpSensor.Name);
                    Console.WriteLine("tmpSensor.Active        == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.Port          == {0}", tmpSensor.Port);
                    Console.WriteLine("tmpSensor.Baudrate      == {0}", tmpSensor.Baudrate.ToString());
                    Console.WriteLine("tmpSensor.LogEnabled    == {0}", (tmpSensor.LogEnabled) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.LogFilePath   == {0}", tmpSensor.LogFilePath);
                    Console.WriteLine("tmpSensor.HTMLEnabled   == {0}", (tmpSensor.HTMLEnabled) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.numLogEntries == {0}", tmpSensor.numLogEntries.ToString());
                    Console.WriteLine("******************************************************************");
                }
            }

            return lst;
        }
    }
}
