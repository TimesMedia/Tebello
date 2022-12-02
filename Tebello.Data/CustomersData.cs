using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subs.Data
{
    public static class CustomersData
    {
        public static List<CustomerData3> CompanyNameUnverified()
        {
            try 
            { 
                List<CustomerData3> lCustomers = new List<CustomerData3>();
                List<int> lCustomerIds = new List<int>();

                SqlConnection lConnection = new SqlConnection();
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = "select CustomerId from Customer where CompanyNameUnverified is not null";
                SqlDataReader lReader = Command.ExecuteReader();

                while (lReader.Read())
                {
                    lCustomerIds.Add(lReader.GetInt32(0));
                }

                foreach (int item in lCustomerIds)
                {
                    lCustomers.Add(new CustomerData3(item));
                }
                return lCustomers;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomersData", "CompanyNameUnverified", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }
    }
}
