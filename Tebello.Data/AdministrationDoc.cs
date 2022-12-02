using System;
using System.Data.SqlClient;
namespace Subs.Data
{

    public partial class AdministrationDoc
    {

        public string RefreshEnums()
        {
            try
            {
                EnumTableDataTable lTable = new EnumTableDataTable();
                AdministrationDocTableAdapters.EnumTableTableAdapter lAdaptor = new AdministrationDocTableAdapters.EnumTableTableAdapter();
                lAdaptor.AttachConnection();

                foreach (int i in Enum.GetValues(typeof(Data.SubscriptionType)))
                {
                    lAdaptor.MergeEnum("SubscriptionType", i, Enum.GetName(typeof(Data.SubscriptionType), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.SubscriptionMedium)))
                {
                    lAdaptor.MergeEnum("SubscriptionMedium", i, Enum.GetName(typeof(Data.SubscriptionMedium), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.Title)))
                {
                    lAdaptor.MergeEnum("Title", i, Enum.GetName(typeof(Data.Title), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.AddressType)))
                {
                    lAdaptor.MergeEnum("AddressType", i, Enum.GetName(typeof(Data.AddressType), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.SubStatus)))
                {
                    lAdaptor.MergeEnum("SubStatus", i, Enum.GetName(typeof(Data.SubStatus), i));
                }


                foreach (int i in Enum.GetValues(typeof(Data.PaymentMethod)))
                {
                    lAdaptor.MergeEnum("PaymentMethod", i, Enum.GetName(typeof(Data.PaymentMethod), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.ReferenceType)))
                {
                    lAdaptor.MergeEnum("ReferenceType", i, Enum.GetName(typeof(Data.ReferenceType), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.DeliveryMethod)))
                {
                    lAdaptor.MergeEnum("DeliveryMethod", i, Enum.GetName(typeof(Data.DeliveryMethod), i));
                }

                foreach (int i in Enum.GetValues(typeof(Data.Operation)))
                {
                    lAdaptor.MergeEnum("Operation", i, Enum.GetName(typeof(Data.Operation), i));
                }


                foreach (int i in Enum.GetValues(typeof(Data.Correspondence)))
                {
                    lAdaptor.MergeEnum("Correspondence", i, Enum.GetName(typeof(Data.Correspondence), i));
                }


                foreach (int i in Enum.GetValues(typeof(Data.PaymentData.PaymentState)))
                {
                    lAdaptor.MergeEnum("PaymentState", i, Enum.GetName(typeof(Data.PaymentData.PaymentState), i));
                }


                return "OK";
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "RefreshEnums", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        private partial class CountryDataTable
        {
        }

        public partial class CompanyDataTable
        {
        }
    }

}

namespace Subs.Data.AdministrationDocTableAdapters
{
    public partial class EnumTableTableAdapter
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

    public partial class GUIDTableTableAdapter
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
                this.Adapter.InsertCommand.Connection = myConnection;
                //this.Adapter.DeleteCommand.Connection = myConnection;
            }
            catch (Exception ex)
            {
                ExceptionData.WriteException(1, ex.Message, this.ToString(), "AttachConnection", "");
                throw new Exception(this.ToString() + " : " + "AttachConnection" + " : ", ex);
            }

        }
    }




    public partial class CompanyTableAdapter
    {
        private readonly SqlConnection myConnection = new SqlConnection();

        public void AttachConnection()
        {
            try
            {
                // Set the connectionString for this object

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
}






