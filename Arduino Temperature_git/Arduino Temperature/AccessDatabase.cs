using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Temperature
{
    static class AccessDatabase
    {
        static public int TrackEnvironment(string sName, double BTemp, double BHuman, double TTemp, double THuman)
        {
            string ConnectionString =
                    @"Data Source=LTBIMI\TIBIMISQL;" +
                     "Initial Catalog=CONTAINER;" +
                     "User id=bimi;" +
                     "Password=B!m!2016#;";
            int returnVal = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "insert into Meassure (SensorName, BTemperature, BHumanidity, TTemperature, THumanidity) VALUES (@sName, @BTemp, @BHuman, @TTemp, @THuman)";
                    command.Parameters.AddWithValue("@sName", sName);
                    command.Parameters.AddWithValue("@BTemp", BTemp);
                    command.Parameters.AddWithValue("@BHuman", BHuman);
                    command.Parameters.AddWithValue("@TTemp", TTemp);
                    command.Parameters.AddWithValue("@THuman", THuman);

                    try
                    {
                        connection.Open();
                        returnVal = command.ExecuteNonQuery(); //affected rows
                    }
                    catch (SqlException sEx)
                    {
                        Console.WriteLine(sEx.Message);
                        // error here
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return returnVal;
        }

    }
}
