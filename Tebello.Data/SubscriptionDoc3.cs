using System;
using System.Data.SqlClient;
namespace Subs.Data.SubscriptionDoc3TableAdapters
{
    partial class SubscriptionDerivedTableAdapter
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

    public partial class InvoiceTableAdapter
    {
    }

    public partial class InvoiceTableAdapter
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

    public partial class PromotionTableAdapter
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

    public partial class AuthorizationsTableAdapter
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

    public partial class SubscriptionCandidateTableAdapter
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

    public partial class SubscriptionListTableAdapter
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

    public partial class SubscriptionTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachTransaction(ref SqlTransaction myTransaction)
        {
               // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = myTransaction.Connection;
                    myCommand.Transaction = myTransaction;

                }

                this.Adapter.UpdateCommand.Transaction = myTransaction;
                this.Adapter.InsertCommand.Transaction = myTransaction;
                this.Adapter.DeleteCommand.Transaction = myTransaction;

                this.Adapter.UpdateCommand.Connection = myTransaction.Connection;
                this.Adapter.InsertCommand.Connection = myTransaction.Connection;
                this.Adapter.DeleteCommand.Connection = myTransaction.Connection;
        }


        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object

                if (Settings.ConnectionString == "")
                {
                    throw new Exception("No connectionString was set");
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

                this.Adapter.UpdateCommand.Connection = myConnection;
                this.Adapter.InsertCommand.Connection = myConnection;
                this.Adapter.DeleteCommand.Connection = myConnection;

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

    public partial class SubscriptionIssueTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachTransaction(ref SqlTransaction myTransaction)
        {
    
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = myTransaction.Connection;
                    myCommand.Transaction = myTransaction;

                }

                this.Adapter.UpdateCommand.Transaction = myTransaction;
                this.Adapter.InsertCommand.Transaction = myTransaction;
                this.Adapter.DeleteCommand.Transaction = myTransaction;

                this.Adapter.UpdateCommand.Connection = myTransaction.Connection;
                this.Adapter.InsertCommand.Connection = myTransaction.Connection;
                this.Adapter.DeleteCommand.Connection = myTransaction.Connection;
        }


        public void AttachConnection()
        {
              
                    myConnection.ConnectionString = Settings.ConnectionString;
 
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = myConnection;
                }

                this.Adapter.UpdateCommand.Connection = myConnection;
                this.Adapter.InsertCommand.Connection = myConnection;
                this.Adapter.DeleteCommand.Connection = myConnection;
        }

        internal void RegisterDelivery(int pSubscriptionId, int pIssueId)
        {
            throw new NotImplementedException();
        }
    }

}


namespace Subs.Data
{
    public partial class SubscriptionDoc3
    {
        public partial class SubscriptionDataTable
        {
        }

        public partial class SubscriptionIssueKeepDataTable
        {
        }

        private partial class DormantDataTable
        {
        }

        private partial class SubscriptionOldDataTable
        {
        }
    }
}
