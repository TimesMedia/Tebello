using Subs.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;


namespace Subs.Presentation
{
    public partial class SubscriptionDetailControl2 : UserControl, ISubscriptionPicker
    {
        public class SubscriptionSelector: Subs.Data.BaseModel
        {
            string _StatusString;
            public int SubscriptionId { get;set; }
            public string StatusString { 
                get
                {return _StatusString; } 
                
                
                set 
                {
                    _StatusString = value;
                    NotifyPropertyChanged("StatusString");
                }
            }
            public string PayerSurname { get; set; }
            public string ReceiverSurname { get; set; }
            public string ProductName { get; set; }
            public int InvoiceId { get; set; }
            public int ProFormaId { get; set; }
            public bool RenewalNotice { get; set; }
            public bool AutomaticRenewal { get; set; }
            public int PayerId { get; set; }
            public int ReceiverId { get; set; }
            public int UnitsPerIssue { get; set; }
            public string PayerCompany { get; set; }
        }

        #region Globals

        public readonly ObservableCollection<SubscriptionSelector> gSubscriptions;
        private System.Windows.Data.CollectionViewSource gSubscriptionViewSource;
        private SubscriptionPicker2 gContainer;

        #endregion

        public SubscriptionDetailControl2(SubscriptionPicker2 pContainer, SubscriptionDoc3.SubscriptionDerivedDataTable pSubscriptionDerived)
        {
            InitializeComponent();
            gContainer = pContainer;
            subscriptionDetailDataGrid.ContextMenu = gContainer.gContextMenu;
            gSubscriptions = new ObservableCollection<SubscriptionSelector>();

            foreach (SubscriptionDoc3.SubscriptionDerivedRow   s in pSubscriptionDerived)
            {
                SubscriptionSelector lSubscription = new SubscriptionSelector()
                {
                    SubscriptionId = s.SubscriptionId,
                    StatusString = s.StatusString,
                    PayerSurname = s.PayerSurname,
                    ReceiverSurname = s.ReceiverSurname,
                    ProductName = s.ProductName,
                    InvoiceId = s.InvoiceId,
                    ProFormaId = s.ProFormaId,
                    RenewalNotice = s.RenewalNotice,
                    AutomaticRenewal = s.AutomaticRenewal,
                    PayerId = s.PayerId,
                    ReceiverId = s.ReceiverId,
                    UnitsPerIssue = s.UnitsPerIssue,
                    PayerCompany = s.PayerCompany
                };

                gSubscriptions.Add(lSubscription);
            }
            gSubscriptionViewSource = new CollectionViewSource() { Source = gSubscriptions };
            subscriptionDetailDataGrid.ItemsSource = gSubscriptionViewSource.View;

        }
     

        public int GetCurrentSubscriptionId()
        {
            try
            {
                //System.Data.DataRowView lRowView = (System.Data.DataRowView)gSubscriptionViewSource.View.CurrentItem;
                //if (lRowView != null)
                //{
                //    SubscriptionDoc3.SubscriptionDerivedRow lRow = (SubscriptionDoc3.SubscriptionDerivedRow)lRowView.Row;
                //    return lRow.SubscriptionId;
                //}

                SubscriptionSelector lSubscriptionSelector = (SubscriptionSelector)gSubscriptionViewSource.View.CurrentItem;
                if (lSubscriptionSelector != null)
                {
                    return lSubscriptionSelector.SubscriptionId;
                }
                else
                {
                    return 0;
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

        public void ReflectSubscriptionCancelled(int pSubscriptionId)
        {
            try
            {
                SubscriptionSelector lSubscriptionSelector = (SubscriptionSelector)gSubscriptions.Where(x => x.SubscriptionId == pSubscriptionId).Single();
                lSubscriptionSelector.StatusString = Enum.GetName(typeof(SubStatus), (int)Subs.Data.SubStatus.Cancelled);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ReflectSubscriptionCancelled", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        private void Click_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Type lType = e.OriginalSource.GetType();
            if (lType.Name == "DataGridHeaderBorder")
            {
                // Do not consider the event handled. Invoke the context menu.
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            subscriptionDetailDataGrid.Height = this.ActualHeight - 50;
        }

        private void Click_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            gContainer.DisplayStatusAndHistory();
        }

        private void subscriptionDetailDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
