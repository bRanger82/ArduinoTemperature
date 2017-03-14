using System;
using System.Collections;
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

    public static class XML
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

        public static string Title { get { return getValue("/root/titel"); } set { setValue("/root/titel", value); } }
        public static string HtmlFile { get { return getValue("/root/HTML/FileHTML"); } set { setValue("/root/HTML/FileHTML", value); } }
        public static string HtmlHeadText { get { return getValue("/root/HTML/HTMLHEAD"); } set { setValue("/root/HTML/HTMLHEAD", value); } }
        public static bool HtmlEnabled { get { return checkBool(getValue("/root/HTML/Enabled")); } set { setValueBool("/root/HTML/Enabled", value); } }

        private static void setValueBool(string nodePath, bool value)
        {
            if (value)
                setValue(nodePath, "Y");
            else
                setValue(nodePath, "N");
        }

        private static void setValue(string nodePath, string Value)
        {
            loadXML();

            xDoc.SelectSingleNode(nodePath).InnerText = Value;
            xDoc.Save(XmlFileName);

        }

        private const int minHTMLUpdateFrequency = 60;

        public static void setHttpUpdateFrequency(int val)
        {
            if (val < minHTMLUpdateFrequency)
                throw new ArgumentOutOfRangeException("Fehler: die Update-Frequenz für HTML darf nicht unter " + minHTMLUpdateFrequency.ToString() + " liegen!");

            setValue("/root/HTML/UpdateFrequency", val.ToString());
        }

        public static int HttpUpdateFrequency()
        {
            string xmlValue = getValue("/root/HTML/UpdateFrequency");
            int frequency;
            if (int.TryParse(xmlValue, out frequency) && frequency >=60) //minimum every 60 seconds
            {
                return frequency;
            }
            else
            {
                return (int)60;
            }
        }

        public static bool getTopMost { get { return checkBool(getValue("/root/ProgrammEinstellungen/Topmost")); } set { setValueBool("/root/ProgrammEinstellungen/Topmost", value); } }

        private static string getValue(string nodePath)
        {

            loadXML();

            XmlNode t = xDoc.SelectSingleNode(nodePath);
            if (t == null)
                return String.Empty;
            else
                return t.InnerText.ToString();
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

        public static void updateSensorValue(string SensorName, string node, bool value)
        {
            if (value)
                updateSensorValue(SensorName, node, "Y");
            else
                updateSensorValue(SensorName, node, "N");
        }

        public static void updateSensorValue(string SensorName, string node, string value)
        {
            loadXML();
            XmlNode nodeXML = null;

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    if (getInnerText(child, "Bezeichnung") == SensorName)
                    {
                        nodeXML = child;
                        break;
                    }
                }
            }

            if (nodeXML != null)
            {
                nodeXML.SelectSingleNode(node).InnerText = value;
                xDoc.Save(XmlFileName);
            }

        }

        public static List<XMLSensorObject> getSensorItemsFromXML()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();
            ArrayList items = new ArrayList();
            items.Clear();

            loadXML();

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    XMLSensorObject tmpSensor = new XMLSensorObject();

                    tmpSensor.Name = getInnerText(child, "Bezeichnung");
                    //Name = Unique Identifier, not allowed to exist more than one time
                    if (items.IndexOf(tmpSensor.Name) >= 0)
                        throw new Exception("Fehler XML: Die Bezeichnung muss für jeden Sensor eindeutig sein.\nDer Name '" + tmpSensor.Name + "' existiert bereits!");
                    else
                        items.Add(tmpSensor.Name);

                    tmpSensor.Active = checkBool(getInnerText(child, "Aktiv"));

                    tmpSensor.LogEnabled = checkBool(getInnerText(child, "LogEnabled"));

                    tmpSensor.HTMLEnabled = checkBool(getInnerText(child, "WriteHTML"));

                    tmpSensor.DtrEnabled = checkBool(getInnerText(child, "DtrEnabled"));

                    tmpSensor.numLogEntries = DataObject.HistoryMinDefaultEntries; //default
                    int numEntries;
                    if (int.TryParse(getInnerText(child, "NumLogItems"), out numEntries))
                    {
                        if (numEntries >= DataObject.HistoryMinDefaultEntries && numEntries <= DataObject.HistoryMaxDefaultEntries)
                            tmpSensor.numLogEntries = numEntries;
                        else
                            throw new Exception("XML Fehler: Die Anzahl der Num Einträge muss zwischen " + DataObject.HistoryMinDefaultEntries.ToString() + " und " + DataObject.HistoryMaxDefaultEntries.ToString() + " liegen!");
                    } else
                    {
                        throw new Exception("XML Fehler: Die Anzahl der Num Einträge kann nicht verarbeitet werden!");
                    }

                    int Baudrate;
                    if (int.TryParse(getInnerText(child, "Baudrate"), out Baudrate))
                    {
                        if (Baudrate >= 8)
                            tmpSensor.Baudrate = Baudrate;
                        else
                            throw new Exception("XML Fehler: Baudrate hat einen Wert kleiner 8");
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
                            throw new Exception("XML Fehler: Die maximale Größe des Logfiles muss zwischen 1.048.576 und 1.073.741.824 liegen!");
                    }
                    else
                    {
                        throw new Exception("XML Fehler: Die maximale Größe des Logfiles kann nicht verarbeitet werden!");
                    }
                    tmpSensor.maxLogFileSize = maxLogFileSize;

                    
                    tmpSensor.Port = getInnerText(child, "Port");
                    tmpSensor.LogFilePath = getInnerText(child, "LogFile");

                    lst.Add(tmpSensor);

                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("tmpSensor.Name          == {0}", tmpSensor.Name);
                    Console.WriteLine("tmpSensor.Active        == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.Port          == {0}", tmpSensor.Port);
                    Console.WriteLine("tmpSensor.Baudrate      == {0}", tmpSensor.Baudrate.ToString());
                    Console.WriteLine("tmpSensor.DtrEnabled    == {0}", (tmpSensor.DtrEnabled) ? "Y" : "N");
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
