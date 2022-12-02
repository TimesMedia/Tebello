using System;
using System.Data.SqlClient;

namespace Subs.Data.SBDebitOrderDocTableAdapters
{
    public partial class SBDebitOrderTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object
                //;
                if (Settings.ConnectionString == "")
                {
                    throw new Exception("No connectionstring was set for this connection");
                }
                else
                {
                    myConnection.ConnectionString = Settings.ConnectionString;
                }


                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = myConnection;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachConnection", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return false;
            }
        }
    }

    public partial class DebitOrderHistoryTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object
                //;
                if (Settings.ConnectionString == "")
                {
                    throw new Exception("No connectionstring was set for this connection");
                }
                else
                {
                    myConnection.ConnectionString = Settings.ConnectionString;
                }


                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = myConnection;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachConnection", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return false;
            }
        }
    }



}

namespace Subs.Data
{
    public partial class SBDebitOrderDoc
    {
        partial class SBDebitOrderDataTable
        {
        }
    }
}
