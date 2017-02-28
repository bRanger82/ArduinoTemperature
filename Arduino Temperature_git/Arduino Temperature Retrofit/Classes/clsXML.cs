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

        public static bool getHtmlEnabled { get { return (getValue("/root/ProgrammEinstellungen/WriteHTML").ToUpper() == "Y"); } }

        public static bool getTopMost { get { return (getValue("/root/ProgrammEinstellungen/Topmost").ToUpper() == "Y"); } }

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

        public static List<XMLSensorObject> getSensorItemsFromXML()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();

            loadXML();

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    XMLSensorObject tmpSensor = new XMLSensorObject();
                    if (getInnerText(child, "Aktiv").ToUpper() == "Y")
                        tmpSensor.Active = true;
                    else
                        tmpSensor.Active = false;

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

                    tmpSensor.numLogEntries = DataObject.LogMinEntries; //default

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

                    Console.WriteLine("************************************************************");
                    Console.WriteLine("tmpSensor.Active        == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.Name          == {0}", tmpSensor.Name);
                    Console.WriteLine("tmpSensor.Port          == {0}", tmpSensor.Port);
                    Console.WriteLine("tmpSensor.LogFilePath   == {0}", tmpSensor.LogFilePath);
                    Console.WriteLine("tmpSensor.numLogEntries == {0}", tmpSensor.numLogEntries.ToString());
                    Console.WriteLine("************************************************************");
                }
            }

            return lst;
        }
    }
}
