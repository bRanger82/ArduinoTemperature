using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{
    public class DataObject
    {
        public string Temperature { get; set; }
        public string HeatIndex { get; set; }
        public string Humidity { get; set; }
        public DateTime Timepoint { get; set; }
        public string AirPressure { get; set; }
        public bool DataAvailable { get; set; }
        public string AdditionalInformation { get; set; }
        public DataObjectProtocol Protocol { get; set; }
    }

    public enum DataObjectProtocol
    {
        NONE = 0,
        PROTOCOL_ONE = 1, //Luftfeuchtigkeit, Heat Index, Temperatur
        PROTOCOL_TWO = 2  //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck
    }
}
