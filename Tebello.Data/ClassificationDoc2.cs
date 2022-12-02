using System;
using System.Data.SqlClient;

namespace Subs.Data
{
    public partial class ClassificationDoc2
    {
    }
}

namespace Subs.Data.ClassificationDoc2TableAdapters
{
    public partial class ClassificationTableAdapter
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

    public partial class CustomerClassificationTableAdapter
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

}





//partial class Classification2TableAdapter
//{
//    private SqlConnection lConnection = new SqlConnection();

//    public bool AttachConnection()
//    {
//        try
//        {
//            // Set the connectionString for this object

//            lConnection.ConnectionString = Settings.ConnectionString;


//            // Replace the designer's connection with yor own one.
//            foreach (SqlCommand myCommand in CommandCollection)
//            {
//                myCommand.Connection = lConnection;
//            }

//            this.Adapter.UpdateCommand.Connection = lConnection;
//            this.Adapter.InsertCommand.Connection = lConnection;
//            this.Adapter.DeleteCommand.Connection = lConnection;

//            return true;
//        }
//        catch (Exception ex)
//        {
//            //Display all the exceptions

//            Exception CurrentException = ex;
//            int ExceptionLevel = 0;
//            do
//            {
//                ExceptionLevel++;
//                ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachConnection", "");
//                CurrentException = CurrentException.InnerException;
//            } while (CurrentException != null);

//            return false;
//        }
//    }
//}





//partial class Customer_HierarchyClassificationTableAdapter
//{
//    private SqlConnection lConnection = new SqlConnection();

//    public bool AttachConnection()
//    {
//        try
//        {
//            // Set the connectionString for this object

//            lConnection.ConnectionString = Settings.ConnectionString;


//            // Replace the designer's connection with yor own one.
//            foreach (SqlCommand myCommand in CommandCollection)
//            {
//                myCommand.Connection = lConnection;
//            }

//            this.Adapter.UpdateCommand.Connection = lConnection;
//            this.Adapter.InsertCommand.Connection = lConnection;
//            this.Adapter.DeleteCommand.Connection = lConnection;

//            return true;
//        }
//        catch (Exception ex)
//        {
//            //Display all the exceptions

//            Exception CurrentException = ex;
//            int ExceptionLevel = 0;
//            do
//            {
//                ExceptionLevel++;
//                ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachConnection", "");
//                CurrentException = CurrentException.InnerException;
//            } while (CurrentException != null);

//            return false;
//        }
//    }

//    public bool AttachTransaction(ref SqlTransaction myTransaction)
//    {
//        try
//        {
//            // Replace the designer's connection with yor own one.
//            foreach (SqlCommand myCommand in CommandCollection)
//            {
//                myCommand.Connection = myTransaction.Connection;
//                myCommand.Transaction = myTransaction;

//            }

//            this.Adapter.UpdateCommand.Transaction = myTransaction;
//            this.Adapter.InsertCommand.Transaction = myTransaction;
//            this.Adapter.DeleteCommand.Transaction = myTransaction;

//            this.Adapter.UpdateCommand.Connection = myTransaction.Connection;
//            this.Adapter.InsertCommand.Connection = myTransaction.Connection;
//            this.Adapter.DeleteCommand.Connection = myTransaction.Connection;

//            return true;
//        }

//        catch (Exception ex)
//        {
//            //Display all the exceptions

//            Exception CurrentException = ex;
//            int ExceptionLevel = 0;
//            do
//            {
//                ExceptionLevel++;
//                ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AttachTransaction", "");
//                CurrentException = CurrentException.InnerException;
//            } while (CurrentException != null);
//            return false;

//        }

//    }


//    }
















//    public partial class ClassificationTableAdapter {
//    }
//}
