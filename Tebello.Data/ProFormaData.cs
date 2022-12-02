using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subs.Data
{
    public static class ProFormaData
    {
        public static List<int> GetSubscriptionIds(int pProFormaId)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                List<int> lSubscriptionIds = new List<int>();
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = "select subscriptionId from Subscription where ProformaId =" + pProFormaId.ToString();

                using (SqlDataReader dataReader = Command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        lSubscriptionIds.Add((int)dataReader["SubscriptionId"]);
                    }
                }
                return lSubscriptionIds;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "ProFormaData", "GetSubscriptionIds", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null) ;

                throw ex;
            }
            finally
            {
                lConnection.Close();
            }
        }
    }
}