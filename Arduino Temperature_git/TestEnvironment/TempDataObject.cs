using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public XMLProtocol Protocol { get; set; }
        public string URL { get; set; }
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

        public static int HttpUpdateFrequency()
        {
            string xmlValue = GetValue("/root/HTML/UpdateFrequency");
            if (int.TryParse(xmlValue, out int frequency) && frequency >= 60) //minimum every 60 seconds
            {
                return frequency;
            }
            else
            {
                return (int)60;
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

        private static XMLProtocol GetProtocol(string value)
        {
            switch(value.ToLower())
            {
                case "http": return XMLProtocol.HTTP; 
                case "com": return XMLProtocol.COM; 
                default: return XMLProtocol.NOT_DEFINED;
            }
        }

        private static bool CheckBool(string value)
        {
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

            

            tmpSensor.NumLogEntries = 50; //default
            if (int.TryParse(GetInnerText(child, "NumLogItems"), out int numEntries))
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



            if (long.TryParse(GetInnerText(child, "maxLogFileSize"), out long maxLogFileSize))
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

            tmpSensor.Protocol = GetProtocol(GetInnerText(child, "Protocol"));
            if (tmpSensor.Protocol == XMLProtocol.COM)
            {
                tmpSensor.DtrEnabled = CheckBool(GetInnerText(child, "COM/DtrEnabled"));
                tmpSensor.Port = GetInnerText(child, "COM/Port");
                if (int.TryParse(GetInnerText(child, "COM/Baudrate"), out int Baudrate))
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
            } else if (tmpSensor.Protocol == XMLProtocol.HTTP)
            {
                tmpSensor.URL = GetInnerText(child, "HTTP/URL");
            } else
            {
                throw new Exception("XML Fehler: Das Protokoll muss entweder 'HTTP' oder 'COM' sein (Sensor '" + tmpSensor.Name + "').");
            }

            tmpSensor.LogFilePath = GetInnerText(child, "LogFile");

            return tmpSensor;
        }

        public static List<XMLSensorObject> GetSensorItemsFromXML()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();
            

            LoadXML();

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {

                foreach (XmlNode child in xmln.ChildNodes)
                {
                    XMLSensorObject tmpSensor = XmlReadChildItem(child);
                    
                    lst.Add(tmpSensor);

                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("tmpSensor.Name          == {0}", tmpSensor.Name);
                    Console.WriteLine("tmpSensor.Active        == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.Protocol      == {0}", Enum.GetName(typeof(XMLProtocol), tmpSensor.Protocol));
                    if (tmpSensor.Protocol == XMLProtocol.HTTP)
                    {
                        Console.WriteLine("tmpSensor.URL           == {0}", tmpSensor.URL);
                    } else if (tmpSensor.Protocol == XMLProtocol.COM)
                    {
                        Console.WriteLine("tmpSensor.Port          == {0}", tmpSensor.Port);
                        Console.WriteLine("tmpSensor.Baudrate      == {0}", tmpSensor.Baudrate.ToString());
                        Console.WriteLine("tmpSensor.DtrEnabled    == {0}", (tmpSensor.DtrEnabled) ? "Y" : "N");
                    }
                    Console.WriteLine("tmpSensor.LogEnabled    == {0}", (tmpSensor.LogEnabled) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.LogFilePath   == {0}", tmpSensor.LogFilePath);
                    Console.WriteLine("tmpSensor.HTMLEnabled   == {0}", (tmpSensor.HTMLEnabled) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.numLogEntries == {0}", tmpSensor.NumLogEntries.ToString());
                    Console.WriteLine("******************************************************************");
                }
            }

            return lst;
        }
    }

    public class DataObjectExt
    {
        public Dictionary<string, double> SensorData = new Dictionary<string, double>();
    }

    public class DataObject
    {
        private DetailsTimePoint _TemperatureDetail = new DetailsTimePoint();
        public DetailsTimePoint TemperatureDetail { get { return _TemperatureDetail; } set { _TemperatureDetail = value; } } 
        public string Temperature { get; set; }
        public DetailsTimePoint HeatIndexDetail { get; set; }
        public string HeatIndex { get; set; }
        public DetailsTimePoint HumidityDetail { get; set; }
        public string Humidity { get; set; }
        public DetailsTimePoint LUXDetail { get; set; }
        public string LUX { get; set; }
        public DateTime Timepoint { get; set; }
        public DetailsTimePoint AirPressureDetail { get; set; }
        public string AirPressure { get; set; }
        public bool DataAvailable { get; set; }
        public string AdditionalInformation { get; set; }
        public DataObjectProtocol Protocol { get; set; }
    }

    public class DetailsTimePoint
    {
        public double MinValue { get; set; } = double.MaxValue;
        public DateTime MinTimepoint { get; set; } = DateTime.Now;
        public double MaxValue { get; set; } = double.MinValue;
        public DateTime MaxTimepoint { get; set; } = DateTime.Now;
    }

    public enum DataObjectProtocol
    {
        NONE = 0,
        PROTOCOL_ONE = 1,  //Luftfeuchtigkeit, Heat Index, Temperatur
        PROTOCOL_TWO = 2,  //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck
        PROTOCOL_THREE = 3 //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck, Lichtstaerke
    }
}
