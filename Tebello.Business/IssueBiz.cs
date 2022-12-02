using Subs.Data;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Subs.Business
{
    public static class IssueBiz
    {
        private static readonly Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter gSubscriptionIssueAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter(); // This is stateless, so it can be shared
        private static readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);

        static IssueBiz()
        {
            gSubscriptionIssueAdapter.AttachConnection();
        }

        public static string Create(ref SqlTransaction pTransaction, int pSubscriptionId, int pUnitsPerIssue, int pStartIssue, int pEndIssue)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                // Identify the range
                int StartSequence = ProductDataStatic.GetSequence(pStartIssue);
                int EndSequence = ProductDataStatic.GetSequence(pEndIssue);
                int ProductId = ProductDataStatic.GetProductId(pStartIssue);

                // Create a row for each issue in the range

                lTable.Clear();

                for (int i = StartSequence; i <= EndSequence; i++)
                {
                    SubscriptionDoc3.SubscriptionIssueRow myRow = lTable.NewSubscriptionIssueRow();

                    myRow.SubscriptionId = pSubscriptionId;
                    int IssueId = 0;
                    {
                        string lResult;

                        if ((lResult = ProductDataStatic.GetIssueId(ProductId, i, out IssueId)) != "OK")
                        {
                            return lResult;
                        }
                    }
                    myRow.IssueId = IssueId;
                    myRow.Sequence = i;
                    myRow.UnitsLeft = pUnitsPerIssue;
                    //myRow.Paid = false;
                    lTable.AddSubscriptionIssueRow(myRow);
                }

                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdaptor = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdaptor.AttachTransaction(ref pTransaction);
                myAdaptor.Update(lTable);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static IssueBiz", "Create with transaction", pSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        private static bool Load(int SubscriptionId, ref SubscriptionDoc3.SubscriptionIssueDataTable pTable)
        {
            try
            {
                pTable.Clear();
                gSubscriptionIssueAdapter.FillById(pTable, SubscriptionId, "BySubscription");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static IssueBiz", "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        private static bool Load(ref SqlTransaction myTransaction, ref SubscriptionDoc3.SubscriptionIssueDataTable pTable, int SubscriptionId)
        {
            try
            {
                pTable.Clear();
                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdapter.AttachTransaction(ref myTransaction);
                myAdapter.FillById(pTable, SubscriptionId, "BySubscription");

                if (pTable.Count == 0)
                {
                    throw new Exception("There are no issues associated with this subscription");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static IssueBiz", "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static int GetSequenceNumber(int pIssueId)
        {
            MIMS_IssueBiz_GetSequenceNumberResult lResult = gDataContext.MIMS_IssueBiz_GetSequenceNumber(pIssueId).Single();
            return lResult.Column1;

        }

        public static int GetIssueId(int pProductId, int pSequenceNumber)
        {
            MIMS_IssueBiz_GetIssueIdResult lResult = gDataContext.MIMS_IssueBiz_GetIssueId(pProductId, pSequenceNumber).Single();
            return lResult.Column1;
        }


        public static int GetUnitsLeft(int SubscriptionId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                int lUnitsLeft = 0;
                // See if you have this subscription loaded already

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                // Count the units left 
                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    lUnitsLeft += myRow.UnitsLeft;
                }

                return lUnitsLeft;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "GetUnitsLeft", "");
                    throw new Exception("static IssueBiz" + " : " + "GetUnitsLeft" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static bool UnitsLeft(int SubscriptionId, int IssueId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                bool Exists = false;

                // See if you have this subscription loaded already

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                // Count the units left 
                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    if (myRow.IssueId == IssueId)
                    {
                        if (myRow.UnitsLeft > 0)
                        {
                            Exists = true;
                        }
                    }
                }
                return Exists;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "UnitsLeft", "");
                    throw new Exception("static IssueBiz" + " : " + "Deliverable" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static int GetLastIssue(int SubscriptionId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                int IssueId = 0;
                // See if you have this subscription loaded already

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                int LargestSequence = lTable[lTable.Count - 1].Sequence;

                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    if (myRow.Sequence == LargestSequence)
                    {
                        IssueId = myRow.IssueId;
                    }
                }

                return IssueId;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "GetLastIssue", "");
                    throw new Exception("static IssueBiz" + " : " + "GetLastIssue" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }

        }

        public static int GetStartIssue(int SubscriptionId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                int lIssueId = 0;
                // See if you have this subscription loaded already

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                int SmallestSequence = lTable[0].Sequence;

                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    if (myRow.Sequence == SmallestSequence)
                    {
                        lIssueId = myRow.IssueId;
                    }
                }
                return lIssueId;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "GetStartIssue", "");
                    throw new Exception("static IssueBiz" + " : " + "GetStartIssue" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static bool Deliver(ref SqlTransaction myTransaction, int SubscriptionId, int IssueId, out bool Expired, out bool Deliverable)
        {
            Expired = false;
            Deliverable = false;
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                Expired = false; // Just to satisfy the out parameter
                Deliverable = false;

                if (!Load(ref myTransaction, ref lTable, SubscriptionId))
                { return false; }

                // Check to see whether this is deliverable at all

                if (lTable.FindByIssueIdSubscriptionId(IssueId, SubscriptionId).UnitsLeft == 0)
                {
                    Deliverable = false;
                    return true; // There is no point in going any further   6 June 2011. Hein
                }
                else
                {
                    Deliverable = true;
                }

                // Deliver what is left
                lTable.FindByIssueIdSubscriptionId(IssueId, SubscriptionId).UnitsLeft = 0;


                // Check to see if this is the last issue to be delivered. 
                // This is NOT the same as to be the last issue in the sequence

                Expired = true;

                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    if (myRow.UnitsLeft > 0)
                    {
                        Expired = false;
                    }
                }

                // Save it to the database

                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdapter.AttachTransaction(ref myTransaction);
                myAdapter.Update(lTable);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static IssueBiz", "Deliver", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static bool ReverseDelivery(ref SqlTransaction myTransaction, int SubscriptionId, int IssueId, int UnitsReturned, int MaxUnits)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                // This could cause a subscription not to be expired anymore!???

                if (UnitsReturned <= 0)
                {
                    throw new Exception("You cannot return nothing or less than nothing. SubscriptionId = " + SubscriptionId.ToString() + " Issueid = " + IssueId.ToString());
                }

                // Load the whole subscription


                if (!Load(ref myTransaction, ref lTable, SubscriptionId)) { return false; }

                int CurrentUnitsLeft = lTable.FindByIssueIdSubscriptionId(IssueId, SubscriptionId).UnitsLeft;

                if ((CurrentUnitsLeft + UnitsReturned) > MaxUnits)
                {
                    throw new Exception("You cannot return more units than you received. SubscriptionId = " + SubscriptionId.ToString() + " Issueid = " + IssueId.ToString());
                }

                lTable.FindByIssueIdSubscriptionId(IssueId, SubscriptionId).UnitsLeft += UnitsReturned;

                // Save it to the database

                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdapter.AttachTransaction(ref myTransaction);
                myAdapter.Update(lTable);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "ReverseDelivery", "");
                    throw new Exception("static IssueBiz" + " : " + "Return" + " : ", ex);
                }
                else
                {
                    return false; // Just bubble it up
                }
            }
        }

        public static string Skip(int SubscriptionId, int IssueId)
        {
            string Progress = "Start";
            SqlConnection lConnection = new SqlConnection();
            SqlTransaction lTransaction;

            // Start the transaction

            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            lTransaction = lConnection.BeginTransaction("Skip");

            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lSubscriptionIssue = new SubscriptionDoc3.SubscriptionIssueDataTable();

                //You can skip only one at as time

                if (!Load(ref lTransaction, ref lSubscriptionIssue, SubscriptionId))
                { return "Error in load"; }

                Progress = "1";

                // Get the units left to be delivered

                SubscriptionDoc3.SubscriptionIssueRow lSubscriptionIssueRow = lSubscriptionIssue.FindByIssueIdSubscriptionId(IssueId, SubscriptionId);

                int UnitsLeft = 0;

                if (lSubscriptionIssueRow != null)
                {
                    UnitsLeft = lSubscriptionIssueRow.UnitsLeft;
                }
                else
                {
                    return "There is no SubscriptionId = " + SubscriptionId.ToString() + "IssueId = " + IssueId.ToString();
                }

                Progress = "2";

                if (UnitsLeft <= 0)
                {
                    return "There are no units left to skip on SubscriptionId = " + SubscriptionId.ToString() + "IssueId = " + IssueId.ToString();
                }

                // Determine the next sequence number to skip to

                int lLargestSequence = 0;

                foreach (SubscriptionDoc3.SubscriptionIssueRow lRow in lSubscriptionIssue)
                {
                    if (lRow.Sequence > lLargestSequence)
                    {
                        lLargestSequence = lRow.Sequence;
                    }
                }

                Progress = "3";

                // Add an additional issue at the backside

                //      Determine the ProductId

                int ProductId = ProductDataStatic.GetProductId(IssueId);

                //      Create and populate the row

                Progress = "4";

                SubscriptionDoc3.SubscriptionIssueRow lNewRow = lSubscriptionIssue.NewSubscriptionIssueRow();

                lNewRow.SubscriptionId = SubscriptionId;

                int NewIssueId = 0;
                {
                    string lResult;

                    if ((lResult = ProductDataStatic.GetIssueId(ProductId, lLargestSequence + 1, out NewIssueId)) != "OK")
                    {
                        return lResult;
                    }
                }

                Progress = "5";

                lNewRow.IssueId = NewIssueId;
                lNewRow.Sequence = lLargestSequence + 1;
                lNewRow.UnitsLeft = UnitsLeft; //Transfer the remaining units from the skipped to the new one
                lSubscriptionIssue.AddSubscriptionIssueRow(lNewRow);

               
                Progress = "7";

                // Delete the relevant issue

                // Remove the skipped row. Refer to Technical Specification\Peculiarities\Skip Logic
                lSubscriptionIssue.FindByIssueIdSubscriptionId(IssueId, SubscriptionId).Delete();

                Progress = "8";


                // Save it to the database
                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();

                Progress = "9";

                myAdapter.AttachTransaction(ref lTransaction);
                myAdapter.Update(lSubscriptionIssue);

                if (!LedgerData.Skip(ref lTransaction, SubscriptionId, IssueId))
                {
                    lTransaction.Rollback("Skip");
                    return "Error in LedgerData.Skip. SubscriptionId= " + SubscriptionId.ToString() + "IssueId= " + IssueId.ToString();
                }

                lTransaction.Commit();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static Issuebiz", "Skip", "Progress = " + Progress);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                lTransaction.Rollback("Skip");
                return "Error in IssueBiz.Skip. " + ex.Message + "SubscriptionId= " + SubscriptionId.ToString() + "IssueId= " + IssueId.ToString();
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool GetSkipped(int SubscriptionId, int IssueId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                bool lResult = false;

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    //You are assuming that the issues are sorted in order Sequence ascending.
                    if (myRow.IssueId == IssueId)
                    {
                        //  Loop until you get the current issue. 
                        break;
                    }

                    if (myRow.UnitsLeft > 0)
                    {
                        // There are some units left before the issue
                        lResult = true;
                    }
                }
                return lResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "GetSkipped", "");
                    throw new Exception("static IssueBiz" + " : " + "GetSkipped" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static bool Cancel(ref SqlTransaction myTransaction, int SubscriptionId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                // Load all the issues of this subscription

                if (!Load(ref myTransaction, ref lTable, SubscriptionId))
                {
                    return false;
                }

                // Remove all indications of any units left

                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    myRow.UnitsLeft = 0;
                }

                // Save it to the database
                Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdapter.AttachTransaction(ref myTransaction);
                myAdapter.Update(lTable);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static IssueBiz", "Cancel", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static bool DeliverOnCredit(int pSubscriptionId, int pIssueId)
        {
            Subs.Data.SubscriptionDoc3.SubscriptionIssueDataTable lSubscriptionIssue = new SubscriptionDoc3.SubscriptionIssueDataTable();
            Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter lAdapter = new Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
            lAdapter.AttachConnection();

            try
            {
                lAdapter.FillBySubscriptionIssue(lSubscriptionIssue, pSubscriptionId, pIssueId);

                if (lSubscriptionIssue[0].UnitsLeft <= 0)
                {
                    return false;
                }


                if (lSubscriptionIssue[0].DeliverOnCredit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static IssueBiz", "DeliverOnCredit", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }

        public static bool DeliveryReversible(int SubscriptionId, int IssueId)
        {
            try
            {
                SubscriptionDoc3.SubscriptionIssueDataTable lTable = new SubscriptionDoc3.SubscriptionIssueDataTable();

                bool Reversible = false;

                //Unitsleft should be zero.

                if (!Load(SubscriptionId, ref lTable)) { throw new Exception("Load failed"); }

                // Count the units left 
                foreach (SubscriptionDoc3.SubscriptionIssueRow myRow in lTable)
                {
                    if (myRow.IssueId == IssueId)
                    {
                        if (myRow.UnitsLeft == 0)
                        {
                            // To reverse an issue, UnitsLeft should be 0.
                            Reversible = true;
                        }
                    }
                }

                // You should not have reversed this issue of this subscription before.





                return Reversible;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static IssueBiz", "WasDelivered", "");
                    throw new Exception("static IssueBiz" + " : " + "WasDelivered" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }

        }


    }
}
