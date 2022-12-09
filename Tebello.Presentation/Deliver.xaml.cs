using Microsoft.Win32;
using Subs.Business;
using Subs.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Subs.Presentation;

namespace Tebello.Presentation
{
    /// <summary>
    /// Interaction logic for Deliver.xaml
    /// </summary>
    public partial class Deliver : Window
    {
       
        #region Globals
        private Subs.Presentation.IssuePicker2 frmIssuePicker;
        private int gIssueId = 0;
        private Subs.Data.DeliveryDoc gDeliveryDoc = new DeliveryDoc();
        private readonly CollectionViewSource gDeliveryRecordViewSource;
        private readonly BackgroundWorker gBackgroundWorker;
        private readonly BackgroundWorker gBackgroundWorkerPost;
        private string gCurrentProduct = "";

        [Serializable]
        public class DeliveryItem
        {
            public string WaybillNumber = "               ";
            public string Date;
            public string Contact_NameAndSurname;
            public string Company;
            public string Building;
            public string Street;
            public string Suburb;
            public string City;
            public string PostalCode;
            public string Country;
            public string ContactWork_Cell_Phonenumber;
            public decimal Weight = 1;
            public decimal Length = 1;
            public decimal Width = 1;
            public decimal Height = 1;
            public decimal Value = 1;
            public string WeekendDelivery = "No";
            public int Pieces;
            public string Special_Instructions = " |";
            public string EmailAddress;
            public int Reference1;   // CustomerId
            public string Reference2; // IssueDescription X n
            public string Reference3; // Full Name of customer
            public string Importer_Exporter_Code = "21340871";
            public string ProductName;
            public int CustomerId;
        }

        public class ProcessedFile
        {
            public string FileName;
            public DateTime Datum;
        }

        [Serializable]
        public class ProcessedFiles
        {
            public List<ProcessedFile> Items = new List<ProcessedFile>();
        }

        private ProcessedFiles gProcessedFiles = new ProcessedFiles();
        private string gProcessedFileName = Settings.DirectoryPath + "\\Deliveries\\Processed.xml";
        private XmlSerializer gProcessedSerializer = new XmlSerializer(typeof(ProcessedFiles));

        private List<DeliveryItem> gDeliveryItemsRaw = new List<DeliveryItem>();
        private List<DeliveryItem> gDeliveryItems = new List<DeliveryItem>();

        private class PackageCounter
        {
            public int CustomerId;
            public string IssueDescription;
            public int UnitsPerIssue;
        }

        private List<PackageCounter> gPackageCounters = new List<PackageCounter>();

        [Serializable]
        public class InventoryItem
        {
            public string IssueDescription;
            public int Units;
        }

        [Serializable]
        public class DeliveriesMethod
        {
            public string Name;
            public List<InventoryItem> Items = new List<InventoryItem>();
        }

        [Serializable]
        public class Inventory
        {
            public List<DeliveriesMethod> Methods = new List<DeliveriesMethod>();
        }

        Inventory gInventory = new Inventory();


        [Serializable]
        public class SelectedFiles
        {
            public List<string> Files = new List<string>();
        }

        #endregion

        #region Constructor
        public Deliver()
        {
            InitializeComponent();
            gDeliveryRecordViewSource = (CollectionViewSource)this.Resources["deliveryRecordViewSource"];
            gBackgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            gBackgroundWorkerPost = ((BackgroundWorker)this.FindResource("backgroundWorkerPost"));

            gProcessedFiles.Items = new List<ProcessedFile>();
        }
        #endregion

        #region Form Management
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gDeliveryDoc = ((Subs.Data.DeliveryDoc)(this.FindResource("deliveryDoc")));
        }
        #endregion

        #region Propose

        private void buttonProposal(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            gCurrentProduct = "";
            // Get an Issueid
            frmIssuePicker = new Subs.Presentation.IssuePicker2();
            frmIssuePicker.ShowDialog();

            if (frmIssuePicker.IssueWasSelected)
            {
                labelProduct.Content = frmIssuePicker.ProductNaam;
                labelIssue.Content = frmIssuePicker.IssueName;
                gIssueId = frmIssuePicker.IssueId;
            }
            else
            {
                MessageBox.Show("You have not selected an issue. Please try again.");
                return;
            }

            try
            {
                // Ok if you got this far you have a valid issueid - so you can continue

                gDeliveryDoc.Clear();

                {
                    string lResult;

                    if ((lResult = DeliveryDataStatic.Load(gIssueId, ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                    else
                    {
                        int lUnits = gDeliveryDoc.DeliveryRecord.Sum(p => p.UnitsPerIssue);
                        MessageBox.Show("I have generated " + gDeliveryDoc.DeliveryRecord.Count.ToString() + " proposals for " + lUnits.ToString() + " units.");
                    }
                }

                //gProposalValid = false; // Enforce validation
                gCurrentProduct = frmIssuePicker.IssueName;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonProposal", "");
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

        private void buttonProposalActive(object sender, RoutedEventArgs e)
        {
            try
            {
                gCurrentProduct = "";
                labelProduct.Content = "All active products";
                labelIssue.Content = "";

                {
                    string lResult;

                    gDeliveryDoc.Clear();

                    if ((lResult = DeliveryDataStatic.LoadActive(ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                    else
                    {
                        int lUnits = gDeliveryDoc.DeliveryRecord.Count;
                        MessageBox.Show("I have generated a proposal with " + lUnits.ToString() + " delivery records.");
                    }
                }

                gCurrentProduct = "AllActive";
                //buttonPost.IsEnabled = false;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonProposalActive", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }


        #endregion

        #region Validate


        #endregion

        #region Post

        private void buttonPost_Click(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                MessageBox.Show("You are too fast for me. I have not completed the previous task yet. Request will be ignored. ");
                return;
            }

            if (gDeliveryDoc.DeliveryRecord.Where(p => p.ValidationStatus != "Deliverable").Count() > 0)
            {
                MessageBox.Show("The proposal is invalid. It cannot be posted.");
                return;
            }

            // Continue with the proposal

            gDeliveryDoc.DeliveryRecord.AcceptChanges();

            this.Cursor = Cursors.Wait;

            try
            {
                this.Cursor = Cursors.Wait;

                // In case of a crash, you can rerun the delivery from the same XML. The system
                // will not redeliver an issue if is has already been done on the previous run.
                this.Cursor = Cursors.Wait;
                gBackgroundWorkerPost.RunWorkerAsync(gDeliveryDoc);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Post", "");
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

        private void backgroundWorkerPost_DoWork(object sender, DoWorkEventArgs e)
        {
            DeliveryDoc lDeliveryDoc = (DeliveryDoc)e.Argument;
            e.Result = ProductBiz.PostDelivery(lDeliveryDoc, gBackgroundWorkerPost);
        }

        private void backgroundWorkerPost_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressType lProgress = (ProgressType)e.Result;

            if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An error occurred in the background post");
                return;
            }

            MessageBox.Show("Posting successful.");
        }

        private void backgroundWorkerPost_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
        }



        #endregion

        #region Format


        private void FormatRegisteredMail(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            try
            {
                if (checkPayers.IsChecked == false & checkNonPayers.IsChecked == false)
                {
                    MessageBox.Show("This way you are not going to get any labels.");
                    return;
                }
                //OpenFileDialog lFileDialog = new OpenFileDialog();

                //lFileDialog.InitialDirectory = Settings.DirectoryPath;
                //lFileDialog.ShowDialog();
                //string lFileName = lFileDialog.FileName.ToString();
                //if (!File.Exists(lFileName))
                //{
                //    MessageBox.Show("You have not selected a valid source file ");
                //    return;
                //}

                //gDeliveryDoc.Clear();
                //gDeliveryDoc.DeliveryRecord.ReadXml(lFileName);

                OpenFileDialog lFileDialog = new OpenFileDialog();

                lFileDialog.InitialDirectory = Settings.DirectoryPath + "\\Deliveries";
                lFileDialog.Multiselect = true;
                lFileDialog.ShowDialog();

                if (lFileDialog.FileNames.Count() == 0)
                {
                    MessageBox.Show("You have not selected any files.");
                    return;
                }

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    if (!lFileName.Contains("\\RegisteredMail_"))
                    {
                        MessageBox.Show("I can accept only files of which the name starts with 'RegisteredMail_'");
                        return;
                    }
                }

                gDeliveryDoc.DeliveryRecord.Clear();

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    //Append all the files into the DeliveryRecord table
                    gDeliveryDoc.DeliveryRecord.ReadXml(lFileName);
                }

                {
                    string lResult;

                    if ((lResult = ProductBiz.FilterOnPayment((bool)checkPayers.IsChecked, (bool)checkNonPayers.IsChecked, ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // Generate the registered mail list

                //Sort by customerid

                gDeliveryDoc.DeliveryRecord.DefaultView.Sort = "ReceiverId";

                if (!ProductBiz.GenerateRegisteredMail("Surname, Initials", ref gDeliveryDoc))
                {
                    return;
                }

                string OutputFile = Settings.DirectoryPath + "\\Final_RegisteredMailList_" + lFileDialog.SafeFileName;
                gDeliveryDoc.RegisteredMail.WriteXml(OutputFile);

                MessageBox.Show(gDeliveryDoc.RegisteredMail.Count.ToString() + " Records written to " + OutputFile.ToString());

                OutputFile = OutputFile.Replace("xml", "xsd");
                gDeliveryDoc.RegisteredMail.WriteXmlSchema(OutputFile);

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FormatRegisteredMail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }

            finally
            {
                // Stop the thread

                this.Cursor = Cursors.Arrow;
            }
        }

        #endregion
        
        private void FormatMagMail(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            try
            {
                if (checkPayers.IsChecked == false & checkNonPayers.IsChecked == false)
                {
                    MessageBox.Show("This way you are not going to get any labels.");
                    return;
                }

                OpenFileDialog lFileDialog = new OpenFileDialog();

                lFileDialog.InitialDirectory = Settings.DirectoryPath + "\\Deliveries";
                lFileDialog.Multiselect = true;
                lFileDialog.ShowDialog();

                if (lFileDialog.FileNames.Count() == 0)
                {
                    MessageBox.Show("You have not selected any files.");
                    return;
                }

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    if (!lFileName.Contains("\\Mail_"))
                    {
                        MessageBox.Show("I can accept only files of which the name starts with 'RegisteredMail_'");
                        return;
                    }
                }

                gDeliveryDoc.DeliveryRecord.Clear();

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    //Append all the files into the DeliveryRecord table
                    gDeliveryDoc.DeliveryRecord.ReadXml(lFileName);
                }

                {
                    string lResult;

                    if ((lResult = ProductBiz.FilterOnPayment((bool)checkPayers.IsChecked, (bool)checkNonPayers.IsChecked, ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // Generate the mag mail list

                //Sort by customerid

                gDeliveryDoc.DeliveryRecord.DefaultView.Sort = "ReceiverId";

                if (!ProductBiz.GenerateRegisteredMail("Surname, Initials", ref gDeliveryDoc))
                {
                    return;
                }

                string OutputFile = Settings.DirectoryPath + "\\Final_MagMailList_" + lFileDialog.SafeFileName;
                gDeliveryDoc.RegisteredMail.WriteXml(OutputFile);

                MessageBox.Show(gDeliveryDoc.RegisteredMail.Count.ToString() + " Records written to " + OutputFile.ToString());

                OutputFile = OutputFile.Replace("xml", "xsd");
                gDeliveryDoc.RegisteredMail.WriteXmlSchema(OutputFile);

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FormatRegisteredMail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }

            finally
            {
                // Stop the thread

                this.Cursor = Cursors.Arrow;
            }

        }

        private void Skip(object sender, RoutedEventArgs e)
        {
            try
            {
                // Warning

                if (MessageBoxResult.No == MessageBox.Show("Are you sure that you want to skip all these entries. To reverse this operation is cumbersome and dangerous!?",
                    "Warning", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning))
                {
                    return;
                }

                ProductDoc.IssueDataTable lIssue = new ProductDoc.IssueDataTable();
                Subs.Data.ProductDocTableAdapters.IssueTableAdapter lAdaptor = new Subs.Data.ProductDocTableAdapters.IssueTableAdapter();
                lAdaptor.AttachConnection();
                lAdaptor.FillById(lIssue, gDeliveryDoc.DeliveryRecord[0].IssueId, "IssueId");

                if (lIssue[0].StartDate > DateTime.Now)
                {

                    if (MessageBoxResult.No == MessageBox.Show("This issue starts in the future" + ".\n This skip cannot be undone. Do you want to continue?",
                      "Warning", MessageBoxButton.YesNo))
                    {
                        return;
                    }
                }

                int lCount = 0;

                this.Cursor = Cursors.Wait;
                foreach (DeliveryDoc.DeliveryRecordRow lRow in gDeliveryDoc.DeliveryRecord)
                {
                    if (lRow.Skip)
                    {
                        {
                            string lResult;

                            if ((lResult = IssueBiz.Skip(lRow.SubscriptionId, lRow.IssueId)) != "OK")
                            {
                                MessageBox.Show(lResult);
                                return;
                            }
                            else
                            {
                                lCount++;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                } // End of foreach

                MessageBox.Show("Success. You have skipped " + lCount.ToString() + " issues. Please regenerate a proposal.");
            } // End of try
            finally
            {
                gDeliveryDoc.DeliveryRecord.Clear();
                Cursor = Cursors.Arrow;
            }
        }

        private void LoadValidProposal(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog lOpenFileDialog = new OpenFileDialog();

                lOpenFileDialog.InitialDirectory = Settings.DirectoryPath + "\\Recovery\\";
                lOpenFileDialog.ShowDialog();
                string FileName = lOpenFileDialog.FileName.ToString();

                if (!File.Exists(FileName))
                {
                    MessageBox.Show("You have not selected a valid source file ");
                    return;
                }

                gDeliveryDoc.DeliveryRecord.Clear();
                gDeliveryDoc.DeliveryRecord.ReadXml(FileName);



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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "LoadValidProposal", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }

        }

        private void FormatCourierList(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                // Validation and posting is still in progress.
                return;
            }

            this.Cursor = Cursors.Wait;
            int lCurrentReceiverId = 0;
            OpenFileDialog lFileDialog = new OpenFileDialog();
            DeliveryItem lNewDeliveryItem = new DeliveryItem();
            CustomerData3 lCustomerData;
            List<DeliveryItem> International = new List<DeliveryItem>();
            List<DeliveryItem> Economy = new List<DeliveryItem>();


            try
            {
                if (checkPayers.IsChecked == false & checkNonPayers.IsChecked == false)
                {
                    MessageBox.Show("This way you are not going to get any labels.");
                    return;
                }

                SelectProposalFiles();
                if (lFileDialog.FileNames.Count() == 0)
                {
                    MessageBox.Show("You have not selected any files.");
                    return;
                }
                if (!ValidSelection()) return;

                // Combine all the filenames into an ADO table            
                gDeliveryDoc.DeliveryRecord.Clear();
                SelectedFiles lSelectedFiles = new SelectedFiles();
                foreach (string lSelectedFileName in lFileDialog.FileNames)
                {
                    //Append the content of all the files into the DeliveryRecord table
                    gDeliveryDoc.DeliveryRecord.ReadXml(lSelectedFileName);
                    lSelectedFiles.Files.Add(lSelectedFileName);
                    gProcessedFiles.Items.Add(new ProcessedFile() { FileName = lSelectedFileName, Datum = DateTime.Now });
                }

                string lResult = ProductBiz.FilterOnPayment((bool)checkPayers.IsChecked, (bool)checkNonPayers.IsChecked, ref gDeliveryDoc);
                if (lResult != "OK")
                {
                    MessageBox.Show(lResult);
                    return;
                }

                gDeliveryDoc.DeliveryRecord.DefaultView.Sort = "ReceiverId, IssueId";

                CreateRawDeliveryList();

                ConsolidateRawDeliveryList();

                SplitDeliveryListByDeliveryMethod();

                BuildInventoryForAll();

                SerialiseResults();

                //if (RegisterResults())
                //{
                //    MessageBox.Show("Everything succeeded");
                //}
                //else
                //{
                //    MessageBox.Show("Something went wrong with the registration of listings.");
                //}


                //**************************************************************************************************************************************************************

                void SelectProposalFiles()
                {
                    // Select all the delivery proposal files that you want to process.

                    lFileDialog.InitialDirectory = Settings.DirectoryPath + "\\Deliveries";
                    lFileDialog.Multiselect = true;
                    lFileDialog.ShowDialog();
                }

                bool ValidSelection()
                {
                    // Get the details of all the files that have been processed already 
                    if (File.Exists(gProcessedFileName))
                    {
                        FileStream lProcessedStream = new FileStream(gProcessedFileName, FileMode.Open);
                        gProcessedFiles = (ProcessedFiles)gProcessedSerializer.Deserialize(lProcessedStream);
                        lProcessedStream.Close();
                    }

                    foreach (string lFileName in lFileDialog.FileNames)
                    {
                        if (!lFileName.Contains("\\Courier_"))
                        {
                            MessageBox.Show("I can accept only files of which the name starts with 'Courier'");
                            return false;
                        }

                        ProcessedFiles lHits = new ProcessedFiles();

                        lHits.Items = (List<ProcessedFile>)gProcessedFiles.Items.Where(x => x.FileName == lFileName).ToList();

                        if (lHits.Items.Count > 0)
                        {
                            StringBuilder lStringBuilder = new StringBuilder();
                            foreach (ProcessedFile item in lHits.Items)
                            {
                                lStringBuilder.Append(item.Datum.ToString() + " ");
                            }

                            if (MessageBoxResult.No == MessageBox.Show("You have already precessed this file on " + lStringBuilder
                                                                     + " Do you really want to do it again?", "Warning",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }

                void CreateRawDeliveryList()
                {
                    gDeliveryItemsRaw.Clear();
                    gPackageCounters.Clear();
                    foreach (DataRowView lDataRowView in gDeliveryDoc.DeliveryRecord.DefaultView)
                    {
                        DeliveryDoc.DeliveryRecordRow lRow = (DeliveryDoc.DeliveryRecordRow)lDataRowView.Row;
                        lNewDeliveryItem = new DeliveryItem();
                        gDeliveryItemsRaw.Add(lNewDeliveryItem);
                        lCurrentReceiverId = lRow.ReceiverId;

                        if (CustomerData3.Exists(lRow.ReceiverId))
                        {
                            lCustomerData = new CustomerData3(lRow.ReceiverId);
                        }
                        else
                        {
                            MessageBox.Show("It seems as though customer " + lRow.ReceiverId.ToString() + " does not exist anymore.");
                            return;
                        }

                        lNewDeliveryItem.CustomerId = lRow.ReceiverId;
                        lNewDeliveryItem.ProductName = lRow.Product;

                        lNewDeliveryItem.Date = DateTime.Now.ToString("ddMMyyyy");
                        lNewDeliveryItem.Contact_NameAndSurname = lRow.Title + " " + lRow.Initials + " " + lRow.Surname;

                        if (!lRow.IsCompanyNull())
                        {
                            lNewDeliveryItem.Company = lRow.Company;
                        }

                        DeliveryAddressData2 lDeliveryAddressData = new DeliveryAddressData2(lRow.DeliveryAddressId);

                        if (lDeliveryAddressData.Building != "")
                        {
                            lNewDeliveryItem.Building = "Building: " + lDeliveryAddressData.Building;
                            if (lDeliveryAddressData.FloorNo != "")
                            {
                                lNewDeliveryItem.Building = lNewDeliveryItem.Building + " Floor: " + lDeliveryAddressData.FloorNo;
                                if (lDeliveryAddressData.Room != "")
                                {
                                    lNewDeliveryItem.Building = lNewDeliveryItem.Building + " Room: " + lDeliveryAddressData.Room;
                                }
                            };
                        }

                        lNewDeliveryItem.Street = lDeliveryAddressData.StreetNo + " " + lDeliveryAddressData.Street + " " + lDeliveryAddressData.StreetExtension
                                                                            + " " + lDeliveryAddressData.StreetSuffix;
                        lNewDeliveryItem.Suburb = lDeliveryAddressData.Suburb;
                        lNewDeliveryItem.City = lDeliveryAddressData.City;

                        lNewDeliveryItem.PostalCode = lDeliveryAddressData.PostCode;
                        lNewDeliveryItem.Country = lDeliveryAddressData.CountryName;

                        lNewDeliveryItem.ContactWork_Cell_Phonenumber = lCustomerData.CellPhoneNumber;

                        if (lCustomerData.CellPhoneNumber == "")
                        {
                            lNewDeliveryItem.ContactWork_Cell_Phonenumber = lCustomerData.PhoneNumber;
                        }

                        lNewDeliveryItem.Weight = lRow.Weight * lRow.UnitsPerIssue;
                        lNewDeliveryItem.Length = lRow.Length;
                        lNewDeliveryItem.Width = lRow.Width;
                        if (lRow.IsHeightNull())
                        {
                            lNewDeliveryItem.Height = 1;
                        }
                        else
                        {
                            lNewDeliveryItem.Height = lRow.Height;
                        }


                        lNewDeliveryItem.Height = lRow.Height;
                        lNewDeliveryItem.Value = lRow.UnitPrice * lRow.UnitsPerIssue;

                        lNewDeliveryItem.Pieces = lRow.UnitsPerIssue;
                        lNewDeliveryItem.Special_Instructions = lDeliveryAddressData.Room + lDeliveryAddressData.FloorNo + lDeliveryAddressData.Building + lDeliveryAddressData.SDI;

                        lNewDeliveryItem.EmailAddress = lRow.EmailAddress;

                        lNewDeliveryItem.Reference1 = lRow.ReceiverId;
                        lNewDeliveryItem.Reference3 = lRow.Title + " " + lRow.Initials + " " + lRow.Surname;

                        gPackageCounters.Add(new PackageCounter() { CustomerId = lRow.ReceiverId, IssueDescription = lRow.IssueDescription, UnitsPerIssue = lRow.UnitsPerIssue });
                    } // End of foreach loop
                }

                void ConsolidateRawDeliveryList()
                {

                    // Consolidate the raw DeliveryItems by CustomerId

                    var lCustomerGroups = gDeliveryItemsRaw.GroupBy(p => p.CustomerId);

                    foreach (IGrouping<int, DeliveryItem> lCustomerGroup in lCustomerGroups)
                    {
                        DeliveryItem lNewItem = lCustomerGroup.ElementAt(0);

                        lNewItem.Weight = lCustomerGroup.Sum(p => p.Weight);
                        lNewItem.Value = lCustomerGroup.Sum(p => p.Value);

                        lNewItem.Length = lCustomerGroup.Max(p => p.Length);
                        lNewItem.Width = lCustomerGroup.Max(p => p.Width);
                        lNewItem.Height = lCustomerGroup.Max(p => p.Height);

                        int lUnitsPerIssue = 0;
                        string lProductString = "";
                        var lProductGroups = lCustomerGroup.GroupBy(p => p.ProductName);
                        foreach (IGrouping<string, DeliveryItem> lProductGroup in lProductGroups)
                        {
                            lUnitsPerIssue = lProductGroup.Sum(p => p.Pieces);
                            lProductString = lProductString + lProductGroup.ElementAt(0).ProductName + " X " + lUnitsPerIssue.ToString() + "; ";
                        }

                        lNewItem.Reference2 = lProductString;
                        lNewItem.Pieces = lCustomerGroup.Sum(p => p.Pieces);
                        gDeliveryItems.Add(lNewItem);
                    }
                }

                void SplitDeliveryListByDeliveryMethod()
                {
                    foreach (DeliveryItem lItem in gDeliveryItems)
                    {
                        if (lItem.Country != "RSA")
                        {
                            International.Add(lItem);
                        }
                        else
                        {
                            Economy.Add(lItem);
                        }
                    }
                }

                void BuildInventoryForAll()
                {
                    // Build the inventory for all deliverymethods
                    gInventory.Methods.Clear();
                    BuildInventory(International, "International");
                    BuildInventory(Economy, "Economy");
                }


                void SerialiseResults()
                {
                    SerialiseList(International, "International");
                    SerialiseList(Economy, "Economy");

                    // Write the inventory to XML
                    string lInventoryFileName = Settings.DirectoryPath + "\\Final_CourierList_ZInventory " + DateTime.Now.ToString("yyyyMMdd") + ".xml";
                    FileStream lFileStream = new FileStream(lInventoryFileName, FileMode.Create);
                    XmlSerializer lSerializer = new XmlSerializer(typeof(Inventory));
                    lSerializer.Serialize(lFileStream, gInventory);
                    MessageBox.Show(lInventoryFileName + " successfully written to " + Settings.DirectoryPath);

                    // Write the selected files to XML
                    string lSelectionFileName = Settings.DirectoryPath + "\\Final_CourierList_ZSelectedFiles " + DateTime.Now.ToString("yyyyMMdd") + ".xml";
                    lFileStream = new FileStream(lSelectionFileName, FileMode.Create);
                    lSerializer = new XmlSerializer(typeof(SelectedFiles));
                    lSerializer.Serialize(lFileStream, lSelectedFiles);
                    MessageBox.Show(lSelectionFileName + " successfully written to " + Settings.DirectoryPath);

                    FileStream lProcessedFileStream = new FileStream(gProcessedFileName, FileMode.Create);
                    gProcessedSerializer.Serialize(lProcessedFileStream, gProcessedFiles);
                    lProcessedFileStream.Flush();
                    lProcessedFileStream.Close();
                    MessageBox.Show(gProcessedFileName + " successfully written to " + Settings.DirectoryPath);
                }

                bool RegisterResults()
                {
                    foreach (DeliveryDoc.DeliveryRecordRow item in gDeliveryDoc.DeliveryRecord)
                    {
                        if (!SubscriptionData3.RegisterListDelivery(item.SubscriptionId, item.IssueId))
                        {
                            return false;
                        }
                    }
                    return true;
                }




                //**************************************************************************************************************************************************************

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FormatCourierList", "CurrentReceiverId = " + lCurrentReceiverId.ToString());
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

        private void BuildInventory(List<DeliveryItem> international, string v)
        {
            throw new NotImplementedException();
        }

        private void SerialiseList(List<DeliveryItem> international, string v)
        {
            throw new NotImplementedException();
        }

        private void FormatCollectionList(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            try
            {
                if (checkPayers.IsChecked == false & checkNonPayers.IsChecked == false)
                {
                    MessageBox.Show("This way you are not going to get any labels.");
                    return;
                }
                //OpenFileDialog lFileDialog = new OpenFileDialog();

                //lFileDialog.InitialDirectory = Settings.DirectoryPath;
                //lFileDialog.ShowDialog();
                //string lFileName = lFileDialog.FileName.ToString();
                //if (!File.Exists(lFileName))

                //{
                //    MessageBox.Show("You have not selected a valid source file ");
                //    return;
                //}

                //gDeliveryDoc.Clear();
                //gDeliveryDoc.DeliveryRecord.ReadXml(lFileName);

                OpenFileDialog lFileDialog = new OpenFileDialog();

                lFileDialog.InitialDirectory = Settings.DirectoryPath + "Deliveries";
                lFileDialog.Multiselect = true;
                lFileDialog.ShowDialog();

                if (lFileDialog.FileNames.Count() == 0)
                {
                    MessageBox.Show("You have not selected any files.");
                    return;
                }

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    if (!lFileName.Contains("Collect_"))
                    {
                        MessageBox.Show("I can accept only files of which the name starts with 'Collect_'");
                        return;
                    }
                }

                gDeliveryDoc.DeliveryRecord.Clear();

                foreach (string lFileName in lFileDialog.FileNames)
                {
                    //Append all the files into the DeliveryRecord table
                    gDeliveryDoc.DeliveryRecord.ReadXml(lFileName);
                }



                {
                    string lResult;

                    if ((lResult = ProductBiz.FilterOnPayment((bool)checkPayers.IsChecked, (bool)checkNonPayers.IsChecked, ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                //Sort by customerid

                gDeliveryDoc.DeliveryRecord.DefaultView.Sort = "ReceiverId";

                {
                    string lResult;

                    if ((lResult = ProductBiz.CopyToCollectionList(ref gDeliveryDoc)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                string OutputFile = Settings.DirectoryPath + "\\Final_CollectionList_" + lFileDialog.SafeFileName;

                gDeliveryDoc.CollectionList.WriteXml(OutputFile);

                MessageBox.Show(gDeliveryDoc.CollectionList.Count.ToString() + " Records written to " + OutputFile.ToString());

                OutputFile = OutputFile.Replace("xml", "xsd");
                gDeliveryDoc.CollectionList.WriteXmlSchema(OutputFile);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "FormatCollectionList", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                // Stop the thread

                this.Cursor = Cursors.Arrow;
            }

        }

        private void Click_SubscriptionTransactions(object sender, RoutedEventArgs e)
        {
            System.Data.DataRowView lRowView = (System.Data.DataRowView)gDeliveryRecordViewSource.View.CurrentItem;
            if (lRowView != null)
            {
                DeliveryDoc.DeliveryRecordRow lRecord = (DeliveryDoc.DeliveryRecordRow)lRowView.Row;
                SubscriptionPicker2 lSubscriptionPicker = new SubscriptionPicker2();
                lSubscriptionPicker.SelectById(lRecord.SubscriptionId);
                lSubscriptionPicker.ShowDialog();
            }
        }

        private void buttonValidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
                {
                    return;
                }

                this.Cursor = Cursors.Wait;

                if (gDeliveryDoc.DeliveryRecord.Count == 0)
                {
                    MessageBox.Show("There is nothing to validate.");
                    return;
                }

                gDeliveryDoc.DeliveryRecord.AcceptChanges();

                gBackgroundWorker.RunWorkerAsync(gDeliveryDoc);
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

                MessageBox.Show("Error in button ValidateProposal " + ex.Message.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ButtonSplitByDeliveryMethod(object sender, RoutedEventArgs e)
        {
            if (gBackgroundWorker.IsBusy || gBackgroundWorkerPost.IsBusy)
            {
                MessageBox.Show("You are too fast for me. I have not completed the post task yet. Request will be ignored. ");
                return;
            }

            if (gDeliveryDoc.DeliveryRecord.Where(p => p.ValidationStatus != "Deliverable").Count() > 0)
            {
                MessageBox.Show("The proposal is invalid. It cannot proceed to deliver it.");
                return;
            }

            try
            {
                int NumberOfEntries = 0;
                Cursor = Cursors.Wait;

                //Create a file for each delivery method

                foreach (int lKey in Enum.GetValues(typeof(DeliveryMethod)))
                {
                    // Save the proposal, e.g. in order to generate labels or collectionlists or deliverylists later on
                    string FileName = Settings.DirectoryPath + "\\Deliveries\\"
                       + Enum.GetName(typeof(DeliveryMethod), lKey)
                       + "_"
                       + gCurrentProduct
                        + "_"
                        + System.DateTime.Now.ToLongDateString()
                        + ".xml";

                    if (!ProductBiz.SplitByDeliveryMethod(ref gDeliveryDoc, FileName, lKey, out int CurrentNumberOfEntries))
                    {
                        return;
                    }

                    NumberOfEntries += CurrentNumberOfEntries;

                } // End of foreach loop

                int Mismatch = gDeliveryDoc.DeliveryRecord.Count - NumberOfEntries;

                if (Mismatch != 0)
                {
                    string Message = "Your XML files do not cover all the deliveries";
                    ExceptionData.WriteException(1, Message, this.ToString(), "GenerateXML", "Mismatched by " + Mismatch.ToString());
                    MessageBox.Show(Message);
                }
                else
                {
                    //buttonGenerateXML.IsEnabled = false;
                    MessageBox.Show("Done");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GenerateXML", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void buttonCreateXSD(object sender, RoutedEventArgs e)
        
            {
                try
                {

                    MessageBox.Show("Under construction");
                    // Create XSD for use by Excel

                    //XsdDataContractExporter exporter = new XsdDataContractExporter();
                    //ExportOptions lOptions = new ExportOptions();
                    //lOptions.KnownTypes 


                    //exporter.Options = lOptions;

                    //if (exporter.CanExport(typeof(List<DeliveryItem>)))
                    //{
                    //    exporter.Export(typeof(List<DeliveryItem>));

                    //    XmlSchemaSet mySchemas = exporter.Schemas;

                    //    XmlQualifiedName XmlNameValue = exporter.GetRootElementName(typeof(List<DeliveryItem>));
                    //    string lNameSpace = XmlNameValue.Namespace;

                    //    string lFileName = Settings.DirectoryPath + "\\Final_CourierList.xsd";
                    //    FileStream lFileStream = new FileStream(lFileName, FileMode.CreateNew);

                    //    foreach (XmlSchema schema in mySchemas.Schemas(lNameSpace))
                    //    {
                    //        schema.Write(lFileStream);
                    //    }

                    //    lFileStream.Flush();
                    //    lFileStream.Close();

                    //    MessageBox.Show("XSD created as: " + lFileName);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Schema cannot be exported");
                    //} 
                }
                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonCreateXSD", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show(ex.Message);
                }
            }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DeliveryDoc lDeliveryDoc = (DeliveryDoc)e.Argument;
            e.Result = ProductBiz.ValidateProposal(lDeliveryDoc, gBackgroundWorker);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressType lProgress = (ProgressType)e.Result;

            if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An error occurred in the background validation");
            }

            int Rejections = 0;
            //gProposalValid = false;
            Rejections = lProgress.Counter2;

            ////Special logging of short fall of stock
            //if (lProgress.StockShortCount > 0)
            //{
            //    MessageBox.Show("There was a stock short fall of " + lProgress.StockShortCount.ToString());
            //}

            if (Rejections == 0)
            {
                // Save the proposal in case there are problems with the post processing.

                gDeliveryDoc.DeliveryRecord.WriteXml(Settings.DirectoryPath + "\\Recovery\\OutputFromSuccessValidation" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");

                int lUnits = gDeliveryDoc.DeliveryRecord.Sum(p => p.UnitsPerIssue);

                MessageBox.Show("There where " + gDeliveryDoc.DeliveryRecord.Count.ToString() + " deliverable records for " + lUnits.ToString() + " units. You may proceed to post them!");
                Cursor = Cursors.Arrow;
                buttonValidate.IsEnabled = true;
            }
            else
            {
                //gProposalValid = false;
                //buttonPost.IsEnabled = false;
                MessageBox.Show("There are " + Rejections.ToString() + " invalid proposals.");
                return;
            }
        }
    }
    }

    

    

