using Subs.Business;
using Subs.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tebello.Presentation;

namespace Subs.Presentation
{
    public partial class DebitOrderBankStatement : Window
    {
        #region Globals

        private Subs.Data.PaymentDoc gPaymentDoc;
        private readonly Subs.Data.PaymentDocTableAdapters.DebitOrderBankStatementTableAdapter gBankStatementAdapter = new Subs.Data.PaymentDocTableAdapters.DebitOrderBankStatementTableAdapter();
        private readonly CollectionViewSource gDebitOrderBankStatementViewSource;
        private readonly List<string> NonPayLines = new List<string>();
        private readonly List<string> PayLines = new List<string>();

        private readonly Subs.Data.SBDebitOrderDocTableAdapters.SBDebitOrderTableAdapter gDebitOrderUserAdapter = new Subs.Data.SBDebitOrderDocTableAdapters.SBDebitOrderTableAdapter();
        private Subs.Data.SBDebitOrderDoc gSBDebitOrderDoc;
        private Subs.Data.SBDebitOrderDocTableAdapters.DebitOrderHistoryTableAdapter gDebitOrderHistoryAdapter = new Subs.Data.SBDebitOrderDocTableAdapters.DebitOrderHistoryTableAdapter();

        private List<DebitOrderProposal> gDebitOrderProposals = new List<DebitOrderProposal>();

        private readonly Subs.Data.LedgerDoc2 gLedgerDoc = new LedgerDoc2();
        private readonly Regex gRegEx1 = new Regex(@"INV[\d]{3,8}"); // 3 to 8  digits too long.

        #endregion

        #region Constructor

        public DebitOrderBankStatement()
        {
            InitializeComponent();
            gBankStatementAdapter.AttachConnection();
            gDebitOrderUserAdapter.AttachConnection();
            gDebitOrderHistoryAdapter.AttachConnection();


            gPaymentDoc = (PaymentDoc)this.Resources["paymentDoc"];
            gDebitOrderBankStatementViewSource = (CollectionViewSource)this.Resources["DebitOrderBankStatementViewSource"];

           pickerMonth.SelectedDate = DateTime.Now.AddMonths(-1);
           
        }

        #endregion

        #region Window Management

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            gPaymentDoc = ((Subs.Data.PaymentDoc)(this.FindResource("paymentDoc")));
            gSBDebitOrderDoc = ((Subs.Data.SBDebitOrderDoc)(this.FindResource("sBDebitOrderDoc")));
        }

        private bool LoadBankStatement(string FileName)
        {
            string lRunDate = "";
            StreamReader s = File.OpenText(FileName);
            try
            {
                // Process the file

                string Buffer = null;
                this.Cursor = Cursors.Wait;

                PaymentDoc.DebitOrderBankStatementRow StatementRow;

                int CurrentAllocationNo = 0;
                //myLedgerAdapter.AttachConnection();

                gPaymentDoc.DebitOrderBankStatement.Clear(); //This will prevent you from loading the data twice. It forces you to do only one at a time.

                // Read the first record to get the Run date

                Buffer = s.ReadLine();
                if (Buffer.Substring(0, 2) != "SB")
                {
                    MessageBox.Show("First record does not start with SB ");
                    return false;
                }
                else
                {
                    lRunDate = Buffer.Substring(2, 8);
                }

                // Read the rest of the file

                while ((Buffer = s.ReadLine()) != null)
                {
                    string lFirst2 = Buffer.Substring(0, 2);
                    if (lFirst2 != "SD")
                    {
                        break;  // Do notprocess the first line again.
                    }

                    //        // Capture the values of the line 

                    StatementRow = gPaymentDoc.DebitOrderBankStatement.NewDebitOrderBankStatementRow();

                    string lCustomerIdString = Buffer.Substring(136, 10).Trim();
                    //string lCustomerIdString = Buffer.Substring(126, 10).Trim();



                    // Ensure that it is a number

                    if (!int.TryParse(lCustomerIdString, out int lCustomerId))
                    {
                        MessageBox.Show("CustomerId " + lCustomerIdString + " is invalid. Check your input file!");
                        return false;
                    }
                    else
                    {
                        StatementRow.CustomerId = lCustomerId;
                    }
                    StatementRow.StatementNo = 0;
                    StatementRow.AllocationNo = ++CurrentAllocationNo;
                    StatementRow.BankTransactionType = "DebitOrder";
                    StatementRow.BankPaymentMethod = "DebitOrder";
                    StatementRow.TransactionDate = new DateTime(Convert.ToInt32(lRunDate.Substring(0, 4)),
                                                    Convert.ToInt32(lRunDate.Substring(4, 2)),
                                                    Convert.ToInt32(lRunDate.Substring(6, 2)));
                    double WorkAmount = System.Convert.ToDouble(Buffer.Substring(71, 15)) * 0.01;
                    StatementRow.Amount = System.Convert.ToDecimal(WorkAmount);
                    StatementRow.Reference = "";
                    StatementRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                    StatementRow.ModifiedOn = DateTime.Now;

                    gPaymentDoc.DebitOrderBankStatement.AddDebitOrderBankStatementRow(StatementRow);


                }   // End of while loop

                // Write the stuff to disk

                PaymentData lPaymentData = new PaymentData();

                {
                    string lResult;

                    if ((lResult = lPaymentData.UpdateDebitOrderStatements(gPaymentDoc.DebitOrderBankStatement)) != "OK")
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "LoadBankStatement", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in LoadBankStatement: " + ex.Message);
                return false;
            }

            finally
            {
                this.Cursor = Cursors.Arrow;
                s.Close();
            }
        }


        private bool LoadLinkServ(string FileName)
        {
            string lRunDate = "";
            StreamReader s = File.OpenText(FileName);
            try
            {
                // Process the file

                string Buffer = null;
                this.Cursor = Cursors.Wait;

                PaymentDoc.DebitOrderBankStatementRow StatementRow;

                int CurrentAllocationNo = 0;
                //myLedgerAdapter.AttachConnection();

                gPaymentDoc.DebitOrderBankStatement.Clear(); //This will prevent you from loading the data twice. It forces you to do only one at a time.

                // Read the first record to get the Run date

                Buffer = s.ReadLine();
                if (Buffer.Substring(0, 2) != "SB")
                {
                    MessageBox.Show("First record does not start with SB ");
                    return false;
                }
                else
                {
                    lRunDate = Buffer.Substring(2, 8);
                }

                // Read the rest of the file

                while ((Buffer = s.ReadLine()) != null)
                {
                    string lFirst2 = Buffer.Substring(0, 2);
                    if (lFirst2 != "SD")
                    {
                        break;  // Do notprocess the first line again.
                    }

                    //        // Capture the values of the line 

                    StatementRow = gPaymentDoc.DebitOrderBankStatement.NewDebitOrderBankStatementRow();

                    string lCustomerIdString = Buffer.Substring(131, 10).Trim();


                    // Ensure that it is a number

                    if (!int.TryParse(lCustomerIdString, out int lCustomerId))
                    {
                        MessageBox.Show("CustomerId " + lCustomerIdString + " is invalid. Check your input file!");
                        return false;
                    }
                    else
                    {
                        StatementRow.CustomerId = lCustomerId;
                    }
                    StatementRow.StatementNo = 0;
                    StatementRow.AllocationNo = ++CurrentAllocationNo;
                    StatementRow.BankTransactionType = "DebitOrder";
                    StatementRow.BankPaymentMethod = "DebitOrder";
                    StatementRow.TransactionDate = new DateTime(Convert.ToInt32(lRunDate.Substring(0, 4)),
                                                    Convert.ToInt32(lRunDate.Substring(4, 2)),
                                                    Convert.ToInt32(lRunDate.Substring(6, 2)));
                    double WorkAmount = System.Convert.ToDouble(Buffer.Substring(71, 15)) * 0.01;
                    StatementRow.Amount = System.Convert.ToDecimal(WorkAmount);
                    StatementRow.Reference = "";
                    StatementRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                    StatementRow.ModifiedOn = DateTime.Now;

                    gPaymentDoc.DebitOrderBankStatement.AddDebitOrderBankStatementRow(StatementRow);


                }   // End of while loop

                // Write the stuff to disk

                PaymentData lPaymentData = new PaymentData();

                {
                    string lResult;

                    if ((lResult = lPaymentData.UpdateDebitOrderStatements(gPaymentDoc.DebitOrderBankStatement)) != "OK")
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "LoadBankStatement", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in LoadBankStatement: " + ex.Message);
                return false;
            }

            finally
            {
                this.Cursor = Cursors.Arrow;
                s.Close();
            }
        }

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

                if (!LoadBankStatement(FileName))
                {
                    return;
                }

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLoad_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private bool DateCheck()
        {
            if (pickerMonth.SelectedDate > DateTime.Now.Date)
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

                gBankStatementAdapter.FillBy(gPaymentDoc.DebitOrderBankStatement, "All", pickerMonth.SelectedDate);
                textBalanceOverPeriod.Text = gPaymentDoc.DebitOrderBankStatement.Where(b => b.Posted == true).Sum(a => a.Amount).ToString("#########0.00");
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
                gBankStatementAdapter.FillBy(gPaymentDoc.DebitOrderBankStatement, "Outstanding", pickerMonth.SelectedDate);

                if (gPaymentDoc.DebitOrderBankStatement.Count() == 0)
                {
                    MessageBox.Show("There is nothing that is not posted.");
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
                // Insist on a CustomerId
                if (PaymentRecord.CustomerId == 0)
                {
                    ErrorMessage = "No CustomerId has been supplied.";
                    return true;
                }

                // Validate the rest of the stuff

                CustomerBiz.PaymentValidationResult myResult = new CustomerBiz.PaymentValidationResult();


                {
                    string lResult;

                    if ((lResult = CustomerBiz.ValidatePayment(ref PaymentRecord, ref myResult, ref ErrorMessage)) != "OK")
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
            // Ensure that you validate only on the last batch of debitorders.

            DateTime lLastBatchDate = (DateTime)gBankStatementAdapter.GetLastBatchDate();

            if (gPaymentDoc.DebitOrderBankStatement[0].TransactionDate != lLastBatchDate)
            {
                MessageBox.Show("Sorry, I can validate only the last debit order batch.");
                return;
            }

            gBankStatementAdapter.Update(gPaymentDoc.DebitOrderBankStatement);
            gPaymentDoc.DebitOrderBankStatement.AcceptChanges();
            this.Cursor = Cursors.Wait;
            try
            {
                PaymentData.PaymentRecord myRecord = new PaymentData.PaymentRecord();
                string ErrorMessage = "";
                foreach (PaymentDoc.DebitOrderBankStatementRow lRow in gPaymentDoc.DebitOrderBankStatement.Rows)
                {
                    // skip the ones that has already been validated. 
                    if (!lRow.IsErrorMessageNull())
                    {
                        if (lRow.ErrorMessage == "OK" || lRow.ErrorMessage == "Incorrectly deposited" || lRow.ErrorMessage == "Internal transfer" || lRow.ErrorMessage == "OK_Overridden" || lRow.Posted)
                        {
                            //If the row was validated OK once, automatically or manually, it is good enough for me. 
                            continue;
                        }
                    }

                    // Populate the payment record
                    myRecord.Clear();
                    if (lRow.IsCustomerIdNull() || lRow.CustomerId == 0)
                    {
                        lRow.ErrorMessage = "I cannot do anything without a CustomerId";
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

                    lRow.ErrorMessage = ErrorMessage;

                } // End of for loop

                // Write the stuff to disk

                gBankStatementAdapter.Update(gPaymentDoc.DebitOrderBankStatement);

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
                int Submitted = 0;


                foreach (PaymentDoc.DebitOrderBankStatementRow lBankStatementRow in gPaymentDoc.DebitOrderBankStatement.Rows)
                {
                    // See what is eligible for posting

                    if (lBankStatementRow.Posted)
                    {
                        // This one has already been done
                        continue;
                    }

                    if (lBankStatementRow.ErrorMessage != "OK")
                    {
                        // This one still has some problems
                        if (!lBankStatementRow.ErrorMessage.EndsWith("Overridden"))
                        {
                            continue;
                        }
                    }

                    //Construct an OverallPayment object


                    lPaymentRecord.CustomerId = lBankStatementRow.CustomerId;
                    lPaymentRecord.Amount = lBankStatementRow.Amount; ;
                    lPaymentRecord.PaymentMethod = (int)PaymentMethod.Debitorder;
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
                    gBankStatementAdapter.Update(lBankStatementRow);

                    Submitted++;


                } // End of foreach loop

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

        private void DebitOrderBankStatementDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId == 0)
            {
                // The user did not select a customer
                return;
            }

            DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
            if (lRowView != null)
            {
                PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;
                lRow.CustomerId = Settings.CurrentCustomerId;
            }
            else
            {
                MessageBox.Show("No row has been selected");
            }
        }

        private void GoToCustomer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
            PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;

            if (lRow.IsCustomerIdNull())
            {
                MessageBox.Show("Sorry, there is not customerid to go to.");
                return;
            }

            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.SetCurrentCustomer(lRow.CustomerId);
            lCustomerPicker.ShowDialog();

        }
        private void FindCustomerAndAllocatePayment_Click(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
            PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;

            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.gCustomerPickerViewModel.PaymentAmount = lRow.Amount;
            lCustomerPicker.gCustomerPickerViewModel.PaymentMethod = (int)PaymentMethod.DirectDeposit;
            lCustomerPicker.gCustomerPickerViewModel.ReferenceTypeId = 5;
            lCustomerPicker.gCustomerPickerViewModel.PaymentReference = lRow.TransactionDate.Year.ToString() + "/"
                        + lRow.TransactionDate.Day.ToString().PadLeft(2)
                        + lRow.TransactionDate.Month.ToString().PadLeft(2)
                        + "/" + lRow.AllocationNo.ToString(); ;

            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId == 0)
            {
                // The user did not select a customer
                return;
            }

            // Set the customerid and the paymentTransactionId
            lRow.CustomerId = Settings.CurrentCustomerId;
        }

        private void AcceptPayment_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                // Manually validate the row.

                DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
                PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;

                if (lRow.IsCustomerIdNull())
                {
                    lRow.ErrorMessage = ("I cannot do anything without a CustomerId");
                    return;
                }

                if (lRow.ErrorMessage != "OK")
                {
                    lRow.ErrorMessage = "OK_Overridden";
                }
            }
        }
             
        private void MarkAsIncorrectlyDeposited_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
                PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;
                lRow.ErrorMessage = "ncorrectly deposited";
            }
        }

        private void MarkAsInternalTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Authority >= 2)
            {
                DataRowView lRowView = (DataRowView)gDebitOrderBankStatementViewSource.View.CurrentItem;
                PaymentDoc.DebitOrderBankStatementRow lRow = (PaymentDoc.DebitOrderBankStatementRow)lRowView.Row;
                lRow.ErrorMessage = "Internal transfer";
            }
        }

        #endregion

        #region Edit Debit order user

        private void buttonLoadDOUsers(object sender, RoutedEventArgs e)
        {
            try
            {
                gDebitOrderUserAdapter.Fill(gSBDebitOrderDoc.SBDebitOrder);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLoadDOUsers", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonLoadDOUsers: " + ex.Message);
            }
        }
        private void buttonSaveDOUser(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (SBDebitOrderDoc.SBDebitOrderRow lRow in gSBDebitOrderDoc.SBDebitOrder)
                {
                    if (lRow.RowState == DataRowState.Modified || lRow.RowState == DataRowState.Added)
                    {
                        lRow.ModifiedBy = Environment.UserName;
                        lRow.ModifiedOn = DateTime.Now;
                    }
                }

                gDebitOrderUserAdapter.Update(gSBDebitOrderDoc.SBDebitOrder);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonSaveDOUser", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonSaveDOUser: " + ex.Message);
            }

        }

        private void buttonExitDOUser(object sender, RoutedEventArgs e)
        {
            gSBDebitOrderDoc.SBDebitOrder.Clear();
        }

        #endregion

        private void buttonProposeDebitOrder_Click(object sender, RoutedEventArgs e)
        {
             DateTime lDeliveryMonth;
            try
            {
                if (!calenderDeliver.SelectedDate.HasValue)
                {
                    MessageBox.Show("You have not selected a month.");
                    return;
                }
                else
                {
                    lDeliveryMonth = (DateTime)calenderDeliver.SelectedDate;
                    if (lDeliveryMonth.Day != 1)
                    {
                        MessageBox.Show("I can work only with the first day of the month.");
                        return;
                    }
                }

                Cursor = Cursors.Wait;

                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                gDebitOrderProposals.Clear();
                gDebitOrderProposals = lContext.MIMS_DataContext_DebitOrder_Proposal(lDeliveryMonth).ToList();
                ProposalDataGrid.ItemsSource = gDebitOrderProposals;
                MessageBox.Show("I have generated " + gDebitOrderProposals.Count.ToString() + " proposals.");

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonProposeDebitOrder_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void buttonWriteToXML_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                //Save a list to the database to prevent duplicates in future.
                int lCount = 0;

                foreach (DebitOrderProposal item in gDebitOrderProposals)
                {
                    if (item.IssueId != 0)
                    {
                        // Persist in database
                        gDebitOrderHistoryAdapter.Insert(item.SubscriptionId, item.IssueId, DateTime.Now, Environment.UserName);
                        lCount++;
                    }
                }

                // OK, if this succeeded, write stuff to XML.


                string XMLFile = "c:\\Subs\\DebitOrder_" + calenderDeliver.SelectedDate.Value.Year.ToString()
                          + calenderDeliver.SelectedDate.Value.Month.ToString("0#")
                          + ".xml";
                System.IO.FileStream lOutputFile = System.IO.File.Create(XMLFile);

                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<DebitOrderProposal>));

                writer.Serialize(lOutputFile, gDebitOrderProposals);
                MessageBox.Show("XML written to " + XMLFile);
                lOutputFile.Close();
                 
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonWriteToXML_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonWriteToXML_Click " + ex.Message);
            }
        }

        //private void buttonWriteToXML_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string XMLFile = "c:\\Subs\\DebitOrder_" + calenderDeliver.SelectedDate.Value.Year.ToString()
        //               + calenderDeliver.SelectedDate.Value.Month.ToString("0#")
        //               + ".xml";
        //        System.IO.FileStream lOutputFile = System.IO.File.Create(XMLFile);

        //        System.Xml.Serialization.XmlSerializer writer =
        //        new System.Xml.Serialization.XmlSerializer(typeof(List<DebitOrderProposal>));

        //        writer.Serialize(lOutputFile, gDebitOrderProposals);
        //        MessageBox.Show("XML written to " + XMLFile);
        //        lOutputFile.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        //Display all the exceptions

        //        Exception CurrentException = ex;
        //        int ExceptionLevel = 0;
        //        do
        //        {
        //            ExceptionLevel++;
        //            ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonWriteToXML_Click", "");
        //            CurrentException = CurrentException.InnerException;
        //        } while (CurrentException != null);

        //        MessageBox.Show("Error in buttonWriteToXML_Click " + ex.Message);
        //    }
        //}

        private void buttonWriteToCSV_Click(object sender, RoutedEventArgs e)
        {
            int lCounter = 1;
            string CSVFile = "c:\\Subs\\FNB" + lCounter.ToString().PadLeft(4, '0') + "_" + calenderDeliver.SelectedDate.Value.Year.ToString()
                    + calenderDeliver.SelectedDate.Value.Month.ToString("0#")
                    + ".txt";

            StreamWriter lWriter = File.CreateText(CSVFile);

            int lCurrentCustomerId = 0;
            DebitOrderProposal lCurrentProposal = null;
            DebitOrderProposal lPreviousProposal = null;

            decimal lAmount = 0;
            StringBuilder lString = new StringBuilder(88);

            try
            {
                // Remove all the 'surplus rows

                foreach (DebitOrderProposal lProposal in gDebitOrderProposals)
                {
                    if (lProposal.ProductName == "Surplus")
                    {
                        gDebitOrderProposals.Remove(lProposal);
                    }

                }

                // First three lines

                lString.Clear();
                lString.Append("1st line");
                lWriter.WriteLine(lString.ToString());


                lString.Clear();
                lString.Append(DateTime.Today);
                lWriter.WriteLine(lString.ToString());

                lString.Clear();
                lString.Append("62793076326");
                lWriter.WriteLine(lString.ToString());


                foreach (DebitOrderProposal lProposal in gDebitOrderProposals)
                {
                    lPreviousProposal = lCurrentProposal;
                    lCurrentProposal = lProposal;

                    if (lProposal.CustomerId != lCurrentCustomerId)
                    {
                        // This is a new Customer
                        if (lCurrentCustomerId == 0)
                        {
                            // This is the start of the program
                            lCurrentCustomerId = lProposal.CustomerId;
                            lAmount = lProposal.Subtract;
                        }
                        else
                        {
                            // This is a subsequent row - so it is the start of a subsequent customer. Write out the first one.
                            lString.Clear();
                            lString.Append(lPreviousProposal.AccountHolder);
                            lString.Append("," + lPreviousProposal.AccountNo);
                            lString.Append(","); // Account type
                            lString.Append("," + lPreviousProposal.BankCode.PadLeft(6, '0'));
                            lString.Append("," + lAmount.ToString("#######0.00"));
                            lString.Append(lPreviousProposal.CustomerId.ToString());  // 6
                            lString.Append(","); // 7
                            lString.Append(",True"); // 8
                            lString.Append("," + lPreviousProposal.EmailAddress); // 9
                            lString.Append(","); // 10
                            lString.Append(","); // 11
                            lString.Append(","); // 12
                            lString.Append(","); // 13
                            lString.Append(","); // 14
                            lString.Append(","); // 15
                            lString.Append(","); // 16
                            lString.Append(","); // 17
                            lString.Append(","); // 18
                            lString.Append(","); // 19
                            lString.Append(","); // 20
                            lString.Append(","); // 21
                            lString.Append(","); // 22
                            lString.Append(","); // 23
                            lString.Append(","); // 24
                            lString.Append(","); // 25
                            lString.Append(","); // 26
                            lString.Append(","); // 27
                            lString.Append(","); // 28
                            lString.Append(","); // 29
                            lString.Append(","); // 30
                            lString.Append(","); // 31
                            lString.Append(","); // 32
                            lString.Append(","); // 33
                            lString.Append(","); // 34
                            lString.Append(","); // 35
                            lString.Append(","); // 36
                            lString.Append(",MIMS"); // 37
                            lWriter.WriteLine(lString.ToString());

                        }

                        lCurrentCustomerId = lProposal.CustomerId;
                        lAmount = lProposal.Subtract;
                    }

                    else
                    {
                        // This is a multiple subscription case of the same customer
                        lAmount += lProposal.Subtract;
                    }
                }

                // Write out the last customer

                lString.Clear();
                lString.Append(lPreviousProposal.AccountHolder);
                lString.Append("," + lPreviousProposal.AccountNo);
                lString.Append(","); // Account type
                lString.Append("," + lPreviousProposal.BankCode.PadLeft(6, '0'));
                lString.Append("," + lAmount.ToString("#######0.00"));
                lString.Append(lPreviousProposal.CustomerId.ToString());  // 6
                lString.Append(","); // 7
                lString.Append(",True"); // 8
                lString.Append("," + lPreviousProposal.EmailAddress); // 9
                lString.Append(","); // 10
                lString.Append(","); // 11
                lString.Append(","); // 12
                lString.Append(","); // 13
                lString.Append(","); // 14
                lString.Append(","); // 15
                lString.Append(","); // 16
                lString.Append(","); // 17
                lString.Append(","); // 18
                lString.Append(","); // 19
                lString.Append(","); // 20
                lString.Append(","); // 21
                lString.Append(","); // 22
                lString.Append(","); // 23
                lString.Append(","); // 24
                lString.Append(","); // 25
                lString.Append(","); // 26
                lString.Append(","); // 27
                lString.Append(","); // 28
                lString.Append(","); // 29
                lString.Append(","); // 30
                lString.Append(","); // 31
                lString.Append(","); // 32
                lString.Append(","); // 33
                lString.Append(","); // 34
                lString.Append(","); // 35
                lString.Append(","); // 36
                lString.Append(",MIMS"); // 37

                lWriter.WriteLine(lString.ToString());

                MessageBox.Show("CSV written to " + CSVFile);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonWriteToCSV_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonWriteToCSV_Click " + ex.Message);
            }
            finally
            {
                lWriter.Flush();
                lWriter.Close();
            }
        }
        private void buttonLoadSpecificDOUser(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerPicker3 lPicker = new CustomerPicker3();
                lPicker.ShowDialog();

                gSBDebitOrderDoc.SBDebitOrder.Clear();
                gDebitOrderUserAdapter.FillBy(gSBDebitOrderDoc.SBDebitOrder, Settings.CurrentCustomerId);

                if (gSBDebitOrderDoc.SBDebitOrder.Count() != 1)
                {
                    MessageBox.Show("The selected customer does not use our debitorders.");
                    return;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLoadSpecificUser", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonLoadSpecificUser: " + ex.Message);
            }
        }
        private void buttonAddDOUser(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerPicker3 lPicker = new CustomerPicker3();
                lPicker.ShowDialog();

                gSBDebitOrderDoc.SBDebitOrder.Clear();
                SBDebitOrderDoc.SBDebitOrderRow lNewRow = gSBDebitOrderDoc.SBDebitOrder.NewSBDebitOrderRow();
                lNewRow.CustomerId = Settings.CurrentCustomerId;
                lNewRow.ModifiedBy = System.Environment.UserName;
                lNewRow.ModifiedOn = DateTime.Now;
                gSBDebitOrderDoc.SBDebitOrder.AddSBDebitOrderRow(lNewRow);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAddDOUser", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonAddDOUser: " + ex.Message);
            }
        }
    }
}
