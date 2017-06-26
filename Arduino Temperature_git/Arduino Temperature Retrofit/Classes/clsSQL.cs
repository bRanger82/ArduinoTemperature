using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arduino_Temperature_Retrofit.Classes
{
    static class clsSQL
    {
        //will be returned in case of any (generic) error
        public const int SQL_EXIT_FAILURE = -1; 

        #region Declaration for accessing the database
        public static string user { get; set; }
        public static string password { get; set; }
        public static string server { get; set; }
        public static string scheme { get; set; }
        private static SqlConnection _connection;
        #endregion

        #region Create Database Connection
        public static SqlConnection createSQLConnection(string p_user, string p_password, string p_server, string p_scheme)
        {
            user = p_user;
            password = p_password;
            server = p_server;
            scheme = p_scheme;

            return createSQLConnection();
        }

        public static SqlConnection createSQLConnection()
        {
            string ConnectionString =
                    "Data Source=" + server + "; " +
                    "Initial Catalog=" + scheme + ";" +
                    "User id=" + user + ";" +
                    "Password=" + password + ";";

            bool isValid = Regex.IsMatch(ConnectionString, @"^([^=;]+=[^=;]*)(;[^=;]+=[^=;]*)*;?$");

            if (!isValid)
            {
                throw new ArgumentException("Connection String is not valid!");
            }

            _connection = new SqlConnection(ConnectionString);

            return _connection;
        }

        public static SqlConnection getSqlConnection { get { return _connection; } }
        #endregion

        #region Database operations: Insert Row
        public static int InsertRow(DataObject dObj)
        {
            if (null == _connection)
            {
                return SQL_EXIT_FAILURE;
            }

            if (_connection.State == ConnectionState.Open)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = _connection;
                command.CommandType = CommandType.Text;

                command.CommandText = "insert into Datalog (SensorID, SensorName, Temperature, HeatIndex, Humidity, Pressure, LUX, LogTime) VALUES (@id, @name, @temp, @head, @hum, @press, @lux, getdate())";
                //@id, @name, @temp, @head, @hum, @press, @lux, getdate())";
                command.Parameters.AddWithValue("@id", dObj.uniqueID);
                command.Parameters.AddWithValue("@name", dObj.Name);

                if (DataObjectCategory.HasTemperature(dObj.Protocol))
                    command.Parameters.AddWithValue("@temp", dObj.getItem(DataObjectCategory.Temperatur));
                else
                    command.Parameters.AddWithValue("@temp", "");

                if (DataObjectCategory.HasHeatIndex(dObj.Protocol))
                    command.Parameters.AddWithValue("@head", dObj.getItem(DataObjectCategory.HeatIndex));
                else
                    command.Parameters.AddWithValue("@head", "");

                if (DataObjectCategory.HasHumidity(dObj.Protocol))
                    command.Parameters.AddWithValue("@hum", dObj.getItem(DataObjectCategory.Luftfeuchtigkeit));
                else
                    command.Parameters.AddWithValue("@hum", "");

                if (DataObjectCategory.HasAirPressure(dObj.Protocol))
                    command.Parameters.AddWithValue("@press", dObj.getItem(DataObjectCategory.Luftdruck));
                else
                    command.Parameters.AddWithValue("@press", "");

                if (DataObjectCategory.HasLUX(dObj.Protocol))
                    command.Parameters.AddWithValue("@lux", dObj.getItem(DataObjectCategory.Lichtwert));
                else
                    command.Parameters.AddWithValue("@lux", "");

                int rowsAffacted = command.ExecuteNonQuery();

                return rowsAffacted;
            }
            else
            {
                return SQL_EXIT_FAILURE;
            }
        }

        public static void insertDBAll(Dictionary<string, DataObject> dataObjs)
        {
            
                foreach (KeyValuePair<string, DataObject> kvp in dataObjs)
                {
                    DataObject dObj = (DataObject)kvp.Value;
                    
                    if (null == dObj || !dObj.DataAvailable || !dObj.Active || !dObj.writeToDatabase)
                    {
                        continue;
                    }
                    if (dObj.DataInterfaceType == XMLProtocol.HTTP && dObj.HTTPException != null)
                    {
                        continue;
                    }

                    InsertRow(dObj);
                }
        }
        #endregion 

    }
}
