using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{
    public class DataObject
    {
        public DetailsTimePoint TemperatureDetail { get; set; } 
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
