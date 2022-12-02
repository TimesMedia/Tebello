using Subs.Business;
using Subs.Data;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tebello.Presentation;

namespace Subs.Presentation
{
    public partial class FNBBankStatement : Window
    {
        #region Globals

        private Subs.Data.PaymentDoc gPaymentDoc;
        private readonly Subs.Data.PaymentDocTableAdapters.FNBBankStatementTableAdapter gBankStatementAdapter = new Subs.Data.PaymentDocTableAdapters.FNBBankStatementTableAdapter();
        private readonly CollectionViewSource gCollectionViewSource;

        private readonly Subs.Data.LedgerDoc2 gLedgerDoc = new LedgerDoc2();
        private readonly Regex gRegExInvoice = new Regex(@"INV[\d]{3,8}"); // 3 to 8  digits too long.
        private readonly Regex gRegExStatement = new Regex(@"STA[\d]{3,8}"); // 3 to 8  digits too long.

        #endregion

        #region Constructor

        public FNBBankStatement()
        {
            InitializeComponent();
            gBankStatementAdapter.AttachConnection();

            gCollectionViewSource = (CollectionViewSource)this.Resources["FNBBankStatementViewSource"];

            pickerStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
            pickerEndDate.SelectedDate = DateTime.Now.Date;
        }

        #endregion

        #region Window Management

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            gPaymentDoc = ((Subs.Data.PaymentDoc)(this.FindResource("paymentDoc")));
        }

        private bool LoadBankStatement(string pFileName)
        {
            // Process the file

            StreamReader s = File.OpenText(pFileName);
            string lBuffer = null;
            string[] lBufferParts;
            Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter lLedgerAdapter
                = new Subs.Data.LedgerDoc2TableAdapters.TransactionsTableAdapter();
            int lCurrentAllocationNo = 1;
            this.Cursor = Cursors.Wait;

            try
            {
                const int lMaxEntries = 6;
                char[] lSeperator = { ',' };
                PaymentDoc.FNBBankStatementRow lStatementRow;
                DateTime lTransactionDate;
                DateTime lCurrentTransactionDate;


                for (int i = 0; i < 3; i++)
                {
                    lBuffer = s.ReadLine(); // Read up to before the first data line.
                }

                gPaymentDoc.FNBBankStatement.Clear(); //This will prevent you from loading the data twice. It forces you to do only one at a time.

                while ((lBuffer = s.ReadLine()) != null)
                {

                    lBufferParts = lBuffer.Split(lSeperator, lMaxEntries);

                    lStatementRow = gPaymentDoc.FNBBankStatement.NewFNBBankStatementRow();

                    lTransactionDate = new DateTime(Convert.ToInt32(lBufferParts[0].Substring(0, 4)),
                                                 Convert.ToInt32(lBufferParts[0].Substring(5, 2)),
                                                 Convert.ToInt32(lBufferParts[0].Substring(8, 2)));

                    if (lTransactionDate >= DateTime.Today)
                    {
                        // Accept bank entries only from previous day or further back, 
                        // because I might not get a complete record of this day.\nCurrent dates ignored!
                        continue;
                    }
                    else
                    {
                        lStatementRow.TransactionDate = lTransactionDate;
                        lCurrentTransactionDate = lTransactionDate;
                    }

                    // Amount

                    lStatementRow.Amount = Convert.ToDecimal(lBufferParts[2]);

                    lStatementRow.Reference = lBufferParts[3];
                    lStatementRow.Reference = lStatementRow.Reference.Replace("\"", string.Empty).Trim();

                    // Skip duplicate rows

                    if ((int)gBankStatementAdapter.Hits(lStatementRow.TransactionDate, lStatementRow.Amount,
                                                        lStatementRow.Reference) > 0)
                    {
                        continue;
                    }

                    // Try to extract the PaymentTransactiopId

                    Match myMatch2 = gRegExStatement.Match(lStatementRow.Reference);
                    if (myMatch2.Success)
                    {
                        lStatementRow.PaymentTransactionId = int.Parse(myMatch2.Value.Remove(0, 3));


                        // Now that you have the statement number, try to get the CustomerNo as well

                        int lPayerId = LedgerData.GetPayerByStatement(lStatementRow.PaymentTransactionId);

                        if (lPayerId != 0)
                        {
                            lStatementRow.CustomerId = lPayerId;
                        }
                    }

                    lStatementRow.AllocationNo = 0;
                    lStatementRow.ModifiedBy = System.Environment.UserName;
                    lStatementRow.ModifiedOn = DateTime.Now;

                    gPaymentDoc.FNBBankStatement.AddFNBBankStatementRow(lStatementRow);

                    lBufferParts.Initialize();

                } // End of while loop

                if (gPaymentDoc.FNBBankStatement.Count == 0)
                {
                    MessageBox.Show("There was nothing new to capture from " + pFileName);
                    return true;
                }

                // Sort and then assign allocation numbers

                gPaymentDoc.FNBBankStatement.DefaultView.Sort = "TransactionDate";

                //Get the start date
                DataRowView lRowViewStart = gPaymentDoc.FNBBankStatement.DefaultView[0];
                PaymentDoc.FNBBankStatementRow lRowStart = (PaymentDoc.FNBBankStatementRow)lRowViewStart.Row;
                //DateTime lBatchStartDate = lRowStart.TransactionDate;
                lCurrentTransactionDate = lRowStart.TransactionDate;

                foreach (DataRowView lRowView in gPaymentDoc.FNBBankStatement.DefaultView)
                {
                    PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;

                    if (lRow.TransactionDate != lCurrentTransactionDate)
                    {
                        lCurrentAllocationNo = 1;
                        lCurrentTransactionDate = lRow.TransactionDate;
                    }
                    else
                    {
                        lCurrentAllocationNo++;
                    }

                    lRow.AllocationNo = lCurrentAllocationNo;

                    // Add additional information

                    if (lRow.Reference.Contains("SPEEDPOINT"))
                    {
                        lRow.BankPaymentMethod = Enum.GetName(typeof(PaymentMethod), PaymentMethod.Creditcard);
                    }
                    else
                    {
                        lRow.BankPaymentMethod = Enum.GetName(typeof(PaymentMethod), PaymentMethod.EFTDeposit);
                    }


                    lRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                    lRow.ModifiedOn = DateTime.Now;

                }  // End of foreach loop

                // Write the stuff to disk

                PaymentData lPaymentData = new PaymentData(); // Consider making static

                {
                    string lResult;

                    if ((lResult = lPaymentData.UpdateFNBStatements(gPaymentDoc.FNBBankStatement)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
                }

                MessageBox.Show("Successfully captured " + gPaymentDoc.FNBBankStatement.Count.ToString() + " from " + pFileName);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "LoadBankStatement",
                        pFileName + "CurrentAllocationNumber: " + lCurrentAllocationNo.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                s.Close();
                this.Cursor = Cursors.Arrow;

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
            if (!DateCheck())
            {
                return;
            }

            gBankStatementAdapter.FillBy(gPaymentDoc.FNBBankStatement, "All", pickerStartDate.SelectedDate, pickerEndDate.SelectedDate);

        }

        private void buttonNotPosted_Click(object sender, RoutedEventArgs e)
        {

            if (!DateCheck())
            {
                return;
            }
            gBankStatementAdapter.FillBy(gPaymentDoc.FNBBankStatement, "Outstanding", pickerStartDate.SelectedDate, pickerEndDate.SelectedDate);
        }

        private bool ValidatePayment(ref PaymentData.PaymentRecord PaymentRecord, out string Message)
        {

            Message = "OK";

            try
            {
                // Insist on a CustomerId
                if (PaymentRecord.CustomerId == 0)
                {
                    Message = "No CustomerId has been supplied.";
                    return true;
                }

                // Validate the rest of the stuff

                CustomerBiz.PaymentValidationResult myResult = new CustomerBiz.PaymentValidationResult();


                {
                    string lResult;

                    if ((lResult = CustomerBiz.ValidatePayment(ref PaymentRecord, ref myResult, ref Message)) != "OK")
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
            this.Cursor = Cursors.Wait;
            string lCurrentReference = "";
            try
            {
                gBankStatementAdapter.Update(gPaymentDoc.FNBBankStatement);
                gPaymentDoc.FNBBankStatement.AcceptChanges();

                PaymentData.PaymentRecord myRecord = new PaymentData.PaymentRecord();
                string Message = "";

                foreach (PaymentDoc.FNBBankStatementRow lRow in gPaymentDoc.FNBBankStatement.Rows)
                {
                    lCurrentReference = lRow.Reference;
                    // skip the ones that has already been validated. 

                    if (!lRow.IsMessageNull())
                    {
                        if (lRow.Message == "OK" || lRow.PaymentState == (int)PaymentData.PaymentState.IncorrectlyDeposited
                         || lRow.PaymentState == (int)PaymentData.PaymentState.InternalTransfer
                         || lRow.PaymentState == (int)PaymentData.PaymentState.Overridden
                         || lRow.Posted)
                        {
                            //If the row was validated OK once, automatically or manually, it is good enough for me. 
                            continue;
                        }
                    }


                    // Populate the payment record
                    myRecord.Clear();
                    if (lRow.IsCustomerIdNull() || lRow.CustomerId == 0)
                    {
                        lRow.Message = "I cannot do anything without a CustomerId";
                        continue;
                    }
                    string customer = lRow.CustomerId.ToString();
                    myRecord.CustomerId = lRow.CustomerId;
                    myRecord.Amount = lRow.Amount;
                    myRecord.Date = lRow.TransactionDate;
                    myRecord.PaymentMethod = (int)PaymentMethod.DirectDeposit;
                    myRecord.ReferenceTypeId = 5; // Allocation number
                    myRecord.ReferenceTypeString = "Allocation number";
                    myRecord.Reference = lRow.TransactionDate.Year.ToString() + "/"
                         + lRow.TransactionDate.Day.ToString().PadLeft(2, '0')
                         + lRow.TransactionDate.Month.ToString().PadLeft(2, '0')
                         + "/" + lRow.AllocationNo.ToString();

                    //myRecord.InvoiceId = myRow.InvoiceId;

                    if (!ValidatePayment(ref myRecord, out Message))
                    {
                        return;
                    }

                    lRow.Message = Message;

                } // End of for loop

                // Write the stuff to disk

                gBankStatementAdapter.Update(gPaymentDoc.FNBBankStatement);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonValidate_Click", "Reference = " + lCurrentReference);
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
                int Submitted = 0;

                foreach (PaymentDoc.FNBBankStatementRow lBankStatementRow in gPaymentDoc.FNBBankStatement.Rows)
                {
                    // See what is eligible for posting

                    if (lBankStatementRow.Posted)
                    {
                        // This one has already been done
                        continue;
                    }

                    if (lBankStatementRow.Message != "OK")
                    {
                        // This one still has some problems
                        if (!lBankStatementRow.Message.EndsWith("Overridden"))
                        {
                            continue;
                        }
                    }

                    //Construct a Payment record

                    lPaymentRecord.CustomerId = lBankStatementRow.CustomerId;
                    lPaymentRecord.Amount = lBankStatementRow.Amount; ;
                    lPaymentRecord.PaymentMethod = (int)PaymentMethod.DirectDeposit;
                    lPaymentRecord.ReferenceTypeId = 5;
                    lPaymentRecord.Reference = lBankStatementRow.TransactionDate.Year.ToString() + "/"
                        + lBankStatementRow.TransactionDate.Day.ToString().PadLeft(2)
                        + lBankStatementRow.TransactionDate.Month.ToString().PadLeft(2)
                        + "/" + lBankStatementRow.AllocationNo.ToString();
                    lPaymentRecord.Date = lBankStatementRow.TransactionDate;

                    // Do the overall payment

                    int lPaymentTransactionId = 0;

                    {
                        string lResult;

                        if ((lResult = CustomerBiz.Pay(ref lPaymentRecord, out lPaymentTransactionId)) != "OK")
                        {
                            MessageBox.Show(lResult);
                            return;
                        }
                        lBankStatementRow.PaymentTransactionId = lPaymentTransactionId;
                    }

                    lBankStatementRow.Posted = true;
                    lBankStatementRow.PaymentState = (int)PaymentData.PaymentState.AllocatedToPayer;
                    gBankStatementAdapter.Update(lBankStatementRow);

                    Submitted++;

                    string lResult3;

                    CustomerData3 lCustomerData = new CustomerData3(lBankStatementRow.CustomerId);

                    if ((lResult3 = StatementControl2.SendEmail(CreateAStatement(lBankStatementRow.CustomerId), lBankStatementRow.CustomerId, lCustomerData.StatementEmail)) != "OK")
                    {
                        MessageBox.Show(lResult3);
                        return;
                    }

                } // End of foreach loop Bankstatements

                MessageBox.Show("I have submitted " + Submitted.ToString() + " payments");
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

        private void FNBBankStatementDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        #region Context menu

        private void FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.CurrentCustomerId = 0;

                CustomerPicker3 lCustomerPicker = new CustomerPicker3();
                lCustomerPicker.ShowDialog();

                if (Settings.CurrentCustomerId == 0)
                {
                    // The user did not select a customer
                    MessageBox.Show("No userid has been selected");
                    return;
                }

                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;

                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.CustomerId = Settings.CurrentCustomerId;
                MessageBox.Show("Found and inserted!");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FindCustomer_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void FindCustomerByStatement_Click(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
            PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
            lRow.EndEdit();

            try
            {
                ElicitInteger lElicit = new ElicitInteger("Please provide the statementnumber, providing only the digits.");
                lElicit.ShowDialog();

                if (lElicit.Answer == 0)
                {
                    MessageBox.Show("No proper statementId has been provided");
                    return;
                }

                int lPayerId = LedgerData.GetPayerByStatement(lElicit.Answer);

                if (lPayerId == 0)
                {
                    MessageBox.Show("Sorry, I could not find an associated customerId");
                    return;
                }
                else
                {
                    lRow.CustomerId = lPayerId;
                    MessageBox.Show("Found and inserted!");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FindCustomerByStatement_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);

                return;
            }
        }

        private void GotoCustomer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
            PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;

            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.SetCurrentCustomer(lRow.CustomerId);
            lCustomerPicker.GoToStatement();
            lCustomerPicker.ShowDialog();
        }

        private void MarkAsOverride_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                // Manually validate the row.

                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;

                if (lRow.IsCustomerIdNull())
                {
                    lRow.Message = ("I cannot do anything without a CustomerId");
                    return;
                }

                if (lRow.Message != "OK")
                {
                    lRow.Message = lRow.Message + " Overridden";
                    lRow.PaymentState = (int)PaymentData.PaymentState.Overridden;
                }
            }
        }

        private void MarkAsIncorrectlyDeposited_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.IncorrectlyDeposited;
                lRow.Message = "Incorrectly deposited";
            }
        }
        private void MarkAsInternalTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.InternalTransfer;
                lRow.Message = "Internal transfer";
            }
        }
    
        private void MarkAsMultiplePayer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.ApplicableToMultiplePayers;
            }
        }

        private void MarkAsDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.Debitorders;
            }
        }

        private void MarkAsBankFees_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.BankCharges;
            }
        }

        private void MarkAsBounced_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.Message = "";
                lRow.PaymentState = (int)PaymentData.PaymentState.Reversable;
            }
        }

        private void AddAPayer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;

                if (lRow.PaymentState != (int)PaymentData.PaymentState.ApplicableToMultiplePayers)
                {
                    MessageBox.Show("I can perform this operation only relative to a multiple payer entry.");
                    return;
                }

                PaymentDoc.FNBBankStatementRow lNewRow = gPaymentDoc.FNBBankStatement.NewFNBBankStatementRow();
                lNewRow.TransactionDate = lRow.TransactionDate;
                lNewRow.StatementNo = lRow.StatementNo;

                // Elicit a sub-allocation number

                ElicitInteger lElicitInteger = new ElicitInteger("Please supply a unique sub-allocation number.");
                lElicitInteger.ShowDialog();

                lNewRow.AllocationNo = (lRow.AllocationNo * 1000) + lElicitInteger.Answer;
                lRow.Message = "Sub payment";
                lRow.PaymentState = (int)PaymentData.PaymentState.Undecided;
                gPaymentDoc.FNBBankStatement.AddFNBBankStatementRow(lNewRow);
            }
        }

        private void MarkAsPosted_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gCollectionViewSource.View.CurrentItem;
                PaymentDoc.FNBBankStatementRow lRow = (PaymentDoc.FNBBankStatementRow)lRowView.Row;
                lRow.PaymentState = (int)PaymentData.PaymentState.AllocatedToPayer;
            }
        }

        #endregion



    }
}
