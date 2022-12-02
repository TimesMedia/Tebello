using System.Data.SqlClient;

namespace Subs.Data.ProductDocTableAdapters
{
    //partial class ProductAdminTableAdapter
    //{
    //    private SqlConnection myConnection = new SqlConnection();
    //    public event Subs.Data.Base.FeedbackEventHandler Feedback;
    //    private Subs.Data.Base.FeedbackEventArgs e = new Subs.Data.Base.FeedbackEventArgs();

    //    public bool AttachConnection()
    //    {
    //        try
    //        {
    //            // Set the connectionString for this object

    //            if (Settings.ConnectionString == "")
    //            {
    //                if (Feedback != null)
    //                {
    //                    e.Severity = 1;
    //                    e.Message = "No connectionstring was set for this connection";
    //                    e.Object = this.ToString();
    //                    e.Method = "AttachConnection";
    //                    e.Comment = "";
    //                    Feedback(this, e);
    //                }
    //                return false;
    //            }
    //            else
    //            {
    //                myConnection.ConnectionString = Settings.ConnectionString;
    //            }

    //            // Replace the designer's connection with yor own one.
    //            foreach (SqlCommand myCommand in CommandCollection)
    //            {
    //                myCommand.Connection = myConnection;
    //            }

    //            this.Adapter.UpdateCommand.Connection = myConnection;
    //            this.Adapter.InsertCommand.Connection = myConnection;
    //            this.Adapter.DeleteCommand.Connection = myConnection;

    //            return true;
    //        }
    //        catch (System.Exception Ex)
    //        {
    //            if (Feedback != null)
    //            {
    //                e.Severity = 1;
    //                e.Message = Ex.Message;
    //                e.Object = this.ToString();
    //                e.Method = "AttatchConnection";
    //                e.Comment = "";
    //                Feedback(this, e);
    //            }
    //            return false;
    //        }
    //    }
    //}

    //partial class PromotionDisplayTableAdapter
    //{
    //    private SqlConnection myConnection = new SqlConnection();

    //    public bool AttachConnection()
    //    {
    //        try
    //        {
    //            // Set the connectionString for this object

    //           myConnection.ConnectionString = Settings.ConnectionString;


    //            // Replace the designer's connection with yor own one.
    //            foreach (SqlCommand myCommand in CommandCollection)
    //            {
    //                myCommand.Connection = myConnection;
    //            }

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


    //partial class IssueAdminTableAdapter
    //{
    //    private SqlConnection myConnection = new SqlConnection();
    //    public event Subs.Data.Base.FeedbackEventHandler Feedback;
    //    private Subs.Data.Base.FeedbackEventArgs e = new Subs.Data.Base.FeedbackEventArgs();

    //    public bool AttachConnection()
    //    {
    //        try
    //        {
    //            // Set the connectionString for this object
    //            ;
    //            if (Settings.ConnectionString == "")
    //            {
    //                if (Feedback != null)
    //                {
    //                    e.Severity = 1;
    //                    e.Message = "No connectionstring was set for this connection";
    //                    e.Object = this.ToString();
    //                    e.Method = "AttachConnection";
    //                    e.Comment = "";
    //                    Feedback(this, e);
    //                }
    //                return false;
    //            }
    //            else
    //            {
    //                myConnection.ConnectionString = Settings.ConnectionString;
    //            }

    //            // Replace the designer's connection with yor own one.
    //            foreach (SqlCommand myCommand in CommandCollection)
    //            {
    //                myCommand.Connection = myConnection;
    //            }

    //            this.Adapter.UpdateCommand.Connection = myConnection;
    //            this.Adapter.InsertCommand.Connection = myConnection;
    //            this.Adapter.DeleteCommand.Connection = myConnection;

    //            return true;
    //        }
    //        catch (System.Exception Ex)
    //        {
    //            if (Feedback != null)
    //            {
    //                e.Severity = 1;
    //                e.Message = Ex.Message;
    //                e.Object = this.ToString();
    //                e.Method = "AttatchConnection";
    //                e.Comment = "";
    //                Feedback(this, e);
    //            }
    //            return false;
    //        }
    //    }
    //}

    //partial class ProductTableAdapter
    //{
    //    private SqlConnection myConnection = new SqlConnection();
    //    public event Subs.Data.Base.FeedbackEventHandler Feedback;
    //    private Subs.Data.Base.FeedbackEventArgs e = new Subs.Data.Base.FeedbackEventArgs();

    //    public bool AttachConnection()
    //    {
    //        try
    //        {
    //            // Set the connectionString for this object
    //            ;
    //            if (Settings.ConnectionString == "")
    //            {
    //                if (Feedback != null)
    //                {
    //                    e.Severity = 1;
    //                    e.Message = "No connectionstring was set for this connection";
    //                    e.Object = this.ToString();
    //                    e.Method = "AttachConnection";
    //                    e.Comment = "";
    //                    Feedback(this, e);
    //                }
    //                return false;
    //            }
    //            else
    //            {
    //                myConnection.ConnectionString = Settings.ConnectionString;
    //            }

    //            // Replace the designer's connection with yor own one.
    //            foreach (SqlCommand myCommand in CommandCollection)
    //            {
    //                myCommand.Connection = myConnection;
    //            }

    //            this.Adapter.UpdateCommand.Connection = myConnection;
    //            this.Adapter.InsertCommand.Connection = myConnection;
    //            this.Adapter.DeleteCommand.Connection = myConnection;

    //            return true;
    //        }
    //        catch (System.Exception Ex)
    //        {
    //            if (Feedback != null)
    //            {
    //                e.Severity = 1;
    //                e.Message = Ex.Message;
    //                e.Object = this.ToString();
    //                e.Method = "AttatchConnection";
    //                e.Comment = "";
    //                Feedback(this, e);
    //            }
    //            return false;
    //        }
    //    }
    //}

    public partial class Product2TableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
 
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
    }

    public partial class IssueTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
 
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
    }


    //    public bool AttachTransaction(ref SqlTransaction myTransaction)
    //    {
    //        try
    //        {
    //            this.InitCommandCollection();

    //            foreach (SqlCommand cmd in _commandCollection)
    //            {
    //                cmd.Connection = myTransaction.Connection;
    //                cmd.Transaction = myTransaction;
    //            }

    //            this.Adapter.UpdateCommand.Connection = myTransaction.Connection;
    //            this.Adapter.UpdateCommand.Transaction = myTransaction;

    //            this.Adapter.InsertCommand.Connection = myTransaction.Connection;
    //            this.Adapter.InsertCommand.Transaction = myTransaction;

    //            this.Adapter.DeleteCommand.Connection = myTransaction.Connection;
    //            this.Adapter.DeleteCommand.Transaction = myTransaction;

    //            return true;
    //        }
    //        catch (System.Exception Ex)
    //        {
    //            if (Feedback != null)
    //            {
    //                e.Severity = 1;
    //                e.Message = Ex.Message;
    //                e.Object = this.ToString();
    //                e.Method = "AttachTransaction";
    //                e.Comment = "";
    //                Feedback(this, e);
    //            }
    //            return false;
    //        }
    //    }
    //}



    //partial class MailTableAdapter
    //{
    //    private SqlConnection myConnection = new SqlConnection();
    //    public event Subs.Data.Base.FeedbackEventHandler Feedback;
    //    private Subs.Data.Base.FeedbackEventArgs e = new Subs.Data.Base.FeedbackEventArgs();

    //    public bool AttachConnection()
    //    {
    //        try
    //        {
    //            // Set the connectionString for this object
    //            ;
    //            if (Settings.ConnectionString == "")
    //            {
    //                if (Feedback != null)
    //                {
    //                    e.Severity = 1;
    //                    e.Message = "No connectionstring was set for this connection";
    //                    e.Object = this.ToString();
    //                    e.Method = "AttachConnection";
    //                    e.Comment = "";
    //                    Feedback(this, e);
    //                }
    //                return false;
    //            }
    //            else
    //            {
    //                myConnection.ConnectionString = Settings.ConnectionString;
    //            }

    //            // Replace the designer's connection with yor own one.
    //            foreach (SqlCommand myCommand in CommandCollection)
    //            {
    //                myCommand.Connection = myConnection;
    //            }

    //            this.Adapter.UpdateCommand.Connection = myConnection;
    //            this.Adapter.InsertCommand.Connection = myConnection;
    //            this.Adapter.DeleteCommand.Connection = myConnection;

    //            return true;
    //        }
    //        catch (System.Exception Ex)
    //        {
    //            if (Feedback != null)
    //            {
    //                e.Severity = 1;
    //                e.Message = Ex.Message;
    //                e.Object = this.ToString();
    //                e.Method = "AttatchConnection";
    //                e.Comment = "";
    //                Feedback(this, e);
    //            }
    //            return false;
    //        }
    //    }
    //}





    public partial class BaseRateTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
   
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
    }




}


namespace Subs.Data
{
    public partial class ProductDoc
    {
        public partial class Product2DataTable
        {
        }

        private partial class PromotionDataTable
        {
        }

        private partial class ProductDataTable
        {
        }
    }
}
