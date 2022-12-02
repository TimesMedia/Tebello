using Subs.Data;
using System;
using System.Data;
using System.Windows;
using System.Windows.Data;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for PostCodeQuery.xaml
    /// </summary>
    public partial class AdministrationPostCode : Window
    {
        #region Globals -  private
        private readonly Subs.Data.PostCodeDocTableAdapters.PostCode_LinearTableAdapter gLinearAdapter = new Subs.Data.PostCodeDocTableAdapters.PostCode_LinearTableAdapter();
        private readonly Subs.Data.PostCodeDocTableAdapters.AddressLine3TableAdapter gAddressLine3Adapter = new Subs.Data.PostCodeDocTableAdapters.AddressLine3TableAdapter();
        private readonly Subs.Data.PostCodeDocTableAdapters.AddressLine4TableAdapter gAddressLine4Adapter = new Subs.Data.PostCodeDocTableAdapters.AddressLine4TableAdapter();
        private Subs.Data.PostCodeDoc gPostCodeDoc;
        private CollectionViewSource gLinearViewSource;
        #endregion

        #region Constructor

        public AdministrationPostCode()
        {
            InitializeComponent();
            gLinearAdapter.AttachConnection();
            gAddressLine3Adapter.AttachConnection();
            gAddressLine4Adapter.AttachConnection();

            gLinearViewSource = (CollectionViewSource)this.Resources["postCode_LinearViewSource"];
        }

        #endregion

        #region Event handlers - private


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                gPostCodeDoc = ((Subs.Data.PostCodeDoc)(this.FindResource("postCodeDoc")));
                gLinearViewSource = (CollectionViewSource)this.Resources["postCode_LinearViewSource"];
                gLinearAdapter.FillByType(gPostCodeDoc.PostCode_Linear, "All");

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Window_Loaded", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }

        }


        private void Click_buttonSearchSuburb(object sender, RoutedEventArgs e)
        {
            try
            {

                if (gPostCodeDoc.PostCode_Linear.Count == 0)
                {
                    throw new Exception("No data loaded");
                }

                if (textSearch.Text.Length < 3)
                {
                    MessageBox.Show("Please supply me with a search string.");
                    return;
                }


                //Search  

                DataView lCodeDataView = gPostCodeDoc.PostCode_Linear.DefaultView;
                lCodeDataView.Sort = "AddressLine3";

                int lIndex = lCodeDataView.Find(textSearch.Text);


                if (lIndex == -1)
                {
                    MessageBox.Show("Search string not found.");
                    return;
                }

                gLinearViewSource.View.MoveCurrentToPosition(lIndex);

                // Scroll to how far you got
                postCode_LinearDataGrid.ScrollIntoView(postCode_LinearDataGrid.SelectedItem);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_buttonSearchSuburb", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_buttonSearchCity(object sender, RoutedEventArgs e)
        {
            try
            {

                if (gPostCodeDoc.PostCode_Linear.Count == 0)
                {
                    throw new Exception("No data loaded");
                }

                if (textSearch.Text.Length < 4)
                {
                    MessageBox.Show("Please supply me with a search string.");
                    return;
                }


                //Search  

                DataView lCodeDataView = gPostCodeDoc.PostCode_Linear.DefaultView;
                lCodeDataView.Sort = "AddressLine4";

                int lIndex = lCodeDataView.Find(textSearch.Text);


                if (lIndex == -1)
                {
                    MessageBox.Show("Search string not found.");
                    return;
                }

                gLinearViewSource.View.MoveCurrentToPosition(lIndex);

                // Scroll to how far you got
                postCode_LinearDataGrid.ScrollIntoView(postCode_LinearDataGrid.SelectedItem);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_buttonSearchSuburb", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_buttonSearchCode(object sender, RoutedEventArgs e)
        {
            try
            {

                if (gPostCodeDoc.PostCode_Linear.Count == 0)
                {
                    throw new Exception("No data loaded");
                }


                if (textSearch.Text.Length != 4 || !int.TryParse(textSearch.Text, out int TestParse))
                {

                    MessageBox.Show("A PostCode should be 4 digits.");
                    return;
                }


                //Search  

                DataView lCodeDataView = gPostCodeDoc.PostCode_Linear.DefaultView;
                lCodeDataView.Sort = "Code";

                int lIndex = lCodeDataView.Find(textSearch.Text);


                if (lIndex == -1)
                {
                    MessageBox.Show("Search string not found.");
                    return;
                }

                gLinearViewSource.View.MoveCurrentToPosition(lIndex);

                // Scroll to how far you got
                postCode_LinearDataGrid.ScrollIntoView(postCode_LinearDataGrid.SelectedItem);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_buttonSearchCode", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }


        private void Click_buttonSource(object sender, RoutedEventArgs e)
        {
            DataRowView lView = (DataRowView)gLinearViewSource.View.CurrentItem;
            if (lView == null)
            {
                MessageBox.Show("No address has been selected. Try again.");
                return;
            }
            PostCodeDoc.PostCode_LinearRow lRow = (PostCodeDoc.PostCode_LinearRow)lView.Row;
            textSource.Text = lRow.AddressLine3Id.ToString();
        }

        private void Click_buttonTarget(object sender, RoutedEventArgs e)
        {
            DataRowView lView = (DataRowView)gLinearViewSource.View.CurrentItem;
            if (lView == null)
            {
                MessageBox.Show("No address has been selected. Try again.");
                return;
            }
            PostCodeDoc.PostCode_LinearRow lRow = (PostCodeDoc.PostCode_LinearRow)lView.Row;
            textTarget.Text = lRow.AddressLine3Id.ToString();
        }

        private void Click_buttonConsolidate(object sender, RoutedEventArgs e)
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

                gAddressLine3Adapter.Consolidate(int.Parse(textSource.Text), int.Parse(textTarget.Text));
                string Message = "Postaddress " + textSource.Text + " consolidated into postaddress " + textTarget.Text;

                // Record the event in the Exception table
                ExceptionData.WriteException(5, Message, this.ToString(), "Click_buttonConsolidate", "");
                MessageBox.Show("Done");

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonConsolidate_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }


        #endregion

        private void Click_buttonSubmit(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PostCodeDoc.PostCode_LinearRow lRow in gPostCodeDoc.PostCode_Linear.Rows)
                {
                    if (lRow.RowState == DataRowState.Modified)
                    {
                        gAddressLine3Adapter.Update(lRow.AddressLine4Id, lRow.AddressLine3, lRow.AddressLine3Id);
                        gAddressLine4Adapter.Update(lRow.CodeId, lRow.AddressLine4, lRow.AddressLine4Id);
                    }
                }
                gPostCodeDoc.PostCode_Linear.Clear();
                gLinearAdapter.FillByType(gPostCodeDoc.PostCode_Linear, "All");
                MessageBox.Show("Changes have been implemented successfully");

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonSubmit_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }
    }
}
