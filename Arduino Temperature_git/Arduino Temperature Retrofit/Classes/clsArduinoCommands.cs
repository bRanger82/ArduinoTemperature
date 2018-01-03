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
        /// Start-Identifier of a dataset (sent by Arduino)
        /// </summary>
        public static string Start { get { return "START"; } }

        /// <summary>
        /// End-Identifier of a message
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

        /// <summary>
        /// Dataset-Member which contains the version information
        /// </summary>
        public static string VersionNo { get { return "VERSION"; } }

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

    public enum DataObjectProtocol
    {
        NONE = 0,
        PROTOCOL_V1 = 1,  //Luftfeuchtigkeit, Heat Index, Temperatur
        PROTOCOL_V2 = 2,  //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck
        PROTOCOL_V3 = 3   //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck, Lichtstaerke
    }
}
