using System;
using System.Data.SqlClient;

namespace Subs.Data
{

    public partial class CustomerDoc2
    {
        public partial class ProposedCustomerDataTable
        {
        }

        public partial class CustomerDataTable
        {
            internal void Clear()
            {
                throw new NotImplementedException();
            }

            internal void AcceptChanges()
            {
                throw new NotImplementedException();
            }
        }
    }
}

namespace Subs.Data.CustomerDoc2TableAdapters
{
    public partial class DiscrepanciesTableAdapter
    {
        private readonly SqlConnection lConnection = new SqlConnection();

        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object

                lConnection.ConnectionString = Settings.ConnectionString;


                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = lConnection;
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

    public partial class LiabilityTableAdapter
    {
        private readonly SqlConnection lConnection = new SqlConnection();

        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object

                lConnection.ConnectionString = Settings.ConnectionString;


                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = lConnection;
                }

                this.Adapter.UpdateCommand.Connection = lConnection;
                this.Adapter.InsertCommand.Connection = lConnection;
                this.Adapter.DeleteCommand.Connection = lConnection;

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

    public partial class Comment2TableAdapter
    {
        private readonly SqlConnection lConnection = new SqlConnection();

        public bool AttachConnection()
        {
            try
            {
                // Set the connectionString for this object

                lConnection.ConnectionString = Settings.ConnectionString;


                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = lConnection;
                }

                this.Adapter.UpdateCommand.Connection = lConnection;
                this.Adapter.InsertCommand.Connection = lConnection;
                this.Adapter.DeleteCommand.Connection = lConnection;

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

    public partial class CustomerTableAdapter
    {
         public bool AttachConnection()
        {
            try
            {
                SqlConnection myConnection = new SqlConnection();

                // Set the connectionString for this object
                ;
                if (Settings.ConnectionString == "")
                {
                    ExceptionData.WriteException(1, "No connectionstring was set for this connection", "CustomerTableAdapter", "AttachConnection", "");
                    throw new System.Exception("No connectionstring was set for this connection");
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
            catch (System.Exception Ex)
            {
                ExceptionData.WriteException(1, Ex.Message, "CustomerTableAdapter", "AttachConnection", "");
                throw new System.Exception(Ex.Message);
            }
        }


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
    }

}