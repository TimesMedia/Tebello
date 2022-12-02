using Subs.Business;
using Subs.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tebello.Presentation;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for ProForma2.xaml
    /// </summary>
    public partial class ProForma2 : Window
    {
        private readonly CollectionViewSource gSubscriptionViewSource;
        private readonly CollectionViewSource gSelectedSubscriptions = new CollectionViewSource();
        private readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);
        private readonly ObservableCollection<SubscriptionData3> gSubscriptions = new ObservableCollection<SubscriptionData3>();


        public ProForma2()
        {
            InitializeComponent();
            gSubscriptionViewSource = (CollectionViewSource)this.Resources["subscriptionViewSource"];

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.CurrentCustomerId != 0)
            {
                CustomerData3 lCustomer = new CustomerData3(Settings.CurrentCustomerId);
                labelCustomerId.Content = "CustomerId = " + Settings.CurrentCustomerId.ToString();
                labelSurname.Content = "Surname = " + lCustomer.Surname;
            }
        }

        private void ButtonList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear out all the old quotes 

                {
                    string lResult;

                    if ((lResult = SubscriptionBiz.DeleteQuoteExpired()) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                Cursor = Cursors.Wait;

                IEnumerable<Subscription> lSelectedSubscriptions = gDataContext.MIMS_SubscriptionData_FillById("ByPayerProposed", Settings.CurrentCustomerId, 0).ToList();

                if (lSelectedSubscriptions.Count() == 0)
                {
                    MessageBox.Show("No subscription found");
                    return;
                }

                gSubscriptions.Clear();
                foreach (Subscription lSelection in lSelectedSubscriptions)
                {
                    SubscriptionData3 lSubscription = new SubscriptionData3(lSelection.SubscriptionId);
                    gSubscriptions.Add(lSubscription);
                }

                gSubscriptionViewSource.Source = gSubscriptions;

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonListClick", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
            finally 
            {
                Cursor = Cursors.Arrow;
            }

        }

        private void ButtonSelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Pick up the pre-existing Customer form

            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();
            CustomerData3 lCustomerData = new CustomerData3(Settings.CurrentCustomerId);
            labelCustomerId.Content = "CustomerId = " + lCustomerData.CustomerId.ToString();
            labelSurname.Content = "Surname = " + lCustomerData.Surname;
        }

        public Stream GetResourceFileStream(string fileName)
        {
            try
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                // Get all embedded resources
                string[] arrResources = currentAssembly.GetManifestResourceNames();

                foreach (string resourceName in arrResources)
                {
                    if (resourceName.Contains(fileName))
                    {
                        return currentAssembly.GetManifestResourceStream(resourceName);
                    }
                }

                return null;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetResourceFileStream", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return null;
            }
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            try
            {
                // Ensure that some subscriptions were selected.

                if (subscriptionDataGrid.SelectedItems.Count < 1)
                {
                    MessageBox.Show("You did not select something that I can generate a proforma invoice from.");
                    return;
                }

                // Create an additional view on the collection that contains only the selected objects.

                gSelectedSubscriptions.Source = subscriptionDataGrid.SelectedItems;

                // Prime the loop in order to get the payer and the invoice number
                gSelectedSubscriptions.View.MoveCurrentToFirst();

                SubscriptionData3 lFirstSubscription = (SubscriptionData3)gSelectedSubscriptions.View.CurrentItem;

                //DataRowView lDataRowView = (DataRowView)gSelectedSubscriptions.View.CurrentItem;
                //SubscriptionDoc3.SubscriptionRow lFirstRow = (SubscriptionDoc3.SubscriptionRow)lDataRowView.Row;

                int lPayerId = lFirstSubscription.PayerId;
                int? lProFormaId;

                if (lFirstSubscription.ProFormaId == null)
                {
                    lProFormaId = null;
                }
                else
                {
                    // Keep track of the InvoiceNumber of the first subscription encountered.
                    lProFormaId = lFirstSubscription.ProFormaId;
                }

                // Loop through all the selected subscriptions and build the whole invoice

                Cursor = Cursors.Wait;

                foreach (SubscriptionData3 lSelectedSubscription in gSelectedSubscriptions.View)
                {
                    //SubscriptionDoc3.SubscriptionRow lSelectedRow = (SubscriptionDoc3.SubscriptionRow)lSelectedSubscription.Row;

                    if (lPayerId != lSelectedSubscription.PayerId)
                    {
                        MessageBox.Show("I cannot generate an invoice for more than one payer at a time.");
                        return;
                    }

                    if (lSelectedSubscription.Status != SubStatus.Proposed)
                    {
                        MessageBox.Show("I can generate a pro forma invoice only on proposed subscriptions.");
                        return;
                    }

                    if (lSelectedSubscription.ProFormaId == null)
                    {
                        // If this is the case, then all of them should be null
                        if (lProFormaId != null)
                        {
                            MessageBox.Show("The Proforma Invoice Numbers of your list should all be null or all have the same value.");
                            return;
                        }
                    }
                    else
                    {
                        if (lSelectedSubscription.ProFormaId != lProFormaId)
                        {
                            MessageBox.Show("The ProFormaId you select should all have the same pro-forma id.");
                            return;
                        }
                    }

                }  // End of foreach loop

                // Assign a unique number to the invoice if it does not have one already

                if (lProFormaId == null)
                {
                    // Generate one
                    int lNewProFormaId = 0;


                    if (!CounterData.GetUniqueNumber("Proforma", ref lNewProFormaId))
                    {
                        return;
                    }

                    lProFormaId = lNewProFormaId;


                    // Link the invoice to each subscription, but only to the selected ones

                    foreach (SubscriptionData3 lSelectedSubscription in gSelectedSubscriptions.View)
                    {
                        SubscriptionData3 lSubscriptionData = new SubscriptionData3(lSelectedSubscription.SubscriptionId);
                        lSubscriptionData.ProFormaId = lProFormaId;

                        if (!lSubscriptionData.Update()) { return; }
                    }
                }

                // Create one invoice  

                string lResult;
                if ((lResult = OneInvoice2(lPayerId, (int)lProFormaId)) != "OK")
                {
                    MessageBox.Show(lResult);

                }
                else
                {
                    MessageBox.Show("Invoice successfully generated and emailed for Customer " + lPayerId.ToString());
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonGenerate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error inButtonGenerate_Click " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }


        private bool SendEmailInvoice(string pFileName, string pDestination, int PayerId)
        {
            try
            {
                string lSubject = "SUBS Proforma Invoice - Customerid = " + PayerId.ToString();
                string lBody = "Dear Client\n\n"
                 + "Attached herewith your quote."
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

                if (CustomerBiz.SendEmail(pFileName, pDestination, lSubject, lBody) != "OK")
                {
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SendEmailInvoice", "To " + pDestination);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        private string OneInvoice2(int pPayerId, int pInvoiceId)
        {
            try 
            { 
                ProFormaControl lInvoiceControl = new ProFormaControl();
                string lResult;
                if ((lResult = lInvoiceControl.LoadAndRenderInvoice(pPayerId, pInvoiceId)) != "OK")
                {
                    return lResult;
                }
               

                // Email to the customer

                CustomerData3 lCustomerData = new CustomerData3(pPayerId);

                string lOutputFileName = Settings.DirectoryPath + "\\PRO"
                + pInvoiceId.ToString()
                + ".pdf";

                if (SendEmailInvoice(lOutputFileName, lCustomerData.EmailAddress, lCustomerData.CustomerId))
                {
                    string lMessage = "Proforma invoice successfully e-mailed to " + lCustomerData.EmailAddress;

                    ExceptionData.WriteException(5, lMessage, this.ToString(), "OneInvoice2", "CustomerId = " + lCustomerData.CustomerId.ToString());

                    return "OK";
                }
                else
                {
                    return "There was a problem emailing the pro forma invoice to " + lCustomerData.EmailAddress;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "OneInvoice2", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }



        private bool OneInvoice(int PayerId, int pInvoiceId)
        {
            string lStage = "Start";

            // Create the report gnerator

            var lExcelStream = GetResourceFileStream("PROTemplate");

            if (lExcelStream == null)
            {
                return false;
            }

            string lOutputFileName = Settings.DirectoryPath + "\\PRO_" + PayerId.ToString()
                    + "_"
                    + pInvoiceId.ToString()
                    + ".xlsx";

            // Prevent override prompt further on.
            if (File.Exists(lOutputFileName))
            {
                File.Delete(lOutputFileName);
            }

            if (File.Exists(lOutputFileName.Replace(".xlsx", ".pdf")))
            {
                File.Delete(lOutputFileName.Replace(".xlsx", ".pdf"));
            }

            using (var fileStream = File.Create(lOutputFileName))
            {
                lExcelStream.Seek(0, SeekOrigin.Begin);
                lExcelStream.CopyTo(fileStream);
            }

            lExcelStream.Close();  // Close the original template

            ExcelIO lExcelIO = new ExcelIO(lOutputFileName);


            try
            {

                // Create one invoice

                CustomerData3 lCustomerData = new CustomerData3(PayerId);

                // Populate all the fields

                lStage = "InvoiceNumber";
                lExcelIO.PutCellString(lStage, 1, 1, "PRO" + pInvoiceId.ToString());

                lStage = "PersonalData";
                lExcelIO.PutCellString(lStage, 1, 1, lCustomerData.PhoneNumber);
                lExcelIO.PutCellString(lStage, 2, 1, lCustomerData.EmailAddress);
                lExcelIO.PutCellString(lStage, 4, 1, lCustomerData.VendorNumber);
                lExcelIO.PutCellString(lStage, 5, 1, lCustomerData.CompanyRegistrationNumber);
                lExcelIO.PutCellString(lStage, 7, 1, PayerId.ToString());

                lStage = "Date";
                lExcelIO.PutCellDate(lStage, 1, 1, DateTime.Now);

                string lRange = "RegisteredAddress";
                lExcelIO.PutCellString(lRange, 1, 1, lCustomerData.CompanyName);
                lExcelIO.PutCellString(lRange, 2, 1, lCustomerData.FullName);

                if (lCustomerData.PhysicalAddressId != null)
                {
                    DeliveryAddressData2 lDeliveryAddressData = new DeliveryAddressData2((int)lCustomerData.PhysicalAddressId);
                    if (!lDeliveryAddressData.Format())
                    {
                        throw new Exception("Error in Format of deliveryaddress");
                    }

                    lExcelIO.PutCellString(lRange, 3, 1, lDeliveryAddressData.PAddress1);
                    lExcelIO.PutCellString(lRange, 4, 1, lDeliveryAddressData.PAddress2);
                    lExcelIO.PutCellString(lRange, 5, 1, lDeliveryAddressData.PAddress3);
                    lExcelIO.PutCellString(lRange, 6, 1, lDeliveryAddressData.PAddress4);
                    lExcelIO.PutCellString(lRange, 7, 1, lDeliveryAddressData.PAddress5);

                    lRange = "DeliveryAddress";

                   lExcelIO.PutCellString(lRange, 1, 1, lDeliveryAddressData.PAddress1);
                   lExcelIO.PutCellString(lRange, 2, 1, lDeliveryAddressData.PAddress2);
                   lExcelIO.PutCellString(lRange, 3, 1, lDeliveryAddressData.PAddress3);
                   lExcelIO.PutCellString(lRange, 4, 1, lDeliveryAddressData.PAddress4);
                   lExcelIO.PutCellString(lRange, 5, 1, lDeliveryAddressData.PAddress5);
                }

                // Write Items
                lStage = "Items";

                // Allocate the rows

                int lNumberOfRows = 0;
                foreach (SubscriptionData3 lSelectedSubscription in gSelectedSubscriptions.View)
                {
                    lNumberOfRows++;
                }

                // Note that you add NumberOfRows - 1
                for (int j = 1; j < lNumberOfRows; j++)
                {
                    lExcelIO.AddRow("Items");
                }

                int i = 0;

                foreach (SubscriptionData3 lSelectedSubscription in gSelectedSubscriptions.View)
                {
                    //SubscriptionDoc3.SubscriptionRow lSelectedSubscription = (SubscriptionDoc3.SubscriptionRow)lSelectedRowView.Row;

                    lExcelIO.PutCellString("Items", i + 1, 1, lSelectedSubscription.SubscriptionId.ToString());
                    lExcelIO.PutCellString("Items", i + 1, 2, lSelectedSubscription.ReceiverSurname);
                    lExcelIO.PutCellString("Items", i + 1, 3, lSelectedSubscription.StartIssueDescription);
                    lExcelIO.PutCellString("Items", i + 1, 4, lSelectedSubscription.LastIssueDescription);
                    lExcelIO.PutCellDecimal("Items", i + 1, 5, lSelectedSubscription.BaseRate);  // Full price
                    lExcelIO.PutCellDecimal("Items", i + 1, 6, lSelectedSubscription.BaseRate * (1 - lSelectedSubscription.DiscountMultiplier));
                    lExcelIO.PutCellDecimal("Items", i + 1, 7, lSelectedSubscription.DeliveryCost);
                    lExcelIO.PutCellDecimal("Items", i + 1, 8, lSelectedSubscription.Vat);
                    lExcelIO.PutCellDecimal("Items", i + 1, 9, lSelectedSubscription.UnitPrice);
                    lExcelIO.PutCellString("Items", i + 1, 10, lSelectedSubscription.UnitsPerIssue.ToString() + " X " + lSelectedSubscription.NumberOfIssues.ToString());
                    lExcelIO.PutCellDecimal("Items", i + 1, 11, lSelectedSubscription.UnitPrice * lSelectedSubscription.UnitsPerIssue * lSelectedSubscription.NumberOfIssues);

                    i++;
                }

                //  Write the invoice to a file
                lStage = "Write invoice";


                {
                    string lResult;

                    if ((lResult = lExcelIO.SaveAsPdf()) != "OK")
                    {
                        throw new Exception(lResult);

                    }
                }

                // Email to the customer

                lStage = "SendEmail";

                if (SendEmailInvoice(lOutputFileName.Replace(".xlsx", ".pdf"), lCustomerData.EmailAddress, lCustomerData.CustomerId))
                {
                    string lMessage = "Proforma invoice successfully e-mailed to " + lCustomerData.EmailAddress;

                    ExceptionData.WriteException(5, lMessage, this.ToString(), "OneInvoice", "CustomerId = " + lCustomerData.CustomerId.ToString());
                    MessageBox.Show(lMessage);
                    return true;
                }
                else
                {
                    MessageBox.Show("There was a problem emailing the pro forma invoice to " + lCustomerData.EmailAddress);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "OneInvoice", "PayerId = " + PayerId.ToString() + " Stage = " + lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);

                return false;
            }

            finally
            {
                lExcelIO.Close();
            }
        }
    }
}
