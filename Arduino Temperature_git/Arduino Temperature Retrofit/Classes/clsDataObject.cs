using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature_Retrofit
{
    public enum DataObjectType
    {
        Temperatur = 0,
        HeatIndex = 1,
        Luftfeuchtigkeit = 2,
        Luftdruck = 3,
        Lichtwert = 4
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

    public enum DataObjectProtocol
    {
        NONE = 0,
        PROTOCOL_V1 = 1,  //Luftfeuchtigkeit, Heat Index, Temperatur
        PROTOCOL_V2 = 2,  //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck
        PROTOCOL_V3 = 3   //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck, Lichtstaerke
    }

    public class DataObjectCategory
    {
        public static bool HasHumidity(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_V1 || dop == DataObjectProtocol.PROTOCOL_V2 || dop == DataObjectProtocol.PROTOCOL_V3);
        }

        public static bool HasHeatIndex(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_V1 || dop == DataObjectProtocol.PROTOCOL_V2 || dop == DataObjectProtocol.PROTOCOL_V3);
        }

        public static bool HasTemperature(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_V1 || dop == DataObjectProtocol.PROTOCOL_V2 || dop == DataObjectProtocol.PROTOCOL_V3);
        }

        public static bool HasAirPressure(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_V2 || dop == DataObjectProtocol.PROTOCOL_V3);
        }

        public static bool HasLUX(DataObjectProtocol dop)
        {
            return (dop == DataObjectProtocol.PROTOCOL_V3);
        }

        public static bool HasCapability(string dobjCat, DataObjectProtocol dop)
        {

            if (dobjCat == DataObjectCategory.Luftdruck.Value)
            {
                return HasAirPressure(dop);
            }
            else if (dobjCat == DataObjectCategory.HeatIndex.Value)
            {
                return HasHeatIndex(dop);
            }
            else if (dobjCat == DataObjectCategory.Luftfeuchtigkeit.Value)
            {
                return HasHumidity(dop);
            }
            else if (dobjCat == DataObjectCategory.Lichtwert.Value)
            {
                return HasLUX(dop);
            }
            else if (dobjCat == DataObjectCategory.Temperatur.Value)
            {
                return HasTemperature(dop);
            }
            else
            {
                return false;
            }
        }

        public static List<DataObjectCategory> GetAvailableProtocols(DataObject dobj)
        {
            List<DataObjectCategory> lstProt = new List<DataObjectCategory>();

            if (HasAirPressure(dobj.Protocol))
                lstProt.Add(DataObjectCategory.Luftdruck);
            if (HasLUX(dobj.Protocol))
                lstProt.Add(DataObjectCategory.Lichtwert);
            if (HasHeatIndex(dobj.Protocol))
                lstProt.Add(DataObjectCategory.HeatIndex);
            if (HasTemperature(dobj.Protocol))
                lstProt.Add(DataObjectCategory.Temperatur);
            if (HasHumidity(dobj.Protocol))
                lstProt.Add(DataObjectCategory.Luftfeuchtigkeit);

            return lstProt;
        }

        public static bool HasCapability(DataObjectCategory dobjCat, DataObjectProtocol dop)
        {
            if (dobjCat.Value == DataObjectCategory.Luftdruck.Value)
            {
                return HasAirPressure(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.HeatIndex.Value)
            {
                return HasHeatIndex(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.Luftfeuchtigkeit.Value)
            {
                return HasHumidity(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.Lichtwert.Value)
            {
                return HasLUX(dop);
            }
            else if (dobjCat.Value == DataObjectCategory.Temperatur.Value)
            {
                return HasTemperature(dop);
            }
            else
            {
                return false;
            }
        }

        private DataObjectCategory(string value) { Value = value; }

        public string Value { get; set; }

        static public string GetSensorValueUnit(string DataObjectCat, bool leadingSpace = true)
        {
            string ret = (leadingSpace) ? " " : "";

            if (DataObjectCat == DataObjectCategory.Luftdruck.Value)
                return ret + "mb";
            else if (DataObjectCat == DataObjectCategory.Temperatur.Value)
                return ret + "°C";
            else if (DataObjectCat == DataObjectCategory.HeatIndex.Value)
                return ret + "°C";
            else if (DataObjectCat == DataObjectCategory.Luftfeuchtigkeit.Value)
                return ret + "%";
            else if (DataObjectCat == DataObjectCategory.Lichtwert.Value)
                return ret + "lux";
            else
                return "N/A";
        }

        static public string GetSensorValueUnit(DataObjectCategory typ, bool leadingSpace = true)
        {
            string ret = (leadingSpace) ? " " : "";

            if (typ.Value == DataObjectCategory.Luftdruck.Value)
                return ret + "mb";
            else if (typ.Value == DataObjectCategory.Temperatur.Value)
                return ret + "°C";
            else if (typ.Value == DataObjectCategory.HeatIndex.Value)
                return ret + "°C";
            else if (typ.Value == DataObjectCategory.Luftfeuchtigkeit.Value)
                return ret + "%";
            else if (typ.Value == DataObjectCategory.Lichtwert.Value)
                return ret + "lux";
            else
                return "N/A";
        }

        
        public static List<string> Items = new List<string>(Enum.GetNames(typeof(DataObjectType)));

        public static List <string> GetCapableItems(DataObjectProtocol dobj)
        {
            List<string> ret = new List<string>();
            foreach(string s in Items)
            {
                if (HasCapability(s, dobj))
                    ret.Add(s);
            }
            return ret;
        }

        public static DataObjectCategory GetObjectCategory(DataObjectType item)
        {
            return new DataObjectCategory(Items[(int)item]);
        }

        public static DataObjectCategory GetObjectCategory(string item)
        {
            if (!Items.Contains(item))
                return null;

            return new DataObjectCategory(Items[Items.IndexOf(item)]);
        }

        public static DataObjectCategory Temperatur { get { return new DataObjectCategory(Items[(int)DataObjectType.Temperatur]); } }
        public static DataObjectCategory HeatIndex { get { return new DataObjectCategory(Items[(int)DataObjectType.HeatIndex]); } }
        public static DataObjectCategory Luftfeuchtigkeit { get { return new DataObjectCategory(Items[(int)DataObjectType.Luftfeuchtigkeit]); } }
        public static DataObjectCategory Luftdruck { get { return new DataObjectCategory(Items[(int)DataObjectType.Luftdruck]); } }
        public static DataObjectCategory Lichtwert { get { return new DataObjectCategory(Items[(int)DataObjectType.Lichtwert]); } }
    }

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


    public class DetailsTimePoint
    {
        public double MinValue { get; set; } = double.MaxValue;
        public DateTime MinTimepoint { get; set; } = DateTime.Now;
        public double MaxValue { get; set; } = double.MinValue;
        public DateTime MaxTimepoint { get; set; } = DateTime.Now;
        public double Value { get; set; } = 0;
        public DataObjectCategory DataObjCategory { get; set; }
    }

    public enum Trend
    {
        UP,
        CONSTANT,
        DOWN
    }

    public class DataObject : SerialPort
    {
        private Dictionary<string, DetailsTimePoint> _Items = new Dictionary<string, DetailsTimePoint>();
        public string Name { get; set; }
        public Dictionary<string, DetailsTimePoint> Items { get { return _Items; } }
        public bool DataAvailable { get; set; }
        public bool Active { get; set; }
        public DataObjectType Type { get; set; }
        public DataObjectProtocol Protocol { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime LastUpdated { get; set; }
        public XMLProtocol DataInterfaceType { get; set; }
        public string URL { get; set; }
        public bool WriteToDatabase { get; set; }
        public int UniqueID { get; set; }
        public Exception HTTPException { get; set; }

        public string GetLastUpdatedFormatted()
        {
            return this.LastUpdated.ToString(Common.DateTimeFormat);
        }

        private int _connectionRetries = 0;

        public int ConnectionRetries { get { return _connectionRetries; } }

        public void IncreaseConnectionRetry()
        {
            _connectionRetries++;
        }

        private bool _firstData = false;
        public bool FirstData { get { return _firstData; } set { _firstData = value; } }

        private string _statusText = string.Empty;
        public string StatusText { get { return _statusText; } set { _statusText = value; } }

        public static int HistoryMinDefaultEntries = 30;
        public static int HistoryMaxDefaultEntries = 3000;

        private int _maxHistoryItemsCount = HistoryMinDefaultEntries;

        public long MaxLogFileSize { get; set; } = 4194304;

        public bool HTMLEnabled { get; set; } = false;

        public bool LoggingEnabled { get; set; } = false;

        public int MaxHistoryItemsSet
        {
            get
            {
                return _maxHistoryItemsCount;
            }
            set
            {
                if (value < HistoryMinDefaultEntries || value > HistoryMaxDefaultEntries)
                {
                    return;
                }
                _maxHistoryItemsCount = value;
            }
        }

        private List<LogObject> _HistoryData = new List<LogObject>();

        public List<string> GetLogTimings()
        {
            List<string> lstDt = new List<string>();
            foreach(LogObject logObj in _HistoryData)
            {
                if (!lstDt.Contains(logObj.Timepoint.ToString("dd.MM.yyyy HH:mm:ss")))
                    lstDt.Add(logObj.Timepoint.ToString("dd.MM.yyyy HH:mm:ss"));
            }
            return lstDt;
        }

        public double GetLogItem(string timepoint, string DataObjectCat)
        {
            Dictionary<DataObjectCategory, LogItem> items = new Dictionary<DataObjectCategory, LogItem>();
            foreach (LogObject logObj in _HistoryData)
            {
                if (logObj.Timepoint.ToString("dd.MM.yyyy HH:mm:ss") == timepoint && logObj.Category.Value == DataObjectCat)
                {
                    return logObj.Value;
                }
            }
            throw new MissingFieldException(string.Format("Es wurde kein Wert zum Zeitpunkt '{0}' und zur Kategorie '{1}' gefunden.", timepoint, DataObjectCat));
        }

        public double GetLogItem(string timepoint, DataObjectCategory Category)
        {
            Dictionary<DataObjectCategory, LogItem> items = new Dictionary<DataObjectCategory, LogItem>();
            foreach (LogObject logObj in _HistoryData)
            {
                if (logObj.Timepoint.ToString("dd.MM.yyyy HH:mm:ss") == timepoint && logObj.Category.Value == Category.Value)
                {
                    return logObj.Value;
                }
            }
            throw new MissingFieldException(string.Format("Es wurde kein Wert zum Zeitpunkt {0} und zur Kategorie {1} gefunden.", timepoint, Category.Value));
        }

        public Dictionary<DataObjectCategory, LogItem> GetLogItems(string timepoint)
        {
            Dictionary<DataObjectCategory, LogItem> items = new Dictionary<DataObjectCategory, LogItem>();
            foreach(LogObject logObj in _HistoryData)
            {
                if (logObj.Timepoint.ToString("dd.MM.yyyy HH:mm:ss") == timepoint)
                {
                    items.Add(logObj.Category, new LogItem(logObj.Value, logObj.Timepoint, logObj.Category));
                }
            }
            return items;
        }

        public List<LogItem> GetLogItems()
        {
            List<LogItem> lst = new List<LogItem>();
            foreach (LogObject logObj in _HistoryData)
            {
                lst.Add(new LogItem(logObj.Value, logObj.Timepoint, logObj.Category));
            }
            return lst;
        }

        public List<LogItem> GetLogItems(DataObjectCategory dobj)
        {
            List<LogItem> lst = new List<LogItem>();

            foreach (LogObject logObj in _HistoryData)
            {
                if (logObj.Category.Value == dobj.Value)
                {
                    lst.Add(new LogItem(logObj.Value, logObj.Timepoint, logObj.Category));
                }
            }
            
            return lst;
        }

        public Trend GetTrend(DataObjectCategory dobjCat)
        {
            List<double> lst = new List<double>();

            foreach (LogObject logObj in _HistoryData)
            {
                if (logObj.Category.Value == dobjCat.Value)
                {
                    lst.Add(logObj.Value);
                }
            }

            //Trend aus den letzten 'numEntriesConsider' Einträgen berechnen
            double calcTrend = Common.CalculateTrend(lst);
            //double stdAbw = Common.CalculateStdDev(lst);

            Console.WriteLine("getTrend::method calcTrend returned {0} for {1}", calcTrend, dobjCat.Value);

            if (calcTrend < 0)
            {
                return Trend.DOWN;
            } else if (calcTrend > 0)
            {
                return Trend.UP;
            } else
            {
                return Trend.CONSTANT;
            }

        }

        public double GetHistoryItemMinValue(DataObjectCategory dObjcat)
        {
            if (!_Items.ContainsKey(dObjcat.Value))
                return 0;
            return _Items[dObjcat.Value].MinValue;
        }

        public double GetHistoryItemMaxValue(DataObjectCategory dObjcat)
        {
            if (!_Items.ContainsKey(dObjcat.Value))
                return 0;
            return _Items[dObjcat.Value].MaxValue;
        }

        public int GetHistoryItemCount(DataObjectCategory dObjCat)
        {
            if (_HistoryData.Count < 1)
                return 0;

            return _HistoryData.Count(i => i.Category.Value == dObjCat.Value);
        }

        public void AddItemToHistory(LogObject logObj)
        {
            // >= because if max is e.g. 200, the 200th element will be removed and a new value will be added
            // otherwise there are 200 entries and one entry is added afterwards -> 201 entries which does not meet the maximum of 200
            while (GetHistoryItemCount(logObj.Category) >= _maxHistoryItemsCount)
            {
                _HistoryData.RemoveAt(0);
            }
            _HistoryData.Add(logObj); // one entry is added
        }

        private bool _IsDataUpToDate = true;
        public bool IsDataUpToDate { get { return _IsDataUpToDate; } set { _IsDataUpToDate = value; } }

        private int _numConnRetries = 0;
        public int NumConnRetries { get { return _numConnRetries; } set { _numConnRetries = value; } }

        private string _LogPath = string.Empty;
        public string LogPath { get { return _LogPath; } set { _LogPath = value; } }

        public bool ItemExists(DataObjectCategory dobjCat)
        {
            return _Items.ContainsKey(dobjCat.Value);
        }

        public bool EnableAddDataToHistory { get; set; } = true;

        public double GetItem(string dobjCat)
        {
            if (!_Items.ContainsKey(dobjCat))
                return double.MinValue;

            return _Items[dobjCat].Value;
        }

        public double GetItem(DataObjectCategory dobjCat)
        {
            if (!ItemExists(dobjCat))
                return double.MinValue;

            return _Items[dobjCat.Value].Value;
        }

        public void AddDataItem(string name, double value, DataObjectCategory dObjCat)
        {
            DateTime timepoint = DateTime.Now;

            if (!_Items.ContainsKey(name))
            {
                DetailsTimePoint dtp = new DetailsTimePoint
                {
                    Value = value,
                    MinValue = value,
                    MaxValue = value,
                    MinTimepoint = timepoint,
                    MaxTimepoint = timepoint,
                    DataObjCategory = dObjCat
                };
                _Items.Add(name, dtp);
            }
            else
            {
                _Items[name].Value = value;
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

            if (EnableAddDataToHistory)
                AddItemToHistory(new LogObject(value, dObjCat, timepoint));

        }
    }

    public class LogItem
    {
        public double Value { get; set; }
        public DateTime Timepoint { get; set; }
        public DataObjectCategory DataObjectCat { get; set; }

        public LogItem(double Value, DateTime Timepoint, DataObjectCategory dobjCat)
        {
            this.Value = Value;
            this.Timepoint = Timepoint;
            this.DataObjectCat = dobjCat;
        }
    }
}





