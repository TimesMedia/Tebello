using Subs.Data;
using System;
using System.Windows;

namespace Subs.Presentation
{
    public partial class PaymentAllocation : Window
    {
        #region Globals
        private readonly Subs.Data.SubscriptionDoc3.SubscriptionIssueDataTable gSubscriptionIssue = new SubscriptionDoc3.SubscriptionIssueDataTable();
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter gSubscriptionIssueAdaptor = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
        private readonly int gSubscriptionId = 0;

        #endregion


        public PaymentAllocation(int pSubscriptionId)
        {
            InitializeComponent();

            try
            {
                gSubscriptionId = pSubscriptionId;
                gSubscriptionIssueAdaptor.AttachConnection();


                gSubscriptionIssueAdaptor.FillById(gSubscriptionIssue, gSubscriptionId, "BySubscription");
                SubscriptionIssueDataGrid.ItemsSource = gSubscriptionIssue;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "PaymentAllocation", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                gSubscriptionIssueAdaptor.Update(gSubscriptionIssue);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_Save", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }
    }
}
