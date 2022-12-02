using Subs.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;

using static Subs.Data.CustomerData3;
using System.IO;
using System.Net.Http.Headers;

namespace Subs.Business
{
    public static class CustomerBiz
    {
        #region Global variables and constructor
 
        public enum PaymentValidationResult
        {
            OK = 1,
            Duplicate = 2,
            TooMuch = 3,
            PayerCancelled = 4,
            NoReference = 5,
            InvalidInvoice = 6,
            InvalidAllocationNumber = 7,
            PayerDoesNotExist = 8,
            NegativeNumber = 9,
            NoInvoicesPast3years = 10
        }


        static CustomerBiz()
        {
           
        }
        #endregion

        #region Payment allocation

        public static string InvoiceBalances(int pPayerId, out List<OutstandingInvoice> pOutstanding)
        {
            pOutstanding = new List<OutstandingInvoice>();

            try
            {
                List<Subs.Data.InvoicesAndPayments> lInvoices;

                {
                    string lResult;
                    if ((lResult = CustomerData3.PopulateInvoice(pPayerId, out lInvoices)) != "OK")
                    {

                        return lResult;
                    }
                }


                foreach (Subs.Data.InvoicesAndPayments item in lInvoices)
                {
                    if (item.LastRow && item.InvoiceId != 0 && item.InvoiceId != 999999999)
                    {
                        if (item.InvoiceBalance > 1)
                        {
                            // Write out a tuple
                            pOutstanding.Add(new OutstandingInvoice() { InvoiceId = item.InvoiceId, Balance = item.InvoiceBalance });
                        }
                    }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "InvoiceBalances", "PayerId = " + pPayerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in InvoiceBalance: " + ex.Message;
            }

        }

        public static decimal DistributePaymentToInvoice(int pPaymentTransactionId, decimal pAmount, OutstandingInvoice pInvoice)
        {
            try
            {
                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString); 

                decimal lPaymentRemaining = -pAmount;
                decimal lAmount = 0M;

                if (pInvoice.Balance >= lPaymentRemaining)
                {
                    // Put everything into this invoice
                    lAmount = lPaymentRemaining;
                    lPaymentRemaining = 0;
                }
                else
                {
                    // Put onto this Invoice as much as you can.
                    lAmount = pInvoice.Balance;
                    lPaymentRemaining = lPaymentRemaining - pInvoice.Balance;
                }

                // Add a record to the InvoicePayment table

                lContext.MIMS_InvoicePayment_Insert(pInvoice.InvoiceId, pPaymentTransactionId, lAmount);
                return lPaymentRemaining;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "DistributePaymentToInvoice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }


        private static bool DistributePaymentToOutstandingInvoices(int pPaymentTransactionId, decimal pAmount, List<OutstandingInvoice> pOutstandingInvoices)
        {
            try
            {
                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                int lInvoiceId = 0;
                decimal lAmount = 0;
                decimal lPaymentRemaining = -pAmount;
                
                foreach (OutstandingInvoice lInvoice in pOutstandingInvoices)
                {
                    lInvoiceId = lInvoice.InvoiceId;

                    if (lInvoice.Balance >= lPaymentRemaining)
                    {
                        // Put everything into this invoice
                        lAmount = lPaymentRemaining;
                        lPaymentRemaining = 0;
                    }
                    else
                    {
                        // Put onto this Invoice as much as you can.
                        lAmount = lInvoice.Balance;
                        lPaymentRemaining = lPaymentRemaining - lInvoice.Balance;
                    }

                    // Add a record to the InvoicePayment table

                    lContext.MIMS_InvoicePayment_Insert(lInvoiceId, pPaymentTransactionId, lAmount);

                    if (lPaymentRemaining < 1)
                    {
                        break;
                    }

                } // End of foreach loop - Invoices ***************************************************************************************

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "DistributePaymentToOutstandingInvoices", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        private static string DistributePayment(int pPayerId, int pPaymentTransactionId, decimal pAmount)
        {
            MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);

            PaymentData.PaymentRecord myRecord = new PaymentData.PaymentRecord();
            List<OutstandingInvoice> lOutstandingInvoices;
            try
            {

                // Now distriubute the payment over the invoices 

                {
                    string lResult;

                    if ((lResult = CustomerBiz.InvoiceBalances(pPayerId, out lOutstandingInvoices)) != "OK")
                    {
                        return lResult;
                    }
                }

                if (lOutstandingInvoices.Count == 0)
                {
                    return "Nothing found";
                }
                else
                {
                    if (!DistributePaymentToOutstandingInvoices(pPaymentTransactionId, pAmount, lOutstandingInvoices))
                    {
                        return "Error in DistributePaymentToSpecificInvoices ";
                    }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "DistributePaymentToFirstInvoices", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "error in DistributePayment: " + ex.Message;
            }

        }

        public static string SynchronizeLiability(CustomerData3 pCustomerData, [CallerMemberName] string pCaller = null)
        {
            try
            {
                decimal lCalculatedLiability = 0M;
                List<LiabilityRecord> lLiabilityRecords = new List<LiabilityRecord>();

                {
                    string lResult2;

                    if ((lResult2 = CustomerData3.CalulateLiability(pCustomerData.CustomerId, ref lLiabilityRecords, ref lCalculatedLiability)) != "OK")
                    {
                        if (!lResult2.Contains("Nothing"))
                        {
                            return lResult2;
                        }
                    }
                }

                pCustomerData.Liability = lCalculatedLiability;


                string lResult;
                if ((lResult = pCustomerData.Update()) != "OK")
                {
                    return lResult;
                }

                ExceptionData.WriteException(2, "CheckpointModification from " + pCaller, "static CustomerBiz", "SynchronizeLiability", "CustomerId = "
                    + pCustomerData.CustomerId.ToString() + " Calculated = " + lCalculatedLiability.ToString());

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "SynchronizeLiability", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in SynchronizeLiability: " + ex.Message;
            }

        }

        public static string DistributeAllPayments(int pPayerId)

        {
            try
            {
                CustomerData3 lCustomer = new CustomerData3(pPayerId);

                if (!lCustomer.AutomaticPaymentAllocation)
                {
                    // You will have to do these allocations manually.
                    return "OK";
                }

                Deallocate(pPayerId);

                string lResult;

                if ((lResult = CustomerData3.PopulateInvoice(pPayerId, out List<InvoicesAndPayments> lInvoicesAndPayments)) != "OK")
                {
                    if (lResult.Contains("Nothing"))
                    {
                        return "OK";
                    }
                    else
                    {
                        return lResult;
                    }
                }

                // Find all the payment-related record balances

                List<InvoicesAndPayments> lPayments;

                lPayments = lInvoicesAndPayments.Where(p => p.LastRow && (
                                                            p.OperationId == (int)Operation.Pay)
                                                         || p.OperationId == (int)Operation.Balance
                                                         || p.OperationId == (int)Operation.Refund
                                                         || p.OperationId == (int)Operation.ReversePayment)
                                                .ToList();


                foreach (InvoicesAndPayments lPayment in lPayments)
                {

                    if (lPayment.InvoiceBalance >= 0)
                    {
                        continue;
                    }

                    {
                        string lResult2;

                        if ((lResult2 = CustomerBiz.DistributePayment(pPayerId,
                                                                      lPayment.TransactionId,
                                                                      lPayment.InvoiceBalance)) != "OK")
                        {
                            if (lResult2.Contains("Nothing"))
                            {
                                continue;
                            }
                            else
                            {
                                return lResult2;
                            }
                        }
                    }

                }  // end of lPayments loop

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "ReallocatePayments", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in ReallocatePayments " + ex.Message;
            }

        }

        public static void Deallocate(int pPayerId)
        {
            MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);

            // Get the current payments


            CustomerData3.PopulateInvoice(pPayerId, out List<InvoicesAndPayments> lInvoicesAndPayments);

            // Delete all current allocations

            IEnumerable<int> lAllocationsToDelete
                = lInvoicesAndPayments.Where(p => (p.OperationId == (int)Operation.Pay || p.OperationId == (int)Operation.Balance))
                                      .ToList()
                                      .Select(p => p.TransactionId);

            foreach (int lTransactionId in lAllocationsToDelete)
            {
                lContext.MIMS_InvoicePayment_DeleteByPaymentTransactionId(lTransactionId);
            }
        }

        public static void DeallocateByInvoiceId(int pInvoiceId)
        {
            try
            {
                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);

                // Get the current payments
                lContext.MIMS_InvoicePayment_DeleteByInvoiceId((int)pInvoiceId);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "staticCustomerBiz", "DeallocateByInvoiceId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
           
        }


        #endregion

        #region Payment

        public static string ValidatePayment(ref Subs.Data.PaymentData.PaymentRecord pRecord, ref PaymentValidationResult pResult, ref string pErrorMessage)
        {
            try
            {
                //Check if the value is positive

                if (pRecord.Amount < 0)
                {
                    pErrorMessage = "If this is a bounced payment, please mark it as such. ";
                    pResult = PaymentValidationResult.NegativeNumber;
                    return "OK";
                }

                //Check on the status of the payer

                if (!CustomerData3.Exists(pRecord.CustomerId))
                {
                    pErrorMessage = "Payer " + pRecord.CustomerId.ToString() + " does not exist";
                    pResult = PaymentValidationResult.PayerDoesNotExist;
                    return "OK";
                }

                CustomerData3 myCustomerData = new CustomerData3(pRecord.CustomerId);

                //if (myCustomerData.Status == CustomerStatus.Cancelled)
                //{
                //    pErrorMessage = "This payer has been cancelled";
                //    pResult = PaymentValidationResult.PayerCancelled;
                //    return "OK";
                //}

                // Check to see if there is .....an applicable invoice

                List<OutstandingInvoice> lOutstandingInvoices;
                {
                    string lResult;

                    if ((lResult = CustomerBiz.InvoiceBalances(pRecord.CustomerId, out lOutstandingInvoices)) != "OK")
                    {
                        pErrorMessage = lResult;
                        pResult = PaymentValidationResult.NoInvoicesPast3years;
                        return "OK";
                    }
                }

                if (lOutstandingInvoices.Count == 0)
                {
                    pErrorMessage = "This payer has no applicable invoices to pay against.";
                    pResult = PaymentValidationResult.InvalidInvoice;
                    return "OK";
                }


                // Check to see if this is a valid allocation number

                //if (pRecord.ReferenceTypeId == 5)
                //{
                //    // This is supposed to be an allocation number

                //    Match myMatch1 = myRegEx1.Match(pRecord.Reference);
                //    if (!myMatch1.Success)
                //    {
                //        pErrorMessage = "This is not a valid allocation number.";
                //        pResult = PaymentValidationResult.InvalidAllocationNumber;
                //        return "OK";
                //    }
                //}


                // Test to ensure that this is not a duplicate entry

                if (LedgerData.DuplicatePayment(pRecord.CustomerId,
                pRecord.Reference, pRecord.Amount) > (int)0)
                {
                    pErrorMessage = "There is already a payment with this reference number!";
                    ExceptionData.WriteException(5, "Duplicate Payment detected", "CustomerBizStatic", "ValidatePayment", "Reference = " + pRecord.Reference);
                    pResult = PaymentValidationResult.Duplicate;
                    return "OK";
                }

                // See what is due


                if ((pRecord.Amount - myCustomerData.Due) > 1)
                {
                    pErrorMessage = "The most this guy owes us is " + myCustomerData.Due.ToString("#######0.00");
                    pResult = PaymentValidationResult.TooMuch;
                    return "OK";
                }
                pResult = PaymentValidationResult.OK;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "ValidatePayment", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in ValidatePayment: " + ex.Message;
            }
        }

        public static string Pay(ref PaymentData.PaymentRecord pRecord, out int pTransactionId)
        {
            pTransactionId = 0;

            // Start the transaction

            if (LedgerData.DuplicatePayment(pRecord.CustomerId, pRecord.Reference, pRecord.Amount) > 1)
            {
                return "Duplicate payment not allowed";
            }

            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;
            SqlTransaction lTransaction;
            lConnection.Open();
            lTransaction = lConnection.BeginTransaction("Pay");

            try
            {

                if (pRecord.Amount < 0)
                {
                    // This is a bogus payment
                    lTransaction.Rollback("Pay");
                    return "What kind of payment is this? I accept only positive numbers.";
                }

                // Update the liability on the payer
                if (!CustomerData3.AddToLiability(ref lTransaction, pRecord.CustomerId, pRecord.Amount))
                {
                    lTransaction.Rollback("Pay");
                    return "Error in updating liability";
                }

                pTransactionId = LedgerData.Pay(ref lTransaction, pRecord);

                lTransaction.Commit();

                {
                    string lResult;

                    if ((lResult = DistributeAllPayments(pRecord.CustomerId)) != "OK")
                    {
                        return lResult;
                    }
                }


                {
                    string lResult;

                    if ((lResult = ProductBiz.DeliverElectronic(pRecord.CustomerId)) != "OK")
                    {
                        // Ignore this, you will catch it on the next general delivery run.
                    }
                }

                return "OK";

            }
            catch (Exception ex)
            {
                lTransaction.Rollback("Pay");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "Pay", "PayerId = " + pRecord.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }
        public static string ReversePayment(CustomerData3 pCustomerData, int pPaymentTransactionId, decimal pAmount, string pExplanation, out int pReverseTransactionId)
        {
            pReverseTransactionId = 0;

            SqlTransaction myTransaction;
            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("ReversePayment");

            try
            {
                if (LedgerData.ReversePaymentCheck(pPaymentTransactionId) > 0)
                {
                    return "This payment has been reversed already.";
                }

                // Update Liability

                CustomerData3 myCustomerData = new CustomerData3(pCustomerData.CustomerId);
                myCustomerData.Liability = myCustomerData.Liability - pAmount;

                {
                    string lResult;

                    if ((lResult = myCustomerData.UpdateInTransaction(ref myTransaction)) != "OK")
                    {
                        myTransaction.Rollback("ReversePayment");
                        return lResult;
                    }
                }

                // Create a transaction entry

                pReverseTransactionId = LedgerData.ReversePayment(ref myTransaction, pCustomerData.CustomerId, pAmount, pPaymentTransactionId, pExplanation);

                if (pReverseTransactionId == 0)
                {
                    myTransaction.Rollback("ReversePayment");
                    return "ReversePayment failed";
                }

                // Remove the InvoicePayment record.

                if (!CustomerData3.RemoveInvoicePayment(ref myTransaction, pPaymentTransactionId))
                {
                    myTransaction.Rollback("ReversePayment");
                    return "RemoveinvoicePayment failed";
                }

                // Done

                myTransaction.Commit();

                return DistributeAllPayments(pCustomerData.CustomerId);

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "ReversePayment", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                myTransaction.Rollback("ReversePayment");
                return "ReversePayment failed due to a technical error";
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static string Refund(int pPaymentTransactionId, int pPayerId, decimal pRefundAmount)
        {
            // Start the transaction
            SqlTransaction lSqlTransaction;
            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            lSqlTransaction = lConnection.BeginTransaction("Refund");
            try
            {

                // Update the liability on the payer
                if (!CustomerData3.AddToLiability(ref lSqlTransaction, pPayerId, -pRefundAmount))
                {
                    lSqlTransaction.Rollback("Refund");
                    return "Error in Refund ";
                }

                if (!LedgerData.Refund(ref lSqlTransaction, pPaymentTransactionId, pPayerId, pRefundAmount))
                {
                    lSqlTransaction.Rollback("Refund");
                    return "Error in Refund ";
                }

                lSqlTransaction.Commit();

                return DistributeAllPayments(pPayerId);

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "Refund", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in Refund " + ex.Message;
            }
            finally
            {
                if (lConnection.State == System.Data.ConnectionState.Open)
                {
                    lConnection.Close();
                }
            }
        }

        public static bool WriteOffMoney(ref PaymentData.PaymentRecord pRecord, out int pTransactionId)
        {
            // Start the transaction

            SqlTransaction myTransaction;
            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("WriteOffMoney");
            pTransactionId = 0;
            try
            {
                // Update the liability on the payer
                if (!CustomerData3.AddToLiability(ref myTransaction, pRecord.CustomerId, pRecord.Amount))
                {
                    myTransaction.Rollback("WriteOffMoney");
                    return false;
                }

                if (!LedgerData.WriteOffMoney(ref myTransaction, pRecord))
                {
                    myTransaction.Rollback("WriteOffMoney");
                    return false;
                }

                myTransaction.Commit();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "WriteOffMoney", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                if (lConnection.State == System.Data.ConnectionState.Open)
                {
                    lConnection.Close();
                }
            }
        }

        public static string ReverseWriteOffMoney(int pTransactionId, int pInvoiceId, int pPayerId, decimal pAmount, string pExplanation)
        {
            Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter lTransactionAdapter = new Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter();
            lTransactionAdapter.AttachConnection();

            // See if the reversal has not occured already in the past

            int lNumberOfReversals = (int)lTransactionAdapter.CheckReference(pExplanation, (int)Operation.ReverseWriteOffMoney, pPayerId);

            if (lNumberOfReversals > 0)
            {
                return "This writeoff reversal has already been done in the past";
            }

            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;

            // Start the transaction
            SqlTransaction lSqlTransaction;
            lConnection.Open();
            lSqlTransaction = lConnection.BeginTransaction("ReverseWriteOffMoney");

            try
            {

                // Update the liability on the customer

                if (!CustomerData3.AddToLiability(ref lSqlTransaction, pPayerId, -pAmount))
                {
                    lSqlTransaction.Rollback("ReverseWriteOffMoney");
                    return "Error in updating liability on customer " + pPayerId.ToString();
                }

                if (!LedgerData.ReverseWriteOffMoney(ref lSqlTransaction, pTransactionId, pInvoiceId, pPayerId, pAmount, pExplanation))
                {
                    lSqlTransaction.Rollback("ReverseWriteOffMoney");
                    return "Error in posting to the ledger. " + pPayerId.ToString();
                }


                // Done

                lSqlTransaction.Commit();
                return "OK";

            }

            catch (Exception ex)
            {
                lSqlTransaction.Rollback("ReverseWriteOffMoney");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "ReverseWriteOffMoney", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }


        #endregion

        #region Customer related
        public static string UpdateCustomer(CustomerData3 pCustomerData)
        {

            // Start the transaction
            SqlTransaction myTransaction;
            SqlConnection lConnection = new SqlConnection();
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("UpdateCustomer");

            try
            {
                // Update the customer

                {
                    string lResult;

                    if ((lResult = pCustomerData.UpdateInTransaction(ref myTransaction)) != "OK")
                    {
                        myTransaction.Rollback("UpdateCustomer");
                        return lResult;
                    }
                }

                LedgerData.UpdateCustomer(pCustomerData);

                // Done

                myTransaction.Commit();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "UpdateCustomer", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                myTransaction.Rollback("UpdateCustomer");
                return "UpdateCustomer failed due to a technical error";
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static string ValidateDuplicate(CustomerData3 pCustomerData)
        {
            // Do I have this customer already

            Subs.Data.CustomerDoc2TableAdapters.CustomerTableAdapter lCustomerAdapter = new Subs.Data.CustomerDoc2TableAdapters.CustomerTableAdapter();
            CustomerDoc2.CustomerDataTable lCustomerTable = new CustomerDoc2.CustomerDataTable();
            lCustomerAdapter.AttachConnection();

            try
            {
                if (string.IsNullOrWhiteSpace(pCustomerData.Initials)) return "Initials are compulsory";
                if (string.IsNullOrWhiteSpace(pCustomerData.Surname)) return "Surname is compulsory";
                if (string.IsNullOrWhiteSpace(pCustomerData.CellPhoneNumber)) return "Cellphone is compulsory";
                if (string.IsNullOrWhiteSpace(pCustomerData.EmailAddress)) return "EmailAddress is compulsory";

                lCustomerAdapter.Like(lCustomerTable,
                pCustomerData.Initials,
                pCustomerData.Surname,
                pCustomerData.CellPhoneNumber,
                pCustomerData.EmailAddress,
                pCustomerData.CompanyId);

                bool lKeep = false;


                foreach (Subs.Data.CustomerDoc2.CustomerRow lRow in lCustomerTable)
                {
                    char[] lInitials = pCustomerData.Initials.ToCharArray();

                    lKeep = false;

                    foreach (char lChar in lInitials)
                    {
                        if (lRow.Initials.Contains(lChar))
                        {
                            // If there is at least one overlapping initial character, this could be a duplicate
                            lKeep = true;
                        }
                    }

                    if (lKeep == false)
                    {
                        lRow.Delete();
                    }
                }

                lCustomerTable.AcceptChanges();

                if (lCustomerTable.Rows.Count > 0)
                {
                    return "You seem to be an existing customer. Please contact MIMS to clarify the issue.";
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static CustomerBiz", "ValidateDuplicate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed in ValidateDuplicate: " + ex.Message;
            }
        }

        public static string DestroyCustomer(ref Subs.Data.CustomerData3 pCustomerData, string pReason)
        {
            SqlConnection lConnection = new SqlConnection();
            int lCustomerId = pCustomerData.CustomerId;

            try
            {
                if (pCustomerData.Liability > 1)
                {
                    return "You cannot cancel a customer while we owe him money!";
                }

                if (pCustomerData.Liability < -1)
                {
                    return "You cannot cancel a customer while he owes us money!";
                }

                // Active subscriptions 
                if (pCustomerData.NumberOfActiveSubscriptions > 0)
                {
                    return "You cannot cancel a customer while he has active subscriptions!";
                }

                {
                    string lResult;

                    if ((lResult = pCustomerData.Destroy()) != "OK")
                    {
                        return lResult;
                    }

                    ExceptionData.WriteException(3, "Destroyed customer = " + lCustomerId.ToString(),"static CustomerBiz", "DestroyCustomer", pReason);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBizStatic", "Destroy", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed due to technical error";
            }
            finally
            {
                lConnection.Close();
            }
        }

        #endregion

        #region Emial
        public static string SendEmail(string pFileName, string pDestination, string pSubject, string pBody)
        {
            var lClient = new HttpClient();

            try
            {
                MailRequesttype lRequest = new MailRequesttype();
                lRequest.headers = new OrderedDictionary();
                lRequest.body = new OrderedDictionary();
                lRequest.options = new OrderedDictionary();

                lRequest.headers.Add("from", "vandermerwer@mims.co.za");
                lRequest.headers.Add("to", pDestination);

                lRequest.headers.Add("subject", pSubject);
                lRequest.headers.Add("reply_to", "vandermerwer@mims.co.za");

                lRequest.body.Add("text", pBody);
                
                if (!String.IsNullOrEmpty(pFileName))
                {
                    FileStream lFileStream = File.OpenRead(pFileName);
                    byte[] lBytes = (byte[])Array.CreateInstance(typeof(byte), (int)lFileStream.Length);
                    lFileStream.Read(lBytes, 0, (int)lFileStream.Length);
                    lFileStream.Close();

                    var attachments = new OrderedDictionary();
                    attachments.Add("filename", pFileName);
                    Byte[] bytes = File.ReadAllBytes(pFileName);
                    String file = Convert.ToBase64String(bytes);
                    attachments.Add("data", file);

                    lRequest.attachments = new OrderedDictionary[1];
                    lRequest.attachments[0] = attachments;
                }

                // Setting content type.                   
                lClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var byteArray = Encoding.ASCII.GetBytes("vanderMerweR@mims.co.za:VTzyC0BG7PXZLB2mROmdQ1y27jTM9cdM_8");

                // Setting Authorization.  
                lClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                // Process HTTP GET/POST REST Web API  

                string jsonString = JsonSerializer.Serialize(lRequest);
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                string UriString = "https://arena.everlytic.net/api/2.0/trans_mails";
                var lResponse = lClient.PostAsync(UriString, content);

                if (lResponse.Result.ReasonPhrase == "Created")
                {
                    return "OK";
                }
                else
                {
                    ExceptionData.WriteException(1, "1" + " " + lResponse.Result.ReasonPhrase, "CustomerBiz static", "SendEmail", pDestination);
                    return lResponse.Result.ReasonPhrase;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerBiz static", "SendEmail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }
#endregion

    }
}
