using Subs.Data;
using System;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for DeliveryAddress.xaml
    /// </summary>
    public partial class DeliveryAddress2 : Window
    {
        #region Globals

        private DeliveryAddressDoc gDeliveryAddressDoc;
        private DeliveryAddressDoc.DeliveryAddressDataTable gDeliveryAddressTable;
        private readonly Subs.Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter gDeliveryAddressAdapter = new Subs.Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter();
        private CollectionViewSource gDeliveryAddressViewSource;

        private CollectionViewSource gCountryViewSource;
        private CollectionViewSource gProvinceViewSource;
        private CollectionViewSource gCityViewSource;
        private CollectionViewSource gSuburbViewSource;
        private CollectionViewSource gStreetViewSource;

        public struct TemplateRows
        {
            public DeliveryAddressDoc.StreetRow StreetRow;
            public DeliveryAddressDoc.SuburbRow SuburbRow;
            public DeliveryAddressDoc.CityRow CityRow;
            public DeliveryAddressDoc.ProvinceRow ProvinceRow;
            public DeliveryAddressDoc.CountryRow CountryRow;
        }

        private bool gPhysicalAddressChecked = false;

        private readonly CustomerData3 gCustomerData;

        #endregion

        #region Housekeeping

        private bool GeneralConstructor()
        {
            try
            {
                gDeliveryAddressDoc = ((DeliveryAddressDoc)(this.FindResource("deliveryAddressDoc")));
                gDeliveryAddressTable = gDeliveryAddressDoc.DeliveryAddress;
                gDeliveryAddressViewSource = (CollectionViewSource)(this.FindResource("deliveryAddressViewSource"));
                gDeliveryAddressAdapter.AttachConnection();

                gCountryViewSource = (CollectionViewSource)(this.FindResource("countryViewSource"));
                gCountryViewSource.Source = DeliveryAddressStatic.DeliveryAddresses.Country;

                gProvinceViewSource = (CollectionViewSource)(this.FindResource("provinceViewSource"));
                gCityViewSource = (CollectionViewSource)(this.FindResource("provinceCityViewSource"));
                gSuburbViewSource = (CollectionViewSource)(this.FindResource("provinceCitySuburbViewSource"));
                gStreetViewSource = (CollectionViewSource)(this.FindResource("provinceCitySuburbStreetViewSource"));

                // Prime with country = RSA

                foreach (System.Data.DataRowView lViewRow in gCountryViewSource.View)
                {
                    DeliveryAddressDoc.CountryRow lCountryRow = (DeliveryAddressDoc.CountryRow)lViewRow.Row;

                    if (lCountryRow.CountryId == 61)
                    {
                        gCountryViewSource.View.MoveCurrentTo(lViewRow);
                    }
                }

                gCountryViewSource.View.SortDescriptions.Add(new System.ComponentModel.SortDescription("CountryName", System.ComponentModel.ListSortDirection.Ascending));
                gProvinceViewSource.View.SortDescriptions.Add(new System.ComponentModel.SortDescription("ProvinceName", System.ComponentModel.ListSortDirection.Ascending));
                gCityViewSource.View.SortDescriptions.Add(new System.ComponentModel.SortDescription("CityName", System.ComponentModel.ListSortDirection.Ascending));
                gSuburbViewSource.View.SortDescriptions.Add(new System.ComponentModel.SortDescription("SuburbName", System.ComponentModel.ListSortDirection.Ascending));
                gStreetViewSource.View.SortDescriptions.Add(new System.ComponentModel.SortDescription("StreetName", System.ComponentModel.ListSortDirection.Ascending));

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GeneralConstructor", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return false;

            }
        }

        public DeliveryAddress2()
        {
            InitializeComponent();
            GeneralConstructor();
        }

        public DeliveryAddress2(int pCustomerId)
        {
            InitializeComponent();

            try
            {
                textCustomerId.Text = pCustomerId.ToString();
                gCustomerData = new CustomerData3(pCustomerId);

                textPhysicalAddressId.Text = gCustomerData.PhysicalAddressId.ToString();
                gPhysicalAddressChecked = true;

                if (!GeneralConstructor()) return;

                gDeliveryAddressTable.Clear();
                gDeliveryAddressAdapter.FillBy(gDeliveryAddressTable, pCustomerId, "ByCustomer");

                // Set the default country

                foreach (System.Data.DataRowView lViewRow in gCountryViewSource.View)
                {
                    DeliveryAddressDoc.CountryRow lCountryRow = (DeliveryAddressDoc.CountryRow)lViewRow.Row;

                    if (lCountryRow.CountryId == gCustomerData.CountryId)
                    {
                        gCountryViewSource.View.MoveCurrentTo(lViewRow);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "DeliveryAddress2(int pCustomerId)", "CustomerId = " + pCustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gPhysicalAddressChecked)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Please ensure that we have a physical address for the invoices");
                gTabControl.SelectedIndex = 0;
            }

        }

        #endregion

        #region Select tab

        private void SelectRow()
        {
            System.Data.DataRowView lView = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
            if (lView != null)
            {
                this.Hide();
            }
        }

        private void Click_ContextSelect(object sender, RoutedEventArgs e)
        {
            SelectRow();
        }

        private void deliveryAddressDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectRow();
        }

        private void EditDeliveryAddress()
        {
            int lProgress = 0;
            try
            {
                gTabControl.SelectedIndex = 1;
                TabEdit.Visibility = Visibility.Visible;
                System.Data.DataRowView lView = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;

                if (lView == null)
                {
                    MessageBox.Show("You have not selected an address to edit. Please select an existing one or add one first");
                    return;
                }

                DeliveryAddressDoc.DeliveryAddressRow lDeliveryAddressCurrentRow = (DeliveryAddressDoc.DeliveryAddressRow)lView.Row;

                lProgress = 1;

                if (!lDeliveryAddressCurrentRow.IsStreetIdNull())
                {
                    TemplateRows lTemplateRows = GetTemplateRows(lDeliveryAddressCurrentRow.StreetId);

                    foreach (System.Data.DataRowView lViewRow in gCountryViewSource.View)
                    {
                        if ((int)lViewRow["CountryId"] == lTemplateRows.CountryRow.CountryId)
                        {
                            gCountryViewSource.View.MoveCurrentTo(lViewRow);
                            countryDataGrid.ScrollIntoView(countryDataGrid.SelectedItem);
                            break;
                        }
                    }

                    lProgress = 2;

                    foreach (System.Data.DataRowView lViewRow in gProvinceViewSource.View)
                    {
                        if ((int)lViewRow["ProvinceId"] == lTemplateRows.ProvinceRow.ProvinceId)
                        {
                            gProvinceViewSource.View.MoveCurrentTo(lViewRow);
                            Province_DataGrid.ScrollIntoView(Province_DataGrid.SelectedItem);
                            break;
                        }
                    }

                    lProgress = 3;

                    foreach (System.Data.DataRowView lViewRow in gCityViewSource.View)
                    {
                        if ((int)lViewRow["CityId"] == lTemplateRows.CityRow.CityId)
                        {
                            gCityViewSource.View.MoveCurrentTo(lViewRow);
                            City_DataGrid.ScrollIntoView(City_DataGrid.SelectedItem);
                            break;
                        }
                    }

                    lProgress = 4;

                    foreach (System.Data.DataRowView lViewRow in gSuburbViewSource.View)
                    {
                        if ((int)lViewRow["SuburbId"] == lTemplateRows.SuburbRow.SuburbId)
                        {
                            gSuburbViewSource.View.MoveCurrentTo(lViewRow);
                            Suburb_DataGrid.ScrollIntoView(Suburb_DataGrid.SelectedItem);
                            break;
                        }
                    }

                    lProgress = 5;

                    foreach (System.Data.DataRowView lViewRow in gStreetViewSource.View)
                    {
                        if ((int)lViewRow["StreetId"] == lTemplateRows.StreetRow.StreetId)
                        {
                            gStreetViewSource.View.MoveCurrentTo(lViewRow);
                            Street_DataGrid.ScrollIntoView(Street_DataGrid.SelectedItem);
                            break;
                        }
                    }

                    gTabControl.SelectedIndex = 1;
                    TabEdit.Visibility = Visibility.Visible;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "EditDeliveryAddress", "Progress = " + lProgress.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void buttonExitEdit_Click(object sender, RoutedEventArgs e)
        {
            gTabControl.SelectedIndex = 0;
        }

        private void Click_ContextEdit(object sender, RoutedEventArgs e)
        {
            EditDeliveryAddress();
        }

        private void buttonRefreshTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("This might take 7 seconds");
                this.Cursor = Cursors.Wait;
                if (DeliveryAddressStatic.Refresh())
                {
                    MessageBox.Show("Refresh was successful");
                }
                else
                {
                    MessageBox.Show("There was a problem with refreshing the deliveryaddress template.");
                }
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Button_Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonDeliveryAddressRecordAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Here I do not add something to the template. I create a new DeliveryAddress and then
                // select from the template, and to that I add streetno etc. 

                DeliveryAddressDoc.DeliveryAddressRow lNewRow = gDeliveryAddressTable.NewDeliveryAddressRow();

                lNewRow.ModifiedBy = Environment.UserName;
                lNewRow.ModifiedOn = DateTime.Now.Date;
                lNewRow.CountryId = gCustomerData.CountryId;
                gDeliveryAddressTable.AddDeliveryAddressRow(lNewRow);

                // Select the new added row.
                gDeliveryAddressViewSource.View.MoveCurrentToLast();

                EditDeliveryAddress();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonDeliveryAddressRecordAdd_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);

                return;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

            System.Data.DataRowView lView = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
            if (lView != null)
            {
                textDelete.Text = lView["DeliveryAddressId"].ToString();
                if (lView != null)
                {
                    textDelete.Text = lView["DeliveryAddressId"].ToString();
                    if (textPhysicalAddressId.Text == textDelete.Text)
                    {
                        MessageBox.Show("You cannot delete the physical address id. You will first have to assign an alternate physical address id.");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("You have not selected an address to be deleted. Please try again.");
            }
        }

        private void buttonRetain_Click(object sender, RoutedEventArgs e)
        {
            System.Data.DataRowView lView = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
            if (lView != null)
            {
                textRetain.Text = lView["DeliveryAddressId"].ToString();
            }
            else
            {
                MessageBox.Show("You have not selected a address to retain. Please try again.");
            }
        }

        private void buttonConsolidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textDelete.Text == textRetain.Text)
                {
                    MessageBox.Show("The source and the target cannot be the same address. Please try again.");
                    return;
                }

                if (!int.TryParse(textDelete.Text, out int Source))
                {
                    MessageBox.Show("No proper address is selected as the address to be deleted.");
                    return;
                }

                if (!int.TryParse(textRetain.Text, out int Target))
                {
                    MessageBox.Show("No proper address is selected as the address to be retained.");
                    return;
                }

                gDeliveryAddressAdapter.Consolidate(Source, Target);

                gDeliveryAddressAdapter.FillBy(gDeliveryAddressTable, int.Parse(textCustomerId.Text), "ByCustomer");

                ExceptionData.WriteException(5, "DeliveryAddress " + Source.ToString() + " consolidated into " + Target.ToString(), this.ToString(), " buttonConsolidateClick ", "");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonConsolidate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }

        }

        #endregion

        #region Edit tab

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gDeliveryAddressTable.Count == 0 || gDeliveryAddressViewSource.View.CurrentItem == null)
                {
                    MessageBox.Show("No deliveryaddress has been selected.");
                    return;
                }

                System.Data.DataRowView myCurrentRow = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
                gCustomerData.PhysicalAddressId = (int)myCurrentRow["DeliveryAddressId"];

                string lResult;
                if ((lResult = gCustomerData.Update()) != "OK")
                {
                    MessageBox.Show(lResult);
                    return;
                }

                gCustomerData.Update();
                textPhysicalAddressId.Text = gCustomerData.PhysicalAddressId.ToString();
                gPhysicalAddressChecked = true;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonRegister_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);

                return;
            }
        }

        private TemplateRows GetTemplateRows(int pStreetId)
        {
            TemplateRows lTemplateRows = new TemplateRows();
            try
            {


                // Find the primary keys for the hierarchy

                lTemplateRows.StreetRow = DeliveryAddressStatic.DeliveryAddresses.Street.FindByStreetId(pStreetId);
                lTemplateRows.SuburbRow = (DeliveryAddressDoc.SuburbRow)lTemplateRows.StreetRow.GetParentRow("FK_Street_Suburb");
                lTemplateRows.CityRow = (DeliveryAddressDoc.CityRow)lTemplateRows.SuburbRow.GetParentRow("FK_Suburb_City");
                lTemplateRows.ProvinceRow = (DeliveryAddressDoc.ProvinceRow)lTemplateRows.CityRow.GetParentRow("FK_City_Province");
                lTemplateRows.CountryRow = (DeliveryAddressDoc.CountryRow)lTemplateRows.ProvinceRow.GetParentRow("FK_Province_Country");
                return lTemplateRows;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetTemplateRows", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return lTemplateRows;
            }

        }

        private void buttonUpdateDeliveryRecord_Click(object sender, RoutedEventArgs e)
        {
            string lProgress = "Start";
            try
            {
                DataRowView lViewRow = (DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
                if (lViewRow == null)
                {
                    MessageBox.Show("You did not select a deliveryAddress for me to edit");
                    return;
                }

                DeliveryAddressDoc.DeliveryAddressRow lRow = (DeliveryAddressDoc.DeliveryAddressRow)lViewRow.Row;
                DataRowView lView = (DataRowView)gStreetViewSource.View.CurrentItem;
                DeliveryAddressDoc.StreetRow lStreetRow = (DeliveryAddressDoc.StreetRow)lView.Row;

                if (lStreetRow == null)
                {
                    MessageBox.Show("You have not selected a street yet.");
                    return;
                }

                if (lRow.IsPostCodeNull())
                {
                    MessageBox.Show("Please provide me with a postal code.");
                    return;
                }


                if (lRow.PostCode.Length < 4)
                {
                    MessageBox.Show("The postcode needs to be at least 4 characters");
                    return;
                }

                TemplateRows lTemplate;

                lTemplate = GetTemplateRows(lStreetRow.StreetId);

                lProgress = "GetTemplateRows";

                lRow.CountryId = lTemplate.CountryRow.CountryId;
                lRow.Province = lTemplate.ProvinceRow.ProvinceName;
                lRow.City = lTemplate.CityRow.CityName;

                lProgress = "CityName";

                lRow.Suburb = lTemplate.SuburbRow.SuburbName;
                lRow.Street = lTemplate.StreetRow.StreetName;

                lProgress = "StreetName";

                if (lTemplate.StreetRow.IsStreetSuffixNull())
                {
                    lRow.StreetSuffix = null;
                }
                else
                { 
                    lRow.StreetSuffix = lTemplate.StreetRow.StreetSuffix;
                }


                if (lTemplate.StreetRow.IsStreetExtensionNull())
                {
                    lRow.StreetExtension = null;
                }
                else
                {
                    lRow.StreetExtension = lTemplate.StreetRow.StreetExtension;
                }
               

                lProgress = "StreetExtension";
               
                lRow.StreetId = lTemplate.StreetRow.StreetId;
                //lRow.Verified = true;

                lProgress = "StreetId";

                lRow.ModifiedBy = System.Environment.UserName;
                lRow.ModifiedOn = DateTime.Now;
                lRow.EndEdit();

                lProgress = "EndEdit";

                gDeliveryAddressAdapter.Update(lRow);
                gDeliveryAddressTable.AcceptChanges();

                // Also update the many to many mapping 

                if (gCustomerData != null)
                {

                    {
                        string lResult;

                        if ((lResult = DeliveryAddressData2.Link(lRow.DeliveryAddressId, gCustomerData.CustomerId)) != "OK")
                        {
                            MessageBox.Show(lResult);
                            return;
                        }
                    }
                }


                MessageBox.Show("DeliveryAddress successfully updated.");

                gTabControl.SelectedIndex = 0;
                TabEdit.Visibility = Visibility.Hidden;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonUpdateDeliveryRecord", "Progress = " + lProgress);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Properties

        public int? SelectedDeliveryAddressId
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gDeliveryAddressViewSource.View.CurrentItem;
                if (lView == null)
                {
                    return null;
                }
                else
                {
                    DeliveryAddressDoc.DeliveryAddressRow lRow = (DeliveryAddressDoc.DeliveryAddressRow)lView.Row;
                    return lRow.DeliveryAddressId;
                }
            }
        }

        #endregion

   
    }
}
