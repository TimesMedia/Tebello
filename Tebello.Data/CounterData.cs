using System;
using System.Data;
using System.Data.SqlClient;

namespace Subs.Data
{
    public static class CounterData
    {

        private static readonly SqlConnection Connection = new SqlConnection();


        static CounterData()
        {

            // Set the connectionString for this object
            ;
            if (Settings.ConnectionString == "")
            {
                // This makes it possible to use the Visual studio designer.
                Connection.ConnectionString = global::Subs.Data.Properties.Settings.Default.MIMSConnectionString;
            }
            else
            {
                Connection.ConnectionString = Settings.ConnectionString;
            }
        }

        public static bool GetUniqueNumber(string CounterName, ref int Value)
        {
            try
            {
                // Get next value
                SqlCommand Command = new SqlCommand();
                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.CounterData.GetUniqueNumber]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@CounterName"].Value = CounterName;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    Value = (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }

                return true;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static CounterData", "GetUniqueNumber", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        public static bool GetValue(string CounterName, ref int Value)
        {
            try
            {
                // Get current value
                SqlCommand Command = new SqlCommand();
                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.CounterData.GetValue]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@CounterName"].Value = CounterName;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    Value = (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }

                return true;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static CounterData", "GetValue", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        public static bool SetValue(string CounterName, int Value)
        {
            try
            {
                // Set the value
                SqlCommand Command = new SqlCommand();
                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.CounterData.SetValue]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@CounterName"].Value = CounterName;
                Command.Parameters["@Value"].Value = Value;
                Command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static CounterData", "SetValue", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                Connection.Close();
            }
        }









    }
}
