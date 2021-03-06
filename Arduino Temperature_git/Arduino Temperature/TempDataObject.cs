﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{

    public class LogObject
    {

        public LogObject(double value, DataObjectCategory category, DateTime timepoint)
        {
            this.Value = value;
            this.Category = category;
            this.Timepoint = timepoint;
        }
        
        public double Value { get; set; }
        public DataObjectCategory Category { get; set; }
        public DateTime Timepoint { get; set; }
    }

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

        public enum ItemList
        {
            Temperature = 0,
            HeatIndex = 1,
            Humidity = 2,
            AirPressure = 3,
            LUX = 4
        }

        public static List<string> Items = new List<string>(new string[] { "Temperature", "HeatIndex", "Humidity", "AirPressure", "LUX" });

        public static DataObjectCategory GetObjectCategory(ItemList item)
        {
            return new DataObjectCategory(Items[(int)item]);
        }

        public static DataObjectCategory GetObjectCategory(string item)
        {
            if (!Items.Contains(item))
                return null;

            return new DataObjectCategory(Items[Items.IndexOf(item)]);
        }

        public static DataObjectCategory Temperature { get { return new DataObjectCategory(Items[(int)ItemList.Temperature]); } }
        public static DataObjectCategory HeatIndex { get { return new DataObjectCategory(Items[(int)ItemList.HeatIndex]); } }
        public static DataObjectCategory Humidity { get { return new DataObjectCategory(Items[(int)ItemList.Humidity]); } }
        public static DataObjectCategory AirPressure { get { return new DataObjectCategory(Items[(int)ItemList.AirPressure]); } }
        public static DataObjectCategory LUX { get { return new DataObjectCategory(Items[(int)ItemList.LUX]); } }
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

        private string _statusText = string.Empty;
        public string StatusText { get { return _statusText; } set { _statusText = value; } }

        public static int LogMinEntries = 600;
        public static int LogMaxEntries = 3000;

        private int _maxLogItemsCount = LogMinEntries;

        public int MaxLogItemsCount { get {return _maxLogItemsCount; } set { if (value < LogMinEntries || value > LogMaxEntries) return;  _maxLogItemsCount = value; } }

        private List<LogObject> _LogData = new List<LogObject>();

        public List<double> GetLogItems(DataObjectCategory dObjcat)
        {
            List<double> lst = new List<double>();

            foreach(LogObject logObj in _LogData)
            {
                if (logObj.Category.Value == dObjcat.Value)
                {
                    lst.Add(logObj.Value);
                }
            }
            
            return lst;
        }

        public double GetLogItemMinValue(DataObjectCategory dObjcat)
        {
            if (!_Items.ContainsKey(dObjcat.Value))
                return 0;
            return _Items[dObjcat.Value].MinValue;
        }

        public double GetLogItemMaxValue(DataObjectCategory dObjcat)
        {
            if (!_Items.ContainsKey(dObjcat.Value))
                return 0;
            return _Items[dObjcat.Value].MaxValue;
        }

        public int GetLogItemCount(DataObjectCategory dObjCat)
        {
            if (_LogData.Count < 1)
                return 0;

            return _LogData.Count(i => i.Category.Value == dObjCat.Value);
        }

        public void AddItemToLog(LogObject logObj)
        {
            while(GetLogItemCount(logObj.Category) > _maxLogItemsCount)
            {
                _LogData.RemoveAt(0);
            }
            _LogData.Add(logObj);
        }

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

        public bool LogEnabled { get; set; } = true;

        public double GetItem(DataObjectCategory dobjCat)
        {
            if (!ItemExists(dobjCat))
                return double.MinValue;

            return _Items[dobjCat.Value].Value;
        }

        public void AddDataItem(string name, double value, DataObjectCategory dObjCat, Common.SensorValueType SensorType)
        {
            DateTime timepoint = DateTime.Now;

            if (!_Items.ContainsKey(name))
            {
                DetailsTimePointExt dtp = new DetailsTimePointExt
                {
                    Value = value,
                    MinValue = value,
                    MaxValue = value,
                    MinTimepoint = timepoint,
                    MaxTimepoint = timepoint,
                    SensorType = SensorType,
                    DataObjCategory = dObjCat
                };
                _Items.Add(name, dtp);
            } else
            {
                _Items[name].Value = value;
                _Items[name].SensorType = SensorType;
                _Items[name].DataObjCategory = dObjCat;
                if (_Items[name].MinValue > value)
                {
                    _Items[name].MinValue = value;
                    _Items[name].MinTimepoint = timepoint;
                }
                if (_Items[name].MaxValue < value)
                {
                    _Items[name].MaxValue = value;
                    _Items[name].MaxTimepoint = timepoint;
                }
            }

            if (LogEnabled)
                AddItemToLog(new LogObject(value, dObjCat, timepoint));

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
