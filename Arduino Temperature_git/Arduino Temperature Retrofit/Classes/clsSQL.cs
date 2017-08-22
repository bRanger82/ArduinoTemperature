using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Arduino_Temperature_Retrofit
{
    public enum eSQLStatus
    {
        Running, 
        Idle, 
        Stopped, 
        NoSQLConnectionCreated,
        Error
    }

    class SQLStatusEventArgs : EventArgs
    {
        public eSQLStatus oldStatus { get; set; }
        public eSQLStatus newStatus { get; set; }
        public SQLStatusEventArgs(eSQLStatus oldState, eSQLStatus newState)
        {
            this.oldStatus = oldState;
            this.newStatus = newState;
        }
    }

    class clsSQL
    {
        
        //will be returned in case of any (generic) error
        public const int SQL_EXIT_FAILURE = -1; 

        #region Declaration for accessing the database
        public string user { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public string scheme { get; set; }
        public eSQLStatus Status { get; private set; }
        private SqlConnection _connection = null;
        private int _connectionTimeout = 30; // default
        #endregion


        public event EventHandler StatusChanged;

        private void StatusChange(eSQLStatus eStatus)
        {
            var handler = StatusChanged;
            if (handler != null)
            {
                handler(this, new SQLStatusEventArgs(Status, eStatus));
            }
            this.Status = eStatus;
        }

        public clsSQL()
        {
            Status = eSQLStatus.NoSQLConnectionCreated;
            StatusChange(eSQLStatus.NoSQLConnectionCreated);
        }

        #region Create Database Connection
        public SqlConnection createSQLConnection(string p_user, string p_password, string p_server, string p_scheme, int p_connectionTimeout)
        {
            user = p_user;
            password = p_password;
            server = p_server;
            scheme = p_scheme;
            _connectionTimeout = p_connectionTimeout;
            return createSQLConnection();
        }

        public SqlConnection createSQLConnection(int p_connectionTimeout)
        {
            _connectionTimeout = p_connectionTimeout;
            return createSQLConnection();
        }

        public SqlConnection createSQLConnection(string p_user, string p_password, string p_server, string p_scheme)
        {
            user = p_user;
            password = p_password;
            server = p_server;
            scheme = p_scheme;

            return createSQLConnection();
        }

        public SqlConnection createSQLConnection()
        {
            try
            {
                string ConnectionString =
                        "Data Source=" + server + "; " +
                        "Initial Catalog=" + scheme + ";" +
                        "User id=" + user + ";" +
                        "Password=" + password + ";" +
                        "Connection Timeout=" + _connectionTimeout.ToString() + ";";

                bool isValid = Regex.IsMatch(ConnectionString, @"^([^=;]+=[^=;]*)(;[^=;]+=[^=;]*)*;?$");

                if (!isValid)
                {
                    return null;
                }

                _connection = new SqlConnection(ConnectionString);

                StatusChange(eSQLStatus.Idle);
                return _connection;
            }
            catch (Exception)
            {
                StatusChange(eSQLStatus.Error);
                return null;
            }
        }

        public SqlConnection getSqlConnection { get { return _connection; } }
        #endregion

        #region Database operations: Insert Row
        public int InsertRow(DataObject dObj)
        {

            StatusChange(eSQLStatus.Running);

            if (null == _connection)
            {
                if (null == createSQLConnection())
                {
                    Status = eSQLStatus.Error;
                    return SQL_EXIT_FAILURE;
                }
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
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

                StatusChange(eSQLStatus.Idle);
                return rowsAffacted;
            }
            else
            {
                StatusChange(eSQLStatus.Error);
                return SQL_EXIT_FAILURE;
            }
        }

        public void insertDBAll(Dictionary<string, DataObject> dataObjs)
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

                if (Status == eSQLStatus.Error)
                {
                    return;
                }
            }
        }
        #endregion 

    }
}
