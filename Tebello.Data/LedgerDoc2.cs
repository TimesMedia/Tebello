using System.Data.SqlClient;

namespace Subs.Data.LedgerDoc2TableAdapters
{

    public partial class TransactionHistoryTableAdapter
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

            //this.Adapter.UpdateCommand.Connection = myConnection;
            //this.Adapter.InsertCommand.Connection = myConnection;
            //this.Adapter.DeleteCommand.Connection = myConnection;
        }
    }



        public partial class TransactionsTableAdapter
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
namespace Subs.Data
{


    public partial class LedgerDoc2
    {
        public partial class TransactionsDataTable
        {
        }
    }
}
