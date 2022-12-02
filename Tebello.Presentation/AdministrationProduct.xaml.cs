using Subs.Data;
using Subs.Business;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Tebello.Presentation;
using Subs.Presentation;

namespace Tebello.Presentation
{
       public partial class AdministrationProduct : Window
    {
        #region Globals
        private readonly CollectionViewSource gProductViewSource;
        private readonly CollectionViewSource gIssueViewSource;
        private readonly CollectionViewSource gBaseRateViewSource;
        private readonly Subs.Data.ProductDoc gProductDoc;

        private ProductDoc.Product2Row gCurrentProductRow;
        private int gCurrentProductId = 0;
        private decimal gProposedBaseRate = 0M;

        private enum Tabs
        {
            Product = 0,
            Issue = 1,
            Baserate = 2
        }

        #endregion

        #region Construction

        private readonly Subs.Data.ProductDocTableAdapters.Product2TableAdapter gProductAdapter = new Subs.Data.ProductDocTableAdapters.Product2TableAdapter();
        private readonly Subs.Data.ProductDocTableAdapters.IssueTableAdapter gIssueAdapter = new Subs.Data.ProductDocTableAdapters.IssueTableAdapter();
        private readonly Subs.Data.ProductDocTableAdapters.BaseRateTableAdapter gBaseRateAdapter = new Subs.Data.ProductDocTableAdapters.BaseRateTableAdapter();
        public AdministrationProduct()
        {
            InitializeComponent();

            try
            {
                gProductDoc = ((Subs.Data.ProductDoc)(this.FindResource("productDoc")));
                gProductViewSource = (CollectionViewSource)this.Resources["product2ViewSource"];
                gIssueViewSource = (CollectionViewSource)this.Resources["issueViewSource"];
                gBaseRateViewSource = (CollectionViewSource)this.Resources["baseRateViewSource"];


                gProductAdapter.AttachConnection();
                gIssueAdapter.AttachConnection();
                gBaseRateAdapter.AttachConnection();

                InitialiseDataSet();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AdministrationProduct", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        private void InitialiseDataSet()
        {
            try
            {
                gProductDoc.BaseRate.Clear();
                gProductDoc.Issue.Clear();
                gProductDoc.Product2.Clear();

                gProductAdapter.FillById(gProductDoc.Product2, 0, "All");
                gProductDoc.Product2.AcceptChanges();
                gIssueAdapter.FillById(gProductDoc.Issue, 0, "All");
                gProductDoc.Issue.AcceptChanges();
                gBaseRateAdapter.Fill(gProductDoc.BaseRate);
                gProductDoc.BaseRate.AcceptChanges();

                foreach (DataRowView lRowView in gProductViewSource.View)
                {
                    ProductDoc.Product2Row lRow = (ProductDoc.Product2Row)lRowView.Row;

                    if (ProductDataStatic.CurrentProduct(lRow.ProductId))
                    {
                        if (lRow.Status != "Active")
                        {
                            lRow.Status = "Active";
                        }
                            
                    }
                    else
                    {
                        if (lRow.Status != "Dormant")
                        {
                            lRow.Status = "Dormant";
                        }
                    }
                }

                gProductViewSource.View.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Ascending));
                gProductViewSource.View.SortDescriptions.Add(new SortDescription("ProductName", ListSortDirection.Ascending));

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "InitialiseDataSet", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            product2DataGrid.Height = this.ActualHeight - 440;
            ProductImage.Height = this.ActualHeight - 440;
            BaseRateDataGrid.Height = this.ActualHeight - 100;
        }


        #endregion

        #region Event handlers Product

        private void ProductTab_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Windows.MessageBox.Show("I have lost keyboard focus");
        }

        private void Product2DataGrid_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
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

        private void product2DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gProductViewSource.View.CurrentItem == null)
                {
                    //This is just initialisation.
                    return;
                }
                DataRowView lDataViewRow = (DataRowView)gProductViewSource.View.CurrentItem;
                gCurrentProductRow = (ProductDoc.Product2Row)lDataViewRow.Row;
                gCurrentProductId = gCurrentProductRow.ProductId;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "product2DataGrid_SelectionChanged", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        private void buttonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabIssue.Visibility = Visibility.Visible;
                TabBaseRate.Visibility = Visibility.Visible; // Just to ensure that product is saved before updating dependent tables. 

                ProductDoc.Product2Row lProductRow = gProductDoc.Product2.NewProduct2Row();
                lProductRow.ProductName = "ZZNew Product";
                lProductRow.Heading = "New heading";
                lProductRow.ProductDescription = "New description";
                lProductRow.Picture = new Byte[] { 0x01 };
                lProductRow.ModifiedBy = System.Environment.UserName;
                lProductRow.ModifiedOn = DateTime.Now;
                gProductDoc.Product2.AddProduct2Row(lProductRow);

                foreach (DataRowView lView in gProductViewSource.View)
                {
                    if ((string)lView["ProductName"] == "ZZNew Product")
                    {
                        gProductViewSource.View.MoveCurrentTo(lView);
                        product2DataGrid.ScrollIntoView(lView);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAddProduct_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                System.Windows.MessageBox.Show(ex.Message);
                return;
            }

        }
        private void buttonAddPicture_Click(object sender, RoutedEventArgs e)
        {
            string lFileName = "c:\\Subs";
            try
            {
                DataRowView lDataRowView = (DataRowView)gProductViewSource.View.CurrentItem;

                ProductDoc.Product2Row lCurrentRow = (ProductDoc.Product2Row)lDataRowView.Row;

                if (lCurrentRow == null)
                {
                    System.Windows.MessageBox.Show("You have not selected a product to add the picture to.");
                    return;
                }

                System.Windows.Forms.OpenFileDialog lDialog = new System.Windows.Forms.OpenFileDialog();
                lDialog.InitialDirectory = "c:\\Subs";
                lDialog.ShowDialog();
                lFileName = lDialog.FileName;

                if (lFileName == "")
                {
                    System.Windows.MessageBox.Show("You have not selected a picture.");
                    return;
                }

                System.Drawing.Image img = System.Drawing.Image.FromFile(lFileName);
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    lCurrentRow.Picture = ms.ToArray();
                }

                System.Windows.MessageBox.Show("Picture added");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAddPicture_Click", "Source= " + lFileName.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                System.Windows.MessageBox.Show(ex.Message);
            }

        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                if (e.Action == ValidationErrorEventAction.Added)
                {

                    System.Windows.MessageBox.Show(e.Error.ErrorContent.ToString());
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

                System.Windows.MessageBox.Show("Error in ValidationError " + ex.Message);
            }
        }

        private void DeliveryOptionsLostFocus(object sender, RoutedEventArgs e)
        {
            DataRowView lRowView = (DataRowView)gProductViewSource.View.CurrentItem;
            ProductDoc.Product2Row lRow = (ProductDoc.Product2Row)lRowView.Row;
            DeliveryOptionsToDictionary lConverter = new DeliveryOptionsToDictionary();

            DefaultDeliveryOption.ItemsSource = (System.Collections.IEnumerable)lConverter.Convert(lRow.DeliveryOptions, typeof(int), new object(), System.Globalization.CultureInfo.CurrentCulture);
        }


        private void buttonSubmitProduct_Click(object sender, RoutedEventArgs e)
        {
            {
                string lResult;

                if ((lResult = UpdateProduct()) != "OK")
                {
                    MessageBox.Show(lResult);
                    return;
                }
            }
            MessageBox.Show("Update of product data successful.");
        }

        private string UpdateProduct()
        {
            try
            {
                //bool lFound = false;
                foreach (ProductDoc.Product2Row lRow in gProductDoc.Product2)
                {
                    if (lRow.ExpirationDuration < 90)
                    {
                        return "ExpirationDuration cannot be less than 90 days.";
                    }

                    if (lRow.RowState == DataRowState.Added | lRow.RowState == DataRowState.Modified)
                    {
                        lRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                        lRow.ModifiedOn = DateTime.Now;
                    }
                }

                gProductAdapter.Update(gProductDoc.Product2);
               
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "UpdateProduct", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed on UpdateProduct " + ex.Message;

            }
        }

       private void product2DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            try
            {
                gProductViewSource.View.SortDescriptions.Clear();
                gProductViewSource.View.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Ascending));
                gProductViewSource.View.SortDescriptions.Add(new SortDescription(e.Column.SortMemberPath, ListSortDirection.Ascending));
                e.Handled = true;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "product2DataGrid_Sorting", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

       private void Exit_Click(object sender, RoutedEventArgs e)
       {
           this.Close();
       }

        #endregion

        #region Event handlers Issue

        private void buttonAddIssue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First submit previous inserts or modifications. 

                if (!SubmitIssue()) return;


                //  Find the latest start date

                DateTime lStartDate;
                ProductDoc.IssueRow lLastRow;
                int lYear = 0;

                gIssueViewSource.View.MoveCurrentToLast();

                if (gIssueViewSource.View.CurrentItem == null)
                {
                    // I am assuming there are not issues yet.
                    lStartDate = DateTime.Now;
                    lYear = DateTime.Now.Year + 1;

                }
                else
                {
                    DataRowView lDataRowView = (DataRowView)gIssueViewSource.View.CurrentItem;
                    lLastRow = (ProductDoc.IssueRow)lDataRowView.Row;
                    lStartDate = lLastRow.EndDate.AddSeconds(1);
                    lYear = lLastRow.Year + 1; // I do this to ensure that the last issue appears at the bottom.
                }

                // Create a new row
                ProductDoc.IssueRow lIssueRow = gProductDoc.Issue.NewIssueRow();
                lIssueRow.IssueDescription = "New Issue";
                lIssueRow.Year = lYear;
                lIssueRow.No = 1;
                lIssueRow.ProductId = gCurrentProductId;
                lIssueRow.StartDate = lStartDate;
                lIssueRow.EndDate = lIssueRow.StartDate.AddMonths(1);
                lIssueRow.SellOption = 3;
                lIssueRow.StockProduced = 0;
                lIssueRow.ModifiedBy = System.Environment.UserName;
                lIssueRow.ModifiedOn = DateTime.Now;
                gProductDoc.Issue.AddIssueRow(lIssueRow);

                gIssueViewSource.View.MoveCurrentToLast();
                DataRowView lDataRowView2 = (DataRowView)gIssueViewSource.View.CurrentItem;
                issueDataGrid.ScrollIntoView(lDataRowView2);

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAddIssue_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                System.Windows.MessageBox.Show(ex.Message);
                return;
            }

        }

        private bool SubmitIssue()
        {
            try
            {
                // At this point I want to access the rows of only one product.

                int Sequence = 1;

                ProductDoc.IssueRow lTypedRow;

                DateTime lPreviousEndDate = new DateTime(1800, 1, 1);


                // Update the auditing info and the hours and minutes
                foreach (ProductDoc.IssueRow lRow in gProductDoc.Issue)
                {
                    if (lRow.RowState == DataRowState.Added | lRow.RowState == DataRowState.Modified)
                    {
                        // Enddate should include the hours of the last day.
                        if (lRow.EndDate.Hour == 0)
                        {
                            lRow.EndDate = lRow.EndDate.AddHours(23);
                            lRow.EndDate = lRow.EndDate.AddMinutes(59);
                            lRow.EndDate = lRow.EndDate.AddSeconds(59);
                        }


                        // Include auditing info.
                        lRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                        lRow.ModifiedOn = DateTime.Now;
                    }
                }

                // Do some health checks and reset the sequence numbers. 


                foreach (DataRowView myRow in gIssueViewSource.View.SourceCollection)
                {
                    lTypedRow = (ProductDoc.IssueRow)myRow.Row;

                    if (lPreviousEndDate == DateTime.Parse("1800/01/01"))
                    {
                        lPreviousEndDate = lTypedRow.EndDate;
                    }
                    else
                    {
                        if (lPreviousEndDate > lTypedRow.StartDate)
                        {
                            System.Windows.MessageBox.Show("I cannot save " + lTypedRow.IssueDescription + " because it overlaps with the previous issue");
                            return false;
                        }
                    }

                    if (lTypedRow.EndDate < lTypedRow.StartDate)
                    {
                        System.Windows.MessageBox.Show("I cannot save " + lTypedRow.IssueDescription + " because the End date is before the Start date.");
                        return false;
                    }

                    lPreviousEndDate = lTypedRow.EndDate;


                    if (lTypedRow.RowState == DataRowState.Deleted)
                    {
                        //Do not give deleted rows sequence numbers. 
                        continue;
                    }


                    if ((int)lTypedRow.Sequence != Sequence)
                    {
                        lTypedRow.Sequence = Sequence;
                    }

                    Sequence++;

                }

                gIssueAdapter.Update(gProductDoc.Issue);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubmitIssue", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);


                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }

        }

        private void buttonSubmitIssue_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitIssue())
            {

                ProductDataStatic.Refresh();

                System.Windows.MessageBox.Show("Done");
            }
        }

        private void buttonExitIssue_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        #endregion

        #region Event handlers Base rate

        private void buttonAddBaseRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductDoc.BaseRateRow lBaseRateRow = gProductDoc.BaseRate.NewBaseRateRow();

                lBaseRateRow.ProductID = gCurrentProductId;
                lBaseRateRow.DateFrom = DateTime.Now;
                lBaseRateRow.Value = gProposedBaseRate;
                lBaseRateRow.ModifiedBy = System.Environment.UserName;
                lBaseRateRow.ModifiedOn = DateTime.Now;
                gProductDoc.BaseRate.AddBaseRateRow(lBaseRateRow);

                gBaseRateViewSource.View.MoveCurrentToLast();
                DataRowView lDataRowView = (DataRowView)gBaseRateViewSource.View.CurrentItem;
                BaseRateDataGrid.ScrollIntoView(lDataRowView);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAddBaseRate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                throw ex;
            }

        }
        private void buttonSubmitBaseRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (ProductDoc.BaseRateRow lRow in gProductDoc.BaseRate)
                {
                    if (lRow.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    if (lRow.DateFrom < DateTime.Now.AddYears(-4))
                    {
                        //Remove old stuff
                        lRow.Delete();
                        continue;
                    }
                    if (lRow.RowState == DataRowState.Added | lRow.RowState == DataRowState.Modified)
                    {
                        lRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                        lRow.ModifiedOn = DateTime.Now;
                    }
                }
                gBaseRateAdapter.Update(gProductDoc.BaseRate);
                MessageBox.Show("Update of base rate data successful.");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonSubmitBaseRate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                throw ex;
            }
        }

        private void buttonExitBaseRate_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        #endregion

        private void buttonCalculateBaseRate_Click(object sender, RoutedEventArgs e)
        {
            decimal lUnitPrice = 0M;
            try
            {
                ElicitDecimal lElicitUnitPrice = new ElicitDecimal("What do you want the final price for the subscription to be for a South African with the default delivery method?");
                lElicitUnitPrice.ShowDialog();
                if (lElicitUnitPrice.Answer == 0M)
                {
                    System.Windows.MessageBox.Show("You have not supplied me with a valid Final Price! Please try again.");
                    return;
                }

                lUnitPrice = lElicitUnitPrice.Answer/ gCurrentProductRow.DefaultNumberOfIssues;

                //ElicitInteger lElicitUnitsPerIssue = new ElicitInteger("How many Issues per subscriptionIssue");
                //lElicitUnitsPerIssue.ShowDialog();
                //if (lElicitUnitPrice.Answer == 0M)
                //{
                //    System.Windows.MessageBox.Show("You have not supplied me with a valid number of units per issue! Please try again.");
                //    return;
                //}

                int lUnitsPerIssue = 1;

                decimal lDeliveryCost = SubscriptionBiz.GetDeliveryCost(61, gCurrentProductRow.Weight * lUnitsPerIssue, (DeliveryMethod)Enum.ToObject(typeof(DeliveryMethod), gCurrentProductRow.DefaultDeliveryOption))
                                                             / lUnitsPerIssue;

                decimal lVatFraction = ProductDataStatic.GetVatRate() / 100M;

                gProposedBaseRate = (lUnitPrice - lDeliveryCost - (lVatFraction * lDeliveryCost)) / (1.0M + lVatFraction);

                TextProposedBaseRate.Text = gProposedBaseRate.ToString("####0.00####");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonCalculateBaseRate_Click", "UnitPrice = " + lUnitPrice.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                throw ex;
            }
        }
    }
}
