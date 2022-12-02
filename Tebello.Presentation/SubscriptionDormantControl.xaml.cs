using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Subs.Presentation
{
    public partial class SubscriptionDormantControl : UserControl, ISubscriptionPicker
    {

        #region Globals 
        private readonly CollectionViewSource gCollectionViewSource;
        #endregion

        public SubscriptionDormantControl(ContextMenu pContextMenu)
        {
            InitializeComponent();
            DormantDataGrid.ContextMenu = pContextMenu;
            gCollectionViewSource = (CollectionViewSource)this.Resources["DormantViewSource"];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            gCollectionViewSource.Source = DeliveryDataStatic.Dormants();
        }

        public int GetCurrentSubscriptionId()
        {
            try
            {

                Dormant lItem = (Dormant)gCollectionViewSource.View.CurrentItem;
                if (lItem != null)
                {
                    return lItem.SubscriptionId;
                }
                else
                {
                    throw new Exception("No subscription has been selected yet");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetCurrentSubscriptionId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        private void Click_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Type lType = e.OriginalSource.GetType();
            if (lType.Name == "DataGridHeaderBorder")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DormantDataGrid.Height = ActualHeight - 50;
        }
    }
}
