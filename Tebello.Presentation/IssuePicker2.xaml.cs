using Subs.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for IssuePicker.xaml
    /// </summary>
    public partial class IssuePicker2 : Window
    {
        #region Globals

        private readonly Subs.Data.ProductDoc gProductDoc;
        private readonly BindingListCollectionView gProductView;
        private readonly BindingListCollectionView gIssueView;
        private int gInitialProductId = 0;
        private int gInitialIssueId = 0;
        private readonly Subs.Data.ProductDocTableAdapters.Product2TableAdapter gProductAdapter = new Subs.Data.ProductDocTableAdapters.Product2TableAdapter();
        private readonly Subs.Data.ProductDocTableAdapters.IssueTableAdapter gIssueAdapter = new Subs.Data.ProductDocTableAdapters.IssueTableAdapter();

        #endregion

        #region Constructor and event handlers.

        public IssuePicker2()
        {
            InitializeComponent();

            try
            {
                gProductDoc = new ProductDoc();

                gProductAdapter.AttachConnection();
                gIssueAdapter.AttachConnection();

                gProductDoc.Issue.Clear();
                gProductDoc.Product2.Clear();

                gProductAdapter.FillById(gProductDoc.Product2, 0, "All");
                gProductDoc.Product2.AcceptChanges();
                ProductDataGrid.ItemsSource = gProductDoc.Product2;

                gIssueAdapter.FillById(gProductDoc.Issue, 0, "All");
                gProductDoc.Issue.AcceptChanges();
                IssueDataGrid.ItemsSource = gProductDoc.Issue;

                gProductView = (BindingListCollectionView)CollectionViewSource.GetDefaultView(ProductDataGrid.ItemsSource);
                gIssueView = (BindingListCollectionView)CollectionViewSource.GetDefaultView(IssueDataGrid.ItemsSource);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "IssuePicker constructor", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                foreach (DataRowView lRowView in gProductView)
                {
                    ProductDoc.Product2Row lRow = (ProductDoc.Product2Row)lRowView.Row;

                    if (CurrentProduct(lRow.ProductId))
                    {
                        lRow.Status = "Active";
                    }
                    else
                    {
                        lRow.Status = "Dormant";
                    }
                }

                gProductView.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Ascending));
                gProductView.SortDescriptions.Add(new SortDescription("ProductName", ListSortDirection.Ascending));

                // Check to see if there is a preferred initial product.

                gProductView.MoveCurrentToFirst(); //Get a meaningful starting point.

                bool lProductFound = false;

                if (gInitialProductId != 0)
                {
                    do
                    {
                        DataRowView lDataRowView = (DataRowView)gProductView.CurrentItem;
                        ProductDoc.Product2Row lRow = (ProductDoc.Product2Row)lDataRowView.Row;
                        if (lRow.ProductId == gInitialProductId)
                        {
                            lProductFound = true;
                            break;
                        }
                    } while (gProductView.MoveCurrentToNext());

                    if (!lProductFound)
                    {
                        throw new Exception("There does not seem to be a product with ID = " + gInitialProductId.ToString());
                    }

                    gIssueView.CustomFilter = IssueFilter;
                }

                DependencyObject CurrentNode = VisualTreeHelper.GetChild(ProductDataGrid, 0);
                CurrentNode = VisualTreeHelper.GetChild(CurrentNode, 0);

                ScrollViewer lScrollViewer = (ScrollViewer)CurrentNode;

                int lScrollPosition = 0;

                if (gProductView.CurrentPosition <= (lScrollViewer.ViewportHeight / 2))
                {
                    lScrollPosition = 0;
                }
                else
                {
                    lScrollPosition = gProductView.CurrentPosition - ((int)Math.Floor(lScrollViewer.ViewportHeight) / 2);
                }

                lScrollViewer.ScrollToVerticalOffset(lScrollPosition);

                // Check to see if there is a preferred initial issue.

                gIssueView.MoveCurrentToFirst();

                bool lIssueFound = false;
                if (gInitialIssueId != 0)
                {
                    do
                    {
                        DataRowView lDataRowView = (DataRowView)gIssueView.CurrentItem;
                        ProductDoc.IssueRow lRow = (ProductDoc.IssueRow)lDataRowView.Row;
                        if (lRow.IssueId == gInitialIssueId)
                        {
                            lIssueFound = true;
                            break;
                        }
                    } while (gIssueView.MoveCurrentToNext());

                    if (!lIssueFound)
                    {
                        throw new Exception("There does not seem to be an active Issue with ID = " + gInitialIssueId.ToString());
                    }

                    CurrentNode = VisualTreeHelper.GetChild(IssueDataGrid, 0);
                    CurrentNode = VisualTreeHelper.GetChild(CurrentNode, 0);

                    lScrollViewer = (ScrollViewer)CurrentNode;

                    lScrollPosition = 0;

                    if (gIssueView.CurrentPosition <= (lScrollViewer.ViewportHeight / 2))
                    {
                        lScrollPosition = 0;
                    }
                    else
                    {
                        lScrollPosition = gIssueView.CurrentPosition - ((int)Math.Floor(lScrollViewer.ViewportHeight) / 2);
                    }

                    lScrollViewer.ScrollToVerticalOffset(lScrollPosition);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Window_Loaded", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in Window_Loaded " + ex.Message);
            }
        }


        private string IssueFilter
        {
            get
            {
                if ((bool)CheckAllIssues.IsChecked)
                {
                    return "ProductId = " + ProductId;
                }
                else
                {
                    return "ProductId = " + ProductId + " AND Year >= " + (DateTime.Now.Year - 1).ToString();
                }
            }
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void IssueDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CheckAllIssues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gIssueView.CustomFilter = IssueFilter;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CheckAllIssues_Checked", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in CheckAllIssues_Checked " + ex.Message);
            }
        }

        private System.Data.EnumerableRowCollection<ProductDoc.IssueRow> GetCurrentIssues(int pProductId)
        {
            return gProductDoc.Issue.Where(p => p.ProductId == pProductId
                                                               && p.StartDate <= DateTime.Now
                                                               && p.EndDate >= DateTime.Now);
        }

        private bool CurrentProduct(int pProductId)
        {
            try
            {
                // Used only to see if the product is active or dormant. 

                if (GetCurrentIssues(pProductId).Count<ProductDoc.IssueRow>() > 0)
                {
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CurrentProduct", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gIssueView == null)
            {
                // You are now too early in the sequence of initialisation
                return;
            }
            System.Data.DataRowView lRowView = (System.Data.DataRowView)gProductView.CurrentItem;

            ProductDoc.Product2Row lRow = (ProductDoc.Product2Row)lRowView.Row;

            gIssueView.CustomFilter = IssueFilter;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ProductDataGrid.Height = this.ActualHeight - 60;
            IssueDataGrid.Height = this.ActualHeight - 90;

        }

        #region Public methods

        public void IssueSelect(int pIssueId)
        {
            gInitialIssueId = pIssueId;

        }

        public void ProductSelect(int pProductId)
        {
            gInitialProductId = pProductId;
        }

        #endregion

        public bool IssueWasSelected
        {
            get
            {
                if (IssueDataGrid.SelectedItems.Count != 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        public int ProductId
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gProductView.CurrentItem;
                return (int)lView["ProductId"];
            }
        }

        public string ProductNaam
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gProductView.CurrentItem;
                return lView["ProductName"].ToString();
            }
        }

        public int IssueId
        {
            get
            {
                try
                {
                    System.Data.DataRowView lView = (System.Data.DataRowView)gIssueView.CurrentItem;
                    if (lView == null)
                    {
                        return 0;
                    }
                    return (int)lView["IssueId"];
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "IssueId", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Failed on IssueId " + ex.Message);
                    return 0;
                }
            }
        }

        public string IssueName
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gIssueView.CurrentItem;
                return lView["IssueDescription"].ToString();
            }
        }

        public int Sequence
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gIssueView.CurrentItem;
                return (int)lView["Sequence"];
            }
        }

        public int No
        {
            get
            {
                System.Data.DataRowView lView = (System.Data.DataRowView)gIssueView.CurrentItem;
                return (int)lView["No"];
            }
        }

        #endregion
    }
}
