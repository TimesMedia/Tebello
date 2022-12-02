using Subs.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Subs.Presentation
{
    public partial class AdministrationCompany : Window
    {
        #region Globals

        private readonly Subs.Data.AdministrationDoc administrationDoc;
        private readonly Subs.Data.AdministrationDocTableAdapters.CompanyTableAdapter gCompanyAdapter = new Subs.Data.AdministrationDocTableAdapters.CompanyTableAdapter();
        private readonly CollectionViewSource gCompanyViewSource;

        private CustomerData3 gCurrentCustomer;


        private readonly CollectionViewSource gCustomersViewSource = new CollectionViewSource();

        private int gCompanySource = 0;
        private int gCompanyTarget = 0;
  
        #endregion

        #region Constructor and forms management

        public AdministrationCompany()
        {
            InitializeComponent();

            try
            {
                administrationDoc = ((Subs.Data.AdministrationDoc)(this.FindResource("administrationDoc")));
                gCompanyViewSource = (CollectionViewSource)this.Resources["companyViewSource"];
                gCompanyAdapter.AttachConnection();
                gCompanyAdapter.Fill(administrationDoc.Company);
 
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AdministrationCustomer", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
          
        }
      
        #endregion

        #region Search 

        public void SearchMethod(string pField)
        {

            try
            {
                if (SearchString.Text.Length < 1)
                {
                    MessageBox.Show("Please enter a search string.");
                    return;
                }

                //Search  

                DataView lDataView = administrationDoc.Company.DefaultView;
                lDataView.Sort = pField;

                if (SearchString.Text == "[None]")
                {
                    MessageBox.Show("What you are looking for is possibly: select * from Customer where companyId = 1");
                    return;
                }

                string lSearchString = pField + " Like '" + SearchString.Text + "%'";

                AdministrationDoc.CompanyRow[] lCompanies = (AdministrationDoc.CompanyRow[])administrationDoc.Company.Select(lSearchString);

                if (lCompanies.Length == 0)
                {
                    MessageBox.Show("No such company found.");
                    return;
                }

                int lIndex = 0;

                switch (pField)
                {
                    case "CompanyName":
                        lIndex = lDataView.Find(lCompanies.Min(a => a.CompanyName));
                        break;
                    case "VatRegistration":
                        lIndex = lDataView.Find(lCompanies.Min(a => a.VatRegistration));
                        break;
                    case "VendorNumber":
                        lIndex = lDataView.Find(lCompanies.Min(a => a.VendorNumber));
                        break;
                    case "CompanyRegistrationNumber":
                        lIndex = lDataView.Find(lCompanies.Min(a => a.CompanyRegistrationNumber));
                        break;

                    case "Add":
                        lIndex = lDataView.Count;
                        break;

                    default:
                        MessageBox.Show("I do not cater for " + pField);
                        break;
                }


                if (lIndex == -1)
                {
                    MessageBox.Show("No such company found.");
                    return;
                }

                gCompanyViewSource.View.MoveCurrentToPosition(lIndex);

                // Scroll to how far you got
                companyDataGrid.ScrollIntoView(companyDataGrid.SelectedItem);

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SearchMethod", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }


        private void SelectCompany()
        {
            if (gCustomersViewSource.View == null)
            {
                MessageBox.Show("There are no unverified addresses.");
                return;
            }

            gCurrentCustomer = (CustomerData3)gCustomersViewSource.View.CurrentItem;
            SearchString.Text = gCurrentCustomer.CompanyNameUnverified;
            TabControl1.SelectedIndex = 1;
        }

        private void Click_Select(object sender, RoutedEventArgs e)
        {
            SelectCompany();
        }

        public string PublicSearchString
        {
            get
            {
                return SearchString.Text;
            }

            set
            {
                SearchString.Text = value;
            }
        }

        private void ButtonSearchOnCompany_Click(object sender, RoutedEventArgs e)
        {
            SearchMethod("CompanyName");
        }

        private void ButtonSearchOnVatregistration_Click(object sender, RoutedEventArgs e)
        {
            SearchMethod("VatRegistration");
        }

        private void ButtonSearchOnVendorNumber_Click(object sender, RoutedEventArgs e)
        {
            SearchMethod("VendorNumber");
        }

        private void ButtonSearchOnCompanyRegistrationNumber_Click(object sender, RoutedEventArgs e)
        {
            SearchMethod("CompanyRegistrationNumber");

        }



        #endregion


        #region Update company
        private void ButtonRemoveCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int lRemoved = (int)gCompanyAdapter.RemoveUnlinked();
                AdministrationData2.RefreshCompany();
                MessageBox.Show(lRemoved.ToString() + " companies removed.");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonRemoveCompany_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        public void AddMethod()
        {
            // Scroll to the bottom

            gCompanyViewSource.View.MoveCurrentToLast();
            companyDataGrid.ScrollIntoView(companyDataGrid.SelectedItem);
        }
        private void ButtonUpdateCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //if ( LogicalTreeHelper.GetChildren(companyDataGrid).OfType<DependencyObject>().All(a => Validation.GetHasError(a)))
                //{
                //    MessageBox.Show("Some rows in the Company Datagrid are in error");
                //    return;
                //}

                foreach (AdministrationDoc.CompanyRow lRow in administrationDoc.Company)
                {
                    if (lRow.RowState == DataRowState.Added)
                    {
                        PublicSearchString = lRow.CompanyName;
                        break;  // Break after the first occurence 
                    }
                }


                gCompanyAdapter.Update(administrationDoc.Company);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateCompany_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void CompanyDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show("Error event triggered");
            ValidationError lError = e.Error;
            MessageBox.Show(lError.Exception.Message);


            //ReadOnlyObservableCollection<ValidationError> lErrors = Validation.GetErrors(companyDataGrid);

            //if (lErrors.Count > 0)
            //{
            //    MessageBox.Show("I cannot do an update while a row is in error. Please delete it or fix it before continuing.");
            //}
        }

        private void ButtonAddCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AdministrationDoc.CompanyRow lNewRow = administrationDoc.Company.NewCompanyRow();
                lNewRow.CompanyName = "ZZZZ";
                lNewRow.ModifiedBy = Environment.UserName;
                lNewRow.ModifiedOn = DateTime.Now.Date;

                administrationDoc.Company.AddCompanyRow(lNewRow);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonAddCompany", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Consolidate

        private void ButtonSelectSource_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView lView = (DataRowView)gCompanyViewSource.View.CurrentItem;
                if (lView == null)
                {
                    MessageBox.Show("No address has been selected. Try again.");
                    return;
                }
                AdministrationDoc.CompanyRow lRow = (AdministrationDoc.CompanyRow)lView.Row;
                gCompanySource = lRow.CompanyId;
                textSource.Text = lRow.CompanyName;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonSelectSource_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonSelectTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView lView = (DataRowView)gCompanyViewSource.View.CurrentItem;
                if (lView == null)
                {
                    MessageBox.Show("No address has been selected. Try again.");
                    return;
                }
                AdministrationDoc.CompanyRow lRow = (AdministrationDoc.CompanyRow)lView.Row;
                gCompanyTarget = lRow.CompanyId;
                textTarget.Text = lRow.CompanyName;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonSelectTarget_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonConsolidateCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBoxResult.No == MessageBox.Show("This cannot be undone. Do you want to continue?",
                    "Warning", MessageBoxButton.YesNo))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(textSource.Text) || string.IsNullOrWhiteSpace(textTarget.Text))
                {
                    MessageBox.Show("You have not given me a source and or a target company.");
                }

                gCompanyAdapter.Consolidate(gCompanySource, gCompanyTarget);
                string Message = "Company " + textSource.Text + " consolidated into company " + textTarget.Text;

                // Record the event in the Exception table
                ExceptionData.WriteException(5, Message, this.ToString(), "Click_ButtonConsolidateCompany", "");
                MessageBox.Show(Message);

                textSource.Text = "";
                textTarget.Text = "";
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonConsolidateCompany_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void Binding_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            MessageBox.Show("TargetUpdated fired");
        }

        private void CompanyDataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            MessageBox.Show("TSourceUpdated fired");
        }


        #endregion

        #region Update customer

        private void ButtonUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                DataRowView lRowView = (DataRowView)gCompanyViewSource.View.CurrentItem;
                AdministrationDoc.CompanyRow lRow = (AdministrationDoc.CompanyRow)lRowView.Row;
                gCurrentCustomer.CompanyId = lRow.CompanyId;
                gCurrentCustomer.CompanyNameUnverified = "";

                string lResult;
                if ((lResult = gCurrentCustomer.Update()) != "OK")
                {
                    MessageBox.Show(lResult);
                }
           
                MessageBox.Show("Customer " + gCurrentCustomer.CustomerId.ToString() + " :  CompanyName is verified.");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateCustomer", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        #endregion
     

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                gCustomersViewSource.Source = CustomerData3.CustomersWithUnverifiedCompany();
                datagridCustomers.DataContext = gCustomersViewSource.View;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "TabItem_GotFocus", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }
     
    }

}
