using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{
    public class DetailsTimePointExt
    {
        public double MinValue { get; set; } = double.MaxValue;
        public DateTime MinTimepoint { get; set; } = DateTime.Now;
        public double MaxValue { get; set; } = double.MinValue;
        public DateTime MaxTimepoint { get; set; } = DateTime.Now;
        public double Value { get; set; } = 0;
        public DataObjectCategory DataObjCategory { get; set; }
        public Common.SensorValueType SensorType { get; set; }
    }

    public class DataObjectCategory
    {
        private DataObjectCategory(string value) { Value = value; }

        public string Value { get; set; }

        public static DataObjectCategory Temperature { get { return new DataObjectCategory("Temperature"); } }
        public static DataObjectCategory HeatIndex { get { return new DataObjectCategory("HeatIndex"); } }
        public static DataObjectCategory Humidity { get { return new DataObjectCategory("Humidity"); } }
        public static DataObjectCategory AirPressure { get { return new DataObjectCategory("AirPressure"); } }
        public static DataObjectCategory LUX { get { return new DataObjectCategory("LUX"); } }
    }

    public class DataObjectExt : SerialPort
    {
        private Dictionary<string, DetailsTimePointExt> _Items = new Dictionary<string, DetailsTimePointExt>();
        public string Name { get; set; }
        public Dictionary<string, DetailsTimePointExt> Items { get { return _Items; } }
        public bool DataAvailable { get; set; }
        public bool Active { get; set; }
        public DataObjectType Type { get; set; }
        public DataObjectProtocol Protocol { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime LastUpdated { get; set; }
        

        private bool _IsDataUpToDate = true;
        public bool IsDataUpToDate { get { return _IsDataUpToDate;  } set { _IsDataUpToDate = value; } }

        private int _numConnRetries = 0;
        public int NumConnRetries { get { return _numConnRetries; } set { _numConnRetries = value; } }

        private string _LogPath = string.Empty;
        public string LogPath { get { return _LogPath; } set { _LogPath = value; } }

        public bool ItemExists(DataObjectCategory dobjCat)
        {
            return _Items.ContainsKey(dobjCat.Value);
        }

        public double getItem(DataObjectCategory dobjCat)
        {
            if (!ItemExists(dobjCat))
                return double.MinValue;

            return _Items[dobjCat.Value].Value;
        }

        public void addDataItem(string name, double value, DataObjectCategory dObjCat, Common.SensorValueType SensorType)
        {
            if (!_Items.ContainsKey(name))
            {
                DetailsTimePointExt dtp = new DetailsTimePointExt();
                dtp.Value = value;
                dtp.MinValue = value;
                dtp.MaxValue = value;
                dtp.MinTimepoint = DateTime.Now;
                dtp.MaxTimepoint = DateTime.Now;
                dtp.SensorType = SensorType;
                dtp.DataObjCategory = dObjCat;
                _Items.Add(name, dtp);
            } else
            {
                _Items[name].Value = value;
                _Items[name].SensorType = SensorType;
                _Items[name].DataObjCategory = dObjCat;
                if (_Items[name].MinValue > value)
                {
                    _Items[name].MinValue = value;
                    _Items[name].MinTimepoint = DateTime.Now;
                }
                if (_Items[name].MaxValue < value)
                {
                    _Items[name].MaxValue = value;
                    _Items[name].MaxTimepoint = DateTime.Now;
                }
            }
        }
    }
    
    public enum DataObjectType
    {
        Temperature, 
        HeatIndex, 
        Humidity,
        AirPressure,
        LUX, 
        Undefined
    }

    public class DataObject
    {
        public Dictionary<string, double> SensorData = new Dictionary<string, double>();
        public string Temperature { get; set; }
        public string HeatIndex { get; set; }
        public string Humidity { get; set; }
        public string LUX { get; set; }
        public DateTime Timepoint { get; set; }
        public string AirPressure { get; set; }
        public bool DataAvailable { get; set; }
        public string AdditionalInformation { get; set; }
        public DataObjectProtocol Protocol { get; set; }
    }

    public class DataObjectDetails
    {
        private DetailsTimePoint _TemperatureDetail = new DetailsTimePoint();
        public DetailsTimePoint TemperatureDetail { get { return _TemperatureDetail; } set { _TemperatureDetail = value; } }
        private DetailsTimePoint _HeatIndexDetail = new DetailsTimePoint();
        public DetailsTimePoint HeatIndexDetail { get { return _HeatIndexDetail; } set { _HeatIndexDetail = value; } }
        private DetailsTimePoint _HumidityDetail = new DetailsTimePoint();
        public DetailsTimePoint HumidityDetail { get { return _HumidityDetail; } set { _HumidityDetail = value; } }
        private DetailsTimePoint _AirPressureDetail = new DetailsTimePoint();
        public DetailsTimePoint AirPressureDetail { get { return _AirPressureDetail; } set { _AirPressureDetail = value; } }
        private DetailsTimePoint _LUXDetail = new DetailsTimePoint();
        public DetailsTimePoint LUXDetail { get { return _LUXDetail; } set { _LUXDetail = value; } }
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

    public static class DataObjectCapabilities
    {
        public static bool HasHumidity(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_ONE || dop == DataObjectProtocol.PROTOCOL_TWO || dop == DataObjectProtocol.PROTOCOL_THREE);
        }

        public static bool HasHeatIndex(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_ONE || dop == DataObjectProtocol.PROTOCOL_TWO || dop == DataObjectProtocol.PROTOCOL_THREE);
        }

        public static bool HasTemperature(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_ONE || dop == DataObjectProtocol.PROTOCOL_TWO || dop == DataObjectProtocol.PROTOCOL_THREE);
        }

        public static bool HasAirPressure(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_TWO || dop == DataObjectProtocol.PROTOCOL_THREE);
        }

        public static bool HasLUX(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_THREE);
        }

        public static bool HasCapability(DataObjectCategory dobjCat, DataObjectProtocol dop)
        {
            
            if (dobjCat.Value == DataObjectCategory.AirPressure.Value)
            {
                return HasAirPressure(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.HeatIndex.Value)
            {
                return HasHeatIndex(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.Humidity.Value)
            {
                return HasHumidity(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.LUX.Value)
            {
                return HasLUX(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.Temperature.Value)
            {
                return HasTemperature(dop);
            }
            else
            {
                return false;
            }
        }
    }
}
