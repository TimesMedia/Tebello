using Subs.Business;
using Subs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Subs.Presentation
{
    public partial class StatementControl2 : UserControl
    {
        private readonly SolidColorBrush gNormal = new SolidColorBrush(Colors.LightPink);
        private readonly SolidColorBrush gAlternate = new SolidColorBrush(Colors.LightBlue);
        private bool gColour = true;

        public StatementControl2(int pCustomerId, int pStatementId)
        {
            InitializeComponent();
            Load(pCustomerId, pStatementId);
        }

        private void RenderPaymentRow(Subs.Data.InvoicesAndPayments pCurrentRow)
        {
            Thickness lRightMargin = new Thickness(0, 0, 10, 0);

            // Build a row
            TableRow lTableRow = new TableRow();


            if (pCurrentRow.FirstRow)
            {
                // Toggle the background colour
                if (gColour)
                {
                    gColour = false;
                }
                else
                {
                    gColour = true;
                }
            }

            if (gColour) lTableRow.Background = gNormal; else lTableRow.Background = gAlternate;

            Paragraph lDateParagraph = new Paragraph(new Run(pCurrentRow.Date.ToString("yyyy-MM-dd"))) { Margin = lRightMargin };

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.TransactionId.ToString())) { Margin = lRightMargin }));
            lTableRow.Cells.Add(new TableCell(lDateParagraph));

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.Operation)) { Margin = lRightMargin }));

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.Reference2)) { Margin = lRightMargin }));

            Paragraph lValueParagraph = new Paragraph(new Run(pCurrentRow.Value.ToString("#######0.00")));
            lValueParagraph.TextAlignment = TextAlignment.Right;
            lValueParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lValueParagraph));

            // Invoice balance and statement balance

            Paragraph lInvoiceBalanceParagraph;
            Paragraph lStatementBalanceParagraph;

            if (pCurrentRow.LastRow)
            {
                lInvoiceBalanceParagraph = new Paragraph(new Run(pCurrentRow.InvoiceBalance.ToString("#######0.00")));
                lStatementBalanceParagraph = new Paragraph(new Run(pCurrentRow.StatementBalance.ToString("#######0.00")));
            }
            else
            {
                lInvoiceBalanceParagraph = new Paragraph(new Run(""));
                lStatementBalanceParagraph = new Paragraph(new Run(""));
            }

            lInvoiceBalanceParagraph.TextAlignment = TextAlignment.Right;
            lInvoiceBalanceParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lInvoiceBalanceParagraph));

            lStatementBalanceParagraph.TextAlignment = TextAlignment.Right;
            lStatementBalanceParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lStatementBalanceParagraph));

            // Add the row to the table
            HistoryTable.RowGroups[1].Rows.Add(lTableRow);

            if (pCurrentRow.LastRow)
            {
                TableRow lBlankRow = new TableRow();
                lBlankRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                HistoryTable.RowGroups[1].Rows.Add(lBlankRow);
            }
        }

        private void RenderInvoiceRow(Subs.Data.InvoicesAndPayments pCurrentRow)
        {
            Thickness lRightMargin = new Thickness(0, 0, 10, 0);

            // Build a row
            TableRow lTableRow = new TableRow();


            if (pCurrentRow.FirstRow)
            {
                // Toggle the background colour
                if (gColour)
                {
                    gColour = false;
                }
                else
                {
                    gColour = true;
                }
            }

            if (gColour) lTableRow.Background = gNormal; else lTableRow.Background = gAlternate;

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.TransactionId.ToString())) { Margin = lRightMargin }));

            Paragraph lDateParagraph = new Paragraph(new Run(pCurrentRow.Date.ToString("yyyy-MM-dd"))) { Margin = lRightMargin };
            lTableRow.Cells.Add(new TableCell(lDateParagraph));

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.Operation)) { Margin = lRightMargin }));

            lTableRow.Cells.Add(new TableCell(new Paragraph(new Run("INV" + pCurrentRow.InvoiceId.ToString())) { Margin = lRightMargin }));
            //lTableRow.Cells.Add(new TableCell(new Paragraph(new Run(pCurrentRow.Reference2)) { Margin = lRightMargin }));

            Paragraph lValueParagraph = new Paragraph(new Run(pCurrentRow.Value.ToString("#######0.00")));
            lValueParagraph.TextAlignment = TextAlignment.Right;
            lValueParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lValueParagraph));

            // Invoice balance and statment balance

            Paragraph lInvoiceBalanceParagraph;
            Paragraph lStatementBalanceParagraph;

            if (pCurrentRow.LastRow)
            {
                lInvoiceBalanceParagraph = new Paragraph(new Run(pCurrentRow.InvoiceBalance.ToString("#######0.00")));
                lStatementBalanceParagraph = new Paragraph(new Run(pCurrentRow.StatementBalance.ToString("#######0.00")));
            }
            else
            {
                lInvoiceBalanceParagraph = new Paragraph(new Run(""));
                lStatementBalanceParagraph = new Paragraph(new Run(""));
            }

            lInvoiceBalanceParagraph.TextAlignment = TextAlignment.Right;
            lInvoiceBalanceParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lInvoiceBalanceParagraph));

            lStatementBalanceParagraph.TextAlignment = TextAlignment.Right;
            lStatementBalanceParagraph.Margin = lRightMargin;
            lTableRow.Cells.Add(new TableCell(lStatementBalanceParagraph));

            // Add the row to the table
            HistoryTable.RowGroups[3].Rows.Add(lTableRow);

            if (pCurrentRow.LastRow || pCurrentRow.Operation == "CreditNote")
            {
                TableRow lBlankRow = new TableRow();
                lBlankRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                HistoryTable.RowGroups[3].Rows.Add(lBlankRow);
            }
        }

        private string Load(int pCustomerId, int pStatementId)
        {
            string lStage = "Start";
            try
            {
                CustomerData3 lCustomerData = new CustomerData3(pCustomerId);
                lStage = "PersonalData";
                string[] Lines = new string[8];
                int lNextIndex = 0;

                lCustomerData = new CustomerData3(pCustomerId);

                if (!String.IsNullOrWhiteSpace(lCustomerData.CompanyName) && lCustomerData.CompanyName != lCustomerData.FullName)
                {
                    Lines[lNextIndex] = lCustomerData.CompanyName; lNextIndex++;
                    Lines[lNextIndex] = lCustomerData.FullName; lNextIndex++;
                }
                else { Line1.Content = lCustomerData.FullName; lNextIndex++; };


                if (!String.IsNullOrWhiteSpace(lCustomerData.Department) && lCustomerData.Department != lCustomerData.FullName)
                {
                    Lines[lNextIndex] = lCustomerData.Department; lNextIndex++;
                }

                if (lCustomerData.PhysicalAddressId != null)
                {
                    DeliveryAddressData2 lDeliveryAddressData = new DeliveryAddressData2((int)lCustomerData.PhysicalAddressId);
                    if (!lDeliveryAddressData.Format())
                    {
                        throw new Exception("Error in Format of deliveryaddress");
                    }

                    if (Lines[lNextIndex - 1] != lDeliveryAddressData.PAddress1)
                    {
                        Lines[lNextIndex] = lDeliveryAddressData.PAddress1; lNextIndex++;
                    }

                    Lines[lNextIndex] = lDeliveryAddressData.PAddress2; lNextIndex++;
                    Lines[lNextIndex] = lDeliveryAddressData.PAddress3; lNextIndex++;
                    Lines[lNextIndex] = lDeliveryAddressData.PAddress4; lNextIndex++;
                    Lines[lNextIndex] = lDeliveryAddressData.PAddress5; lNextIndex++;

                    Line1.Content = Lines[0];
                    Line2.Content = Lines[1];
                    Line3.Content = Lines[2];
                    Line4.Content = Lines[3];
                    Line5.Content = Lines[4];
                    Line6.Content = Lines[5];
                    Line7.Content = Lines[6];
                    Line8.Content = Lines[7];
                }
                StatementNumber.Content = "STA" + pStatementId.ToString();
                PPhoneNumber.Content = lCustomerData.PhoneNumber;
                PEmail.Content = lCustomerData.EmailAddress;
                PVatRegistration.Content = lCustomerData.VATRegistration;
                VendorNumber.Content = lCustomerData.VendorNumber;
                CompanyRegistrationNumber.Content = lCustomerData.CompanyRegistrationNumber;
                PayerId.Content = pCustomerId.ToString();
                StatementDate.Content = DateTime.Now.ToString("dd MMM yyyy");

                // Get the data

                List<Subs.Data.InvoicesAndPayments> lPaymentAndInvoice;

                {
                    string lResult;

                    if ((lResult = CustomerData3.PopulateInvoice(lCustomerData.CustomerId, out lPaymentAndInvoice)) != "OK")
                    {

                        return lResult;
                    }
                }

                IEnumerable<Subs.Data.InvoicesAndPayments> lPayment;
                lPayment = lPaymentAndInvoice.Where(p => p.OperationId == (int)Operation.Pay || p.OperationId == (int)Operation.Refund || p.OperationId == (int)Operation.ReversePayment).ToList();

                IEnumerable<Subs.Data.InvoicesAndPayments> lInvoice;
                lInvoice = lPaymentAndInvoice.Where(p => !(p.OperationId == (int)Operation.Pay || p.OperationId == (int)Operation.Refund || p.OperationId == (int)Operation.ReversePayment)).OrderBy(q => q.Date).ToList();


                // Write Items
                lStage = "Items";

                HistoryTable.RowGroups[1].Rows.Clear();

                foreach (Subs.Data.InvoicesAndPayments lRow in lPayment)
                {

                    RenderPaymentRow(lRow);  // Render the subsequenct rows.

                }

                HistoryTable.RowGroups[3].Rows.Clear();

                foreach (Subs.Data.InvoicesAndPayments lRow in lInvoice)
                {

                    RenderInvoiceRow(lRow);  // Render the subsequenct rows.

                }

                StatementValue = lInvoice.Select(p => p.StatementBalance).Last();

                lStage = "Footer";

                if (lCustomerData.DebitorderUser)
                {
                    Footer1.Content = "Please do not pay, since this is merely a record of your debitorder transactions.";
                    Footer2.Content = "For any queries, please contact me at 011 280 5856  or e-mail: subscriptions@mims.co.za.";
                    Footer3.Content = "";
                }

                if (File.Exists(Settings.DirectoryPath + "\\STA_" + pStatementId.ToString() + "_" + lCustomerData.CustomerId.ToString() + ".pdf"))
                {
                    File.Delete(Settings.DirectoryPath + "\\STA_" + pStatementId.ToString() + "_" + lCustomerData.CustomerId.ToString() + ".pdf");
                }

                byte[] lBuffer = FlowDocumentConverter.XpsConverter.ConverterDoc(this.gFlowDocument);
                MemoryStream lXpsStream = new MemoryStream(lBuffer);
                FileStream lPdfStream = File.OpenWrite(Settings.DirectoryPath + "\\STA_" + pStatementId.ToString() + "_" + lCustomerData.CustomerId.ToString() + ".pdf");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Load", lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed on Load " + ex.Message;
            }
        }


        public static string SendEmail(int pStatementId, int pCustomerId, string pEmailAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pEmailAddress))
                {
                    return "SendEmail failed: Customer " + pCustomerId.ToString() + " does not have an Email Address";
                }

                string lSubject = "Statement for customer " + pCustomerId.ToString();
                string lBody = "Dear Client\n\n"
                     + "Attached herewith your statement.\n\n"
                     + "We trust that this will improve the quality of our service and look forward to a long-lasting relationship with you.\n\n"
                     + "Best\n\n"
                     + "Riëtte van der Merwe\n"
                     + "Subscription and Marketing Manager\n"
                     + "Tel: (011) 280-5856\n"
                     + "Fax: (086) 675 7910\n"
                     + "E-mail: vandermerwer@mims.co.za\n\n"
                     + "Hill on Empire, 16 Empire Rd (cnr Hillside Rd), ParkTown, Johannesburg, 2193\n"
                     + "P O Box 1746, Saxonworld, Johannesburg, 2132\n"
                     + "www.mims.co.za";

                if (CustomerBiz.SendEmail(Settings.DirectoryPath + "\\STA_" + pStatementId.ToString() + "_" + pCustomerId.ToString() + ".pdf", pEmailAddress, lSubject, lBody) != "OK")
                {
                    return "Error in Emailing statement";
                }

                ExceptionData.WriteException(5, "Emailed statement for customer = " + pCustomerId.ToString(), "StatementControl2", "SendEmail", "To " + pEmailAddress);
            

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "StatementControl2", "SendEmailBatch", "To " + pEmailAddress);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error sending Email " + ex.Message + " Customer = " + pCustomerId.ToString();
            }
        }

        public decimal StatementValue { get; set; }

    }
}
