using System;
using System.Data.SqlClient;

namespace Subs.Data
{
    public partial class PostCodeDoc
    {
    }
}

namespace Subs.Data.PostCodeDocTableAdapters
{
    public partial class PostCode_LinearTableAdapter
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

                //this.Adapter.UpdateCommand.Connection = lConnection;
                //this.Adapter.InsertCommand.Connection = lConnection;
                //this.Adapter.DeleteCommand.Connection = lConnection;

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

    public partial class CodeTableAdapter
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

    public partial class AddressLine4TableAdapter
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

    public partial class AddressLine3TableAdapter
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

    public partial class SAPOCodeTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            try
            {
                // Set the connectionString for this object
                ;
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

                this.Adapter.UpdateCommand.Connection = myConnection;
                this.Adapter.InsertCommand.Connection = myConnection;
                this.Adapter.DeleteCommand.Connection = myConnection;
            }
            catch (Exception ex)
            {
                ExceptionData.WriteException(1, ex.Message, this.ToString(), "AttachConnection", "");
                throw new Exception(this.ToString() + " : " + "AttachConnection" + " : ", ex);
            }

        }

    }

    public partial class SAPOCodeFlatTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            try
            {
                // Set the connectionString for this object
                ;
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

                //this.Adapter.UpdateCommand.Connection = myConnection;
                //this.Adapter.InsertCommand.Connection = myConnection;
                //this.Adapter.DeleteCommand.Connection = myConnection;
            }
            catch (Exception ex)
            {
                ExceptionData.WriteException(1, ex.Message, this.ToString(), "AttachConnection", "");
                throw new Exception(this.ToString() + " : " + "AttachConnection" + " : ", ex);
            }

        }

    }




}

