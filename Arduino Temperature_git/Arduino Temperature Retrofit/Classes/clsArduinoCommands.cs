using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature_Retrofit
{
    /// <summary>
    /// Commands which are implemented in the Arduino Sketch to communicate with this program
    /// </summary>
    public class ArduinoCmd
    {
        /// <summary>
        /// Start-Identifier of a dataset
        /// </summary>
        public static string Start { get { return "START"; } }

        /// <summary>
        /// End-Identifier of a dataset
        /// </summary>
        public static string End { get { return "EOF"; } }

        /// <summary>
        /// Start-Identifier of an answer (this program requested data, in this case instead of Start)
        /// </summary>
        public static string Reply { get { return "REPLY"; } }

        /// <summary>
        /// Dataset-Member which contains the number (of dataset-members) of the entire dataset
        /// </summary>
        public static string Length { get { return "LEN"; } }

        /*
            Difference between SeperateFieldChar and SeperateDataMemberChar:
            
            e.g. the arduino sends a dataset like this:
            START|VERSION:2|LEN:9|32.00|26.40|25.93|951.20|1034|EOF
                               ^       ^-- '|' is the SeperateDataMemberChar
                               +---------- ':' is the SeperateFieldChar
            
            SeperateFieldChar seperates a data-member of a dataset
            SeperateDataMemberChar seperates data within a data-member
        */

        /// <summary>
        /// The character which is used to seperate each data-member of an entire dataset
        /// </summary>
        public static char SeperateDataMemberChar { get { return '|'; } }

        /// <summary>
        /// The character which is used to seperate each data-item of a data-member
        /// </summary>
        public static char SeperateFieldChar { get { return ':'; } }

        /// <summary>
        /// Dataset-Member which contains the version information
        /// </summary>
        public static string VersionNo { get { return "VERSION"; } }

        /// <summary>
        /// Converts a Version Number to a DataObjectProtocol enum type
        /// </summary>
        /// <param name="VersionNo">Version-Number received from the Arduino</param>
        /// <returns>If parse was successful DataObjectProtocol type is returned, otherwise DataObjectProtocol.NONE is returned</returns>
        public static DataObjectProtocol ConvertToDOP(int VersionNo)
        {
            if (Enum.TryParse<DataObjectProtocol>(VersionNo.ToString(), out DataObjectProtocol dop))
            {
                return dop;
            }
            else
            {
                return DataObjectProtocol.NONE;
            }
        }
    }

    /// <summary>
    /// Data-members of a dataset for ARDUINO Protocol Version 1
    /// </summary>
    public class ARD_PROT_V1
    {
        /// <summary>
        /// Index for the Humidity value within a dataset
        /// </summary>
        public static int IDX_HUMIDITY { get { return 3; } }
        /// <summary>
        /// Index for the Temperature value within a dataset
        /// </summary>
        public static int IDX_TEMPERATURE { get { return 4; } }
        /// <summary>
        /// Index for the Head-Index value within a dataset
        /// </summary>
        public static int IDX_HEATINDEX { get { return 5; } }
        /// <summary>
        /// Index for the START identifier (Version 1)
        /// </summary>
        public virtual int IDX_START_POS { get { return 0; } }
    }

    /// <summary>
    /// Data-members of a dataset for ARDUINO Protocol Version 2
    /// </summary>
    public class ARD_PROT_V2 : ARD_PROT_V1
    {
        /// <summary>
        /// Index for the Air-Pressure value within a dataset
        /// </summary>
        public static int IDX_AIRPRESURE { get { return 6; } }
        /// <summary>
        /// Index for the START identifier (Version 2)
        /// </summary>
        public override int IDX_START_POS { get { return 0; } }
    }

    /// <summary>
    /// Data-members of a dataset for ARDUINO Protocol Version 3
    /// </summary>
    public class ARD_PROT_V3 : ARD_PROT_V2
    {
        /// <summary>
        /// Index for the Luminosity value within a dataset
        /// </summary>
        public static int IDX_LUMINOSITY { get { return 7; } }
        /// <summary>
        /// Index for the START identifier (Version 3)
        /// </summary>
        public override int IDX_START_POS { get { return 0; } }
    }

    /// <summary>
    /// This specify the version of the data, the Arduino is sending.
    /// Based on the version the dataset sent by the arduino contains different data-members.
    /// </summary>
    public enum DataObjectProtocol
    {
        /// <summary>
        /// Not defined/set or error parsing value
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Dataset contains temperature, heat-index and humidity
        /// </summary>
        PROTOCOL_V1 = 1,
        /// <summary>
        /// Dataset contains temperature, heat-index, air-pressure and humidity
        /// </summary>
        PROTOCOL_V2 = 2,
        /// <summary>
        /// Dataset contains temperature, heat-index, air-pressure, humidity and luminosity
        /// </summary>
        PROTOCOL_V3 = 3
    }
}
