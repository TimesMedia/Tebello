using Subs.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tebello.Presentation
{
      public partial class AdministrationPromotion : Window
    {
        #region Globals
        private readonly Subs.Data.SubscriptionDoc3.PromotionDataTable gPromotionTable = new SubscriptionDoc3.PromotionDataTable();
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.PromotionTableAdapter gPromotionAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.PromotionTableAdapter();
        private SubscriptionDoc3.PromotionRow gSelectedPromotion;
 
        private ObservableCollection<PromotionData> gPromotions = new ObservableCollection<PromotionData>();
        private CollectionViewSource gPromotionViewSource = new CollectionViewSource();

        #endregion

        #region Housekeeping
        public AdministrationPromotion()
        {
            try
            {
                InitializeComponent();

                gPromotionAdapter.AttachConnection();
                gPromotionAdapter.Fill(gPromotionTable);

                foreach (SubscriptionDoc3.PromotionRow lRow in gPromotionTable)
                {
                    gPromotions.Add(new PromotionData(lRow.PromotionId));
                }

                gPromotionViewSource = (CollectionViewSource)this.FindResource("promotionViewSource");
                gPromotionViewSource.Source = gPromotions;
                gPromotionViewSource.SortDescriptions.Add(new SortDescription("ProductName", ListSortDirection.Ascending));
                gPromotionViewSource.SortDescriptions.Add(new SortDescription("PayerSurname", ListSortDirection.Descending));
                BulletinEntry.Text = NoteData.GetBulletin();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AdministrationPromotion", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
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
     
        private void PromotionDataGrid_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectionViewSource lCollection = (CollectionViewSource)(this.FindResource("promotionViewSource"));
                DataRowView lView = (DataRowView)lCollection.View.CurrentItem;
                if (lView != null)
                {
                    gSelectedPromotion = (SubscriptionDoc3.PromotionRow)lView.Row;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "PromotionDataGrid_Selected", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        #endregion


        #region Context menu
        private void AddPayer_Click(object sender, RoutedEventArgs e)
        {
            CustomerPicker3 lCustomerPicker = new CustomerPicker3();
            lCustomerPicker.ShowDialog();

            if (Settings.CurrentCustomerId != 0)
            {
                PromotionData lPromotion = (PromotionData)gPromotionViewSource.View.CurrentItem;
                lPromotion.PayerId = Settings.CurrentCustomerId;
              }
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Subs.Presentation.IssuePicker2 lPicker = new Subs.Presentation.IssuePicker2();
            lPicker.ShowDialog();
            PromotionData lPromotion = (PromotionData)gPromotionViewSource.View.CurrentItem;
            lPromotion.ProductId = lPicker.ProductId;
        }
        private void PayerClear_Click(object sender, RoutedEventArgs e)
        {
            PromotionData lPromotion = (PromotionData)gPromotionViewSource.View.CurrentItem;
            lPromotion.PayerId = null;
        }
        #endregion

        #region Buttons
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PromotionData lPromotion = new PromotionData();
                gPromotions.Add(lPromotion);
                PromotionDataGrid.ScrollIntoView(lPromotion);
                PromotionDataGrid.SelectedItem = lPromotion;
                  
                DataGridRow lRow = (DataGridRow)PromotionDataGrid.ItemContainerGenerator.ContainerFromItem(lPromotion);
                lRow.Background = new SolidColorBrush(Colors.Orange);

                DoubleAnimation lAnimation = new DoubleAnimation();
                lAnimation.From = 01;
                lAnimation.To = 25;
                lAnimation.Duration = TimeSpan.FromSeconds(2);
                lRow.BeginAnimation(DataGridRow.HeightProperty, lAnimation);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "buttonAdd_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }
         private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PromotionData lItem in gPromotions)
                {
                    if (!lItem.Unchanged)
                    {
                        lItem.Update();
                    }
                }
                MessageBox.Show("Update successfull");
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
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void SaveBulletinEntry_Click(object sender, RoutedEventArgs e)
        {
            if(NoteData.SetBulletin(BulletinEntry.Text))
            {
                MessageBox.Show("Bulletin entry succesfully updated.");
            };
        }

    }
}
