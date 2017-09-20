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
        private eSQLStatus oldStatus;

        public eSQLStatus GetOldStatus()
        {
            return oldStatus;
        }

        public void SetOldStatus(eSQLStatus value)
        {
            oldStatus = value;
        }

        private eSQLStatus newStatus;

        public eSQLStatus GetNewStatus()
        {
            return newStatus;
        }

        public void SetNewStatus(eSQLStatus value)
        {
            newStatus = value;
        }

        public SQLStatusEventArgs(eSQLStatus oldState, eSQLStatus newState)
        {
            this.SetOldStatus(oldState);
            this.SetNewStatus(newState);
        }
    }

    class SQL
    {
        
        //will be returned in case of any (generic) error
        public const int SQL_EXIT_FAILURE = -1; 

        #region Declaration for accessing the database
        public string User { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Scheme { get; set; }
        public eSQLStatus Status { get; private set; }
        private SqlConnection _connection = null;
        private int _connectionTimeout = 30; // default
        #endregion


        public event EventHandler StatusChanged;

        private void StatusChange(eSQLStatus eStatus)
        {
            StatusChanged?.Invoke(this, new SQLStatusEventArgs(Status, eStatus));
            this.Status = eStatus;
        }

        public SQL()
        {
            Status = eSQLStatus.NoSQLConnectionCreated;
            StatusChange(eSQLStatus.NoSQLConnectionCreated);
        }

        #region Create Database Connection
        public SqlConnection CreateSQLConnection(string p_user, string p_password, string p_server, string p_scheme, int p_connectionTimeout)
        {
            User = p_user;
            Password = p_password;
            Server = p_server;
            Scheme = p_scheme;
            _connectionTimeout = p_connectionTimeout;
            return CreateSQLConnection();
        }

        public SqlConnection CreateSQLConnection(int p_connectionTimeout)
        {
            _connectionTimeout = p_connectionTimeout;
            return CreateSQLConnection();
        }

        public SqlConnection CreateSQLConnection(string p_user, string p_password, string p_server, string p_scheme)
        {
            User = p_user;
            Password = p_password;
            Server = p_server;
            Scheme = p_scheme;

            return CreateSQLConnection();
        }

        public SqlConnection CreateSQLConnection()
        {
            try
            {
                string ConnectionString =
                        "Data Source=" + Server + "; " +
                        "Initial Catalog=" + Scheme + ";" +
                        "User id=" + User + ";" +
                        "Password=" + Password + ";" +
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

        public SqlConnection GetSqlConnection { get { return _connection; } }
        #endregion

        #region Database operations: Insert Row
        public int InsertRow(DataObject dObj)
        {

            StatusChange(eSQLStatus.Running);

            if (null == _connection)
            {
                if (null == CreateSQLConnection())
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
                SqlCommand command = new SqlCommand
                {
                    Connection = _connection,
                    CommandType = CommandType.Text,

                    CommandText = "insert into Datalog (SensorID, SensorName, Temperature, HeatIndex, Humidity, Pressure, LUX, LogTime) VALUES (@id, @name, @temp, @head, @hum, @press, @lux, getdate())"
                };
                //@id, @name, @temp, @head, @hum, @press, @lux, getdate())";
                command.Parameters.AddWithValue("@id", dObj.UniqueID);
                command.Parameters.AddWithValue("@name", dObj.Name);

                if (DataObjectCategory.HasTemperature(dObj.Protocol))
                    command.Parameters.AddWithValue("@temp", dObj.GetItem(DataObjectCategory.Temperatur));
                else
                    command.Parameters.AddWithValue("@temp", "");

                if (DataObjectCategory.HasHeatIndex(dObj.Protocol))
                    command.Parameters.AddWithValue("@head", dObj.GetItem(DataObjectCategory.HeatIndex));
                else
                    command.Parameters.AddWithValue("@head", "");

                if (DataObjectCategory.HasHumidity(dObj.Protocol))
                    command.Parameters.AddWithValue("@hum", dObj.GetItem(DataObjectCategory.Luftfeuchtigkeit));
                else
                    command.Parameters.AddWithValue("@hum", "");

                if (DataObjectCategory.HasAirPressure(dObj.Protocol))
                    command.Parameters.AddWithValue("@press", dObj.GetItem(DataObjectCategory.Luftdruck));
                else
                    command.Parameters.AddWithValue("@press", "");

                if (DataObjectCategory.HasLUX(dObj.Protocol))
                    command.Parameters.AddWithValue("@lux", dObj.GetItem(DataObjectCategory.Lichtwert));
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

        public void InsertDBAll(Dictionary<string, DataObject> dataObjs)
        {
            foreach (KeyValuePair<string, DataObject> kvp in dataObjs)
            {
                DataObject dObj = (DataObject)kvp.Value;
                    
                if (null == dObj || !dObj.DataAvailable || !dObj.Active || !dObj.WriteToDatabase)
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
