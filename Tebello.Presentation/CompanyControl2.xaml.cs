using Subs.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Subs.Presentation
{
    public partial class CompanyControl2 : UserControl
    {
        #region Globals
        private readonly CollectionViewSource gCompanyViewSource = new CollectionViewSource();
        private string gCompanyName;

        #endregion

        #region Constructor
        public CompanyControl2()
        {
            InitializeComponent();
           
            try
            {
                if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;

                gCompanyViewSource.Source = AdministrationData2.Company;

                CompanyCanvas.DataContext = gCompanyViewSource;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CompanyControl constructor", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }

        }
        #endregion

        #region Methods
        private void Select(int pCompanyId)
        {
            try
            {
                AdministrationDoc.CompanyRow lRow = AdministrationData2.Company.FindByCompanyId(pCompanyId);

                if (lRow == null)
                {
                    MessageBox.Show("No such company found.");
                    return;
                }

                DataView lDataView = AdministrationData2.Company.DefaultView;
                lDataView.Sort = "CompanyName";
                int lIndex = lDataView.Find(lRow.CompanyName);

                if (lIndex == -1)
                {
                    MessageBox.Show("No such company found.");
                    return;
                }

                gCompanyName = lRow.CompanyName; // Remember this in case you want to edit it later.

                gCompanyViewSource.View.MoveCurrentToPosition(lIndex);

                // Scroll to how far you got
                companyDataGrid.ScrollIntoView(companyDataGrid.SelectedItem);

                //CompanyId = pCompanyId;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Select", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Select " + ex.Message);
            }
        }
        #endregion

        #region Event handlers

        private void Search()
        {
            try
            {
                DataView lDataView = AdministrationData2.Company.DefaultView;
                lDataView.Sort = "CompanyName";

                string lSearchString = "CompanyName Like '" + textSearch.Text + "%'";

                AdministrationDoc.CompanyRow[] lCompanies = (AdministrationDoc.CompanyRow[])AdministrationData2.Company.Select(lSearchString);

                if (lCompanies.Length == 0)
                {
                    MessageBox.Show("No such company found.");
                    return;
                }

                int lIndex = lDataView.Find(lCompanies.Min(a => a.CompanyName));

                AdministrationDoc.CompanyRow lRow = (AdministrationDoc.CompanyRow)lDataView[lIndex].Row;

                Select(lRow.CompanyId);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Search", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {

            if (textSearch.Text.Length < 1)
            {
                MessageBox.Show("Please enter a search string.");
                return;
            }

            Search();
        }


        private void companyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gCompanyViewSource.View == null)
                {
                    return;
                }

                if (companyDataGrid.SelectedItem as DataRowView == null)
                {
                    // This is related to a new row.
                    return;
                }

                DataRowView lDataRowView = (DataRowView)companyDataGrid.SelectedItem;
                if (lDataRowView == null)
                {
                    return;
                }

                AdministrationDoc.CompanyRow lDataRow = (AdministrationDoc.CompanyRow)lDataRowView.Row;

                if (lDataRow.CompanyId == -1)
                {
                    //Database not updated yet
                    return;
                }

                // Explicitely change the value of CompanyId

                CompanyId = lDataRow.CompanyId;
            }


            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "companyDataGrid_SelectionChanged", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("companyDataGrid_SelectionChanged " + ex.Message);
            }
        }

        private void companyDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
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

        #region Properties - public

        public static readonly DependencyProperty CompanyIdProperty;

        static CompanyControl2()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(CompanyChanged));
            CompanyControl2.CompanyIdProperty = DependencyProperty.Register("CompanyId", typeof(int), typeof(CompanyControl2), lMetaData);
        }

        private static void CompanyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == -1)
            {
                return;
            }

            CompanyControl2 lControl = (CompanyControl2)o;
            lControl.Select((int)e.NewValue);
        }

        public int CompanyId
        {
            get
            {
                return (int)GetValue(CompanyControl2.CompanyIdProperty);
            }

            set
            {
                SetValue(CompanyControl2.CompanyIdProperty, value);
            }
        }
        #endregion

        private void Click_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                AdministrationCompany lWindow = new AdministrationCompany();
                lWindow.PublicSearchString = gCompanyName;
                lWindow.SearchMethod("CompanyName");
                //lWindow.SelectTab(AdministrationCustomer.Tabs.Company);

                lWindow.ShowDialog();
                AdministrationData2.RefreshCompany();

                // Reset the selection
                textSearch.Text = gCompanyName;
                Search();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Edit", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        private void Click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                AdministrationCompany lWindow = new AdministrationCompany();
                lWindow.AddMethod();
                //lWindow.SelectTab(AdministrationCustomer.Tabs.Company);
                lWindow.ShowDialog();
                AdministrationData2.RefreshCompany();

                // Reset the selection
                textSearch.Text = lWindow.PublicSearchString;
                Search();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Add", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }
    }
}
