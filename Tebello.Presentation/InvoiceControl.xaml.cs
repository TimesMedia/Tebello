using Subs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for InvoiceControl.xaml
    /// </summary>
    public partial class InvoiceControl : UserControl
    {
        private LedgerDoc2 gLedgerDoc = new LedgerDoc2();
        private CustomerData3 gCustomerData;
        private readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);

        public InvoiceControl()
        {
            InitializeComponent();
        }

        public string LoadAndRenderInvoice(int pInvoiceId)
        {
            return Render(gDataContext.MIMS_InvoiceControl_LoadInvoice(pInvoiceId).ToList());
        }

        public string Render(List<Invoice> pInvoice)
        {
            string lStage = "Start";
            try
            {
                gCustomerData = new CustomerData3(pInvoice[0].PayerId);


                lStage = "Registered address";

                if (gCustomerData.CompanyName != "")
                { 
                    Line1.Content = gCustomerData.CompanyName;
                    Line2.Content = gCustomerData.FullName;
                }
                else { Line1.Content = gCustomerData.FullName; };

                if (gCustomerData.PhysicalAddressId != null)
                {
                    DeliveryAddressData2 lDeliveryAddressData = new DeliveryAddressData2((int)gCustomerData.PhysicalAddressId);

                    if (!lDeliveryAddressData.Format())
                    {
                        throw new Exception("Error in Format of deliveryaddress");
                    }

                    if (!String.IsNullOrWhiteSpace(gCustomerData.Department))
                    {
                        Line3.Content = gCustomerData.Department;
                        Line4.Content = lDeliveryAddressData.PAddress1;
                        Line5.Content = lDeliveryAddressData.PAddress2;
                        Line6.Content = lDeliveryAddressData.PAddress3;
                        Line7.Content = lDeliveryAddressData.PAddress4;
                        Line8.Content = lDeliveryAddressData.PAddress5;
                    }
                    else
                    {
                        Line3.Content = lDeliveryAddressData.PAddress1;
                        Line4.Content = lDeliveryAddressData.PAddress2;
                        Line5.Content = lDeliveryAddressData.PAddress3;
                        Line6.Content = lDeliveryAddressData.PAddress4;
                        Line7.Content = lDeliveryAddressData.PAddress5;
                    }

                    Line3.Content = gCustomerData.Department;
                    Line4.Content = lDeliveryAddressData.PAddress1;
                    Line5.Content = lDeliveryAddressData.PAddress2;
                    Line6.Content = lDeliveryAddressData.PAddress3;
                    Line7.Content = lDeliveryAddressData.PAddress4;
                    Line8.Content = lDeliveryAddressData.PAddress5;
    
                    lStage = "Delivery address";

                    PLine1.Content = lDeliveryAddressData.PAddress1;
                    PLine2.Content = lDeliveryAddressData.PAddress2;
                    PLine3.Content = lDeliveryAddressData.PAddress3;
                    PLine4.Content = lDeliveryAddressData.PAddress4;
                    PLine5.Content = lDeliveryAddressData.PAddress5;
                }

                InvoiceNumber.Content = "INV" + pInvoice[0].InvoiceId;
                PPhoneNumber.Content = gCustomerData.PhoneNumber;
                PEmail.Content = gCustomerData.EmailAddress;
                PVatRegistration.Content = gCustomerData.VATRegistration;
                VendorNumber.Content = gCustomerData.VendorNumber;
                CompanyRegistrationNumber.Content = gCustomerData.CompanyRegistrationNumber;
                PayerId.Content = gCustomerData.CustomerId;
                InvoiceDate.Content = pInvoice[0].DateFrom.ToString("dd MMM yyyy");

                // Write Items

                lStage = "Text margin";

                Thickness lTextMargin = new Thickness(0, 0, 10, 0);
                //Thickness lRightMargin = new Thickness(0, 0, 0, 0);

                decimal lTotalExc = 0;
                decimal lTotalVat = 0;
                decimal lPayable = 0;

                foreach (Invoice lInvoice in pInvoice)
                {
                    lStage = "Items - CurrentSubscriptionId = "+ lInvoice.SubscriptionId.ToString();

                    SubscriptionData3 lSubscription = new SubscriptionData3(lInvoice.SubscriptionId);

                    OrderNo.Content = lSubscription.OrderNumber; // Assuming that you are going to end up with the last order number.

                    // Build a row
                    TableRow lTableRow = new TableRow();

                    Paragraph lSubIdParagraph = new Paragraph(new Run(lSubscription.SubscriptionId.ToString()));
                    lTableRow.Cells.Add(new TableCell(lSubIdParagraph));

                    lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(lSubscription.ReceiverId.ToString())) { Margin = lTextMargin }));

                    lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(lSubscription.ReceiverCompany)) { Margin = lTextMargin }));

                    lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(ProductDataStatic.GetIssueDescription(lSubscription.StartIssue))) { Margin = lTextMargin }));

                    lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(ProductDataStatic.GetIssueDescription(lSubscription.LastIssue)))));

                    Paragraph lDebitValueParagraph = new Paragraph(new Run(lSubscription.BaseRate.ToString("R ######0.00")));
                    lDebitValueParagraph.TextAlignment = TextAlignment.Right;
                    lTableRow.Cells.Add(new TableCell(lDebitValueParagraph));

                    decimal lDiscountedPrice = lSubscription.BaseRate * lSubscription.DiscountMultiplier;
                    decimal lDiscount = lSubscription.BaseRate - lDiscountedPrice;
                    Paragraph lDiscountParagraph = new Paragraph(new Run(lDiscount.ToString("R ######0.00")));
                    lDiscountParagraph.TextAlignment = TextAlignment.Right;
                     lTableRow.Cells.Add(new TableCell(lDiscountParagraph));

                    Paragraph lDeliveryParagraph = new Paragraph(new Run(lSubscription.DeliveryCost.ToString("R ######0.00")));
                    lDeliveryParagraph.TextAlignment = TextAlignment.Right;
                    lTableRow.Cells.Add(new TableCell(lDeliveryParagraph));

                    decimal lUnitPriceExc = lSubscription.UnitPrice - lSubscription.Vat;


                    Paragraph lUnitPriceParagraph = new Paragraph(new Run(lUnitPriceExc.ToString("R ######0.00")));
                    lUnitPriceParagraph.TextAlignment = TextAlignment.Right;
                    lTableRow.Cells.Add(new TableCell(lUnitPriceParagraph));

                    int lNumberOfUnits = lSubscription.UnitsPerIssue * lSubscription.NumberOfIssues;
                    Paragraph lUnitParagraph = new Paragraph(new Run(lSubscription.UnitsPerIssue.ToString() + " X " + lSubscription.NumberOfIssues.ToString()));
                    lUnitParagraph.TextAlignment = TextAlignment.Right;
                    lTableRow.Cells.Add(new TableCell(lUnitParagraph));

                    decimal lTotalPerSubExc = (lSubscription.UnitPrice - lSubscription.Vat) * lNumberOfUnits;
                    Paragraph lTotalParagraph = new Paragraph(new Run(lTotalPerSubExc.ToString("R #######0.00")));
                    lTotalParagraph.TextAlignment = TextAlignment.Right;
                     lTableRow.Cells.Add(new TableCell(lTotalParagraph));

                    // Calculate totals

                    lTotalExc = lTotalExc + lTotalPerSubExc;
                    lTotalVat = lTotalVat + (lSubscription.Vat * lNumberOfUnits);


                    // Add the row to the table
                    HistoryTable.RowGroups[1].Rows.Add(lTableRow);

                    // Add a creditnote row if it is appriopriate

                    if (lSubscription.CreditNoteValue < 0)
                    {
                        lTableRow = new TableRow();

                        Paragraph lCreditNoteParagraph = new Paragraph(new Run(lSubscription.CreditNoteName));
                        lTableRow.Cells.Add(new TableCell(lCreditNoteParagraph));
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTableRow.Cells.Add(new TableCell());
                        lTotalPerSubExc = lSubscription.CreditNoteValue * ((100 / (100 + lSubscription.VatPercentage)));
                        Paragraph lCNParagraph = new Paragraph(new Run(lTotalPerSubExc.ToString("R #######0.00")));
                        lCNParagraph.TextAlignment = TextAlignment.Right;
                        lTableRow.Cells.Add(new TableCell(lCNParagraph));
                        HistoryTable.RowGroups[1].Rows.Add(lTableRow);

                        lTotalExc = lTotalExc + lTotalPerSubExc;
                        lTotalVat = lTotalVat + (lSubscription.CreditNoteValue - lTotalPerSubExc);
                    }

                } // End of foreach loop

                // Add the totals                              

                // Calculate the credit

                decimal lDue = gCustomerData.Due;

                lPayable = lTotalExc + lTotalVat;

                TotalExc.Content = lTotalExc.ToString("R ######0.00");
                Vat.Content = lTotalVat.ToString("R ######0.00");

                Payable.Content = lPayable.ToString("R ######0.00");

                // Save to disk

                string lFileName = "INV"
                + pInvoice[0].InvoiceId.ToString()
                + ".pdf";

                if (File.Exists(Settings.DirectoryPath + "\\" + lFileName))
                {
                    File.Delete(Settings.DirectoryPath + "\\" + lFileName);
                }

                byte[] lBuffer = FlowDocumentConverter.XpsConverter.ConverterDoc(this.gFlowDocument);
                MemoryStream lXpsStream = new MemoryStream(lBuffer);
                FileStream lPdfStream = File.OpenWrite(Settings.DirectoryPath + "\\" + lFileName);
                PdfSharp.Xps.XpsConverter.Convert(lXpsStream, lPdfStream, false);
                lPdfStream.Position = 0;
                lPdfStream.Flush();
                lPdfStream.Close();

                return "OK";

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Render", lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed on Load " + ex.Message;
            }
        }

    }
}
