using Subs.Business;
using Subs.Data;
using System;
using System.Collections.Generic;
using System.IO;


using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace Subs.Presentation
{
    public static class ExtensionMethods
    {
        private static readonly Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    public partial class Maintenance : Window
    {
        #region Globals

        private int gProgressCounter = 0;
        private int gMaxProgressCount = 0;
        private LedgerDoc2 gLedgerDoc = new LedgerDoc2();
        private readonly int LastTransactionIdInvoice = (int)0;
        private int gSelectedIssue = 0;

        private readonly SubscriptionDoc3 gSubscriptionDoc = new SubscriptionDoc3();
        private readonly SubscriptionDoc3.SubscriptionDataTable gSubscription;
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter gSubscriptionAdaptor = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();

        #endregion

        #region Constructor

        public Maintenance()
        {
            InitializeComponent();


            if (!CounterData.GetValue("LastTransactionIdInvoice", ref LastTransactionIdInvoice))
            {
                return;
            }
            textFromTransactionId.Text = (LastTransactionIdInvoice + 1).ToString();

            gSubscriptionAdaptor.AttachConnection();
            gSubscriptionAdaptor.ClearBeforeFill = false;
            gSubscription = gSubscriptionDoc.Subscription;

        }

        #endregion

        #region Window Utilities
        private void Log(string Message)
        {
            try
            {
                // Clear the listbox if it becomes too full
                if (ListBox1.Items.Count == 500) { this.ListBox1.Items.Clear(); }

                Message = System.DateTime.Now + " " + Message;
                ListBox1.Items.Add(Message);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Log", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in Log " + ex.Message);
            }
        }

        private void SetVisibility(object sender, RoutedEventArgs e)
        {
            FrameworkElement lFrameworkElement = (FrameworkElement)sender;

            if (string.IsNullOrWhiteSpace((string)lFrameworkElement.Tag))
            {
                // This event handler is dependent on the Tag property
                return;
            }

            if (Settings.Authority == 4 && ((string)lFrameworkElement.Tag == "AuthorityHighest"
                                         || (string)lFrameworkElement.Tag == "AuthorityHigh"
                                         || (string)lFrameworkElement.Tag == "AuthorityMedium"))
            {
                lFrameworkElement.Visibility = Visibility.Visible;
            }
            else
            {
                if (Settings.Authority == 3 && ((string)lFrameworkElement.Tag == "AuthorityHigh"
                                             || (string)lFrameworkElement.Tag == "AuthorityMedium"))
                {
                    lFrameworkElement.Visibility = Visibility.Visible;
                }
                else
                {
                    if (Settings.Authority == 2 && (string)lFrameworkElement.Tag == "AuthorityMedium")
                    {
                        lFrameworkElement.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lFrameworkElement.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #endregion

        #region Statements

        private void RenderStatements(string pFileName)
        {
            string lStage = "Start";

            int lCurrentCustomerId = 0;

            try
            {
                string lDirectoryPath = Settings.DirectoryPath;

                // Ensure that the pointer to the data directory has been set

                if (lDirectoryPath.Length == 0)
                {
                    // Record the problem
                    ExceptionData.WriteException(1,
                          "Statement run failed. The pointer to the data directory has not been set.",
                          this.ToString(),
                          "Start",
                          "");
                    return;
                }

                // See if the previous run is cleaned up.

                string XMLFile = lDirectoryPath + "\\" + pFileName + "Done.xml";

                if (File.Exists(XMLFile))
                {
                    // Record the problem
                    ExceptionData.WriteException(3,
                         "Statement run failed. There already exists a file called " + pFileName + "Done.xml",
                         this.ToString(),
                         "Start",
                         "");

                    return;
                }

                // Identify the statements to be generated 

                XMLFile = lDirectoryPath + "\\" + pFileName + ".xml";

                if (File.Exists(XMLFile))
                {
                    lStage = "ReadXML";
                    gLedgerDoc.StatementBatch.Clear();
                    gLedgerDoc.StatementBatch.ReadXml(XMLFile);
                }
                else
                {
                    ExceptionData.WriteException(5,
                           XMLFile + " was not found. Assumed not to be scheduled",
                           this.ToString(),
                           "GenerateStatements",
                           "");
                    return;
                }

                gProgressCounter = 0;
                gMaxProgressCount = gLedgerDoc.StatementBatch.Count;

                // Process each statement

                Cursor = Cursors.Wait;

                lStage = "ForEach on StatementBatch";

                foreach (LedgerDoc2.StatementBatchRow lRow in gLedgerDoc.StatementBatch)
                {
                    // Show the progress
                    gProgressCounter++;
                    if (gProgressCounter % 10 == 0)
                    {
                        ProgressBar.Value = 100 * gProgressCounter / gMaxProgressCount;
                        ProgressBar.Refresh();
                    }


                    lCurrentCustomerId = lRow.CustomerId; // For error reporting purposes

                    if (lRow.CustomerId <= Convert.ToInt32(textDebitOrderCustomerId.Text))
                    {
                        continue;
                    }


                    {
                        string lResult;

                        if ((lResult = RenderStatement(lRow.CustomerId, lRow.StatementId)) != "OK")
                        {
                            ExceptionData.WriteException(1, 1 + " " + lResult, this.ToString(), "RenderStatements", "CustomerId = " + lCurrentCustomerId.ToString()
                            + " StatementNumber: " + lRow.StatementNumber);

                            MessageBox.Show(lResult + "CustomerId = " + lCurrentCustomerId.ToString()
                                + " StatementNumber: " + lRow.StatementNumber);
                            return;
                        }
                    }

                }

                // Rename the XML file, so that it does not get picked up again.

                File.Move(XMLFile, lDirectoryPath + "\\" + pFileName + "Done.xml");

                // Record the success

                string lMessage = "Statement run was successful. Last UserId is recorded in the comment in Exception table";
                ExceptionData.WriteException(5,
                    lMessage,
                    this.ToString(),
                    "ButtonStatement",
                    lCurrentCustomerId.ToString());

                MessageBox.Show(lMessage);
                return;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "RenderStatements", "Stage = " + lStage + " CustomerId = " + lCurrentCustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                //gThread.Join(1000);
            }

        }
        private string RenderStatement(int pCustomerId, int pStatementId)
        {
            try
            {
                StatementControl2 lStatementControl = new StatementControl2(pCustomerId, pStatementId);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "RenderStatement", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in RenderStatement " + ex.Message;
            }

        }

        private void EmailStatements(string pFileName)
        {
            string lStage = "Start";

            int lCurrentCustomerId = 0;

            try
            {
                string lDirectoryPath = Settings.DirectoryPath;

                // Ensure that the pointer to the data directory has been set

                if (lDirectoryPath.Length == 0)
                {
                    // Record the problem
                    ExceptionData.WriteException(1,
                          "Email statement run failed. The pointer to the data directory has not been set.",
                          this.ToString(),
                          "Start",
                          "");
                    return;
                }

                // See if the previous run is cleaned up.

                string XMLFile = lDirectoryPath + "\\" + pFileName + "Done.xml";

                if (!File.Exists(XMLFile))
                {
                    // Record the problem
                    ExceptionData.WriteException(3,
                         "Email statement run failed. There is no file called " + pFileName + "Done.xml",
                         this.ToString(),
                         "Start",
                         "");

                    return;
                }

                lStage = "ReadXML";
                gLedgerDoc.StatementBatch.Clear();
                gLedgerDoc.StatementBatch.ReadXml(XMLFile);

                gProgressCounter = 0;
                gMaxProgressCount = gLedgerDoc.StatementBatch.Count;

                // Process each statement

                Cursor = Cursors.Wait;

                lStage = "ForEach on StatementBatch";
                bool lError = false;

                foreach (LedgerDoc2.StatementBatchRow lRow in gLedgerDoc.StatementBatch)
                {
                    // Show the progress
                    gProgressCounter++;
                    if (gProgressCounter % 10 == 0)
                    {
                        ProgressBar.Value = 100 * gProgressCounter / gMaxProgressCount;
                        ProgressBar.Refresh();
                    }


                    lCurrentCustomerId = lRow.CustomerId; // For error reporting purposes

                    if (lRow.CustomerId <= Convert.ToInt32(textDebitOrderCustomerId.Text))
                    {
                        continue;
                    }

                    {
                        string lResult;

                        if ((lResult = StatementControl2.SendEmail(lRow.StatementId, lCurrentCustomerId, lRow.EMailAddress)) != "OK")
                        {
                            lError = true;
                            Log(lResult);
                        }
                        else
                        {
                            Log("Statement successfully Emailed to " + lCurrentCustomerId.ToString());
                        }
                    }
                } // End of foreach

                if (lError)
                {
                    MessageBox.Show("Error in Emailing PDFs. See log for details.");
                }
                else
                {
                    MessageBox.Show("Email PDFs run completed successfully. See log for details.");
                }

                return;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonDebitOrderRender", "Stage = " + lStage + " CustomerId = " + lCurrentCustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                //gThread.Join(1000);
            }

        }

        private void buttonNonDebitOrderDirective_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonNonDebitOrderDirective.IsEnabled = false;
                // See if the previous run is cleaned up.
                string XMLFile = Settings.DirectoryPath + "\\StatementNonDebitOrderBatchDone.xml";

                if (File.Exists(XMLFile))
                {
                    MessageBox.Show("Error in Customer.btnStatementDebitOrder_Click; File StatementNonDebitOrderBatchDone already exists.");
                    return;
                }

                this.Cursor = Cursors.Wait;

                {
                    string lResult;

                    if ((lResult = LedgerData.LoadStatementBatch(ref gLedgerDoc, "NonDebitOrder")) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }


                // Generate the statement numbers

                int StatementInteger = 0;

                foreach (LedgerDoc2.StatementBatchRow lRow in gLedgerDoc.StatementBatch)
                {
                    // Get a unique id

                    if (!CounterData.GetUniqueNumber("Statement", ref StatementInteger))
                    {
                        return;
                    }

                    //Record this

                    lRow.StatementId = StatementInteger;

                    lRow.StatementNumber = "STA" + StatementInteger.ToString();

                    LedgerData.Statement(lRow.CustomerId, lRow.StatementId, lRow.Amount);
                }


                // Copy the results out to disk

                gLedgerDoc.StatementBatch.WriteXml(Settings.DirectoryPath + "\\StatementNonDebitOrderBatch.xml");


                // Generate a log entry to show what you did.

                MessageBox.Show("The XML file for the STATEMENT run was successfully generated");

                // Generate the Mail merge spreadsheet

                buttonRenderNonDebitOrder.IsEnabled = true;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonNonDebitOrderDirective_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }
        private void buttonRenderNonDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            buttonRenderNonDebitOrder.IsEnabled = false;
            RenderStatements("StatementNonDebitOrderBatch");
            buttonEmailNonDebitOrder.IsEnabled = true;
        }
        private void buttonEmailNonDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            buttonEmailNonDebitOrder.IsEnabled = false;
            EmailStatements("StatementNonDebitOrderBatch");
            buttonNonDebitOrderDirective.IsEnabled = true;
        }
        private void buttonDebitOrderDirective_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonDebitOrderDirective.IsEnabled = false;
                // See if the previous run is cleaned up.
                string XMLFile = Settings.DirectoryPath + "\\StatementDebitOrderBatchDone.xml";

                if (File.Exists(XMLFile))
                {
                    MessageBox.Show("Error in Customer.btnStatementDebitOrder_Click; File StatementDebitOrderBatchDone already exists.");
                    return;
                }

                this.Cursor = Cursors.Wait;

                {
                    string lResult;

                    if ((lResult = LedgerData.LoadStatementBatch(ref gLedgerDoc, "DebitOrder")) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // Generate the statement numbers

                int StatementInteger = 0;

                foreach (LedgerDoc2.StatementBatchRow lRow in gLedgerDoc.StatementBatch)
                {
                    // Get a unique id

                    if (!CounterData.GetUniqueNumber("Statement", ref StatementInteger))
                    {
                        return;
                    }

                    lRow.StatementId = StatementInteger;
                    lRow.StatementNumber = "STA" + StatementInteger.ToString("000000#");

                    LedgerData.Statement(lRow.CustomerId, lRow.StatementId, lRow.Amount);

                }


                // Copy the results out to disk

                gLedgerDoc.StatementBatch.WriteXml(Settings.DirectoryPath + "\\StatementDebitOrderBatch.xml");


                // Generate a log entry to show what you did.
                buttonRenderDebitOrder.IsEnabled = true;
                MessageBox.Show("The XML file for the STATEMENT directives was successfully generated");

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "Static CounterData", "buttonDebitOrderDirective_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }
        private void buttonRenderDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            buttonRenderDebitOrder.IsEnabled = false;
            RenderStatements("StatementDebitOrderBatch");
            buttonEmailDebitOrder.IsEnabled = true;
        }
        private void buttonEmailDebitOrder_Click(object sender, RoutedEventArgs e)
        {
            buttonEmailDebitOrder.IsEnabled = false;
            EmailStatements("StatementDebitOrderBatch");
            buttonDebitOrderDirective.IsEnabled = true;
        }

        #endregion

        #region Invoices

      

    

       
        private void buttonInvoiceForPayerId_Click(object sender, RoutedEventArgs e)
        {
            InvoiceDirective("ForCustomerId", int.Parse(textForPayerId.Text));
        }

        private void buttonInvoiceDirective_Click(object sender, RoutedEventArgs e)
        {
            InvoiceDirective("FromTransactionId", int.Parse(textFromTransactionId.Text));
        }


        private void InvoiceDirective(string pSelector, int pId)
        {
            try
            {
                // See if the previous run is cleaned up.
                string XMLFile = Settings.DirectoryPath + "\\InvoiceBatch.xml";

                if (File.Exists(XMLFile))
                {
                    MessageBox.Show("Error in Customer.btnInvoiceRun_Click; File InvoiceBatch already exists.");
                    //Record the event in the Exception table
                    ExceptionData.WriteException(3, "File InvoiceBatch already exists.", this.ToString(), "btnInvoiceRun", "");
                    return;
                }

                XMLFile = Settings.DirectoryPath + "\\InvoiceBatchDone.xml";

                if (File.Exists(XMLFile))
                {
                    MessageBox.Show("Error in Customer.btnInvoiceRun_Click; File InvoiceBatchDone already exists.");
                    //Record the event in the Exception table
                    ExceptionData.WriteException(3, "File InvoiceBatchDone already exists.", this.ToString(), "btnInvoiceRun", "");
                    return;
                }


                this.Cursor = Cursors.Wait;

                LedgerData.LoadInvoiceBatch(pSelector, pId, ref gLedgerDoc);

                if (gLedgerDoc.InvoiceBatch.Rows.Count == 0)
                {
                    MessageBox.Show("There are no invoices to generate");
                    return;
                }

                List<int> lCustomersWithoutPhysicalAddress = gLedgerDoc.InvoiceBatch.Where( x => x.PhysicalAddressId == 0 ).Select(y => y.PayerId).Distinct().ToList();

                if (lCustomersWithoutPhysicalAddress.Count > 0)
                {
                    StringBuilder lMessage = new StringBuilder("The following customers do not have physical addresses: ");
                    foreach (int item in lCustomersWithoutPhysicalAddress)
                    {
                        lMessage.Append(item.ToString() + " ");
                    }
                
                    lMessage.Append(" Do you want to continue?"); 

                    if (MessageBoxResult.No == MessageBox.Show(lMessage.ToString(),
                       "Warning", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning))
                    {
                        return;
                    }
                }

                //Assign the invoice numbers

                int InvoiceId = 0;
                int lCurrentInvoiceId = 0;
                int lCurrentPayerId = 0;
                int lLargestTransactionId = 0;
                decimal lCurrentInvoiceValue = 0;

                foreach (LedgerDoc2.InvoiceBatchRow lRow in gLedgerDoc.InvoiceBatch)
                {
                    //Keep track of the largest TransactionId
                    if (lRow.TransactionId > lLargestTransactionId) { lLargestTransactionId = lRow.TransactionId; }

                    // Determine where a new invoice should start
                    if (lRow.PayerId != lCurrentPayerId)
                    {
                        if (lCurrentPayerId != 0)
                        {
                            //Put an entry in the ledger for the PREVIOUS invoice

                            LedgerData.Invoice(lCurrentPayerId, lCurrentInvoiceId, LastTransactionIdInvoice, lCurrentInvoiceValue);
                            lCurrentInvoiceValue = 0;
                        }

                        // Start a new invoice

                        InvoiceId = AdministrationData2.GetInvoiceId();
                        lCurrentInvoiceId = InvoiceId;
                    }

                    lRow.BeginEdit();
                    lRow.InvoiceId = InvoiceId;
                    lRow.EndEdit();

                    //Link the invoice to each subscription

                    SubscriptionData3 lSubscriptionData = new SubscriptionData3(lRow.SubscriptionId);
                    lSubscriptionData.InvoiceId = InvoiceId;

                    if (!lSubscriptionData.Update()) { return; }

                    lCurrentPayerId = lRow.PayerId;
                    lCurrentInvoiceValue = lCurrentInvoiceValue + lSubscriptionData.UnitPrice * lSubscriptionData.UnitsPerIssue * lSubscriptionData.NumberOfIssues;

                } // End of foreach loop - one loop per transaction


                if (lCurrentPayerId != 0)
                {
                    //Put an entry in the ledger for the PREVIOUS invoice

                    LedgerData.Invoice(lCurrentPayerId, lCurrentInvoiceId, LastTransactionIdInvoice, lCurrentInvoiceValue);

                    lCurrentInvoiceValue = 0;
                }

                // Copy the results out to disk

                gLedgerDoc.InvoiceBatch.WriteXml(Settings.DirectoryPath + "\\InvoiceBatch.xml");

                //Keep track of the last transactionid

                if (!CounterData.SetValue("LastTransactionIdInvoice", lLargestTransactionId))
                {
                    return;
                }

                // Generate a log entry to show what you did.

                MessageBox.Show("The XML file for the INVOICE run was successfully generated");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonInvoiceDirective_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonInvoiceDirective_Click " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

     
        private void buttonRenderInvoice_Click(object sender, RoutedEventArgs e)
        {
            string lStage = "Start";

            int lCurrentPayerId = 0;

            try
            {
                string lDirectoryPath = Settings.DirectoryPath;

                // Ensure that the pointer to the data directory has been set

                if (lDirectoryPath.Length == 0)
                {
                    // Record the problem
                    ExceptionData.WriteException(1,
                          "Invoice run failed. The pointer to the data directory has not been set.",
                          this.ToString(),
                          "Start",
                          "");
                    return;
                }

                // See if the previous run is cleaned up.

                string XMLFile = lDirectoryPath + "\\" + "InvoiceBatchDone.xml";

                if (File.Exists(XMLFile))
                {
                    // Record the problem
                    ExceptionData.WriteException(3,
                         "Invoice run failed. There already exists a file called " + "InvoiceBatchDone.xml",
                         this.ToString(),
                         "Start",
                         "");

                    return;
                }

                // Identify the Invoices to be generated 

                XMLFile = lDirectoryPath + "\\InvoiceBatch.xml";

                if (File.Exists(XMLFile))
                {
                    lStage = "ReadXML";
                    gLedgerDoc.InvoiceBatch.Clear();
                    gLedgerDoc.InvoiceBatch.ReadXml(XMLFile);
                }
                else
                {
                    ExceptionData.WriteException(5,
                           XMLFile + " was not found. Assumed not to be scheduled",
                           this.ToString(),
                           "buttonRenderInvoice",
                           "");
                    return;
                }

                gProgressCounter = 0;
                gMaxProgressCount = gLedgerDoc.InvoiceBatch.Count;

                // Process each statement

                Cursor = Cursors.Wait;

                lStage = "ForEach on StatementBatch";

                List<Invoice> lInvoiceList = new List<Invoice>();


                foreach (LedgerDoc2.InvoiceBatchRow lRow in gLedgerDoc.InvoiceBatch)
                {
                    // There is one row per subscription, but an invoice can contain more than one subscriptions. 

                    // Show the progress
                    gProgressCounter++;
                    if (gProgressCounter % 10 == 0)
                    {
                        ProgressBar.Value = 100 * gProgressCounter / gMaxProgressCount;
                        ProgressBar.Refresh();
                    }

                    // Prime the process load the first subscription

                    if (lCurrentPayerId == 0)
                    {
                        lCurrentPayerId = lRow.PayerId;
                        goto Load;
                    }

                    if (lCurrentPayerId == lRow.PayerId)
                    {
                        // This is not a new payer, so just add item to the list

                        goto Load;
                    }
                    else
                    {
                        // This is a new payer id - so process the previous subs

                        RenderInvoice();

                        Log("Invoice successfully rendered for customer " + lInvoiceList[0].PayerId.ToString());

                        lInvoiceList.Clear();  // Prepare for the next iteration

                        // Record the fact that the payer is a new one


                        lCurrentPayerId = lRow.PayerId;

                    } // End of if new payer statement

                    Load:
                    // Load the next subscription into a structure.

                    lInvoiceList.Add(new Invoice()
                    {
                        InvoiceId = lRow.InvoiceId,
                        PayerId = lRow.PayerId,
                        SubscriptionId = lRow.SubscriptionId,
                        DateFrom = DateTime.Now
                    });

                }  // End of foreach loop


                // Process the last list

                RenderInvoice();

                Log("Invoice successfully rendered for customer " + lInvoiceList[0].PayerId.ToString());
                ProgressBar.Value = 100;
                ProgressBar.Refresh();


                // Rename the XML file, so that it does not get picked up again.

                File.Move(XMLFile, lDirectoryPath + "\\InvoiceBatchDone.xml");

                // Record the success

                string lMessage = "Invoice run was successful";
                buttonInvoiceEmail.IsEnabled = true;
                ExceptionData.WriteException(5,
                    lMessage,
                    this.ToString(),
                    "buttonRenderInvoice",
                    "PayerId =  " + lCurrentPayerId.ToString());

                MessageBox.Show(lMessage);
                return;

                void RenderInvoice()
                {
                    string lResult;

                    // But this is only for one subscription!
                    InvoiceControl lInvoiceControl = new InvoiceControl();
                    if ((lResult = lInvoiceControl.Render(lInvoiceList)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }


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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonRenderInvoice", "Stage = " + lStage + " CustomerId = " + lCurrentPayerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void buttonInvoiceEmail_Click(object sender, RoutedEventArgs e)
        {
            int CurrentCustomerId = 0;
            int lMailErrors = 0;

            try
            {
                //if (!Directory.Exists(Settings.DirectoryPath + "\\Post") | !File.Exists(Settings.DirectoryPath + "\\InvoiceBatchDone.xml"))
                //{
                //    MessageBox.Show("Error in Customer.btnEmailInvoices_Click; Fax or Post Folders or InvoiceBatchDone File does not exist.");
                //    // Record the event in the Exception table
                //    ExceptionData.WriteException(3,
                //        "Fax or Post Folders or InvoiceBatchDone File does not exist.",
                //        this.ToString(), "SendEmailInvoice", "");
                //    return;
                //}

                gLedgerDoc.InvoiceBatch.Clear();
                gLedgerDoc.InvoiceBatch.ReadXml(Settings.DirectoryPath + "\\InvoiceBatchDone.xml");

                this.Cursor = Cursors.Wait;

                gProgressCounter = 0;
                gMaxProgressCount = gLedgerDoc.InvoiceBatch.Count;


                // We use the FIRST row of each payer. So on first encounter we send the mail

                foreach (LedgerDoc2.InvoiceBatchRow lRow in gLedgerDoc.InvoiceBatch)
                {
                    if (lRow.PayerId <= Convert.ToInt32(textInvoiceCustomerId.Text))
                    {
                        continue;
                    }

                    gProgressCounter++;
                    if (gProgressCounter % 10 == 0)
                    {
                        ProgressBar.Value = 100 * gProgressCounter / gMaxProgressCount;
                        ProgressBar.Refresh();
                    }

                    if (CurrentCustomerId != lRow.PayerId)
                    {
                        // This is a new customerid, so send the email to her.

                        string FilePrefix = "\\INV" + lRow.InvoiceId.ToString();

                        if (!lRow.IsEMailAddressNull())
                        {
                            string lSubject = "SUBS Tax Invoice - Customerid = " + lRow.PayerId.ToString();

                            string lBody = "Dear Client\n\n"
                             + "Attached herewith your Tax invoice. This tax invoice is a legal document and should be submitted to SARS with your VAT return.\n"
                             + "Kindly supply the vat invoice number along with your payment.\n"
                             + "Statements will be issued monthly to confirm all transactions on your account.\nThese statements will be e-mailed or faxed to you.\n\n"
                             + "Please forward this document to your accounts department.\n\n"
                             + "We trust that this will improve the quality of our service and look forward to a long-lasting relationship with you.\n\n"
                             + "Best\n\n"
                             + "Riëtte van der Merwe\n"
                             + "Subscription and Marketing Manager\n"
                             + "Tel: (011) 280-5856\n"
                             + "Fax: (086) 675 7910\n"
                             + "E-mail: vandermerwer@mims.co.za\n\n"
                             + "Hill on Empire, 16 Empire Rd (cnr Hillside Rd), ParkTown, Johannesburg, 2193\n"
                             + "P O Box 1746, Saxonworld, Johannesburg, 2132\n"
                             + "www.mims.co.za";

                            if (CustomerBiz.SendEmail(Settings.DirectoryPath + FilePrefix + ".pdf", lRow.EMailAddress, lSubject, lBody) != "OK")
                            {
                                lMailErrors++;
                                continue;
                            }
                            string Message = "Email successfully sent to " + lRow.PayerId.ToString();
                            Log(Message);
                            ExceptionData.WriteException(5, "The Email was sent successfully.", "Maintenance", "buttonInvoiceEmail", lRow.PayerId.ToString() + " " + lRow.EMailAddress);
                        }
 
                        CurrentCustomerId = lRow.PayerId;
                    }
                }

                if (lMailErrors > 0)
                {
                    MessageBox.Show("There were " + lMailErrors.ToString() + " mailing errors.");
                }
                else 
                { 
                    MessageBox.Show("All Emails were successfully sent.");
                    ExceptionData.WriteException(5,
                    "The Email run was successfull. See comment for last CustomerId",
                    "Maintenance", "btnEmailInvoice", CurrentCustomerId.ToString());
                }

                // Record the event in the Exception table
                
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonInvoiceEmail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonInvoiceEmail " + ex.Message);
            }

            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }


        #endregion

        #region Event handlers for Skip
        private void buttonInvoiceSelectSkipIssue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IssuePicker2 lIssuePicker = new Subs.Presentation.IssuePicker2();

                lIssuePicker.ShowDialog();

                if (lIssuePicker.IssueWasSelected)
                {
                    gSelectedIssue = lIssuePicker.IssueId;
                    labelSkipSelectedIssue.Content = lIssuePicker.IssueName;
                    buttonGlobalSkip.IsEnabled = true;
                }
                else
                {
                    gSelectedIssue = 0;
                    MessageBox.Show("Please select an issue.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonInvoiceSelectSkipIssue_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonInvoiceSelectSkipIssue_Click " + ex.Message);
            }
        }

        private void buttonGlobalSkip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SubscriptionDoc3 lDoc = new SubscriptionDoc3();
                Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter myAdaptor = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                myAdaptor.AttachConnection();
                myAdaptor.FillById(lDoc.SubscriptionIssue, gSelectedIssue, "ByIssue");

                this.Cursor = Cursors.Wait;

                foreach (SubscriptionDoc3.SubscriptionIssueRow lRow in lDoc.SubscriptionIssue)
                {
                    {
                        string lResult;

                        if ((lResult = IssueBiz.Skip(lRow.SubscriptionId, gSelectedIssue)) != "OK")
                        {
                            MessageBox.Show(lResult);
                            return;
                        }
                    }
                }

                gSelectedIssue = 0;
                buttonGlobalSkip.IsEnabled = false;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonGlobalSkip_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #endregion

        #region Event handlers for reversal of delivery run

        private void buttonSelectReversalIssue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IssuePicker2 lIssuePicker = new Subs.Presentation.IssuePicker2();
                lIssuePicker.ShowDialog();

                if (lIssuePicker.IssueWasSelected)
                {
                    gSelectedIssue = lIssuePicker.IssueId;
                    labelSelectedReversalIssue.Content = lIssuePicker.IssueName;
                    buttonReverse.IsEnabled = true;
                }
                else
                {
                    gSelectedIssue = 0;
                    MessageBox.Show("Please select an issue.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonInvoiceSelectSkipIssue_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonInvoiceSelectSkipIssue_Click " + ex.Message);
            }
        }

        private void buttonReverse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                DateTime lTime;
                {
                    string lResult;
                    if (DateTimePicker.SelectedDate != null)
                    {
                        lTime = (DateTime)DateTimePicker.SelectedDate;
                    }
                    else
                    {
                        MessageBox.Show("Please supply a valid time for the start of the delivery job.");
                        return;
                    }


                    if ((lResult = DeliveryDataStatic.ReverseDelivery(gSelectedIssue, lTime)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }


                string Message = "Reverse delivery successful for Issue:" + gSelectedIssue.ToString() + ", Time: " + lTime.ToString("yyyymmddHHmmss");
                MessageBox.Show(Message);
                ExceptionData.WriteException(3, Message, this.ToString(), "buttonReverseDeliveryRun_Click", " ");
                MessageBox.Show("Done");

                buttonReverse.IsEnabled = false;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;

            }
        }

        #endregion

        #region Miscellaneous
        private void buttonLiabilities_Click(object sender, RoutedEventArgs e)
        {
            CustomerDoc2.LiabilityDataTable lLiabilityTable = new CustomerDoc2.LiabilityDataTable();
            Subs.Data.CustomerDoc2TableAdapters.LiabilityTableAdapter lLiabilityAdapter = new Subs.Data.CustomerDoc2TableAdapters.LiabilityTableAdapter();
            int lCurrentPayer = 0;
            int lCounter = 0;
            try
            {
                this.Cursor = Cursors.Wait;

                ExceptionData.WriteException(5, "Liability job started on  " + DateTime.Now.ToString(), this.ToString(), "buttonLiabilities_Click", "");

                lLiabilityAdapter.AttachConnection();
                lLiabilityAdapter.Fill(lLiabilityTable);


                foreach (CustomerDoc2.LiabilityRow lRow in lLiabilityTable)
                {
                    lCounter++;
                    lCurrentPayer = lRow.PayerId;

                    List<LiabilityRecord> lLiabilityRecords = new List<LiabilityRecord>();
                    decimal lJournalLiability = 0;

                    {
                        string lResult;

                        if ((lResult = CustomerData3.CalulateLiability(lRow.PayerId, ref lLiabilityRecords, ref lJournalLiability)) != "OK")
                        {
                            if (lResult.Contains("Nothing"))
                            {
                                lRow.JournalLiability = 0M;
                                lRow.Datum = DateTime.Now;
                                goto Record;
                            }
                            else
                            {
                                MessageBox.Show(lResult);
                                return;
                            }
                        }
                    }

                    lRow.JournalLiability = lJournalLiability;
                    lRow.Datum = DateTime.Now;

                    Record:
                    lLiabilityAdapter.Update(lRow);

                    if (lCounter % 100 == 0)
                    {
                        ExceptionData.WriteException(5, "Liability job progressed on  " + DateTime.Now.ToString(), this.ToString(), "buttonLiabilities_Click", "Counter= " + lCounter.ToString());
                    }
                }

                ExceptionData.WriteException(5, "Liability job finished on  " + DateTime.Now.ToString(), this.ToString(), "buttonLiabilities_Click", "Counter= " + lCounter.ToString());
                MessageBox.Show("Done!");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLiabilities_Click",
                        "PayerId = " + lCurrentPayer.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);


            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void buttonReallocateAllInvoices_Click(object sender, RoutedEventArgs e)
        {
            CustomerDoc2.LiabilityDataTable lLiabilityTable = new CustomerDoc2.LiabilityDataTable();
            Subs.Data.CustomerDoc2TableAdapters.LiabilityTableAdapter lLiabilityAdapter = new Subs.Data.CustomerDoc2TableAdapters.LiabilityTableAdapter();
            int lCurrentPayer = 0;
            int lCounter = 0;
            try
            {
                this.Cursor = Cursors.Wait;
                ExceptionData.WriteException(5, "Reallocation job started on  " + DateTime.Now.ToString(), this.ToString(), "buttonReallocateAllInvoices_Click", "");

                lLiabilityAdapter.AttachConnection();
                lLiabilityAdapter.Fill(lLiabilityTable);


                foreach (CustomerDoc2.LiabilityRow lRow in lLiabilityTable)
                {
                    lCounter++;
                    lCurrentPayer = lRow.PayerId;

                    {
                        string lResult;

                        if ((lResult = CustomerBiz.DistributeAllPayments(lRow.PayerId)) != "OK")
                        {
                            if (lResult.Contains("Nothing"))
                            {
                                continue;
                            }
                            else
                            {
                                MessageBox.Show(lResult);
                                return;
                            }
                        }
                    }

                    if (lCounter % 100 == 0)
                    {
                        ExceptionData.WriteException(5, "Reallocation job progressed on  " + DateTime.Now.ToString(), this.ToString(), "buttonReallocateAllInvoices_Click", "Counter= " + lCounter.ToString());
                    }
                }

                ExceptionData.WriteException(5, "Reallocation job finished on  " + DateTime.Now.ToString(), this.ToString(), "buttonReallocateAllInvoices_Click", "Counter= " + lCounter.ToString());
                MessageBox.Show("Done!");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonReallocateAllInvoices_Click",
                        "PayerId = " + lCurrentPayer.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #endregion

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            string lSubject = "SUBS Tax Invoice - Customerid = 118957";

            string lBody = "Dear Client\n\n"
             + "Attached herewith your Tax invoice. This tax invoice is a legal document and should be submitted to SARS with your VAT return.\n"
             + "Kindly supply the vat invoice number along with your payment.\n"
             + "Statements will be issued monthly to confirm all transactions on your account.\nThese statements will be e-mailed or faxed to you.\n\n"
             + "Please forward this document to your accounts department.\n\n"
             + "We trust that this will improve the quality of our service and look forward to a long-lasting relationship with you.\n\n"
             + "Best\n\n"
             + "Riëtte van der Merwe\n"
             + "Subscription and Marketing Manager\n"
             + "Tel: (011) 280-5856\n"
             + "Fax: (086) 675 7910\n"
             + "E-mail: vandermerwer@mims.co.za\n\n"
             + "Hill on Empire, 16 Empire Rd (cnr Hillside Rd), ParkTown, Johannesburg, 2193\n"
             + "P O Box 1746, Saxonworld, Johannesburg, 2132\n"
             + "www.mims.co.za";

            MessageBox.Show(CustomerBiz.SendEmail(Settings.DirectoryPath + "\\INV00000" + ".pdf", "heinreitmann@gmail.com", lSubject, lBody ));            

        }
    }
}
