using Subs.Business;
using Subs.Data;
using Tebello.MimsWeb.Models;
using Tebello.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace Tebello.MimsWeb.Controllers
{
    public class PromotionController : Controller
    {
        private ProductData gProductData;
        private readonly MIMSDataContext gMIMSDataContext = new MIMSDataContext(Settings.ConnectionString);

        public PromotionController()
        {
            gProductData = new ProductData();
        }


        public ActionResult GetProducts(int ProductSelector = (int)WebProductClassifications.All)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = ModelState.ToString();
                    return View("Error");
                }

                WebProducts lWebProducts = new WebProducts();
                lWebProducts.ProductSelector = ProductSelector;
                lWebProducts.Bulletin = NoteData.GetBulletin();
                LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);

                lWebProducts.ListOfWebProducts = gProductData.WebProducts(lWebProducts.ProductSelector);


                if (lWebProducts.ListOfWebProducts.Count() == 0)
                {
                    ViewBag.Message = "Sorry, no product satisfies these requirements";
                    return View("List", lWebProducts);
                }
                else
                {
                    if (lLoginRequest.CustomerId != null)
                    {
                        foreach (WebProduct item in lWebProducts.ListOfWebProducts)
                        {
                            item.Price = SubscriptionBiz.StandardProductPrice(item.ProductId, (int)lLoginRequest.CustomerId);
                        }
                    }

                    // Save this as a session variable, and use it to populate the model.

                    SessionHelper.Set(Session, SessionKey.WebProducts, lWebProducts);

                    return View("List", lWebProducts);   //
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetProducts - post", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;

                return View("Error");
            }
        }



        [HttpGet]
        public ViewResult List()
        {

            //Silke insisted that the page where you query what category to select from and the view where you display the products, be the same page.
            //Hence, this view can operate in two modes. The first is just a query, and the second is the query plus the result.

            try
            {

                WebProducts lPreviousSelection = SessionHelper.GetWebProducts(Session);

                //22222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222


                // Add the original prices if the user has logged in in the mean time.

                LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
                if (lLoginRequest.CustomerId != null && lPreviousSelection.ListOfWebProducts != null)
                {
                    foreach (WebProduct item in lPreviousSelection.ListOfWebProducts)
                    {
                        item.Price = SubscriptionBiz.StandardProductPrice(item.ProductId, (int)lLoginRequest.CustomerId);
                    }
                }

                return View("List", lPreviousSelection);

                //22222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "List", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;

                return View("Error");
            }
        }


        public ActionResult GoToBasket()
        {
            if (SessionHelper.GetLoginRequest(Session).CustomerId == null)
            {
                ViewBag.Message = "You cannot have a basket if you are not logged in first";
                return View("List");
            }

            Basket lBasket;
            lBasket = SessionHelper.GetBasket(Session);
            if (lBasket.BasketItems.Count == 0)
            {
                ViewBag.Message = "Your basket is empty";
                return View("List");
            }

            if (lBasket.BasketItems.Count() == 1)
            {
                ViewBag.Message = "There is " + lBasket.BasketItems.Count() + " product selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
            }
            else
            {
                ViewBag.Message = "There are " + lBasket.BasketItems.Count() + " products selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
            }

            return View("Basket", lBasket);
        }

        public ActionResult AddToBasket(int ProductId)
        {
            string lStage = "GetBasket";

            Basket lBasket = SessionHelper.GetBasket(Session);

            try
            {
                if (lBasket == null)
                {
                    lBasket = new Basket();
                }

                // If there is one already, add to units per issue

                bool lFound = false;
                foreach (BasketItem item in lBasket.BasketItems)
                {
                    if (item.Subscription.ProductId == ProductId)
                    {
                        lFound = true;
                        item.Subscription.UnitsPerIssue++;
                    }
                }

                lStage = "Found?";

                if (!lFound)
                {
                    // create a new BasketItem
                    LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);


                    SubscriptionData3 lSubscription = new SubscriptionData3();

                    lSubscription.PayerId = (int)lLoginRequest.CustomerId;
                    lSubscription.ReceiverId = (int)lLoginRequest.CustomerId;
                    lSubscription.ProductId = ProductId;
                    DateTime lNextDate;

                    {
                        string lResult;
                        if ((lResult = SubscriptionBiz.NextDate((int)lLoginRequest.CustomerId, ProductId, out lNextDate)) != "OK")
                        {
                            ViewBag.Message = lResult;
                            return View("Basket", lBasket);
                        }
                    }

                    {
                        MimsValidationResult lResult = SubscriptionBiz.SetInitialValues(lSubscription, lNextDate);

                        if (lResult.Message != "OK" && !lResult.Prompt)
                        {
                            ViewBag.Message = lResult.Message;
                            return View("Basket", lBasket);
                        }
                    }

                    BasketItem lBasketItem = new BasketItem() { Subscription = lSubscription };
                    lBasketItem.ProductName = ProductDataStatic.GetProductName(ProductId);
                    lBasket.BasketItems.Add(lBasketItem);

                    lStage = "Calculate";


                    if (SubscriptionBiz.CalculateBasket(lBasket.BasketItems))
                    {
                        lBasket.TotalPrice = 0;
                        lBasket.TotalDiscount = 0;
                        lBasket.TotalDiscountedPrice = 0;


                        foreach (BasketItem item in lBasket.BasketItems)
                        {
                            lBasket.TotalPrice += item.Price;
                            lBasket.TotalDiscount += item.Discount;
                        }

                        lBasket.TotalDiscountedPrice = lBasket.TotalPrice - lBasket.TotalDiscount;

                        if (lBasket.BasketItems.Count() == 1)
                        {
                            ViewBag.Message = "There is " + lBasket.BasketItems.Count() + " product selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
                        }
                        else
                        {
                            ViewBag.Message = "There are " + lBasket.BasketItems.Count() + " products selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "There was a problem calculating the basket value";
                        return View("Empty");
                    }
                }

                lStage = "Set";

                SessionHelper.Set(Session, SessionKey.Basket, lBasket);
                return View("Basket", lBasket);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AddToBasket", "Stage = " + lStage, (int)SessionHelper.GetLoginRequest(Session).CustomerId);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return View("Basket", lBasket);
            }
        }

        [HttpPost]
        public ActionResult BasketModification(IList<BasketModification> pModifications)
        {
            string lStage = "Start";

            try
            {
                // For some reason I cannot get it via a parameter. Possibly, because there has not been a preceding [HTTPGet]

                // NameValueCollection lForm = this.ControllerContext.RequestContext.HttpContext.Request.Form;

                //First, you will have to rebuild the basket, substitute the changed values and then you can continue.

                Basket lBasket = SessionHelper.GetBasket(Session);

                int i = 0;
                ObservableCollection<BasketItem> lToBeRemoved = new ObservableCollection<BasketItem>();


                lStage = "foreach";

                foreach (BasketItem item in lBasket.BasketItems)
                {
                    BasketModification lModification = pModifications[i];

                    if (lModification.Drop)
                    {
                        lToBeRemoved.Add(item);
                    }
                    item.Subscription.UnitsPerIssue = lModification.UnitsPerIssue;
                    item.Subscription.DeliveryMethodInt = lModification.DeliveryMethod;
                    i++;
                }

                // Remove the marked entries.

                lStage = "Remove";

                foreach (BasketItem item in lToBeRemoved)
                {
                    lBasket.BasketItems.Remove(item);
                }
                lToBeRemoved.Clear();


                int Changes = lBasket.BasketItems.Count;

                // Recalculate the basket

                lStage = "Calculate";


                if (SubscriptionBiz.CalculateBasket(lBasket.BasketItems))
                {
                    lBasket.TotalPrice = 0;
                    lBasket.TotalDiscount = 0;
                    lBasket.TotalDiscountedPrice = 0;

                    foreach (BasketItem lBasketItem in lBasket.BasketItems)
                    {
                        lBasket.TotalPrice += lBasketItem.Price;
                        lBasket.TotalDiscount += lBasketItem.Discount;
                    }

                    lBasket.TotalDiscountedPrice = lBasket.TotalPrice - lBasket.TotalDiscount;

                    LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);

                    CustomerData3 lCustomerData = new CustomerData3((int)lLoginRequest.CustomerId);
                    if (lCustomerData.PhysicalAddressId == 0)
                    {

                        // If there are non electronic deliverymethods, you are going to need a deliveryaddress

                        DeliveryMethod[] lMethodsRequiringAddress = { DeliveryMethod.Courier, DeliveryMethod.Mail, DeliveryMethod.RegisteredMail };

                        int lPhysicalAddressNeeded = lBasket.BasketItems.Where(p => lMethodsRequiringAddress.Contains(p.Subscription.DeliveryMethod)).Count();

                        if (lPhysicalAddressNeeded > 0)
                        {
                            lBasket.RequiresDeliveryAddress = true;
                        }
                    }
                    else
                    {
                        lBasket.RequiresDeliveryAddress = false;
                    }

                    lStage = "Count";

                    if (lBasket.BasketItems.Count() == 0)
                    {
                        lBasket.TotalDiscount = 0;
                        lBasket.TotalDiscountedPrice = 0;
                        lBasket.TotalPrice = 0;

                        ViewBag.Message = "There is no product selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
                    }

                    else if (lBasket.BasketItems.Count() == 1)
                    {
                        ViewBag.Message = "There is " + lBasket.BasketItems.Count() + " product selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
                    }
                    else
                    {
                        ViewBag.Message = "There are " + lBasket.BasketItems.Count() + " products selected, resulting in a price of " + lBasket.TotalDiscountedPrice.ToString("R #####0.00");
                    }
                }
                else
                {
                    ViewBag.Message = "There was a problem calculating the basket value";
                    return View("Empty");
                }

                lStage = "Set";

                SessionHelper.Set(Session, SessionKey.Basket, lBasket);

                return View("Basket", lBasket);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "BasketModification", "Stage = " + lStage, (int)SessionHelper.GetLoginRequest(Session).CustomerId);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = "There was a problem calculating the basket value";
                return View("Empty");
            }

        }
       

        public ActionResult Basket(Basket pBasket)
        {
            return View("Basket", pBasket);
        }

        [HttpGet]
        public ActionResult Submit()
        {
            Basket lBasket = SessionHelper.GetBasket(Session);
            LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
            StringBuilder lPositiveResult = new StringBuilder();

            if (lBasket.BasketItems.Count() == 0)
            {
                ViewBag.Message = "There is nothing to finalise";
                return View("Empty");
            }

            try
            {
                // Validate all the subscriptions

                foreach (BasketItem lBasketItem in lBasket.BasketItems)
                {
                    Subs.Data.MimsValidationResult lValidationResult = SubscriptionBiz.Validate(lBasketItem.Subscription);

                    if (lValidationResult.Message != "OK")
                    {
                        if (lValidationResult.Prompt)
                        {
                            if (lValidationResult.Message.Contains("overlaps"))
                            {
                                lBasketItem.Warning = "This subscription will overlap with an existing one. Please remove it from the cart. Overlapping subscriptions have to be ordered telephonically.";
                                return View("Basket", lBasket);
                            }
                        }
                        else
                        {
                            ExceptionData.WriteException(1, lValidationResult.Message, this.ToString(), "Submit", "Receiver = " + lBasketItem.Subscription.ReceiverId.ToString());
                            ViewBag.Message = lValidationResult.Message;
                            return View("Empty");
                        }
                    }

                    lBasketItem.Subscription.gReadyToSubmit = true;

                } // End of validation loop


                // Submit all the subscriptions
                // Start the transaction

                SqlConnection lConnection = new SqlConnection();
                lConnection.ConnectionString = Settings.ConnectionString;

                if (lConnection.State != ConnectionState.Open)
                {
                    lConnection.Open();
                }

                SqlTransaction lTransaction = lConnection.BeginTransaction("Submit");

                lPositiveResult.AppendLine("The following subscriptions have been submitted successfully: ");

                foreach (BasketItem lBasketItem in lBasket.BasketItems)
                {
                    // Initialise the subscription
                    string ResultOfInitialise = SubscriptionBiz.Initialise(lTransaction, lBasketItem.Subscription);
                    if (ResultOfInitialise != "OK")
                    {
                        lTransaction.Rollback("Submit");
                        ViewBag.Message = ResultOfInitialise;
                        return View("Empty");
                    }
                    else
                    {
                        lPositiveResult.AppendLine(lBasketItem.Subscription.SubscriptionId.ToString());
                    }

                }  //End of foreach
                lTransaction.Commit();

                // Generate and submit an invoice.

                List<InvoiceSpecification> lInvoiceSpecification = gMIMSDataContext.MIMS_LedgerData_LoadInvoiceBatch("ForCustomerId", lLoginRequest.CustomerId).ToList();

                int lInvoiceId = AdministrationData2.GetInvoiceId();
                lBasket.InvoiceId = lInvoiceId;
                SessionHelper.Set(Session, SessionKey.Basket, lBasket);

                decimal lCurrentInvoiceValue = 0M;

                foreach (InvoiceSpecification item in lInvoiceSpecification)
                {
                    //Link the invoice to each subscription

                    SubscriptionData3 lSubscriptionData = new SubscriptionData3(item.SubscriptionId);
                    lSubscriptionData.InvoiceId = lInvoiceId;

                    if (!lSubscriptionData.Update())
                    {
                        ViewBag.Message = "Error updating subscription: " + item.SubscriptionId.ToString();
                        return View("Empty");
                    }

                    lCurrentInvoiceValue = lCurrentInvoiceValue + lSubscriptionData.UnitPrice * lSubscriptionData.UnitsPerIssue * lSubscriptionData.NumberOfIssues;
                }

                // Write an entry to the log
                LedgerData.Invoice((int)lLoginRequest.CustomerId, lInvoiceId, 0, lCurrentInvoiceValue);

                // Process the invoice in a seperate single apartment thread.
                //Thread lWorkerThread = new Thread(ProcessInvoice);
                //lWorkerThread.SetApartmentState(ApartmentState.STA);
                //object pState = lInvoiceId;  // Box it
                //lWorkerThread.Start(pState);
                //lWorkerThread.Join();


                // Email the invoice

                string lFileName = Settings.DirectoryPath + "\\"
                    + "INV"
                    + lInvoiceId.ToString()
                    + ".pdf";

                string lSubject = "SUBS Tax Invoice - Customerid = " + lLoginRequest.CustomerId;
                string lBody = "Dear Client\n\n"
                             + "Attached herewith your Tax invoice. This tax invoice is a legal document and should be submitted to SARS with your VAT return.\n"
                             + "Kindly supply the vat invoice number along with your payment.\n"
                             + "Statements will be issued monthly to confirm all transactions on your account.\nThese statements will be e-mailed or faxed to you.\n\n"
                             + "Please forward this document to your accounts department.\n\n"
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

                if (CustomerBiz.SendEmail(lFileName, lLoginRequest.Email, lSubject, lBody) != "OK")
                {
                    ViewBag.Message = "There was a problem emailing the invoice";
                    return View("Empty");
                }

                lPositiveResult.AppendLine("A corresponding invoice has been emailed to you.");
                ViewBag.Message = "";  //lPositiveResult;
                return RedirectToAction("Pay");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Submit", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = "There was a problem in processing the order.";
                return View("Empty");
            }
        }

        //private void ProcessInvoice(object pState)
        //{
        //    try
        //    {

        //        InvoiceControl lInvoiceControl = new InvoiceControl();
        //        string lResult;
        //        if ((lResult = lInvoiceControl.LoadAndRenderInvoice((int)pState)) != "OK")
        //        {
        //            ViewBag.Message = lResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Display all the exceptions

        //        Exception CurrentException = ex;
        //        int ExceptionLevel = 0;
        //        do
        //        {
        //            ExceptionLevel++;
        //            ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ProcessInvoice", "");
        //            CurrentException = CurrentException.InnerException;
        //        } while (CurrentException != null);
        //    }
        //}

        [HttpGet]
        public ActionResult Pay()
        {
            try
            {
                return View("Pay");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Pay", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return View("Empty");
            }
        }

        [HttpGet]
        public ActionResult PayEFT()
        {
            try
            {
                return View("PayEFT");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Pay", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return View("Empty");
            }
        }
    }
}