using Subs.Business;
using Subs.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tebello.Presentation;

namespace Subs.Presentation
{
    public partial class SubscriptionCaptureChange : Window
    {
        #region Globals 
        private readonly BasketItem gBasketItem;
        private readonly ProductDoc gProductDoc;
       
        #endregion

        #region Construction

        public SubscriptionCaptureChange(BasketItem pBasketItem)
        {
            InitializeComponent();

            try
            {
                gBasketItem = pBasketItem;

                gProductDoc = (ProductDoc)this.Resources["productDoc"];

                Window.DataContext = gBasketItem;

                Dictionary<int, string> lDictionary = ProductBiz.GetDeliveryOptions(gBasketItem.Subscription.ProductId);

                // Prevent international mail
              
                if (gBasketItem.Subscription.ReceiverCountryId != 61)
                {
                    // This is an international customer, therefore international mail is not allowed for a delivery option.
                    lDictionary.Remove((int)DeliveryMethod.Mail);
                    gBasketItem.Subscription.DeliveryMethod = DeliveryMethod.Courier;
                }

                ComboDeliveryMethod.ItemsSource = (Dictionary<int, string>)lDictionary;

                if (gBasketItem.Subscription.ProductId != 0)
                {
                    textProduct.Text = gBasketItem.ProductName;
                }

                if (Settings.Authority < 3)
                {
                    textDiscount.Visibility = Visibility.Hidden;
                    textFinalPrice.Visibility = Visibility.Hidden;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubscriptionCapture2(SubscriptionData, Quote)", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in Subscription2Capture constructor. " + ex.Message);
            }
        }

        #endregion

        #region Event handlers

        private void buttonReceiver_Click(object sender, RoutedEventArgs e)
        {
            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId != 0)
            {
                CustomerData3 lCustomerData = new CustomerData3(Settings.CurrentCustomerId);
            }
        }

        private void buttonSame_Click(object sender, RoutedEventArgs e)
        {
             gBasketItem.Subscription.PayerId = gBasketItem.Subscription.ReceiverId;
        }

        private void buttonPayer_Click(object sender, RoutedEventArgs e)
        {
            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId != 0)
            {
                CustomerData3 lPayer = new CustomerData3(Settings.CurrentCustomerId);

                // Reset the payer, just in case you hit the 'Payer = Receiver' button before. 
                gBasketItem.Subscription.PayerId = 0;
                gBasketItem.Subscription.PayerId = lPayer.CustomerId;
 
            }
        }

        private void textDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(textDiscount.Text, out int lDiscount))
            {
                MessageBox.Show("This is not a number between 0 and 100");
                textDiscount.Clear();
                return;
            }
        }

        private void buttonStartIssue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IssuePicker2 lIssuePicker = new Subs.Presentation.IssuePicker2();
                if (gBasketItem.Subscription.ProductId != 0)
                {
                    lIssuePicker.ProductSelect(gBasketItem.Subscription.ProductId);

                    //gIssuePicker.ShowLatestIssues();

                    // Prime with the proposed startissue if this is a renewal
                    if (gBasketItem.Subscription.ProposedStartIssue != 0)
                    {
                        lIssuePicker.IssueSelect(gBasketItem.Subscription.ProposedStartIssue);
                    }
                }

                lIssuePicker.ShowDialog();

                if (lIssuePicker.IssueWasSelected)
                {

                    // Force them to go through the Last issue button again!

                    gBasketItem.Subscription.ProposedLastIssue = 0;
                    gBasketItem.Subscription.ProposedLastSequence = 0;
                    this.textLastIssueName.Text = "";

                    // Capture this data

                    gBasketItem.Subscription.ProductId = lIssuePicker.ProductId;
                    gBasketItem.Subscription.ProposedStartIssue = lIssuePicker.IssueId;
                    this.textStartIssueName.Text = lIssuePicker.IssueName;
                    gBasketItem.Subscription.ProposedStartSequence = lIssuePicker.Sequence;
                    buttonLastIssue.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("You have not selected a start issue. Try again.");
                    return;
                };
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonStartIssue_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLastIssue_Click(object sender, RoutedEventArgs e)
        {
            buttonLastIssue.IsEnabled = false; // Prevent user from hitting this twice.
            try
            {
                IssuePicker2 lIssuePicker = new Subs.Presentation.IssuePicker2();
                if (gBasketItem.Subscription.ProposedStartSequence == 0)
                {
                    MessageBox.Show("You have to select the start issue first.");
                    return;
                }

                //Propose a last issue

                var lContext = new MIMSDataContext(Settings.ConnectionString);
                var lLastIssue = from lValues in lContext.MIMS_SubscriptionBiz_ProposeLastIssue(gBasketItem.Subscription.ProductId, gBasketItem.Subscription.ProposedStartSequence)
                                 select lValues;
                MIMS_SubscriptionBiz_ProposeLastIssueResult lResult = lLastIssue.Single();
 

                // Prime the dialog

                lIssuePicker.ProductSelect(gBasketItem.Subscription.ProductId);
                if (lResult != null)
                {
                    // It is possible to propose an issueId
                    lIssuePicker.IssueSelect(lResult.LastIssue);
                }

                lIssuePicker.ShowDialog();

                if (!lIssuePicker.IssueWasSelected)
                {
                    MessageBox.Show("You have not selected a last issue. Please try again.");
                    return;
                }

                this.textLastIssueName.Text = lIssuePicker.IssueName;

                if (this.textProduct.Text.CompareTo(lIssuePicker.ProductNaam.ToString()) != 0)
                {
                    MessageBox.Show("Are you mad! How can you start with one product and end with another one?");
                    return;
                }

                int NumberOfIssues = lIssuePicker.Sequence - gBasketItem.Subscription.ProposedStartSequence + 1;

                if (NumberOfIssues < 1)
                {
                    MessageBox.Show("I cannot accept less than one issue.");
                    return;
                }

                // Displays the MessageBox.

                string lMessage = "This will give you " + NumberOfIssues.ToString() + " issues. Is this OK?";

                if (MessageBoxResult.Yes == MessageBox.Show(lMessage, "Warning", MessageBoxButton.YesNo))
                {
                    gBasketItem.Subscription.NumberOfIssues = NumberOfIssues;
                    gBasketItem.Subscription.ProposedLastIssue = lIssuePicker.IssueId;
                    gBasketItem.Subscription.ProposedLastSequence = lIssuePicker.Sequence;
                }

            }
            catch (Exception Ex)
            {
                //Display all the exceptions

                Exception CurrentException = Ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonLastIssue_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in buttonLastIssue " + Ex.Message.ToString());
                return;
            }
            finally
            {
                buttonLastIssue.IsEnabled = false;
            }
        }

        private void buttonSelectDeliveryAddress_Click(object sender, RoutedEventArgs e)
        {
            //DeliveryAddress lDeliveryAddressPicker = DeliveryAddress.GetSingleton();


            if (gBasketItem.Subscription.ReceiverId == 0)
            {
                MessageBox.Show("You have to select a receiver first.");
                return;
            }

            // Check the delivery addresses




            if (!DeliveryAddressStatic.Loaded)
            {
                MessageBox.Show("DeliveryAddresses not loaded yet. Try again in 3 seconds");
                return;
            }

            Subs.Presentation.DeliveryAddress2 lDeliveryAddress = new Subs.Presentation.DeliveryAddress2(gBasketItem.Subscription.ReceiverId);
            lDeliveryAddress.ShowDialog();

            if (lDeliveryAddress.SelectedDeliveryAddressId != null)
            {
                gBasketItem.Subscription.DeliveryAddressId = (int)lDeliveryAddress.SelectedDeliveryAddressId;
            }
            else
            {
                MessageBox.Show("No deliveryaddress has been selected. ");
            }

        }

        private void buttonReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ensure that the Issue and the product tally

                ProductDataStatic.GetIssueId(gBasketItem.Subscription.ProductId, gBasketItem.Subscription.ProposedStartSequence, out int lRegisteredIssueId);

                if (lRegisteredIssueId != gBasketItem.Subscription.ProposedStartIssue)
                {
                    MessageBox.Show("Your Issue does not tally with the Product");
                    return;
                }

                // Confirm that this is a free subscription

                if (gBasketItem.Subscription.UnitPrice == 0)
                {
                    if (MessageBoxResult.No == MessageBox.Show("This is going to be a free subscription. Is this OK?", "Warning", MessageBoxButton.YesNo))
                    {
                        return;
                    }
                }

                // Validate  all the data

                Subs.Data.MimsValidationResult lValidationResult = SubscriptionBiz.Validate(gBasketItem.Subscription);

                if (lValidationResult.Message != "OK")
                {
                    if (lValidationResult.Prompt)
                    {
                        // Query
                        if (MessageBoxResult.No == MessageBox.Show(lValidationResult.Message + " Do you want to continue?", "Warning", MessageBoxButton.YesNo))
                        {
                            return;
                        }
                    }
                    else
                    {
                        ExceptionData.WriteException(5, "Bypassed " + lValidationResult.Message, this.Name, "buttonSubmit_Click", "Receiver = " + gBasketItem.Subscription.ReceiverId.ToString());
                        MessageBox.Show(lValidationResult.Message);
                        return;
                    }
                }



                gBasketItem.Subscription.gReadyToSubmit = true;

                // Note that this subscription will be perservered only once you call SubscriptionBiz.Initialise. 

                this.Close(); ;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonSubmitSubscription_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message); // So, the original level exception is returned. 
            }
            return;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                if (e.Action == ValidationErrorEventAction.Added)
                {

                    MessageBox.Show(e.Error.ErrorContent.ToString());
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ValidationError", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in ValidationError " + ex.Message);
            }
        }

        #endregion
        
    }
}
