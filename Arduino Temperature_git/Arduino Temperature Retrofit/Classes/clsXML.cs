using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Arduino_Temperature_Retrofit
{
    public enum XMLProtocol
    {
        COM,
        HTTP,
        NOT_DEFINED
    }

    public class XMLSensorObject
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Port { get; set; }
        public string LogFilePath { get; set; }
        public int NumLogEntries { get; set; }
        public long MaxLogFileSize { get; set; }
        public bool LogEnabled { get; set; }
        public bool HTMLEnabled { get; set; }
        public int Baudrate { get; set; }
        public bool DtrEnabled { get; set; }
        public XMLProtocol DataInterfaceType { get; set; }
        public string URL { get; set; }
        public bool WriteToDatabase { get; set; }
    }

    public class XMLSQLObject
    {
        public bool Active { get; set; }
        public string Server { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }
        public int UpdateFrequency { get; set; }
        public string Scheme { get; set; }
    }

    public static class XML
    {
        private static XmlDocument xDoc = new XmlDocument();
        private static string XmlFileName = @"Settings.xml";

        private static string GetInnerText(XmlNode node, string name)
        {
            XmlNode eNode = node.SelectSingleNode(name);
            if (eNode != null)
                return eNode.InnerText;
            else
                return string.Empty;
        }

        public static string SQLServer { get { return GetValue("/root/SQL/Server"); } set { SetValue("/root/SQL/Server", value); } }
        public static bool SQLActive { get { return CheckBool(GetValue("/root/SQL/Active")); } set { SetValueBool("/root/SQL/Active", value); } }
        public static string SQLUser { get { return GetValue("/root/SQL/DBUser"); } set { SetValue("/root/SQL/DBUser", value); } }
        public static string SQLPassword { get { return GetValue("/root/SQL/DBPass"); } set { SetValue("/root/SQL/DBPass", value); } }
        public static string SQLScheme { get { return GetValue("/root/SQL/Scheme"); } set { SetValue("/root/SQL/Scheme", value); } }

        public static int SQLFrequency()
        {
            string xmlValue = GetValue("/root/SQL/Frequency");
            int frequency;
            if (int.TryParse(xmlValue, out frequency) && frequency >= 1) //minimum every 60 seconds / 1 Minute
            {
                return frequency;
            }
            else
            {
                return (int)5;
            }
        }

        public static string Title { get { return GetValue("/root/titel"); } set { SetValue("/root/titel", value); } }
        public static string HtmlFile { get { return GetValue("/root/HTML/FileHTML"); } set { SetValue("/root/HTML/FileHTML", value); } }
        public static string HtmlHeadText { get { return GetValue("/root/HTML/HTMLHEAD"); } set { SetValue("/root/HTML/HTMLHEAD", value); } }
        public static bool HtmlEnabled { get { return CheckBool(GetValue("/root/HTML/Enabled")); } set { SetValueBool("/root/HTML/Enabled", value); } }

        private static void SetValueBool(string nodePath, bool value)
        {
            if (value)
                SetValue(nodePath, "Y");
            else
                SetValue(nodePath, "N");
        }

        private static void SetValue(string nodePath, string Value)
        {
            LoadXML();

            xDoc.SelectSingleNode(nodePath).InnerText = Value;
            xDoc.Save(XmlFileName);

        }

        private const int minHTMLUpdateFrequency = 60;

        public static void SetHttpUpdateFrequency(int val)
        {
            if (val < minHTMLUpdateFrequency)
                throw new ArgumentOutOfRangeException("Fehler: die Update-Frequenz für HTML darf nicht unter " + minHTMLUpdateFrequency.ToString() + " liegen!");

            SetValue("/root/HTML/UpdateFrequency", val.ToString());
        }

        public static int HttpUpdateFrequency()
        {
            int frequency;
            string xmlValue = GetValue("/root/HTML/UpdateFrequency");
            if (int.TryParse(xmlValue, out frequency) && frequency > 0) //minimum every 1 seconds
            {
                return frequency;
            }
            else
            {
                throw new ArgumentOutOfRangeException("XML: Frequenz hat einen ungültigen Wert!");
            }
        }

        public static bool GetTopMost { get { return CheckBool(GetValue("/root/ProgrammEinstellungen/Topmost")); } set { SetValueBool("/root/ProgrammEinstellungen/Topmost", value); } }

        private static string GetValue(string nodePath)
        {

            LoadXML();

            XmlNode t = xDoc.SelectSingleNode(nodePath);
            if (t == null)
                return String.Empty;
            else
                return t.InnerText.ToString();
        }

        private static bool LoadXML()
        {

            xDoc.Load(XmlFileName);

            return true;
        }

        private static bool CheckBool(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return (value.ToUpper() == "Y");
        }

        public static void UpdateSensorValue(string SensorName, string node, bool value)
        {
            if (value)
                UpdateSensorValue(SensorName, node, "Y");
            else
                UpdateSensorValue(SensorName, node, "N");
        }

        public static void UpdateSensorValue(string SensorName, string node, string value)
        {
            LoadXML();
            XmlNode nodeXML = null;

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    if (GetInnerText(child, "Bezeichnung") == SensorName)
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

        private static XMLProtocol GetProtocol(string value)
        {
            switch (value.ToLower())
            {
                case "http": return XMLProtocol.HTTP;
                case "com": return XMLProtocol.COM;
                default: return XMLProtocol.NOT_DEFINED;
            }
        }

        public static XMLSensorObject XmlReadChildItem(XmlNode child)
        {
            XMLSensorObject tmpSensor = new XMLSensorObject();
            ArrayList items = new ArrayList();
            items.Clear();

            tmpSensor.Name = GetInnerText(child, "Bezeichnung");
            //Name = Unique Identifier, not allowed to exist more than one time
            if (items.IndexOf(tmpSensor.Name) >= 0)
                throw new Exception("Fehler XML: Die Bezeichnung muss für jeden Sensor eindeutig sein.\nDer Name '" + tmpSensor.Name + "' existiert bereits!");
            else
                items.Add(tmpSensor.Name);

            tmpSensor.Active = CheckBool(GetInnerText(child, "Aktiv"));

            tmpSensor.LogEnabled = CheckBool(GetInnerText(child, "LogEnabled"));

            tmpSensor.HTMLEnabled = CheckBool(GetInnerText(child, "WriteHTML"));

            tmpSensor.WriteToDatabase = CheckBool(GetInnerText(child, "WriteToDatabase"));

            tmpSensor.NumLogEntries = 50; //default
            int numEntries;
            if (int.TryParse(GetInnerText(child, "NumLogItems"), out numEntries))
            {
                if (numEntries >= 1 && numEntries <= 500000)
                    tmpSensor.NumLogEntries = numEntries;
                else
                    throw new Exception("XML Fehler: Die Anzahl der Num Einträge muss zwischen 1 und 500000 liegen!");
            }
            else
            {
                throw new Exception("XML Fehler: Die Anzahl der Num Einträge kann nicht verarbeitet werden!");
            }


            long maxLogFileSize;
            if (long.TryParse(GetInnerText(child, "maxLogFileSize"), out maxLogFileSize))
            {
                if (maxLogFileSize >= 1048576 && maxLogFileSize <= 1073741824)
                    tmpSensor.MaxLogFileSize = maxLogFileSize;
                else
                    throw new Exception("XML Fehler: Die maximale Größe des Logfiles muss zwischen 1.048.576 und 1.073.741.824 liegen!");
            }
            else
            {
                throw new Exception("XML Fehler: Die maximale Größe des Logfiles kann nicht verarbeitet werden!");
            }
            tmpSensor.MaxLogFileSize = maxLogFileSize;

            tmpSensor.DataInterfaceType = GetProtocol(GetInnerText(child, "Protocol"));

            if (tmpSensor.DataInterfaceType == XMLProtocol.COM)
            {
                tmpSensor.DtrEnabled = CheckBool(GetInnerText(child, "COM/DtrEnabled"));
                tmpSensor.Port = GetInnerText(child, "COM/Port");
                int Baudrate;
                if (int.TryParse(GetInnerText(child, "COM/Baudrate"), out Baudrate))
                {
                    if (Baudrate >= 8)
                        tmpSensor.Baudrate = Baudrate;
                    else
                        throw new Exception("XML Fehler: Baudrate hat einen Wert kleiner 8");
                }
                else
                {
                    tmpSensor.Baudrate = 115200;
                }
            }
            else if (tmpSensor.DataInterfaceType == XMLProtocol.HTTP)
            {
                tmpSensor.URL = GetInnerText(child, "HTTP/URL");
            }
            else
            {
                throw new Exception("XML Fehler: Das Protokoll muss entweder 'HTTP' oder 'COM' sein (Sensor '" + tmpSensor.Name + "').");
            }

            tmpSensor.LogFilePath = GetInnerText(child, "LogFile");

            return tmpSensor;
        }

        public static List<XMLSensorObject> GetSensorItemsFromXML()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();
            ArrayList items = new ArrayList();
            items.Clear();

            LoadXML();

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    
                    XMLSensorObject tmpSensor = XmlReadChildItem(child);
                    
                    lst.Add(tmpSensor);

                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("Read Sensor item ...");
                    Console.WriteLine("Sensor.Name          == {0}", tmpSensor.Name);
                    Console.WriteLine("Sensor.Active        == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("Sensor.Protocol      == {0}", Enum.GetName(typeof(XMLProtocol), tmpSensor.DataInterfaceType));
                    if (tmpSensor.DataInterfaceType == XMLProtocol.HTTP)
                    {
                        Console.WriteLine("Sensor.URL           == {0}", tmpSensor.URL);
                    } else if (tmpSensor.DataInterfaceType == XMLProtocol.COM)
                    {
                        Console.WriteLine("Sensor.Port          == {0}", tmpSensor.Port);
                        Console.WriteLine("Sensor.Baudrate      == {0}", tmpSensor.Baudrate.ToString());
                        Console.WriteLine("Sensor.DtrEnabled    == {0}", (tmpSensor.DtrEnabled) ? "Y" : "N");
                    }
                    Console.WriteLine("Sensor.writeToDB     == {0}", (tmpSensor.WriteToDatabase) ? "Y" : "N");
                    Console.WriteLine("Sensor.LogEnabled    == {0}", (tmpSensor.LogEnabled) ? "Y" : "N");
                    Console.WriteLine("Sensor.LogFilePath   == {0}", tmpSensor.LogFilePath);
                    Console.WriteLine("Sensor.HTMLEnabled   == {0}", (tmpSensor.HTMLEnabled) ? "Y" : "N");
                    Console.WriteLine("Sensor.numLogEntries == {0}", tmpSensor.NumLogEntries.ToString());
                    Console.WriteLine("Sensor item was read successfully!");
                    Console.WriteLine("******************************************************************");
                }
            }

            return lst;
        }
    }
}
