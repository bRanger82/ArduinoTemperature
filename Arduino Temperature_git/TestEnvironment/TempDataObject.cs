using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment
{
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
