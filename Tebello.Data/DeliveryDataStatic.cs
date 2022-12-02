using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Subs.Data
{
    public static class DeliveryDataStatic
    {
        private static MIMSDataContext gMimsDataContext = new MIMSDataContext(Settings.ConnectionString);


        public static string LoadActive(ref DeliveryDoc pDeliveryDoc)
        {
            foreach (int lIssueId in ProductDataStatic.CurrentIssues())
            {

                {
                    string lResult;

                    if ((lResult = Load(lIssueId, ref pDeliveryDoc)) != "OK")
                    {
                        return lResult;
                    }
                }
            }
            return "OK";
        }
   


        public static string Load(int IssueId, ref DeliveryDoc pDoc)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                // Cleanup before you start a new one

                //pDoc.Clear();

                // Get new data

                SqlCommand lCommand = new SqlCommand();
                SqlDataAdapter lAdaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                lCommand.Connection = lConnection;
                lCommand.CommandType = CommandType.StoredProcedure;
                lCommand.CommandText = "dbo.[MIMS.DeliveryDataStatic.Load]";
                SqlCommandBuilder.DeriveParameters(lCommand);
                lAdaptor.SelectCommand = lCommand;
                lCommand.Parameters["@IssueId"].Value = IssueId;
                lCommand.Parameters["@Status"].Value = SubStatus.Deliverable;

                lAdaptor.Fill(pDoc.DeliveryRecord);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryDataStatic", "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool LoadLabelBySubscription(int SubscriptionId, ref DeliveryDoc pDeliveryDoc)
        {
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);
            SqlCommand Command = new SqlCommand();
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            try
            {
                // Get new data

                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.DeliveryData.LoadLabelByCustomer]";
                SqlCommandBuilder.DeriveParameters(Command);
                Adaptor.SelectCommand = Command;
                Command.Parameters["@Type"].Value = "BySubscription";
                Command.Parameters["@Id"].Value = SubscriptionId;
                pDeliveryDoc.Label.Clear();
                Adaptor.Fill(pDeliveryDoc.Label);

                if (pDeliveryDoc.Label.Rows.Count == 0)
                {
                    ExceptionData.WriteException(5, "There was no data for subscription", "DeliveryData", "LoadLabelBySubscription", SubscriptionId.ToString());
                    return false;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryData", "LoadLabelBySubscription", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static string GetDeliverableElectronic(int pReceiverId, out List<MIMS_DeliveryDataStatic_DeliverableElectronicResult> pDeliverables)
        {
            try
            {
                pDeliverables = gMimsDataContext.MIMS_DeliveryDataStatic_DeliverableElectronic(pReceiverId).ToList();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryDataStatic", " GetDeliverableElectronic", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                pDeliverables = null;
                return ex.Message;
            }
        }

        public static bool CreateDeliveryList(ref DeliveryDoc pDeliveryDoc)
        {
            int CurrentSubscriptionId = 0; // Used for diagnostic purposes
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);
            SqlCommand Command = new SqlCommand();
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            try
            {
                // Cleanup before you start a new one

                pDeliveryDoc.DeliveryList1.Clear();

                // Get new data

                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "MIMS.DeliveryDataStatic.CreateDeliveryList";
                SqlCommandBuilder.DeriveParameters(Command);
                Adaptor.SelectCommand = Command;
                pDeliveryDoc.DeliveryList1.Clear();

                foreach (DataRowView lRowView in pDeliveryDoc.DeliveryRecord.DefaultView)
                {
                    DeliveryDoc.DeliveryRecordRow lRow = (DeliveryDoc.DeliveryRecordRow)lRowView.Row;
                    CurrentSubscriptionId = lRow.SubscriptionId;
                    Command.Parameters["@SubscriptionId"].Value = lRow.SubscriptionId;
                    Adaptor.Fill(pDeliveryDoc.DeliveryList1);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static DeliveryData", "LoadDeliveryList", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                lConnection.Close();
            }

        }

        public static string ReverseDelivery(int IssueToReverseId, DateTime DeliveryDate)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.DeliveryDataStatic.ReverseDelivery]";
                SqlCommandBuilder.DeriveParameters(Command);
                //Command.Parameters["@DeliveriesReversed"].Direction = ParameterDirection.Output;
                //Command.Parameters["@DeliveriesReversed"].IsNullable = false;

                Command.Parameters["@IssueToReverseId"].Value = IssueToReverseId;
                Command.Parameters["@DeliveryDate"].Value = DeliveryDate;

                Command.ExecuteNonQuery();

                //int DeliveriesReversed = (int)Command.Parameters["@DeliveriesReversed"].Value;

                ExceptionData.WriteException(5, "I have reversed all deliveries for issue " + IssueToReverseId.ToString() + " executed on " + DeliveryDate.ToString("yyyymmddHHss"), "DeliveryDataStatic", "ReverseDelivery", "");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryDataStatic", "ReverseDelivery", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }

            finally
            {
                lConnection.Close();
            }
        }

        public static List<Dormant> Dormants()
        {
            List<Dormant> lResult = new List<Dormant>();

            try
            {
                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                return lContext.MIMS_DeliveryDataStatic_Dormants().ToList();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static DeliveData", "Dormants", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return lResult;
            }
        }


        public static bool StandarizePostCodes()
        {
            PostCodeDocTableAdapters.SAPOCodeTableAdapter lAdapter = new PostCodeDocTableAdapters.SAPOCodeTableAdapter();
            PostCodeDoc.SAPOCodeDataTable lTable = new PostCodeDoc.SAPOCodeDataTable();

            try
            {
                lAdapter.AttachConnection();
                lAdapter.Fill(lTable);

                foreach (PostCodeDoc.SAPOCodeRow lRow in lTable)
                {
                    if (!lRow.IsBoxCodeNull())
                    {
                        lRow.BoxCode = lRow.BoxCode.PadLeft(4, '0');
                    }

                    if (!lRow.IsStreetCodeNull())
                    {
                        lRow.StreetCode = lRow.StreetCode.PadLeft(4, '0');
                    }
                }

                lAdapter.Update(lTable);


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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DaliveryDataStatic", "StandardizePostCodes", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }
    }
}
