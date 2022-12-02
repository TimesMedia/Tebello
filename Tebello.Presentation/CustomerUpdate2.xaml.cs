using Subs.Data;
using Subs.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Subs.Presentation
{
    public partial class CustomerUpdate2 : Window
    {
        #region Globals - private
        private CustomerData3 gCustomerData;
        private Subs.Data.CommentData gCommentData;
        private bool _Cancelled = true;
        //private bool gAddressLine4Found = false;
        //private bool gAddressLine3Found = false;
        //private bool gAddressCodeFound = false;
        //private bool gFullAddressFound = false;

        private readonly static PostCodeDoc.PostCode_LinearDataTable gPostCodeLinear = new PostCodeDoc.PostCode_LinearDataTable();
        private readonly Subs.Data.PostCodeDocTableAdapters.PostCode_LinearTableAdapter gLinearAdapter = new Subs.Data.PostCodeDocTableAdapters.PostCode_LinearTableAdapter();
        private readonly Dictionary<int, string> gTitleDictionary = new Dictionary<int, string>();
        #endregion

        #region Constructors

        private bool GenericInitialisation()
        {
            try
            {

                foreach (int i in Enum.GetValues(typeof(Title)))
                {
                    ComboItem lNewItem = new ComboItem();
                    lNewItem.Key = i;
                    lNewItem.Value = Enum.GetName(typeof(Title), i);
                    ComboTitle.Items.Add(lNewItem);
                }
                CountryControl.CustomerData = gCustomerData;
                CustomerGrid.DataContext = gCustomerData;

                gCommentData = new CommentData(gCustomerData.CustomerId);
                CommentDataGrid.DataContext = gCommentData.gDoc.Comment2;

                gLinearAdapter.AttachConnection();

                gPostCodeLinear.Clear();
                gLinearAdapter.FillByType(gPostCodeLinear, "All");

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GenericInitialisation", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("GenericInitialisation " + ex.Message);
                return false;
            }
        }

        public CustomerUpdate2(CustomerData3 pCustomerData)
        {
            InitializeComponent();   // This is where the controls gets initialised as well. 

            gCustomerData = pCustomerData;

            if (!GenericInitialisation())
            {
                return;
            }

        }

        private void CustomerUpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTemplate();
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public DataGridCell GetCell(DataGrid host, DataGridRow row, int columnIndex)
        {
            if (row == null) return null;

            var presenter = GetVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            // try to get the cell but it may possibly be virtualized
            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell == null)
            {
                // now try to bring into view and retreive the cell
                host.ScrollIntoView(row, host.Columns[columnIndex]);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            }
            return cell;
        }
        public DataGridRow GetRow(DataGrid pGrid, int pIndex)
        {
            try
            {
                if (!pGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                    throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

                if (pIndex < 0 || pIndex > (pGrid.Items.Count - 1))
                    throw new ArgumentException(string.Format("{0} is an invalid row index.", pIndex));

                object lItem = pGrid.Items[pIndex]; // = Product X

                DataGridRow lRow = pGrid.ItemContainerGenerator.ContainerFromItem(lItem) as DataGridRow;

                if (lRow == null)
                {
                    /* bring the data item (Product object) into view
                     * in case it has been virtualized away */
                    pGrid.ScrollIntoView(lItem);
                    pGrid.UpdateLayout();
                    lRow = pGrid.ItemContainerGenerator.ContainerFromItem(lItem) as DataGridRow;
                }

                return lRow;

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "CustomerUpdate2", "GetRow", "DataGrid = " + pGrid.Name + " Index = " + pIndex.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return new DataGridRow();
            }
        }

        private void ShowTemplate()
        {
            try
            {
                // Start with a clean slate
                //CodeDataGrid.ItemsSource = null;
                //SuburbDataGrid.ItemsSource = null;

                //************************************************* PostStreet ********************************************************************************

                //IEnumerable<PostCodeLinear> lCodeList = new List<PostCodeLinear>();
                //List<PostCodeLinear> lSuburbList = new List<PostCodeLinear>();
                //ICollectionView lCodeView = new CollectionView(lCodeList);
                //ICollectionView lSuburbView = new CollectionView(lSuburbList);

                //int lCodeListCount = 0;
                //int lSuburbListCount = 0;

                //if (gCustomerData.AddressType == AddressType.PostStreet)
                //{
                //    if (!string.IsNullOrWhiteSpace(gCustomerData.Address5))
                //    {
                //        var lQuery = from matchCode in gPostCodeLinear
                //                     where (matchCode.Type == AddressType.PostStreet.ToString() && matchCode.Code == gCustomerData.Address5)
                //                     orderby matchCode.Code, matchCode.AddressLine4, matchCode.AddressLine3
                //                     select new PostCodeLinear() { Code = matchCode.Code, AddressLine4 = matchCode.AddressLine4, AddressLine3 = matchCode.AddressLine3, AddressLine3Id = matchCode.AddressLine3Id };

                //        lCodeList = lQuery.ToList<PostCodeLinear>();
                //        lCodeView = CollectionViewSource.GetDefaultView(lCodeList);
                //        CodeDataGrid.ItemsSource = lCodeView;
                //        lCodeListCount = lCodeList.Count();
                //    }

                //    if (!string.IsNullOrWhiteSpace(gCustomerData.Address3))
                //    {
                //        var lQueryReverse = from matchSuburb in gPostCodeLinear
                //                            where (matchSuburb.Type == AddressType.PostStreet.ToString() && matchSuburb.AddressLine3.ToUpper().Contains(gCustomerData.Address3.ToUpper()))
                //                            orderby matchSuburb.AddressLine3, matchSuburb.AddressLine4, matchSuburb.Code
                //                            select new PostCodeLinear() { Code = matchSuburb.Code, AddressLine4 = matchSuburb.AddressLine4, AddressLine3 = matchSuburb.AddressLine3, AddressLine3Id = matchSuburb.AddressLine3Id };

                //        lSuburbList = lQueryReverse.ToList<PostCodeLinear>();
                //        lSuburbView = CollectionViewSource.GetDefaultView(lSuburbList);
                //        SuburbDataGrid.ItemsSource = lSuburbView;
                //        lSuburbListCount = lSuburbList.Count();
                //    }
                //}


                ////************************************************* PostBox ********************************************************************************

                //if (gCustomerData.AddressType == AddressType.PostBox)
                //{
                //    if (!string.IsNullOrWhiteSpace(gCustomerData.Address5))
                //    {
                //        var lQuery = from matchCode in gPostCodeLinear
                //                     where (matchCode.Type == AddressType.PostBox.ToString() && matchCode.Code == gCustomerData.Address5)
                //                     orderby matchCode.Code, matchCode.AddressLine4, matchCode.AddressLine3
                //                     select new PostCodeLinear() { Code = matchCode.Code, AddressLine4 = matchCode.AddressLine4, AddressLine3 = matchCode.AddressLine3, AddressLine3Id = matchCode.AddressLine3Id };

                //        lCodeList = lQuery.ToList<PostCodeLinear>();
                //        lCodeView = CollectionViewSource.GetDefaultView(lCodeList);
                //        CodeDataGrid.ItemsSource = lCodeView;
                //        CodeDataGrid.UpdateLayout();
                //        lCodeListCount = lCodeList.Count();
                //    }

                //    if (!string.IsNullOrWhiteSpace(gCustomerData.Address3))
                //    {
                //        var lQueryReverse = from matchSuburb in gPostCodeLinear
                //                            where (matchSuburb.Type == AddressType.PostBox.ToString() && matchSuburb.AddressLine3.ToUpper().Contains(gCustomerData.Address3.ToUpper()))
                //                            orderby matchSuburb.AddressLine3, matchSuburb.AddressLine4, matchSuburb.Code
                //                            select new PostCodeLinear() { Code = matchSuburb.Code, AddressLine4 = matchSuburb.AddressLine4, AddressLine3 = matchSuburb.AddressLine3, AddressLine3Id = matchSuburb.AddressLine3Id };

                //        lSuburbList = lQueryReverse.ToList<PostCodeLinear>();
                //        lSuburbView = CollectionViewSource.GetDefaultView(lSuburbList);
                //        SuburbDataGrid.ItemsSource = lSuburbView;
                //        lSuburbListCount = lSuburbList.Count();
                //    }
                //}
                ////*********************************************************************************************************************************************


                //if (lCodeListCount == 0 && lSuburbListCount == 0)
                //{
                //    // Nothing to show

                //    return;
                //}

                //// Code grid **************************************************************************************************************************                

                //// Find first instance that is a match or a partial match.

                //if (lCodeListCount == 0)
                //{
                //    goto Suburb;
                //}


                //// Initialisation
                //gAddressCodeFound = false;
                //gAddressLine3Found = false;
                //gAddressLine4Found = false;
                //gFullAddressFound = false;

                //int lAddressCodeIndex = 0;
                //int lAddressLine4Index = 0;
                //int lAddressLine3Index = 0;


                //for (int i = 0; i < CodeDataGrid.Items.Count; i++)
                //{
                //    PostCodeLinear lPostCodeLinearRow = (PostCodeLinear)CodeDataGrid.Items[i];
                //    lAddressCodeIndex = i;

                //    if (lPostCodeLinearRow.AddressLine4.ToUpper().StartsWith(gCustomerData.Address4.ToUpper()))
                //    {
                //        if (gAddressLine4Found != true)
                //        {
                //            gAddressLine4Found = true;
                //            lAddressLine4Index = i;  // This is the first one found.
                //        }

                //        if (lPostCodeLinearRow.AddressLine3.ToUpper().StartsWith(gCustomerData.Address3.ToUpper()))
                //        {
                //            gAddressLine3Found = true;
                //            lAddressLine3Index = i;
                //            break; //Break out of the loop
                //        }
                //        else continue;
                //    }
                //    else continue;
                //}


                //if (gAddressLine3Found)
                //{
                //    DataGridRow lRow = GetRow(CodeDataGrid, lAddressLine3Index);

                //    GetCell(CodeDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    GetCell(CodeDataGrid, lRow, 1).Background = Brushes.LightPink;
                //    GetCell(CodeDataGrid, lRow, 2).Background = Brushes.LightPink;

                //    //CodeDataGrid.ScrollIntoView(CodeDataGrid.SelectedItem);

                //    CodeDataGrid.ScrollIntoView(lRow.Item);

                //    gFullAddressFound = true;

                //}

                //else if (gAddressLine4Found)
                //{
                //    DataGridRow lRow = GetRow(CodeDataGrid, lAddressLine4Index);
                //    GetCell(CodeDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    GetCell(CodeDataGrid, lRow, 1).Background = Brushes.LightPink;
                //    CodeDataGrid.ScrollIntoView(lRow.Item);
                //}

                //else
                //{
                //    DataGridRow lRow = GetRow(CodeDataGrid, 0);
                //    GetCell(CodeDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    CodeDataGrid.ScrollIntoView(lRow.Item);
                //}

                //// Suburb grid ***************************************************************************************************************************                

                //Suburb:
                //// Find first instance that is a match or a partial match.

                //// Initialisation
                //gAddressLine3Found = false;
                //gAddressLine4Found = false;
                //gAddressCodeFound = false;

                //lAddressCodeIndex = 0;
                //lAddressLine4Index = 0;
                //lAddressLine3Index = 0;

                //if (lSuburbListCount == 0)
                //{
                //    return;
                //}

                //for (int i = 0; i < SuburbDataGrid.Items.Count; i++)
                //{
                //    PostCodeLinear lPostCodeLinearRow = (PostCodeLinear)SuburbDataGrid.Items[i];
                //    lAddressLine3Index = i;

                //    if (lPostCodeLinearRow.AddressLine4.ToUpper().StartsWith(gCustomerData.Address4))
                //    {
                //        if (gAddressLine4Found != true)
                //        {
                //            gAddressLine4Found = true;
                //            lAddressLine4Index = i;  // This is the first one found.
                //        }
                //        if (lPostCodeLinearRow.Code == gCustomerData.Address5)
                //        {
                //            gAddressCodeFound = true;
                //            lAddressCodeIndex = i;
                //            break;
                //        }
                //        else continue;
                //    }
                //    else continue;
                //}


                //if (gAddressCodeFound)
                //{
                //    DataGridRow lRow = GetRow(SuburbDataGrid, lAddressCodeIndex);

                //    GetCell(SuburbDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    GetCell(SuburbDataGrid, lRow, 1).Background = Brushes.LightPink;
                //    GetCell(SuburbDataGrid, lRow, 2).Background = Brushes.LightPink;

                //    SuburbDataGrid.ScrollIntoView(lRow.Item);
                //    gFullAddressFound = true;

                //}

                //else if (gAddressLine4Found)
                //{
                //    DataGridRow lRow = GetRow(SuburbDataGrid, lAddressLine4Index);
                //    GetCell(SuburbDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    GetCell(SuburbDataGrid, lRow, 1).Background = Brushes.LightPink;

                //    SuburbDataGrid.ScrollIntoView(lRow.Item);
                //}

                //else
                //{
                //    DataGridRow lRow = GetRow(SuburbDataGrid, 0);

                //    GetCell(SuburbDataGrid, lRow, 0).Background = Brushes.LightPink;
                //    SuburbDataGrid.ScrollIntoView(lRow.Item);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ShowTemplate", "CustomerId = " + gCustomerData.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Event handlers - private

        private void CustomerUpdateWindow_Closing(object sender, CancelEventArgs e)
        {
            if (gCustomerData.Modified)
            {
                if (MessageBoxResult.No == MessageBox.Show("You have modified the customer data. Do you want to exit without saving?", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
                {
                    e.Cancel = true;
                }
                else
                {
                    gCustomerData.Modified = false;
                    MessageBox.Show("Nothing Done");
                    Cancelled = true;
                }
            }
        }

        private void Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                FocusManager.SetFocusedElement(this, (Button)sender);
 
                //Do not continue while one of the controls are still in error.

                bool lHasError = false;

                foreach (UIElement lElement in CustomerGrid.Children)
                {
                    if (Validation.GetHasError(lElement))
                    {
                        lHasError = true;
                    }
                }

                if (lHasError)
                {
                    MessageBox.Show("I cannot continue while some of the input are still in error");
                    return;
                }


                if (string.IsNullOrWhiteSpace(this.textBoxEmailAddress.Text))
                {
                    string lMessage = "There is no EMail address. Do you want to continue without it?";

                    if (MessageBoxResult.Yes == MessageBox.Show(lMessage, "Warning", MessageBoxButton.YesNo))
                    {
                        ExceptionData.WriteException(5, "Attempt to capture new customer without EMail address.", this.ToString(), "Click_Submit",
                            "Capturer = " + System.Environment.UserName);
                    }
                    else
                    {
                        return;
                    }
                }


                // Update - assume that it will do an overall check.

                gCustomerData.VerificationDate = DateTime.Now.Date;

                {
                    string lResult;

                    if ((lResult = gCustomerData.Update()) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // Update the comments
                {
                    string lResult;

                    if ((lResult = gCommentData.Update(gCustomerData.CustomerId)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                Cancelled = false;

                MessageBox.Show("Customer " + gCustomerData.CustomerId.ToString() + " updated.");
                this.Close();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Submit", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                Cancelled = true;

                MessageBox.Show("Submit failed  due to a technical error");
            }

        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            gCustomerData.Modified = false;
            MessageBox.Show("Nothing Done");
            Cancelled = true;
            Close();
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

        //private void ButtonKeepCode_Click(object sender, RoutedEventArgs e)
        //{
        //    PostCodeLinear lPostcodeLinear = (PostCodeLinear)CodeDataGrid.SelectedItem;
        //    if (lPostcodeLinear == null)
        //    {
        //        MessageBox.Show("You have not selected an address to keep");
        //        return;
        //    }
        //    gCustomerData.Address5 = lPostcodeLinear.Code;
        //    gCustomerData.Address4 = lPostcodeLinear.AddressLine4;
        //    gCustomerData.Address3 = lPostcodeLinear.AddressLine3;
        //    gCustomerData.PostAddressId = lPostcodeLinear.AddressLine3Id;

        //    gFullAddressFound = true;
        //}

        //private void ButtonKeepSuburb_Click(object sender, RoutedEventArgs e)
        //{
        //    PostCodeLinear lPostcodeLinear = (PostCodeLinear)SuburbDataGrid.SelectedItem;
        //    if (lPostcodeLinear == null)
        //    {
        //        MessageBox.Show("You have not selected an address to keep");
        //        return;
        //    }
        //    gCustomerData.Address5 = lPostcodeLinear.Code;
        //    gCustomerData.Address4 = lPostcodeLinear.AddressLine4;
        //    gCustomerData.Address3 = lPostcodeLinear.AddressLine3;
        //    gCustomerData.PostAddressId = lPostcodeLinear.AddressLine3Id;

        //    gFullAddressFound = true;

        //}

        //private void ButtonKeepText_Click(object sender, RoutedEventArgs e)
        //{


        //    string lMessage = "This address is going to be saved as type = " + AddressTypeControl.AddressType.ToString() + ". Is this what you want?";

        //    if (MessageBoxResult.No == MessageBox.Show(lMessage, "Warning", MessageBoxButton.YesNo))
        //    {
        //        return;
        //    }

        //    if (AddressTypeControl.AddressType == AddressType.International || AddressTypeControl.AddressType == AddressType.UnAssigned)
        //    {
        //        return;
        //    }


        //    int AddressLine3Id = PostCodeData.Merge(gCustomerData.AddressType.ToString(),
        //                        gCustomerData.Address5,
        //                        gCustomerData.Address3,
        //                        gCustomerData.Address4);

        //    if (AddressLine3Id != 0)
        //    {
        //        PostCodeDoc.PostCode_LinearRow lRow = gPostCodeLinear.FindByAddressLine3Id(AddressLine3Id);

        //        if (lRow == null)
        //        {
        //            if (!PostCodeData.AddLinearRow(gPostCodeLinear, AddressLine3Id))
        //            {
        //                MessageBox.Show("Error adding the new address to the internal template");
        //                return;
        //            }
        //        }

        //        gCustomerData.PostAddressId = AddressLine3Id;

        //        ShowTemplate();
        //        MessageBox.Show("New address saved successfully in template and in the data");
        //        gFullAddressFound = true;
        //    }
        //    else
        //    {
        //        MessageBox.Show("There was an error in saving the text address");
        //    }
        //}

        private void checkAutomaticPaymentAllocation_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox lCheckBox = (CheckBox)e.OriginalSource;
            if (!(bool)lCheckBox.IsChecked)
            {
                // Deallocate after warning

                if (MessageBoxResult.Yes == MessageBox.Show("This will remove all previous payment allocations, whether automatic or manual. Do you want to continue?", "Warning", MessageBoxButton.YesNo))
                {
                    CustomerBiz.Deallocate(gCustomerData.CustomerId);
                }
            }
        }

        private void CommentDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            CustomerDoc2.Comment2Row lRow = (CustomerDoc2.Comment2Row)e.NewItem;
            lRow.ModifiedOn = DateTime.Now;
            lRow.ModifiedBy = Environment.UserDomainName;
            lRow.AcceptChanges();
        }

        #endregion

        #region Properties public
        public bool Cancelled
        {
            get
            {
                return _Cancelled;
            }
            set
            {
                _Cancelled = (bool)value;
            }
        }

        #endregion

       
    }
}
