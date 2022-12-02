using System;
using System.Data.SqlClient;
namespace Subs.Data.PaymentDocTableAdapters
{
    public partial class PayUResultTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public bool AttachConnection()
        {
            try
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
        private readonly SqlConnection myConnection = new SqlConnection();

        public bool AttachConnection()
        {
            try
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

    public partial class PayerCandidateTableAdapter
    {
        private readonly SqlConnection gConnection = new SqlConnection();

        public bool AttachTransaction(ref SqlTransaction pTransaction)
        {
            try
            {
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand lCommand in CommandCollection)
                {
                    lCommand.Connection = pTransaction.Connection;
                    lCommand.Transaction = pTransaction;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachTransaction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool AttachConnection()
        {
            try
            {
                gConnection.ConnectionString = Settings.ConnectionString;

                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = gConnection;
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

    public partial class SBBankStatementTableAdapter
    {
        private readonly SqlConnection gConnection = new SqlConnection();

        public bool AttachTransaction(ref SqlTransaction pTransaction)
        {
            try
            {
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand lCommand in CommandCollection)
                {
                    lCommand.Connection = pTransaction.Connection;
                    lCommand.Transaction = pTransaction;
                }

                this.Adapter.UpdateCommand.Transaction = pTransaction;
                this.Adapter.InsertCommand.Transaction = pTransaction;
                this.Adapter.DeleteCommand.Transaction = pTransaction;

                this.Adapter.UpdateCommand.Connection = pTransaction.Connection;
                this.Adapter.InsertCommand.Connection = pTransaction.Connection;
                this.Adapter.DeleteCommand.Connection = pTransaction.Connection;

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachTransaction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool AttachConnection()
        {
            try
            {
                gConnection.ConnectionString = Settings.ConnectionString;

                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = gConnection;
                }

                this.Adapter.UpdateCommand.Connection = gConnection;
                this.Adapter.InsertCommand.Connection = gConnection;
                this.Adapter.DeleteCommand.Connection = gConnection;

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

    public partial class FNBBankStatementTableAdapter
    {
        private readonly SqlConnection gConnection = new SqlConnection();

        public bool AttachTransaction(ref SqlTransaction pTransaction)
        {
            try
            {
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand lCommand in CommandCollection)
                {
                    lCommand.Connection = pTransaction.Connection;
                    lCommand.Transaction = pTransaction;
                }

                this.Adapter.UpdateCommand.Transaction = pTransaction;
                this.Adapter.InsertCommand.Transaction = pTransaction;
                this.Adapter.DeleteCommand.Transaction = pTransaction;

                this.Adapter.UpdateCommand.Connection = pTransaction.Connection;
                this.Adapter.InsertCommand.Connection = pTransaction.Connection;
                this.Adapter.DeleteCommand.Connection = pTransaction.Connection;

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachTransaction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool AttachConnection()
        {
            try
            {
                gConnection.ConnectionString = Settings.ConnectionString;

                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = gConnection;
                }

                this.Adapter.UpdateCommand.Connection = gConnection;
                this.Adapter.InsertCommand.Connection = gConnection;
                this.Adapter.DeleteCommand.Connection = gConnection;

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

    public partial class DebitOrderBankStatementTableAdapter
    {
        private readonly SqlConnection gConnection = new SqlConnection();

        public bool AttachTransaction(ref SqlTransaction pTransaction)
        {
            try
            {
                // Replace the designer's connection with yor own one.
                foreach (SqlCommand lCommand in CommandCollection)
                {
                    lCommand.Connection = pTransaction.Connection;
                    lCommand.Transaction = pTransaction;
                }

                this.Adapter.UpdateCommand.Transaction = pTransaction;
                this.Adapter.InsertCommand.Transaction = pTransaction;
                this.Adapter.DeleteCommand.Transaction = pTransaction;

                this.Adapter.UpdateCommand.Connection = pTransaction.Connection;
                this.Adapter.InsertCommand.Connection = pTransaction.Connection;
                this.Adapter.DeleteCommand.Connection = pTransaction.Connection;

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachTransaction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool AttachConnection()
        {
            try
            {
                gConnection.ConnectionString = Settings.ConnectionString;

                // Replace the designer's connection with yor own one.
                foreach (SqlCommand myCommand in CommandCollection)
                {
                    myCommand.Connection = gConnection;
                }

                this.Adapter.UpdateCommand.Connection = gConnection;
                this.Adapter.InsertCommand.Connection = gConnection;
                this.Adapter.DeleteCommand.Connection = gConnection;

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


    public partial class PaymentDoc
    {
        public partial class SBBankStatementDataTable
        {
        }

        public partial class PayerCandidateDataTable
        {
        }
    }
}
