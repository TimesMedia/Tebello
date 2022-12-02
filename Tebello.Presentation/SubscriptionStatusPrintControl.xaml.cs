using Subs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Subs.Presentation
{

    public partial class SubscriptionStatusPrintControl : UserControl
    {
        #region Global variables
       
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter lAdapterSubscriptionIssue
                    = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
        private readonly Subs.Data.LedgerDoc2TableAdapters.TransactionHistoryTableAdapter lAdaptorTransactionHistory =
                   new Subs.Data.LedgerDoc2TableAdapters.TransactionHistoryTableAdapter();
        private SubscriptionData3 gSubscriptionData;
        CustomerData3 gCustomerData;
        private readonly SubscriptionDoc3 gDataset = new SubscriptionDoc3();
        private readonly LedgerDoc2 gLedgerDoc = new LedgerDoc2();

        public class IssueStatus
        {
            public string  IssueDescription { get; set;}
            public int      Sequence { get; set; }
            public int      UnitsLeft { get; set; }
            public bool DeliveryOnCredit { get; set; }
        }


        public class SubscriptionStatus
        {
            public List<IssueStatus> List { get; set;} = new List<IssueStatus>();

        }

        #endregion

        #region Construction
        public SubscriptionStatusPrintControl()
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubscriptionStatusDisplayControl", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void Load(int pSubscriptionId)

        {
            try
            {
                // Populate Subscription

                gSubscriptionData = new SubscriptionData3(pSubscriptionId);
                Basic.DataContext = (object)gSubscriptionData;

                gCustomerData = new CustomerData3(gSubscriptionData.PayerId);

                // Populate SubscriptionIssues

                lAdapterSubscriptionIssue.FillById(gDataset.SubscriptionIssue, pSubscriptionId, "BySubscription");

                SubscriptionStatus lSubscriptionStatus = new SubscriptionStatus();

                foreach (Data.SubscriptionDoc3.SubscriptionIssueRow item in gDataset.SubscriptionIssue)
                {
                    IssueStatus lStatus = new IssueStatus();
                    lStatus.IssueDescription = ProductDataStatic.GetIssueDescription(item.IssueId);
                    lStatus.Sequence = item.Sequence;
                    lStatus.UnitsLeft = item.UnitsLeft;
                    lStatus.DeliveryOnCredit = item.DeliverOnCredit;
                    lSubscriptionStatus.List.Add(lStatus);
                }


                Issues.DataContext =  lSubscriptionStatus.List;     //gDataset.SubscriptionIssue;

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

                //// Save the result as a Pdf

                //byte[] lBuffer = FlowDocumentConverter.PdfConverter.ConvertDoc(this.gFlowDocument);

                //using (MemoryStream lStream = new System.IO.MemoryStream(lBuffer, true))
                //{

                //    lStream.Write(lBuffer, 0, lBuffer.Length);
                //    lStream.Position = 0;

                //    // Save a copy to disk
                //    FileStream lPdfStream = File.OpenWrite(Settings.DirectoryPath + "\\STATUS_" + gSubscriptionData.PayerId.ToString() + ".pdf");
                //    lStream.WriteTo(lPdfStream);
                //    lPdfStream.Position = 0;
                //    lPdfStream.Flush();
                //    lPdfStream.Close();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }

        }

        #endregion

        #region Properties - public

        public static readonly DependencyProperty SubscriptionIdProperty;

        static SubscriptionStatusPrintControl()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(SubscriptionChanged));
            SubscriptionStatusPrintControl.SubscriptionIdProperty = DependencyProperty.Register("SubscriptionId", typeof(int), typeof(SubscriptionStatusPrintControl), lMetaData);
        }

        private static void SubscriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == -1)
            {
                return;
            }

            SubscriptionStatusPrintControl lControl = (SubscriptionStatusPrintControl)o;
            lControl.Load((int)e.NewValue);
        }

        public int SubscriptionId
        {
            get
            {
                return (int)GetValue(SubscriptionStatusPrintControl.SubscriptionIdProperty);
            }

            set
            {
                SetValue(SubscriptionStatusPrintControl.SubscriptionIdProperty, value);
            }
        }

        #endregion

        #region Event handlers

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog lPrintDialog = new PrintDialog();
                lPrintDialog.UserPageRangeEnabled = true;
               
                if (lPrintDialog.ShowDialog() == true)
                {
                    lPrintDialog.PrintDocument(
                         ((IDocumentPaginatorSource)gFlowDocument).DocumentPaginator, "SubscriptionStatusDisplayControl");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Print_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);


            }
        }
 
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IDocumentPaginatorSource lSource = gFlowDocument;
                PrintDialog lDialog = new PrintDialog();
                lDialog.PrintQueue = new LocalPrintServer().GetPrintQueue("Microsoft Print to PDF");
 
                if ((bool)lDialog.ShowDialog())
                {
                    lDialog.PrintDocument(lSource.DocumentPaginator, "Save as PDF");
                }

                MessageBox.Show("Done!");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Save_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

            }
        }

        #endregion
    }
}
