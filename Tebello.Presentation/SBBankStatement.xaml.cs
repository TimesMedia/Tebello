using Subs.Business;
using Subs.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using Tebello.Presentation;

namespace Subs.Presentation
{
    public partial class SBBankStatement : Window
    {
        #region Globals

        private Subs.Data.PaymentDoc gPaymentDoc;
        private readonly Subs.Data.PaymentDocTableAdapters.SBBankStatementTableAdapter gBankStatementAdapter = new Subs.Data.PaymentDocTableAdapters.SBBankStatementTableAdapter();
        private readonly CollectionViewSource gSBBankStatementViewSource;
        private readonly List<string> NonPayLines = new List<string>();
        private readonly List<string> PayLines = new List<string>();
        private readonly Subs.Data.LedgerDoc2 gLedgerDoc = new LedgerDoc2();
        private readonly Regex gRegEx1 = new Regex(@"INV[\d]{3,8}"); // 3 to 8  digits too long.
        private readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);

        #endregion

        #region Constructor

        public SBBankStatement()
        {
            InitializeComponent();
            gBankStatementAdapter.AttachConnection();

            gPaymentDoc = (PaymentDoc)this.Resources["paymentDoc"];
            gSBBankStatementViewSource = (CollectionViewSource)this.Resources["SBBankStatementViewSource"];

            // Fill your validation collections

            NonPayLines.Add("BRANCH");
            NonPayLines.Add("ACC-NO");
            NonPayLines.Add("ACCNO");
            NonPayLines.Add("OPEN");
            NonPayLines.Add("CLOSE");
            NonPayLines.Add("FEE");

            PayLines.Add("OPEN");
            PayLines.Add("CLOSE");

            PayLines.Add("SF");
            PayLines.Add("CATSPT");
            PayLines.Add("ACB");
            PayLines.Add("INTPAY");
            PayLines.Add("INTFD");
            PayLines.Add("PAY");
            PayLines.Add("DEP");
            PayLines.Add("TRFR");
            PayLines.Add("CHEQ");
            PayLines.Add("EFTPOS");
            PayLines.Add("ATMDEP");
            PayLines.Add("UNPD");

            pickerStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
            pickerEndDate.SelectedDate = DateTime.Now.Date;
        }

        #endregion

        #region Window Management

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            gPaymentDoc = ((Subs.Data.PaymentDoc)(this.FindResource("paymentDoc")));
        }

        private bool LoadBankStatement(string FileName)
        {
            // Process the file

            StreamReader s = File.OpenText(FileName);
            string Buffer = null;
            string[] BufferParts;
            Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter myLedgerAdapter = new Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter();
            this.Cursor = Cursors.Wait;
            Regex myRegEx1 = new Regex(@"INV[\d]{3,8}"); // This is one digit too long.

            try
            {
                const int MaxEntries = 7;
                char[] Seperator = { ',' };
                PaymentDoc.SBBankStatementRow StatementRow;
                DateTime TransactionDate;


                int CurrentStatementNo = 0;
                decimal CurrentAllocationNo = 0.0M;
                string TransactionType = "";
                myLedgerAdapter.AttachConnection();

                gPaymentDoc.SBBankStatement.Clear(); //This will prevent you from loading the data twice. It forces you to do only one at a time.

                BufferParts = new string[MaxEntries];

                int lRowsAdded = 0;

                while ((Buffer = s.ReadLine()) != null)
                {
                    BufferParts = Buffer.Split(Seperator, MaxEntries);

                    // Select only the valid lines according to the type of transaction

                    TransactionType = BufferParts[2].Replace("\"", "").Replace(" ", "");

                    if (!PayLines.Contains(TransactionType))
                    {
                        if (!NonPayLines.Contains(TransactionType))
                        {
                            // This ensures completeness of the union of my original 2 lists.
                            string Message = "I do not know what transaction type " + TransactionType + " means.";
                            ExceptionData.WriteException(1, Message, this.ToString(), "LoadBankStatement", "");
                            MessageBox.Show(Message);
                            return false;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // Skip lines where the statementnumber is 0000

                    if (BufferParts[0].Replace("\"", "") == "0000") { continue; }

                    // Capture the values of the line 

                    StatementRow = gPaymentDoc.SBBankStatement.NewSBBankStatementRow();

                    StatementRow.StatementNo = Convert.ToInt32(BufferParts[0].Replace("\"", ""));

                    // Keep track of new statement numbers and allocation numbers

                    if (StatementRow.StatementNo != CurrentStatementNo)
                    {
                        // This is a new statement no, or the first one in the batch
                        if (TransactionType != "OPEN")
                        {
                            MessageBox.Show("Open balance missing!");
                            return false;
                        }
                        CurrentAllocationNo = -1.0M;
                        CurrentStatementNo = StatementRow.StatementNo;
                    }

                    StatementRow.AllocationNo = ++CurrentAllocationNo;

                    // Capture the rest

                    StatementRow.BankTransactionType = BufferParts[2].Replace("\"", "").TrimEnd();
                    StatementRow.BankPaymentMethod = BufferParts[4].Replace("\"", "").TrimEnd();
                    TransactionDate = new DateTime(Convert.ToInt32(BufferParts[1].Substring(1, 4)),
                                                   Convert.ToInt32(BufferParts[1].Substring(5, 2)),
                                                   Convert.ToInt32(BufferParts[1].Substring(7, 2)));

                    if (TransactionDate >= DateTime.Today)
                    {

                        MessageBox.Show("I accept bank entries only from previous day or futher back, because I might not get a complete record of this day.\nPlease select a range that ends in the past.");
                        gPaymentDoc.SBBankStatement.Clear();
                        return false;
                    }
                    else { StatementRow.TransactionDate = TransactionDate; }

                    StatementRow.Amount = Convert.ToDecimal(BufferParts[3]);
                    StatementRow.Reference = BufferParts[5].Replace("\"", "").TrimEnd();

                    StatementRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                    StatementRow.ModifiedOn = DateTime.Now;

                    gPaymentDoc.SBBankStatement.AddSBBankStatementRow(StatementRow);
                    lRowsAdded = lRowsAdded + 1;

                    BufferParts.Initialize();

                } // End of while loop

                // Write the stuff to disk

                PaymentData lPaymentData = new PaymentData();

                int lCount = gPaymentDoc.SBBankStatement.Count();


                {
                    string lResult;

                    if ((lResult = lPaymentData.UpdateSBStatements(gPaymentDoc.SBBankStatement)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
                }

                MessageBox.Show("I have added :" + lRowsAdded.ToString() + " to the MIMS system.");

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "LoadBankStatement", "Buffer = " + Buffer);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                this.Cursor = Cursors.Arrow;
                s.Close();
            }
        }

        #endregion

        #region Event handlers

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the name of the path to the bank statement.

                System.Windows.Forms.OpenFileDialog lOpenFileDialog = new System.Windows.Forms.OpenFileDialog();

                lOpenFileDialog.InitialDirectory = "c:\\SUBS";
                lOpenFileDialog.ShowDialog();
                string FileName = lOpenFileDialog.FileName.ToString();


                if (!File.Exists(FileName))
                {
                    MessageBox.Show("You have not selected a valid source file ");
                    return;
                }

                LoadBankStatement(FileName);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLoad_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private bool DateCheck()
        {
            if (pickerStartDate.SelectedDate > pickerEndDate.SelectedDate)
            {
                MessageBox.Show("Are you crazy. How can your start date be after the end date?");
                return false;
            }

            if (pickerStartDate.SelectedDate > DateTime.Now.Date)
            {
                MessageBox.Show("I do not cater for payments in the future!");
                return false;
            }
            return true;
        }

        private void buttonSelectRange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DateCheck())
                {
                    return;
                }

                gBankStatementAdapter.FillBy(gPaymentDoc.SBBankStatement, "All", pickerStartDate.SelectedDate, pickerEndDate.SelectedDate);
                textBalanceOverPeriod.Text = gPaymentDoc.SBBankStatement.Sum(a => a.Amount).ToString("#########0.00");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonSelectRange_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonSelectRange_Click: " + ex.Message);
            }

        }

        private void buttonNotPosted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DateCheck())
                {
                    return;
                }
                gBankStatementAdapter.FillBy(gPaymentDoc.SBBankStatement, "Outstanding", pickerStartDate.SelectedDate, pickerEndDate.SelectedDate);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonNotPosted_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonNotPosted_Click: " + ex.Message);
            }
        }

        private bool ValidatePayment(ref PaymentData.PaymentRecord PaymentRecord, out string ErrorMessage)
        {

            ErrorMessage = "OK";

            try
            {
                // Validate the rest of the stuff

                CustomerBiz.PaymentValidationResult lValidationResult = new CustomerBiz.PaymentValidationResult();


                {
                    string lResult;

                    if ((lResult = CustomerBiz.ValidatePayment(ref PaymentRecord, ref lValidationResult, ref ErrorMessage)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ValidatePayment", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        private void buttonValidate_Click(object sender, RoutedEventArgs e)
        {

            gBankStatementAdapter.Update(gPaymentDoc.SBBankStatement);
            gPaymentDoc.SBBankStatement.AcceptChanges();

            this.Cursor = Cursors.Wait;
            try
            {
                PaymentData.PaymentRecord myRecord = new PaymentData.PaymentRecord();
                string ErrorMessage = "";
                foreach (PaymentDoc.SBBankStatementRow lRow in gPaymentDoc.SBBankStatement.Rows)
                {
                    if (lRow.BankTransactionType == "OPEN" || lRow.BankTransactionType == "CLOSE")
                    {
                        continue;
                    }

                    if (lRow.PaymentState == (int)PaymentData.PaymentState.AllocatedToPayer
                        || lRow.PaymentState == (int)PaymentData.PaymentState.BouncedPayment
                        || lRow.PaymentState == (int)PaymentData.PaymentState.Debitorders
                        || lRow.PaymentState == (int)PaymentData.PaymentState.TransferBetweenBanks
                        || lRow.PaymentState == (int)PaymentData.PaymentState.BankCharges)

                    {
                        //Processed already
                        continue;
                    }

                    // Populate the payment record
                    myRecord.Clear();
                    if (lRow.IsCustomerIdNull() || lRow.CustomerId == 0)
                    {
                        lRow.Message = "I cannot do anything without a CustomerId";
                        lRow.PaymentState = (int)PaymentData.PaymentState.Undecided;
                        continue;
                    }

                    string customer = lRow.CustomerId.ToString();
                    myRecord.CustomerId = lRow.CustomerId;
                    myRecord.Amount = lRow.Amount;
                    myRecord.Date = lRow.TransactionDate;
                    myRecord.PaymentMethod = (int)PaymentMethod.DirectDeposit;
                    myRecord.ReferenceTypeId = 5; // Allocation number
                    myRecord.ReferenceTypeString = "Allocation number";
                    myRecord.Reference = lRow.TransactionDate.Year.ToString() + "/" + lRow.StatementNo.ToString() + "/" + lRow.AllocationNo.ToString();

                    if (!ValidatePayment(ref myRecord, out ErrorMessage))
                    {
                        return;
                    }

                    lRow.Message = ErrorMessage;
                    if (ErrorMessage == "OK")
                    {
                        lRow.PaymentState = (int)PaymentData.PaymentState.Postable;
                    }

                } // End of for loop

                // Write the stuff to disk

                gBankStatementAdapter.Update(gPaymentDoc.SBBankStatement);

                this.buttonPost.IsEnabled = true;
                MessageBox.Show("Done");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonValidate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void buttonPost_Click(object sender, RoutedEventArgs e)
        {
            buttonPost.IsEnabled = false;  // Prevent this button from being hit twice. Can be reset only via a new validate. 

            this.Cursor = Cursors.Wait;
            try
            {
                PaymentData.PaymentRecord lPaymentRecord = new PaymentData.PaymentRecord();
                int lSubmitted = 0;

                foreach (PaymentDoc.SBBankStatementRow lBankStatementRow in gPaymentDoc.SBBankStatement.Rows)
                {
                    if (lBankStatementRow.BankTransactionType == "OPEN" || lBankStatementRow.BankTransactionType == "CLOSE")
                    {
                        continue;
                    }

                    // See what is eligible for posting

                    if (lBankStatementRow.PaymentState == (int)PaymentData.PaymentState.AllocatedToPayer || lBankStatementRow.PaymentState == (int)PaymentData.PaymentState.BouncedPayment)
                    {
                        // This  has already been posted
                        continue;
                    }


                    // Bounced

                    if (lBankStatementRow.PaymentState == (int)PaymentData.PaymentState.Reversable)
                    {
                        CustomerData3 lCustomerData = new CustomerData3(lBankStatementRow.CustomerId);

                        // Find the PaymentTransactionId

                        Subs.Data.PaymentDocTableAdapters.DebitOrderBankStatementTableAdapter lStatementAdapter = new Subs.Data.PaymentDocTableAdapters.DebitOrderBankStatementTableAdapter();
                        lStatementAdapter.AttachConnection();
                        int lPaymentTransactionId = (int)lStatementAdapter.GetPaymentTransactionId(lBankStatementRow.CustomerId, -lBankStatementRow.Amount, lBankStatementRow.TransactionDate);

                        if (lPaymentTransactionId == 0)
                        {
                            lBankStatementRow.Message = "I could not find the payment transactionid to be bounced.";
                            continue;
                        }

                        int lReverseTransactionId = 0;

                        {
                            string lResult;
                            if ((lResult = CustomerBiz.ReversePayment(lCustomerData, lPaymentTransactionId, -lBankStatementRow.Amount, "Debitorder bounced", out lReverseTransactionId)) != "OK")
                            {
                                lBankStatementRow.Message = lResult;
                                continue;
                            }
                            else
                            {
                                lBankStatementRow.Message = "";
                                lBankStatementRow.PaymentTransactionId = lReverseTransactionId;
                                lBankStatementRow.PaymentState = (int)PaymentData.PaymentState.AllocatedToPayer;
                                lSubmitted++;
                            }
                        }
                    }


                    // Postable
                    if (lBankStatementRow.PaymentState == (int)PaymentData.PaymentState.Postable)
                    {
                        //Construct a PaymentRecord

                        lPaymentRecord.CustomerId = lBankStatementRow.CustomerId;
                        lPaymentRecord.Amount = lBankStatementRow.Amount; ;
                        lPaymentRecord.PaymentMethod = (int)PaymentMethod.DirectDeposit;
                        lPaymentRecord.ReferenceTypeId = 5;
                        lPaymentRecord.Reference = lBankStatementRow.TransactionDate.Year.ToString() + "/"
                            + lBankStatementRow.TransactionDate.Day.ToString().PadLeft(2)
                            + lBankStatementRow.TransactionDate.Month.ToString().PadLeft(2)
                            + "/" + lBankStatementRow.AllocationNo.ToString();
                        lPaymentRecord.Date = lBankStatementRow.TransactionDate;

                        int lPaymentTransactionId2 = 0;
                        {
                            string lResult;

                            if ((lResult = CustomerBiz.Pay(ref lPaymentRecord, out lPaymentTransactionId2)) != "OK")
                            {
                                MessageBox.Show(lResult);
                                return;
                            }
                        }
                        lBankStatementRow.Message = "";
                        lBankStatementRow.PaymentTransactionId = lPaymentTransactionId2;
                        lBankStatementRow.PaymentState = (int)PaymentData.PaymentState.AllocatedToPayer;
                        lSubmitted++;

                        string lResult3;

                        CustomerData3 lCustomerData= new CustomerData3(lBankStatementRow.CustomerId);

                        if ((lResult3 = StatementControl2.SendEmail(CreateAStatement(lBankStatementRow.CustomerId), lBankStatementRow.CustomerId, lCustomerData.StatementEmail)) != "OK")
                        {
                            MessageBox.Show(lResult3);
                            return;
                        }
                    } // End of foreach loop

                    gBankStatementAdapter.Update(lBankStatementRow);
                    MessageBox.Show("I have submitted " + lSubmitted.ToString() + " payments or reversals");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonPost_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }


        private int CreateAStatement(int pPayerId)
        {
            try
            {
                int lStatementId = 0;
                CounterData.GetUniqueNumber("Statement", ref lStatementId);
                StatementControl2 lStatementControl = new StatementControl2(pPayerId, lStatementId);
                LedgerData.Statement(pPayerId, lStatementId, lStatementControl.StatementValue);
                return lStatementId;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CreateAStatement(PayerId)", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }



        private void buttonGenerateCashbook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DOCashBook> lDOCash = (List<DOCashBook>)gDataContext.MIMS_PaymentData_DOCashbook(pickerStartDate.SelectedDate).ToList();
                List<SBCashBook> lSBCash = (List<SBCashBook>)gDataContext.MIMS_PaymentData_SBCashbook(pickerStartDate.SelectedDate).ToList();


                // Tally the totals
                if (lSBCash.Where(p => p.PaymentType == "Debitorders").Count() != 1)
                {
                    MessageBox.Show("There is no entry for DebitOrders in the SB bank statements");
                    return;
                }

                SBCashBook lDebitOrder = lSBCash.Where(p => p.PaymentType == "Debitorders").Single();
                decimal lDebitOrderTotal = lDOCash.Sum(p => p.Amount);
                if (Math.Abs(lDebitOrder.Amount - lDebitOrderTotal) > 1)
                {
                    MessageBox.Show("Warning: DebitOrder total = " + lDebitOrderTotal.ToString("###########0.000") + " while SB records " + lDebitOrder.Amount.ToString("###########0.000"));
                }

                foreach (SBCashBook lObject in lSBCash)
                {
                    lObject.Nationality = "Local";

                    if (lObject.PaymentType != "AllocatedToPayer")
                    {
                        lObject.Payment = lObject.Amount; // This is not distributed between products.
                    }
                }

                // Build an object for each debitorder product 

                /// Modify the SB line item pertaining to DebitOrders 
                lDebitOrder.Nationality = lDOCash[0].Nationality;
                lDebitOrder.ProductName = lDOCash[0].ProductName.Substring(0, Math.Min(9, lDOCash[0].ProductName.Length));
                lDebitOrder.Payment = lDOCash[0].Amount;
                lDebitOrder.PaymentType = "AllocatedToPayer";

                /// Create additional line items to cater for all the DO products. Note that i = 1, rather than 0, which has been modified above. 

                for (int i = 1; i < lDOCash.Count(); i++)
                {
                    SBCashBook lNew = new SBCashBook();
                    lNew.StatementNo = lDebitOrder.StatementNo;
                    lNew.AllocationNo = lDebitOrder.AllocationNo;
                    lNew.StatementDate = lDebitOrder.StatementDate;
                    lNew.Reference = lDebitOrder.Reference;
                    lNew.PaymentType = "AllocatedToPayer";
                    lNew.BankTransactionType = lDebitOrder.BankTransactionType;
                    lNew.BankPaymentMethod = lDebitOrder.BankPaymentMethod;
                    lNew.Amount = lDebitOrder.Amount;
                    lNew.Nationality = lDOCash[i].Nationality;
                    lNew.ProductName = lDOCash[i].ProductName.Substring(0, Math.Min(9, lDOCash[i].ProductName.Length));
                    lNew.Payment = lDOCash[i].Amount;
                    lSBCash.Add(lNew);
                }

                // Export the result as XML
                string lFileName = @"c:\Subs\SBCashbook.xml";
                FileStream lXMLFile = new FileStream(lFileName, FileMode.Create);
                XmlSerializer lSerializer = new XmlSerializer(typeof(List<SBCashBook>));
                lSerializer.Serialize(lXMLFile, lSBCash.OrderBy(p => p.StatementNo).ThenBy(q => q.AllocationNo).ToList());
                lXMLFile.Flush();
                MessageBox.Show("I have created " + lSBCash.Count().ToString() + " cashbook records in " + lFileName);

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonGenerateCashbook_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonGenerateCashbook_Click " + ex.Message);
            }

        }

        private void SBBankStatementDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

            // Force users to invoke contextmenu only from Header, becasue in this way the row is also selected. 
            Type lType = e.OriginalSource.GetType();
            if (lType.Name == "DataGridHeaderBorder")
            {
                // Invoke context menu only when mouse is in Header border. 
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

        }

        #endregion

        #region Context menu handlers

        private void FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId == 0)
            {
                // The user did not select a customer
                return;
            }

            DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
            if (lRowView != null)
            {
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.CustomerId = Settings.CurrentCustomerId;
            }
            else
            {
                MessageBox.Show("No row has been selected");
            }
        }

        private void GotoCustomer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
            PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;

            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.SetCurrentCustomer(lRow.CustomerId);
            lCustomerPicker.GoToStatement();
            lCustomerPicker.ShowDialog();
        }

        private void Override_Click(object sender, RoutedEventArgs e)
        {

            if (Settings.Authority >= 2)
            {
                // Manually validate the row.

                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;

                if (lRow.IsCustomerIdNull())
                {
                    lRow.Message = ("I cannot do anything without a CustomerId");
                    lRow.PaymentState = (int)PaymentData.PaymentState.Undecided;
                    return;
                }

                if (lRow.Message != "OK")
                {
                    lRow.Message = "OK " + lRow.Message + " Overridden";
                    lRow.PaymentState = (int)PaymentData.PaymentState.Postable;
                }
            }
        }

        private void MarkAsIncorrectlyDeposited_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.IncorrectlyDeposited;
            }
        }

        private void MarkAsInternalTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.TransferBetweenBanks;
            }
        }

        private void MarkAsOtherDivision_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.InternalTransfer;
            }
        }

        private void MarkAsMultiplePayer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.ApplicableToMultiplePayers;
            }
        }

        private void MarkAsDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.Debitorders;
            }
        }

        private void MarkAsBankFees_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.BankCharges;
            }
        }

        private void MarkAsBounced_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.Message = "";
                lRow.PaymentState = (int)PaymentData.PaymentState.Reversable;
            }
        }

        private void AddAPayer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;

                if (lRow.PaymentState != (int)PaymentData.PaymentState.ApplicableToMultiplePayers)
                {
                    MessageBox.Show("I can perform this operation only relative to a multiple payer entry.");
                    return;
                }

                PaymentDoc.SBBankStatementRow lNewRow = gPaymentDoc.SBBankStatement.NewSBBankStatementRow();
                lNewRow.TransactionDate = lRow.TransactionDate;
                lNewRow.StatementNo = lRow.StatementNo;

                // Elicit a sub-allocation number

                ElicitInteger lElicitInteger = new ElicitInteger("Please supply a unique sub-allocation number.");
                lElicitInteger.ShowDialog();

                lNewRow.AllocationNo = (lRow.AllocationNo * 1000) + lElicitInteger.Answer;
                lRow.Message = "Sub payment";
                lRow.PaymentState = (int)PaymentData.PaymentState.Undecided;
                gPaymentDoc.SBBankStatement.AddSBBankStatementRow(lNewRow);
            }
        }

        private void MarkAsPosted_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gSBBankStatementViewSource.View.CurrentItem;
                PaymentDoc.SBBankStatementRow lRow = (PaymentDoc.SBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.AllocatedToPayer;
            }
        }

        #endregion

    }
}
