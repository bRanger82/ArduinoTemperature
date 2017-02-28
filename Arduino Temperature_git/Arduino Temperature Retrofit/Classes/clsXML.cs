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

        public static List<XMLSensorObject> test()
        {
            List<XMLSensorObject> lst = new List<XMLSensorObject>();

            xDoc.Load(XmlFileName);

            foreach (XmlNode xmln in xDoc.SelectNodes("/root/Sensoren"))
            {
                
                foreach (XmlNode child in xmln.ChildNodes)
                {
                    XMLSensorObject tmpSensor = new XMLSensorObject();
                    if (getInnerText(child, "Aktiv").ToUpper() == "Y")
                        tmpSensor.Active = true;
                    else
                        tmpSensor.Active = false;

                    tmpSensor.Name = getInnerText(child, "Bezeichnung");
                    tmpSensor.Port = getInnerText(child, "Port");
                    tmpSensor.LogFilePath = getInnerText(child, "LogFile");

                    lst.Add(tmpSensor);

                    Console.WriteLine("************************************************************");
                    Console.WriteLine("tmpSensor.Active      == {0}", (tmpSensor.Active) ? "Y" : "N");
                    Console.WriteLine("tmpSensor.Name        == {0}", tmpSensor.Name);
                    Console.WriteLine("tmpSensor.Port        == {0}", tmpSensor.Port);
                    Console.WriteLine("tmpSensor.LogFilePath == {0}", tmpSensor.LogFilePath);
                    Console.WriteLine("************************************************************");
                }
            }

            return lst;
        }
    }
}
