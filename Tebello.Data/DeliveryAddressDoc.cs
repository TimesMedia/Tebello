using Subs.Data;
using System.Data.SqlClient;

namespace Tebello.Data.DeliveryAddressDocTableAdapters
{
    public partial class DeliveryAddressTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public object FillByIdReturnValue(int Index)
        {
            return CommandCollection[Index].Parameters[0].Value;
        }

        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }

        public bool AttachTransaction(ref SqlTransaction myTransaction)
        {
            try
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

                return true;
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, this.ToString(), "AttachTransaction", "");
                    throw new System.Exception(this.ToString() + " : " + "AttachTransaction" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }
    }

    public partial class CountryTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }
    }

    public partial class DeliveryCostTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }
    }

    public partial class ProvinceTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();
        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }
    }

    public partial class CityTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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
            return;
        }
    }

    public partial class SuburbTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }
    }

    public partial class StreetTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            // Set the connectionString for this object

            if (Settings.ConnectionString == "")
            {
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

            return;
        }
    }
}


namespace Tebello.Data
{
    public partial class DeliveryAddressDoc
    {
        public partial class DeliveryAddressDataTable
        {
        }
    }
}
