using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Subs.Presentation
{

    /// <summary>
    /// Interaction logic for SubscriptionStatusDisplayControl2.xaml
    /// </summary>
    public partial class SubscriptionStatusDisplayControl2 : UserControl
    {
        #region Global variables
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter lAdapterSubscription
          = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter lAdapterSubscriptionIssue
                    = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
        private readonly Subs.Data.LedgerDoc2TableAdapters.TransactionHistoryTableAdapter lAdaptorTransactionHistory =
           new Subs.Data.LedgerDoc2TableAdapters.TransactionHistoryTableAdapter();


        private SubscriptionData3 gSubscriptionData;
        private readonly SubscriptionDoc3 gDataset = new SubscriptionDoc3();
        private readonly LedgerDoc2 gLedgerDoc = new LedgerDoc2();

        #endregion

        #region Construction
        public SubscriptionStatusDisplayControl2()
        {
            InitializeComponent();
            try
            {
                lAdapterSubscriptionIssue.AttachConnection();
                lAdaptorTransactionHistory.AttachConnection();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubscriptionHistory", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        public void Clear()
        {
            Basic.DataContext = null;
            Issues.DataContext = null;
            History.ItemsSource = null;
        }

        private void Load(int pSubscriptionId)

        {
            try
            {
                // Populate Subscription

                gSubscriptionData = new SubscriptionData3(pSubscriptionId);
                Basic.DataContext = (object)gSubscriptionData;

                // Populate SubscriptionIssues

                lAdapterSubscriptionIssue.FillById(gDataset.SubscriptionIssue, pSubscriptionId, "BySubscription");
                Issues.DataContext = gDataset.SubscriptionIssue;

                // History

                lAdaptorTransactionHistory.FillBy(gLedgerDoc.TransactionHistory, pSubscriptionId);

                foreach (LedgerDoc2.TransactionHistoryRow lRow in gLedgerDoc.TransactionHistory)
                {
                    if (lRow.Operation == 22)
                    {
                        // this is a creditnote
                        lRow.CreditValue = -lRow.Value;
                    }
                }

                History.ItemsSource = gLedgerDoc.TransactionHistory;
                this.Refresh();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }

        }

        #endregion

        #region Properties - public

        public static readonly DependencyProperty SubscriptionIdProperty;

        static SubscriptionStatusDisplayControl2()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(SubscriptionChanged));
            SubscriptionStatusDisplayControl2.SubscriptionIdProperty = DependencyProperty.Register("SubscriptionId", typeof(int), typeof(SubscriptionStatusDisplayControl2), lMetaData);
        }

        private static void SubscriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == -1 || (int)e.NewValue == 0)
            {
                return;
            }

            SubscriptionStatusDisplayControl2 lControl = (SubscriptionStatusDisplayControl2)o;
            lControl.Load((int)e.NewValue);
        }

        public int SubscriptionId
        {
            get
            {
                return (int)GetValue(SubscriptionStatusDisplayControl2.SubscriptionIdProperty);
            }

            set
            {
                SetValue(SubscriptionStatusDisplayControl2.SubscriptionIdProperty, value);
            }
        }

        #endregion

        #region Event handlers

        #endregion


    }
}
