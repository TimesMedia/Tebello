using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Subs.Data
{
   
    public static class LedgerData
    {
        private static readonly string gConnectionString = "";
        private static readonly MIMSDataContext gMimsDataContext = new MIMSDataContext(Settings.ConnectionString);

        static LedgerData()
        {
            // Set the connectionString for this object
            ;
            gConnectionString = Settings.ConnectionString;
        }

        public static void LoadInvoiceForSubscription(int pSubscriptionId, ref LedgerDoc2 pLedgerDoc)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                // Cleanup before you start a new one

                pLedgerDoc.Clear();

                // Get new data

                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[dbo].[MIMS.LedgerData.LoadInvoiceForSubscription]";

                SqlCommandBuilder.DeriveParameters(Command);
                Command.Parameters["@SubscriptionId"].Value = pSubscriptionId;

                Adaptor.SelectCommand = Command;
                Adaptor.Fill(pLedgerDoc.InvoiceBatch);
            }


            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(typeof(WarningException) == ex.GetType() ? 3 : 1, ex.Message, "static.LedgerData", "LoadInvoiceBatchForSubscription", "");
                    throw new Exception("static.LedgerData" + " : " + "LoadInvoiceBatchForSubscription" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static void LoadInvoiceBatch(string pSelector, int pId, ref LedgerDoc2 pLedgerDoc)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                // Cleanup before you start a new one

                pLedgerDoc.Clear();

                // Get new data

                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.LedgerData.LoadInvoiceBatch]";

                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@Type"].Value = pSelector;
                Command.Parameters["@Id"].Value = pId;

                Adaptor.SelectCommand = Command;
                Adaptor.Fill(pLedgerDoc.InvoiceBatch);
            }


            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(typeof(WarningException) == ex.GetType() ? 3 : 1, ex.Message, "static.LedgerData", "LoadInvoiceBatch", "");
                    throw new Exception("static.LedgerData" + " : " + "LoadInvoiceBatch" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }
  
        public static string LoadStatementBatch(ref LedgerDoc2 pLedgerDoc, string pType)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                // Cleanup before you start a new one

                pLedgerDoc.StatementBatch.Clear();

                // Get new data

                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[MIMS.LedgerDoc.StatementBatch.Load]";

                SqlCommandBuilder.DeriveParameters(Command);
                Adaptor.SelectCommand = Command;
                Command.Parameters["@Type"].Value = pType;
                Adaptor.SelectCommand = Command;
                Adaptor.Fill(pLedgerDoc.StatementBatch);

                if (pLedgerDoc.StatementBatch.Rows.Count == 0)
                {
                    throw new Exception("There are no statements to generate.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "LoadStatementBatch", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static int PayU(int pCustomerId, string pPayUReference, string pStatus, int pInvoiceId, Decimal pAmount)
        {
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);
            try
            {
                lConnection.Open();
                SqlCommand Command = new SqlCommand();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@CreditValue", SqlDbType.Decimal);
                Command.Parameters.Add("@PaymentMethod", SqlDbType.Int);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@PayerId"].Value = pCustomerId;
                Command.Parameters["@Operation"].Value = Operation.Pay;
                Command.Parameters["@CreditValue"].Value = pAmount;
                Command.Parameters["@PaymentMethod"].Value = PaymentMethod.PayU;
                Command.Parameters["@InvoiceId"].Value = pInvoiceId;
                Command.Parameters["@ReferenceType3"].Value = "Status";
                Command.Parameters["@Reference3"].Value = pStatus;
                Command.Parameters["@ReferenceType3"].Value = "PayUReference";
                Command.Parameters["@Reference3"].Value = pPayUReference;
                Command.Parameters["@DateFrom"].Value = DateTime.Now;
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "PayU", "CustomerId = " + pCustomerId.ToString());
                    throw new Exception("static LedgerData" + " : " + "PayU" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally { 
                if(lConnection.State == ConnectionState.Open) {
                lConnection.Close();
                }
            }
        }



        public static int Pay(ref SqlTransaction myTransaction, PaymentData.PaymentRecord myRecord)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@CreditValue", SqlDbType.Decimal);
                Command.Parameters.Add("@PaymentMethod", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceTypeId", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@PayerId"].Value = myRecord.CustomerId;
                Command.Parameters["@Operation"].Value = Operation.Pay;
                Command.Parameters["@CreditValue"].Value = myRecord.Amount;
                Command.Parameters["@PaymentMethod"].Value = myRecord.PaymentMethod;
                Command.Parameters["@ReferenceTypeId"].Value = myRecord.ReferenceTypeId;
                Command.Parameters["@Reference"].Value = myRecord.Reference;
                Command.Parameters["@InvoiceId"].Value = myRecord.InvoiceId;
                Command.Parameters["@ReferenceType3"].Value = myRecord.ReferenceType3;
                Command.Parameters["@Reference3"].Value = myRecord.Reference3;

                Command.Parameters["@DateFrom"].Value = myRecord.Date;
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Pay", "CustomerId = " + myRecord.CustomerId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Pay" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static int ReversePaymentCheck(int pPaymentTransactionId)
        {
            try
            {
                MIMS_LedgerData_ReversePaymentCheckResult lResult = gMimsDataContext.MIMS_LedgerData_ReversePaymentCheck(pPaymentTransactionId).Single();
                return (int)lResult.Column1;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "ReversePaymentCheck", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }


        }

        public static int ReversePayment(ref SqlTransaction myTransaction, int pPayerId, decimal pAmount, int pPaymentTransactionId, string pExplanation)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DebitValue", SqlDbType.Decimal);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);

                Command.Parameters["@PayerId"].Value = pPayerId;
                Command.Parameters["@Operation"].Value = Operation.ReversePayment;
                Command.Parameters["@DebitValue"].Value = pAmount;
                Command.Parameters["@Reference2"].Value = pPaymentTransactionId.ToString();
                Command.Parameters["@ReferenceType3"].Value = "Reason";
                Command.Parameters["@Reference3"].Value = pExplanation;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "ReversePayment", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }

        public static bool Refund(ref SqlTransaction pSqlTransactionId, int pPaymentTransactionId, int pPayerId, decimal pAmount)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = pSqlTransactionId.Connection;
                Command.Transaction = pSqlTransactionId;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DebitValue", SqlDbType.Decimal);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@PayerId"].Value = pPayerId;
                Command.Parameters["@Operation"].Value = Operation.Refund;
                Command.Parameters["@DebitValue"].Value = pAmount;
                Command.Parameters["@ReferenceType2"].Value = "PaymentTransactionId";
                Command.Parameters["@Reference2"].Value = pPaymentTransactionId.ToString();
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "Refund", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static bool AssignRefund(int pRefundTransactionId, int pPayerId, int pPaymentTransactionId)
        {
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);
            try
            {
                SqlCommand Command = new SqlCommand();

                lConnection.Open();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = lConnection;
                Command.CommandText = "[dbo].[MIMS.LedgerData.AssignRefund]";

                Command.Parameters.Add("@RefundTransactionId", SqlDbType.Int);
                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@PaymentTransactionId", SqlDbType.Int);

                Command.Parameters["@RefundTransactionId"].Value = pRefundTransactionId;
                Command.Parameters["@PayerId"].Value = pPayerId;
                Command.Parameters["@PaymentTransactionId"].Value = pPaymentTransactionId;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "AssignRefund", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static bool WriteOffMoney(ref SqlTransaction myTransaction, PaymentData.PaymentRecord myRecord)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@CreditValue", SqlDbType.Decimal);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);
                Command.Parameters.Add("@InvoiceId", SqlDbType.NVarChar, 80);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@PayerId"].Value = myRecord.CustomerId;
                Command.Parameters["@Operation"].Value = Operation.WriteOffMoney;
                Command.Parameters["@CreditValue"].Value = myRecord.Amount;
                Command.Parameters["@Explanation"].Value = myRecord.Explanation;
                Command.Parameters["@InvoiceId"].Value = myRecord.InvoiceId;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                //Command.ExecuteNonQuery();
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "WriteOffMoney", "Payer = " + myRecord.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static bool ReverseWriteOffMoney(ref SqlTransaction myTransaction, int pTransactionId, int pInvoiceId, int pPayerId, decimal pAmount, string pExplanation)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);

                Command.Parameters.Add("@DebitValue", SqlDbType.Decimal);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 80);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@PayerId"].Value = pPayerId;
                Command.Parameters["@Operation"].Value = Operation.ReverseWriteOffMoney;
                Command.Parameters["@InvoiceId"].Value = pInvoiceId;
                Command.Parameters["@DebitValue"].Value = pAmount;
                Command.Parameters["@Explanation"].Value = pExplanation;
                Command.Parameters["@ReferenceType2"].Value = "Writeoff transactionId";
                Command.Parameters["@Reference2"].Value = pTransactionId.ToString();
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return true;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "ReverseWriteoffMoney", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static void Expire(ref SqlTransaction myTransaction, int SubscriptionId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";
                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Expire;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Expire", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Expire" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static void Statement(int pPayerId, int pStatementId, decimal pAmount)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@StatementId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@Value", SqlDbType.Decimal);

                Command.Parameters["@PayerId"].Value = pPayerId;
                Command.Parameters["@StatementId"].Value = pStatementId;
                Command.Parameters["@Operation"].Value = Operation.Statement;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@Value"].Value = pAmount;

                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Statement", "StatementId = " + pStatementId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Statement" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static void Suspend(ref SqlTransaction myTransaction, int SubscriptionId, string Reference)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Suspend;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Suspend", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Suspend" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static bool Skip(ref SqlTransaction myTransaction, int SubscriptionId, int IssueId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";
                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@IssueId", SqlDbType.Int);

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Skip;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@IssueId"].Value = IssueId;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "Skip", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static void Resume(ref SqlTransaction myTransaction, int SubscriptionId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";
                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Resume;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Resume", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Resume" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static bool InitialiseSubscription(ref SqlTransaction myTransaction, int pSubscriptionId, decimal pTotalCost)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@Value", SqlDbType.Decimal);

                Command.Parameters["@SubscriptionId"].Value = pSubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Init_Sub;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@ReferenceType2"].Value = "Total value";
                Command.Parameters["@Reference2"].Value = pTotalCost.ToString("########0.00");
                Command.Parameters["@Value"].Value = pTotalCost;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static LedgerData", "InitialiseSubscription", "SubscriptionId = " + pSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }


  
        public static bool Invoice(ref SqlTransaction pSqlTransaction, int PayerId, int InvoiceId, int LastTransactionId, decimal pTotalValue)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = pSqlTransaction.Connection;
                Command.Transaction = pSqlTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);

                Command.Parameters["@PayerId"].Value = PayerId;
                Command.Parameters["@Operation"].Value = Operation.VATInvoice;
                Command.Parameters["@InvoiceId"].Value = InvoiceId;
                Command.Parameters["@ReferenceType3"].Value = "From transactionId of batch";
                Command.Parameters["@Reference3"].Value = LastTransactionId;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@Explanation"].Value = "Total value = " + pTotalValue.ToString("########0.00");

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "Invoice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                lConnection.Close();
            }
        }

        public static bool Invoice(int PayerId, int InvoiceId, int LastTransactionId, decimal pTotalValue)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@Value", SqlDbType.Decimal);

                Command.Parameters["@PayerId"].Value = PayerId;
                Command.Parameters["@Operation"].Value = Operation.VATInvoice;
                Command.Parameters["@InvoiceId"].Value = InvoiceId;
                Command.Parameters["@ReferenceType3"].Value = "From transactionId of batch";
                Command.Parameters["@Reference3"].Value = LastTransactionId;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@Value"].Value = pTotalValue;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "Invoice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                lConnection.Close();
            }
        }

        public static bool InvoiceDirective(ref SqlTransaction pSqlTransaction, int pInvoiceId, byte[] pDirective)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = pSqlTransaction.Connection;
                Command.Transaction = pSqlTransaction;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandText = "[dbo].[MIMS.LedgerData.InvoiceDirective]";
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@Directive", SqlDbType.NVarChar, 20);
                Command.Parameters["@InvoiceId"].Value = pInvoiceId;
                Command.Parameters["@Directive"].Value = pDirective;
                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "InvoiceDirective", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                lConnection.Close();
            }
        }

        public static void CreditNote(int SubscriptionId, int InvoiceId, int Reduction, string CreditNoteNumber, string Explanation, decimal pReductionValue)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceTypeId", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@InvoiceId", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType3", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference3", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);
                Command.Parameters.Add("@Value", SqlDbType.Decimal);

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.CreditNote;
                Command.Parameters["@ReferenceTypeId"].Value = 7; //Reduction in units
                Command.Parameters["@Reference"].Value = Reduction.ToString();
                Command.Parameters["@InvoiceId"].Value = InvoiceId;
                Command.Parameters["@ReferenceType3"].Value = "Creditnote number";
                Command.Parameters["@Reference3"].Value = CreditNoteNumber;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@Explanation"].Value = Explanation;
                Command.Parameters["@Value"].Value = -pReductionValue;

                Command.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "CreditNote", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "CreditNote" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static void Return(int SubscriptionId, decimal Money, int IssueId, int UnitsReturned, string Reference, string Reason)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@CreditValue", SqlDbType.Decimal);
                Command.Parameters.Add("@CreditUnits", SqlDbType.Int);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Return;
                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@IssueId"].Value = IssueId;
                Command.Parameters["@CreditValue"].Value = Money;
                Command.Parameters["@CreditUnits"].Value = UnitsReturned;
                Command.Parameters["@Explanation"].Value = Reason;

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "Return", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Return" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool ReturnCheck(int SubscriptionId, int IssueId)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                int lHits = 0;

                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[dbo].[MIMS.LedgerData.ReturnCheck]";
                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@IssueId"].Value = IssueId;
                lHits = (int)Command.ExecuteScalar();

                if (lHits > 0)
                {
                    // This issue has been returned already.
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "ReturnCheck", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "Return" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static void WriteOffStock(int SubscriptionId, decimal Money, int IssueId, int Units, string Reference, string Reason)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {
                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;

                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@CreditValue", SqlDbType.Decimal);
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@CreditUnits", SqlDbType.Int);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar, 200);

                // Record the credit given to the customer

                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.Credit;
                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@IssueId"].Value = IssueId;
                Command.Parameters["@CreditValue"].Value = Money;
                Command.Parameters["@CreditUnits"].Value = Units;
                Command.Parameters["@Explanation"].Value = Reason;
                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "WriteOff", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "WriteOff" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static void CancelSubscription(ref SqlTransaction myTransaction, int SubscriptionId, string Reference)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@SubscriptionId"].Value = SubscriptionId;
                Command.Parameters["@Operation"].Value = Operation.CancelSubscription;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "CancelSubscription", "SubscriptionId = " + SubscriptionId.ToString());
                    throw new Exception("static LedgerData" + " : " + "CancelSubscription" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static void CancelCustomer(ref SqlTransaction myTransaction, int CustomerId, string Reference)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@Reference", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@PayerId"].Value = CustomerId;
                Command.Parameters["@Operation"].Value = Operation.CancelCustomer;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "CancelCustomer", "CustomerId = " + CustomerId.ToString());
                    throw new Exception("static LedgerData" + " : " + "CancelCustomer" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static int DuplicatePayment(int PayerId, string Reference, decimal Value)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {

                // Check for duplicates
                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.LedgerData.DuplicatePayment]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@PayerId"].Value = PayerId;
                Command.Parameters["@Reference"].Value = Reference;
                Command.Parameters["@Value"].Value = Value;
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "DuplicatePayment", "PayerId = " + PayerId.ToString());
                    throw new Exception("static LedgerData" + " : " + "DuplicatePayment" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static int GetPayerByStatement(int pStatementId)
        {
            SqlConnection lConnection = new SqlConnection();

            try
            {

                // Check for duplicates
                SqlCommand Command = new SqlCommand();
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[dbo].[MIMS.LedgerData.GetPayerByStatement]";
                SqlCommandBuilder.DeriveParameters(Command);
                Command.Parameters["@StatementId"].Value = pStatementId;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    return (int)Result;
                }
                else
                {
                    return 0;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static LedgerData", "GetPayerByStatement", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static bool Deliver(ref SqlTransaction myTransaction, int pSubscriptionId, int pIssueId, decimal pUnitPrice, int pUnitsPerIssue)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;

                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@DebitValue", SqlDbType.Decimal);
                Command.Parameters.Add("@DebitUnits", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = pSubscriptionId;
                Command.Parameters["@IssueId"].Value = pIssueId;
                Command.Parameters["@Operation"].Value = Operation.Deliver;
                Command.Parameters["@DebitValue"].Value = pUnitPrice * pUnitsPerIssue;
                Command.Parameters["@DebitUnits"].Value = pUnitsPerIssue;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "|static LedgerData", "Deliver", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static void ChangeRenewalNotice(ref SqlTransaction pTransaction, SubscriptionData3 pSubscriptionData, bool pRenew)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = pTransaction.Connection;
                Command.Transaction = pTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = pSubscriptionData.SubscriptionId;
                Command.Parameters["@PayerId"].Value = pSubscriptionData.PayerId;
                Command.Parameters["@Operation"].Value = (int)Operation.ChangeRenewalNotice;
                Command.Parameters["@ReferenceType2"].Value = "RenewalNotice";
                Command.Parameters["@Reference2"].Value = pRenew.ToString();
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static LedgerData", "ChangeRenewalNotice", "Renew = " + pRenew.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        public static void ChangeAutomaticRenewal(ref SqlTransaction pTransaction, SubscriptionData3 pSubscriptionData, bool pAutomaticRenew)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = pTransaction.Connection;
                Command.Transaction = pTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = pSubscriptionData.SubscriptionId;
                Command.Parameters["@PayerId"].Value = pSubscriptionData.PayerId;
                Command.Parameters["@Operation"].Value = (int)Operation.ChangeAutomaticRenewal;
                Command.Parameters["@ReferenceType2"].Value = "AutomaticRenewal";
                Command.Parameters["@Reference2"].Value = pAutomaticRenew.ToString();
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static LedgerData", "ChangeAutomaticRenewal", "AutomaticRenew = " + pAutomaticRenew.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        public static void ChangeReceiver(ref SqlTransaction myTransaction, SubscriptionData3 mySubscriptionData, int PreviousReceiverId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@SubscriptionId", SqlDbType.Int);
                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@ReferenceType2", SqlDbType.NVarChar, 20);
                Command.Parameters.Add("@Reference2", SqlDbType.NVarChar, 40);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);

                Command.Parameters["@SubscriptionId"].Value = mySubscriptionData.SubscriptionId;
                Command.Parameters["@PayerId"].Value = mySubscriptionData.PayerId;
                Command.Parameters["@Operation"].Value = (int)Operation.ChangeReceiver;
                Command.Parameters["@ReferenceType2"].Value = "Previous ReceiverId";
                Command.Parameters["@Reference2"].Value = PreviousReceiverId.ToString();
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static LedgerData", "ChangeReceiver", "");
                    throw new Exception("static LedgerData" + " : " + "ChangeReceiver" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static void UpdateCustomer(CustomerData3 pCustomerData)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
                Command.Parameters.Add("@Explanation", SqlDbType.NVarChar);

                Command.Parameters["@PayerId"].Value = pCustomerData.CustomerId;
                Command.Parameters["@Operation"].Value = Operation.UpdateCustomer;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
                Command.Parameters["@Explanation"].Value = pCustomerData.Address1;

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "LedgerDataStatic", "UpdateCustomer", "");
                    throw new Exception("LedgerDataStatic" + " : " + "Method" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static void InitialiseCustomer(CustomerData3 pCustomerData)
        {
            SqlConnection lConnection = new SqlConnection();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                lConnection.ConnectionString = gConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandText = "dbo.[MIMS.LedgerData.Update]";

                Command.Parameters.Add("@PayerId", SqlDbType.Int);
                Command.Parameters.Add("@Operation", SqlDbType.Int);
                Command.Parameters.Add("@DateFrom", SqlDbType.DateTime);
 
                Command.Parameters["@PayerId"].Value = pCustomerData.CustomerId;
                Command.Parameters["@Operation"].Value = Operation.Init_Customer;
                Command.Parameters["@DateFrom"].Value = System.DateTime.Now;
 
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "LedgerDataStatic", "InitialiseCustomer", "");
                    throw new Exception("LedgerDataStatic" + " : " + "Method" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                lConnection.Close();
            }
        }




    }
}
