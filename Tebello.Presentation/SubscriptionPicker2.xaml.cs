using Subs.Business;
using Subs.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Tebello.Presentation;

namespace Subs.Presentation
{
    public interface ISubscriptionPicker
    {
        int GetCurrentSubscriptionId();
    }

    public enum NonDeliveryOptions
    {
        Returned = 0,
        Lost = 1
    }

    public partial class SubscriptionPicker2 : Window
    {
        #region Globals

        private SubscriptionData3 gSubscriptionData;
        private readonly System.Windows.Forms.SaveFileDialog gSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
        private string gSelectedIssueDescription;
        private readonly ObservableCollection<BasketItem> gBasket = new ObservableCollection<BasketItem>();
        private readonly RenewalData gRenewalData = new RenewalData();

        private readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);
        private SubscriptionDoc3.SubscriptionDerivedDataTable gSubscriptionDerived = new SubscriptionDoc3.SubscriptionDerivedDataTable();
        private Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionDerivedTableAdapter gAdapterSubscription = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionDerivedTableAdapter();
        public ContextMenu gContextMenu;
        private Subs.Presentation.SubscriptionDetailControl2 gUserControl;

        #endregion

        #region Constructors
        public SubscriptionPicker2()
        {
            string lStage = "InitializeComponent";
            try
            {
                InitializeComponent();
                gContextMenu =(ContextMenu)Resources["ContextTransactions"];

                gAdapterSubscription.AttachConnection();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubscriptionPicker2 constructor", lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        #endregion

        #region Window management

        public enum SubscriptionTabs
        {
            Select = 0,
            Report = 1,
            Result2 = 2
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //SubscriptionGrid.Height = this.ActualHeight - 220;
        }

        private void SetVisibility(object sender, RoutedEventArgs e)
        {
            Utilities.SetVisibility(sender);
        }

        private void Mainform_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Title = "Subscription Picker";
                //Subs.Data.LedgerDoc2 ledgerDoc2 = ((Subs.Data.LedgerDoc2)(this.FindResource("ledgerDoc2")));
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Mainform_loaded", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private bool SelectTab(SubscriptionTabs Tab)
        {
            TabControl.SelectedIndex = (int)Tab;
            //this.Show(); // Just in case the bugger was hidden
            return true;
        }

        private int SelectIssue(int ProductId)
        {
            Subs.Presentation.IssuePicker2 frmIssuePicker = new Subs.Presentation.IssuePicker2();

            try
            {
                //Select the product
                frmIssuePicker.ProductSelect(ProductId);

                //frmIssuePicker.ProductVisible = false;

                //Get the IssueId
                frmIssuePicker.ShowDialog();

                if (frmIssuePicker.IssueWasSelected)
                {
                    return frmIssuePicker.IssueId;
                }
                else
                {
                    MessageBox.Show("You have not selected an Issue. Try again.");
                    return 0;
                }
            }
            finally
            {
                //frmIssuePicker.ProductVisible = true;
            }
        }

        #endregion

        #region Select tab

        private void SelectById_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ElicitInteger lElicit = new ElicitInteger("Please enter the subscriptionId as an integer number?");
                lElicit.ShowDialog();
                if (lElicit.Answer == 0)
                {
                    return;
                }

                int lSubscriptionId = (int)lElicit.Answer;

                if (lSubscriptionId == 0)
                {
                    // The operation has been cancelled
                    return;
                }

                SelectById(lSubscriptionId);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }

        }

        public void SelectById(int pSubscriptionId)
        {
            try
            {

                gAdapterSubscription.FillBy(gSubscriptionDerived, "BySubscription", pSubscriptionId, 0);

                if (gSubscriptionDerived.Count() == 0)
                {
                    MessageBox.Show("There is no subscription with number " + pSubscriptionId.ToString());
                    return;
                }

               gUserControl = new Subs.Presentation.SubscriptionDetailControl2(this, gSubscriptionDerived);
               SubscriptionGrid.Content = gUserControl;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SelectById", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void SelectByPayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerPicker3 lCustomerPicker = new CustomerPicker3();
                lCustomerPicker.ShowDialog();

                if (Settings.CurrentCustomerId == 0)
                {
                    // The user did not select a customer
                    return;
                }

                Cursor = Cursors.Wait;

                gAdapterSubscription.FillBy(gSubscriptionDerived, "ByPayer", Settings.CurrentCustomerId, 0);

                if (gSubscriptionDerived.Count() == 0)
                {
                    MessageBox.Show("There is no subscription for payer with number " + Settings.CurrentCustomerId.ToString());
                    return;
                }

                gUserControl = new Subs.Presentation.SubscriptionDetailControl2(this, gSubscriptionDerived);
                SubscriptionGrid.Content = gUserControl;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SelectByPayer_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void SelectByReceiver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerPicker3 lCustomerPicker = new CustomerPicker3();
                lCustomerPicker.ShowDialog();

                if (Settings.CurrentCustomerId == 0)
                {
                    // The user did not select a customer
                    return;
                }

                Cursor = Cursors.Wait;

                gAdapterSubscription.FillBy(gSubscriptionDerived, "ByReceiver", Settings.CurrentCustomerId, 0);

                if (gSubscriptionDerived.Count() == 0)
                {
                    MessageBox.Show("There is no subscription for receiver with number " + Settings.CurrentCustomerId.ToString());
                    return;
                }

                gUserControl = new Subs.Presentation.SubscriptionDetailControl2(this, gSubscriptionDerived);
                SubscriptionGrid.Content = gUserControl;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SelectByReceiver_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally 
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void SelectForRenewalNotices_Click(object sender, RoutedEventArgs e)
        {
            Subs.Presentation.IssuePicker2 frmIssuePicker = new Subs.Presentation.IssuePicker2();

            try
            {
                // First cancel expirations that have been hanging around for too long.
                Cursor = Cursors.Wait;

                if (!SubscriptionBiz.CancelExpiredTooLong())
                {
                    return;
                }

                Cursor = Cursors.Arrow;

                // Get the issue information

                frmIssuePicker.ShowDialog();

                if (!frmIssuePicker.IssueWasSelected)
                {
                    MessageBox.Show("You have not selected an Issue. Try again.");
                    return;
                }

                this.Cursor = Cursors.Wait;

                gAdapterSubscription.FillBy(gSubscriptionDerived, "ByRenewal", frmIssuePicker.IssueId, 0);

                if (gSubscriptionDerived.Count() == 0)
                {
                    MessageBox.Show("There is no subscription for this issue " + frmIssuePicker.IssueId.ToString());
                    return;
                }
              
                Cursor = Cursors.Wait;

                gSelectedIssueDescription = frmIssuePicker.IssueName;

                gUserControl = new Subs.Presentation.SubscriptionDetailControl2(this, gSubscriptionDerived);
                SubscriptionGrid.Content = gUserControl;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SelectForRenewal_Click", "IssueId = " + frmIssuePicker.IssueId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SelectDormant_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionGrid.Content = new Subs.Presentation.SubscriptionDormantControl((ContextMenu)this.Resources["ContextTransactions"]);
        }

        private void SelectAutomaticallyRenewable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gAdapterSubscription.FillBy(gSubscriptionDerived, "ByAutomaticRenewal", 0, 0);


               
                if (gSubscriptionDerived.Count() == 0)
                {
                    MessageBox.Show("There is no subscription that is expired and automatically renewable");
                    return;
                }

                gUserControl = new Subs.Presentation.SubscriptionDetailControl2(this, gSubscriptionDerived);
                SubscriptionGrid.Content = gUserControl;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SelectAutomaticallyRenewable_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void NewSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionsCapture lSubscriptionsCapture = new SubscriptionsCapture();
            lSubscriptionsCapture.ShowDialog();
        }

        private void CancelSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId = 0;
            try
            {
                ElicitString lElicitString = new ElicitString("Why do you want to cancel these subscriptions?");
                lElicitString.ShowDialog();
                if (lElicitString.Answer == "")
                {
                    return;
                }

                ElicitRichText lElicit = new ElicitRichText("Please provide me with a comma delimited list of subscription ids. ");
                lElicit.ShowDialog();
                char[] lSeperator = { '\r', '\n' };
                string[] lSubscriptions = lElicit.Answer.Split(lSeperator, StringSplitOptions.RemoveEmptyEntries);
                lElicit.Close();
                MainWindow.Refresh();
                int lCounter = 0;
                Cursor = Cursors.Wait;

                foreach (string lItem in lSubscriptions)
                {
                    SubscriptionData3 lSubscription = new SubscriptionData3(Int32.Parse(lItem));
                    lCurrentSubscriptionId = lSubscription.SubscriptionId;
                    {
                        string lResult;

                        if ((lResult = SubscriptionBiz.Cancel(lSubscription, lElicitString.Answer)) != "OK")
                        {
                            MessageBox.Show("SubscriptionId " + lSubscription.SubscriptionId.ToString() + " " + lResult);
                            continue;
                        }
                        else
                        {
                            lCounter++;
                        }
                    }
                }
                MessageBox.Show(lCounter.ToString() + " subscriptions cancelled.");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CancelSubscriptions_Click",
                                                 "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
                
        }

        #endregion

        #region Transaction event handlers

        private void Click_Activate(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId = 0;

            try
            {

                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);


                // Get the order number
                ElicitString lElicitString = new ElicitString("What is the order number for the new subscription?");
                lElicitString.ShowDialog();
                if (lElicitString.Answer.Length == 0)
                {
                    MessageBox.Show("I cannot activate this subscription without an order number.");
                    return;
                }
                else
                {
                    gSubscriptionData.OrderNumber = lElicitString.Answer;
                }


                if (gSubscriptionData.Status != SubStatus.Proposed)
                {
                    MessageBox.Show("I cannot activate a subscription, unless it is a proposed subscription.");
                    return;
                }

                if (!SubscriptionBiz.Activate(gSubscriptionData))
                {
                    return;
                }

                MessageBox.Show("The proposed subscription is now activated.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Activate", "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId = 0;

            try
            {
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                // Check for cancelled already - unless it has no invoice.

                if (gSubscriptionData.Status == SubStatus.Cancelled)
                {
                    MessageBox.Show("Subscription " + lCurrentSubscriptionId.ToString() + " is already cancelled.");
                    return;
                }

                // Check for deliveries.

                if (IssueBiz.GetUnitsLeft(gSubscriptionData.SubscriptionId) < (gSubscriptionData.UnitsPerIssue * gSubscriptionData.NumberOfIssues))
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Some units have already been delivered. Do you wish to return or write them off first before cancelling?",
                    "Warning", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning))
                    {
                        return;
                    }
                }

                // Get the reason
                ElicitString lElicitString = new ElicitString("Why do you want to cancel this subscription?");
                lElicitString.ShowDialog();
                if (lElicitString.Answer.Length == 0)
                {
                    MessageBox.Show("I cannot cancel this subscription without a reason.");
                    return;
                }

                // OK, proceed with the operation

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.Cancel(gSubscriptionData, lElicitString.Answer)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // Change the status in the list of subscriptions

                gUserControl.ReflectSubscriptionCancelled(gSubscriptionData.SubscriptionId);


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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Cancel", "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_ChangeAutomaticRenewal(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId;
            ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

            if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
            {
                MessageBox.Show("You have not selected a valid subscription");
                return;
            }

            gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

            string lMessage = "Automatic Renewal is currently set to " + gSubscriptionData.AutomaticRenewal.ToString() + " Do you want to have automatic renewal?";


            // Query
            if (MessageBoxResult.Yes == MessageBox.Show(lMessage, "Query", MessageBoxButton.YesNo))
            {
                if (!SubscriptionBiz.AutomaticRenewal(ref gSubscriptionData, true))
                {
                    MessageBox.Show("Error in SubscriptionBiz.AutomaticRenewal");
                    return;
                }
            }
            else
            {
                if (!SubscriptionBiz.AutomaticRenewal(ref gSubscriptionData, false))
                {
                    MessageBox.Show("Error in SubscriptionBiz.AutomaticRenewal");
                    return;
                }
            }

        }

        private void Click_ChangeReceiver(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId = 0;

            try
            {

                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                // What is the new ReceiverId

                CustomerPicker3 lCustomerPicker = new CustomerPicker3();
                lCustomerPicker.ShowDialog();

                int lNewReceiverId = Settings.CurrentCustomerId;

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.ChangeReceiver(gSubscriptionData, lNewReceiverId)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                    MessageBox.Show("Done");
                }

                // Also change the delivery address. 

                ChangeDeliveryAddress();

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_ChangeReceiver", "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_ChangeDeliveryMethod(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Development in progress");
            return;

        }

        private void ChangeDeliveryAddress()
        {
            int lCurrentSubscriptionId = 0;

            try
            {
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                DeliveryAddress2 lDeliveryAddress = new DeliveryAddress2(gSubscriptionData.ReceiverId);

                lDeliveryAddress.ShowDialog();


                if (lDeliveryAddress.SelectedDeliveryAddressId != null)
                {
                    gSubscriptionData.DeliveryAddressId = lDeliveryAddress.SelectedDeliveryAddressId;

                    gSubscriptionData.Update();
                    MessageBox.Show("Done!");

                }
                else
                {
                    MessageBox.Show("No deliveryaddress has been selected. ");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ChangeDeliveryAddress", "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }


        }

        private void Click_ChangeDeliveryAddress(object sender, RoutedEventArgs e)
        {
            ChangeDeliveryAddress();
        }

        private void Click_ChangeRenewalNotice(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId = 0;

            try
            {
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                string Message = "Do you want to receive renewal notifications?";
                string Caption = "Request for information";
                MessageBoxResult Result;

                // Displays the MessageBox.

                Result = MessageBox.Show(Message, Caption, MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.Yes,
                    MessageBoxOptions.RightAlign);

                if (Result == MessageBoxResult.Yes)
                {
                    if (!SubscriptionBiz.RenewalNotice(gSubscriptionData, true))
                    {
                        MessageBox.Show("Error in SubscriptionBiz.RenewalNotice");
                        return;
                    }
                }
                else
                {
                    if (!SubscriptionBiz.RenewalNotice(gSubscriptionData, false))
                    {
                        MessageBox.Show("Error in SubscriptionBiz.RenewalNotice");
                        return;
                    }
                }

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Cancel", "SubscriptionId = " + lCurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_DeliverOnCredit(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId;
            ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;
            if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
            {
                MessageBox.Show("You have not selected a valid subscription");
                return;
            }

            Subs.Presentation.PaymentAllocation lPaymentAllocation = new Subs.Presentation.PaymentAllocation(lCurrentSubscriptionId);
            lPaymentAllocation.ShowDialog();
        }

        private void Click_GenerateInvoiceDirective(object sender, RoutedEventArgs e)
        {
            ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;
            LedgerDoc2 lLedgerDoc = new LedgerDoc2();
            int lCurrentSubscriptionId;
            try
            {
                // See if the previous run is cleaned up.
                string XMLFile = Settings.DirectoryPath + "\\InvoiceBatch.xml";

                if (File.Exists(XMLFile))
                {
                    MessageBox.Show("Error in Click_GenerateInvoiceDirectivek; File InvoiceBatchDone already exists.");
                    ExceptionData.WriteException(3, "File InvoiceBatchDone already exists.", this.ToString(), "Click_GenerateInvoiceDirective", "");
                    return;
                }

                this.Cursor = Cursors.Wait;

                // Create an invoice

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                LedgerData.LoadInvoiceForSubscription(lCurrentSubscriptionId, ref lLedgerDoc);

                if (lLedgerDoc.InvoiceBatch.Rows.Count != 1)
                {
                    MessageBox.Show("There is no invoice to generate");
                    return;
                }

                LedgerDoc2.InvoiceBatchRow lRow = lLedgerDoc.InvoiceBatch[0];

                //Assign the invoice number

                lRow.BeginEdit();
                lRow.InvoiceId = AdministrationData2.GetInvoiceId();
                lRow.EndEdit();

                //Link the invoice to each subscription

                SubscriptionData3 lSubscriptionData = new SubscriptionData3(lRow.SubscriptionId);
                lSubscriptionData.InvoiceId = lRow.InvoiceId;
                if (!lSubscriptionData.Update()) { return; }

                // Log the new invoice 
     
                LedgerData.Invoice(lRow.PayerId, lRow.InvoiceId, lRow.TransactionId, lSubscriptionData.UnitPrice * lSubscriptionData.UnitsPerIssue * lSubscriptionData.NumberOfIssues);


                // Copy the results out to disk

                lLedgerDoc.InvoiceBatch.WriteXml(Settings.DirectoryPath + "\\InvoiceBatch.xml");

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_GenerateInvoiceDirective", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in Click_GenerateInvoiceDirective " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Click_SubscriptionReport(object sender, RoutedEventArgs e)
        {
            SelectTab(SubscriptionTabs.Report);
        }

        private void Click_Renew(object sender, RoutedEventArgs e)
        {
            try
            {
                int lCurrentSubscriptionId = 0;
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                SubscriptionData3 lSourceSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                Subs.Presentation.SubscriptionsCapture lSubscriptionsCapture = new Subs.Presentation.SubscriptionsCapture(lSourceSubscriptionData);
                lSubscriptionsCapture.ShowDialog();

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Renew", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Click_Renew failed due to technical error");
            }
        }

        private void Click_Resume(object sender, RoutedEventArgs e)
        {
            try
            {
                int lCurrentSubscriptionId = 0;
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.Resume(gSubscriptionData)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                MessageBox.Show("Subscription " + gSubscriptionData.SubscriptionId.ToString() + " successfully resumed.");

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Failed due to technical error");
            }
        }

        private void Click_ReturnIssue(object sender, RoutedEventArgs e)
        {
            NonDeliveryProcessing(NonDeliveryOptions.Returned);
        }

        private void NonDeliveryProcessing(NonDeliveryOptions pOption)
        {
            int lIssueSelected = 0;
            int lCurrentSubscriptionId = 0;

            try
            {

                string lReason;
                int lNumberOfUnits;

                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);
                // Select an issue

                if ((lIssueSelected = SelectIssue(gSubscriptionData.ProductId)) == 0)
                {
                    return;
                }

                ElicitInteger lElicit = new ElicitInteger("Please enter a number of units involved?");
                lElicit.ShowDialog();
                if (lElicit.Answer == 0)
                {
                    return;
                }
                lNumberOfUnits = (int)lElicit.Answer;

                if (lNumberOfUnits == 0)
                {
                    MessageBox.Show("I cannot do this operation with zero issues.");
                    return;
                }


                // Reversibility check check

                if (!IssueBiz.DeliveryReversible(gSubscriptionData.SubscriptionId, lIssueSelected))
                {
                    MessageBox.Show("The delivery is not reversible.");
                    return;
                }

                // Return check

                if (!LedgerData.ReturnCheck(gSubscriptionData.SubscriptionId, lIssueSelected))
                {
                    MessageBox.Show("This issue has already been returned, and cannot be returned twice.");
                    return;
   
                }

                // Get the reason
                ElicitString lElicitString = new ElicitString("Please enter a reason for the return or write-off?");
                lElicitString.ShowDialog();
                if (lElicitString.Answer.Length == 0)
                {
                    MessageBox.Show("I cannot do this operation without a reason.");
                    return;
                }
                else
                {
                    lReason = lElicitString.Answer;
                }


                // Reverse the delivery and give him his money. Also log it.

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.ReverseDelivery(gSubscriptionData, lIssueSelected, lNumberOfUnits, lReason)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }


                if (pOption == NonDeliveryOptions.Returned)
                {
                    // Update the ledger and the stock counts and the ledger

                    decimal Money = lNumberOfUnits * gSubscriptionData.UnitPrice;
                    LedgerData.Return(gSubscriptionData.SubscriptionId, Money, lIssueSelected, lNumberOfUnits, "Delivery returned", lReason);

                    ProductDataStatic.PostReturn(lIssueSelected, lNumberOfUnits);
                }


                if (pOption == NonDeliveryOptions.Lost)
                {
                    decimal Money = Convert.ToInt32(lNumberOfUnits) * gSubscriptionData.UnitPrice;
                    LedgerData.WriteOffStock(gSubscriptionData.SubscriptionId,
                        Money, lIssueSelected,
                        lNumberOfUnits,
                        "Stock written off", lReason);

                    // Record the loss in the Issue table
                    ProductDataStatic.PostLoss(lIssueSelected, lNumberOfUnits);
                }

                // The question now is what to do about future deliveries


                ElicitEnumSingle lFutureAction = new ElicitEnumSingle(typeof(ReturnAction), "What should I do with the returned merchandice?");
                lFutureAction.ShowDialog();

                switch (lFutureAction.Answer)
                {
                    case (int)ReturnAction.Redeliver:
                        {
                            MessageBox.Show("Done.");
                            break;
                        }
                    case (int)ReturnAction.Skip:
                        {
                            // Skip this issue and append it to the end of the subscription

                            {
                                string lResult;

                                if ((lResult = IssueBiz.Skip(gSubscriptionData.SubscriptionId, lIssueSelected)) != "OK")
                                {
                                    MessageBox.Show(lResult);
                                    return;
                                }
                            }

                            MessageBox.Show("Done.");
                            break;
                        }
                    default:
                        {
                            break;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "NonDeliveryProcessing",
                        "SubscriptionId=" + lCurrentSubscriptionId.ToString() + " IssueId= " + lIssueSelected.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Subscription non-delivery recording failed.");

                return;

            }
        }

        private void Click_Skip(object sender, RoutedEventArgs e)
        {
            try
            {
                int lCurrentSubscriptionId = 0;
                // Select the issue
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }

                gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);

                int lIssueSelected = SelectIssue(gSubscriptionData.ProductId);

                // See if the issue exists for this subscription

                if (!IssueBiz.UnitsLeft(gSubscriptionData.SubscriptionId, lIssueSelected))
                {
                    MessageBox.Show("The issue that you selected does not exist in this subscription.");
                    return;
                }
                else
                {
                    {
                        string lResult;

                        if ((lResult = IssueBiz.Skip(gSubscriptionData.SubscriptionId, lIssueSelected)) != "OK")
                        {
                            MessageBox.Show(lResult);
                            return;
                        }
                    }

                    MessageBox.Show("Done.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_Suspend(object sender, RoutedEventArgs e)
        {
            int lCurrentSubscriptionId;
            string lReason;
            ElicitString lElicitString = new ElicitString("What is the reason for the suspension?");
            lElicitString.ShowDialog();
            if (lElicitString.Answer.Length == 0)
            {
                MessageBox.Show("I cannot activate this subscription without a reason.");
                return;
            }
            else
            {
                lReason = lElicitString.Answer;
            }


            // Select the issue
            ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;

            if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
            {
                MessageBox.Show("You have not selected a valid subscription");
                return;
            }

            gSubscriptionData = new SubscriptionData3(lCurrentSubscriptionId);
            {
                string lResult;

                if ((lResult = SubscriptionBiz.Suspend(gSubscriptionData, lReason)) != "OK")
                {
                    MessageBox.Show(lResult);
                    return;
                }
            }
        }

        private void Click_WriteOffIssue(object sender, RoutedEventArgs e)
        {
            NonDeliveryProcessing(NonDeliveryOptions.Lost);
        }

        #endregion

        #region Report tab
        public void DisplayStatusAndHistory()
        {
            int lCurrentSubscriptionId;
            try
            {

                // Dynamically get the subscriptionId, irrespective of the layout of the selection data grid. = Polymorphism.
                ISubscriptionPicker lSubscriptionGrid = (ISubscriptionPicker)SubscriptionGrid.Content;
                if ((lCurrentSubscriptionId = lSubscriptionGrid.GetCurrentSubscriptionId()) == 0)
                {
                    MessageBox.Show("You have not selected a valid subscription");
                    return;
                }
               

                this.Cursor = Cursors.Wait;

                lSubscriptionStatusPrintControl.SubscriptionId = lCurrentSubscriptionId;
                SelectTab(SubscriptionTabs.Result2);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "DisplayStatusAndHistory", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Click_StatusAndHistoryReport2(object sender, RoutedEventArgs e)
        {
            DisplayStatusAndHistory();
        }
        private void Click_GenerateRenewalNotices(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check that date has been updated.
                if (DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please select a payment date first.");
                    return;
                }

                if (DatePicker.SelectedDate < DateTime.Now.Date)
                {
                    MessageBox.Show("Please select a date in the future.");
                    return;
                }

                // See if there are any subscriptions selected for renewal

                if (gSubscriptionDerived.Count() <= 0)
                {
                    MessageBox.Show("You did not select any subscriptions for renewal. Please do that first.");
                    return;
                }

                string lRenewalFileName = "c:\\SUBS\\RenewalNotice"
                     + "_"
                     + gSelectedIssueDescription
                     + "_"
                     + System.DateTime.Now.ToLongDateString()
                     + ".xml";

                this.Cursor = Cursors.Wait;

                if (!Generate((DateTime)DatePicker.SelectedDate, lRenewalFileName))
                {
                    return;
                }

                MessageBox.Show("Written to " + lRenewalFileName.ToString());
                // Here you should refer to the renewals that has been generated on the Search tab.
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_RenewalNotices", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        public bool Generate(DateTime pPayTime, string pRenewalFileName)
        {
            try
            {
                RenewalDoc1 lRenewal = new RenewalDoc1();

                int i = 0;

                foreach (SubscriptionDoc3.SubscriptionDerivedRow lRow in gSubscriptionDerived)
                {
                    i++;
                    if (i % 5 == 0)
                    {
                        this.ProgressBar1.Value = i * 100 / gSubscriptionDerived.Count();
                        this.ProgressBar1.Refresh();
                    }

                    SubscriptionData3 lCurrentSubscription = new SubscriptionData3(lRow.SubscriptionId);

                    if (!BuildRenewalRecord(lCurrentSubscription, ref lRenewal, pPayTime))
                    {
                        return false;
                    }

                } // End of for each loop

                // Write out to Excel
                lRenewal.RenewalRecord.WriteXml(pRenewalFileName, System.Data.XmlWriteMode.IgnoreSchema);

                string OutputFile = pRenewalFileName.Replace("xml", "xsd");
                lRenewal.RenewalRecord.WriteXmlSchema(OutputFile);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Generate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool BuildRenewalRecord(SubscriptionData3 pSubscription, ref RenewalDoc1 pRenewal, DateTime pPayTime)
        {
            //ProductData lProductData = new ProductData();

            try
            {
                // Create a new record
                RenewalDoc1.RenewalRecordRow lNewRow = pRenewal.RenewalRecord.NewRenewalRecordRow();

                // Get data from database

                {
                    string lResult;

                    if ((lResult = gRenewalData.Load(lNewRow, pSubscription.SubscriptionId)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
                }

                //Create a dummy subscription to emulate the future renewal

                SubscriptionData3 lSubscriptionData = new SubscriptionData3(pSubscription.SubscriptionId);


                // Adjust it to fit the proposed renewal

                // Calculate the First issue
                int PreviousLastIssue = IssueBiz.GetLastIssue(pSubscription.SubscriptionId);

                int FirstIssue = 0;

                {
                    string lResult;

                    if ((lResult = ProductDataStatic.IssueId(PreviousLastIssue, 1, ref FirstIssue)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
                }

                // Get the default number of issues

                if (!ProductDataStatic.GetNumberOfIssuesByIssue(FirstIssue, out int NumberOfIssues))
                {
                    throw new Exception("Error in calculating the number of issues for " + pSubscription.SubscriptionId.ToString());
                }

                lSubscriptionData.NumberOfIssues = NumberOfIssues;

                // Status

                lSubscriptionData.Status = SubStatus.Deliverable;


                // Pricing

                if (!lSubscriptionData.SetBaseRateVatPercentage(pPayTime))
                {
                    throw new Exception("Error in calculating base rate and VAT percentage for " + pSubscription.SubscriptionId.ToString());
                }

                decimal Total = 0;

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.SetUnitPriceAndVat(lSubscriptionData)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return false;
                    }
                }

                Total = lSubscriptionData.UnitsPerIssue * lSubscriptionData.NumberOfIssues * lSubscriptionData.UnitPrice;


                // Populate the renewal record with the rest of the data

                lNewRow.UnitsPerIssue = lSubscriptionData.UnitsPerIssue.ToString();
                lNewRow.NumberOfIssues = lSubscriptionData.NumberOfIssues.ToString();
                lNewRow.UnitPrice = lSubscriptionData.UnitPrice.ToString("C");
                lNewRow.TotalPrice = Total.ToString("C");
                lNewRow.Discount = "0";

                // Get the name of the last issue

                string LastIssueName = ProductDataStatic.GetIssueDescription(PreviousLastIssue);
                lNewRow.LastIssue = LastIssueName;

                // Add the record
                pRenewal.RenewalRecord.AddRenewalRecordRow(lNewRow);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "BuildRenewalRecord", "SubscriptionId = " + pSubscription.SubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return false;
            }
        }


        #endregion

    }
}
