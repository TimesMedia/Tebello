
using Newtonsoft.Json;
using System.Web;
using Tebello.MimsWeb.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Subs.Data;
using Subs.Business;
using System.Windows.Data;

namespace Tebello.MimsWeb.Controllers
{
    public class PayUController : Controller
    {
        private PaymentDoc.PayUResultDataTable gPayUResultTable = new PaymentDoc.PayUResultDataTable();
        private Subs.Data.PaymentDocTableAdapters.PayUResultTableAdapter gPayUResultAdapter = new Subs.Data.PaymentDocTableAdapters.PayUResultTableAdapter();


        private static string lPayUUrl = "https://secure.payu.co.za";  //"https://staging.payu.co.za";
        private static string serviceAPI = "/service/PayUAPI?wsdl";
        private static string redirectAPI = "/rpp.do?PayUReference=";
        private static string SafeKey = "{DD8F8C7B-9245-471F-9E24-8086BCFFFEBE}"; //{07F70723-1B96-4B97-B891-7BF708594EEA}";

        public PayUController()
        {
            gPayUResultAdapter.AttachConnection();
        }


        public ActionResult Index(string orderid)
        {
            LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
            CustomerData3 lCustomerData = new CustomerData3((int)lLoginRequest.CustomerId);
            Basket lBasket = SessionHelper.GetBasket(Session);





            //****************************************************************
            //LoginRequest lLoginRequest = new LoginRequest();
            //CustomerData3 lCustomerData = new CustomerData3(118957);
            //Basket lBasket = new Basket();
            //lBasket.InvoiceId = 000000;
            //lBasket.TotalDiscountedPrice = 0.05M;

            //****************************************************************

            try
            { 
                string _url = lPayUUrl + serviceAPI; //web service url
                string lPayUReturnUrl = "https://subs.mims.co.za";

                string soapContent = ""; //soap content

                soapContent = "<ns1:setTransaction>";
                soapContent += "<Api>ONE_ZERO</Api>";
                soapContent += "<Safekey>" + SafeKey + "</Safekey>";

                //Payment type   
                soapContent += "<TransactionType>PAYMENT</TransactionType>";

                // Additional
                soapContent += "<AdditionalInformation>";
                soapContent += "<redirectChannel>responsive</redirectChannel>";
                // The cancel url for redirect shuold be configured accordingly
 
                soapContent += "<merchantReference>" + "INV" + lBasket.InvoiceId.ToString() + "</merchantReference>";

                // The return url for redirect should be configured accordingly
                soapContent += "<cancelUrl>" + lPayUReturnUrl + "/PayU/Cancelled</cancelUrl>";
                soapContent += "<returnUrl>" + lPayUReturnUrl + "/PayU/PayUTransactionResult</returnUrl>";
                soapContent += "<notificationUrl>" + lPayUReturnUrl + "/PayU/Notified</notificationUrl>";
                soapContent += "<secure3d>true</secure3d>";

                soapContent += "<supportedPaymentMethods>CREDITCARD</supportedPaymentMethods>";
                soapContent += "</AdditionalInformation>";
                

                //Customer
                soapContent += "<Customer>";
                soapContent += "<merchantUserId>" + lCustomerData.CustomerId + "</merchantUserId>";
                soapContent += "<email>" + lCustomerData.EmailAddress + "</email>";
                soapContent += "<firstName>" + lCustomerData.FirstName + "</firstName>";
                soapContent += "<lastName>" + lCustomerData.Surname + "</lastName>";
                soapContent += "<mobile>" + lCustomerData.PhoneNumber + "</mobile>";
                soapContent += "</Customer>";

                // Basket
                soapContent += "<Basket>";
                soapContent += "<amountInCents>" + Decimal.ToInt32(lBasket.TotalDiscountedPrice * 100) + "</amountInCents>";
                soapContent += "<currencyCode>ZAR</currencyCode>";
                soapContent += "<description>Order No: " + "INV" + lBasket.InvoiceId.ToString() + "</description>";
                soapContent += "</Basket>";
                soapContent += "</ns1:setTransaction>";
                // construct soap object

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(soapContent);

                // create username and password namespace
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
                nsmgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");

                // set soap username
                XmlNode userName = soapEnvelopeXml.SelectSingleNode("//wsse:Username", nsmgr);
                userName.InnerText = "201538";// "200021";

                // set soap password
                XmlNode userPassword = soapEnvelopeXml.SelectSingleNode("//wsse:Password", nsmgr);
                userPassword.InnerText = "s5aOPvCp"; //      "WSAUFbw6";


                // construct web request object
                HttpWebRequest webRequest = CreateWebRequest(_url);

                // insert soap envelope into web request
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // get the PayU reference number from the completed web request.
                string soapResult;
                using (WebResponse webResponse = webRequest.GetResponse())
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                //***************************************************************************************

                // Analyse the result for success.

                // create an empty soap result object
                XmlDocument soapResultXml = new XmlDocument();
                soapResultXml.LoadXml(soapResult.ToString());

                string lSuccess = soapResultXml.SelectSingleNode("//successful").InnerText;
                if (lSuccess == "true")
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append(lPayUUrl + redirectAPI);

                    // retrieve payU reference & build url with payU reference query string
                    string lPayUReference = soapResultXml.SelectSingleNode("//payUReference").InnerText;
                    builder.Append(lPayUReference);

                    //Subs.MimsWeb.SessionHelper.Set(Session, SessionKey.PayUReference, lPayUReference);

                    // Redirect to payU site - so that use can enter his creditcard details etc.
                    return Redirect(builder.ToString());
                }
                else
                { 
                    ViewBag.Message = soapResult.ToString();
                    return View();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Index", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public ViewResult PayUTransactionResult(string PayUReference)
        {
            PayUResult lPayUResult = new PayUResult();
            string lStage = "start";

            PaymentDoc.PayUResultRow lNewRow = gPayUResultTable.NewPayUResultRow();

            try
            { 
                string _url = lPayUUrl + serviceAPI; //web service url         

                string soapContent = ""; //soap content

                soapContent = "<ns1:getTransaction>";
                soapContent += "<Api>ONE_ZERO</Api>";
                soapContent += "<Safekey>" + SafeKey + "</Safekey>";

                // Additional
                soapContent += "<AdditionalInformation>";
                soapContent += "<payUReference>" + PayUReference + "</payUReference>";
                soapContent += "</AdditionalInformation>";

                soapContent += "</ns1:getTransaction>";
                // construct soap object

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(soapContent);

                // create username and password namespace
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
                nsmgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");

                // set soap username
                XmlNode userName = soapEnvelopeXml.SelectSingleNode("//wsse:Username", nsmgr);
                userName.InnerText = "201538";

                // set soap password
                XmlNode userPassword = soapEnvelopeXml.SelectSingleNode("//wsse:Password", nsmgr);
                userPassword.InnerText = "s5aOPvCp";
            
                // construct web request object
                HttpWebRequest webRequest = CreateWebRequest(_url);

                // insert soap envelope into web request
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // get the PayU reference number from the completed web request.
                string soapResult;


                using (WebResponse webResponse = webRequest.GetResponse())
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                // create an empty soap result object
                XmlDocument soapResultXml = new XmlDocument();
                soapResultXml.LoadXml(soapResult.ToString());

                StringBuilder builder = new StringBuilder();

                builder.Append(lPayUUrl + redirectAPI);

                lStage = "Collect SoapResult";


                // Write this to a log

                lNewRow.Reference = PayUReference;
                lStage = "CustomerId";
                lNewRow.CustomerId = (int)SessionHelper.GetLoginRequest(Session).CustomerId;
                lNewRow.AmountPayed = Decimal.Parse(soapResultXml.SelectSingleNode("//amountInCents") == null ? "" : soapResultXml.SelectSingleNode("//amountInCents").InnerText)/100;
                lStage = "Currency";
                lNewRow.Currency = soapResultXml.SelectSingleNode("//currencyCode") == null ? "" : soapResultXml.SelectSingleNode("//currencyCode").InnerText;
                lNewRow.CardExpiry = soapResultXml.SelectSingleNode("//cardExpiry") == null ? "" : soapResultXml.SelectSingleNode("//cardExpiry").InnerText;
                lNewRow.CardNumber = soapResultXml.SelectSingleNode("//cardNumber") == null ? "" : soapResultXml.SelectSingleNode("//cardNumber").InnerText;
                lNewRow.GatewayReference = soapResultXml.SelectSingleNode("//gatewayReference") == null ? "" : soapResultXml.SelectSingleNode("//gatewayReference").InnerText;
                lNewRow.CardType = soapResultXml.SelectSingleNode("//information") == null ? "" : soapResultXml.SelectSingleNode("//information").InnerText;
                lStage = "NameOnCard";
                lNewRow.NameOnCard = soapResultXml.SelectSingleNode("//nameOnCard") == null ? "" : soapResultXml.SelectSingleNode("//nameOnCard").InnerText;
                lNewRow.PointOfFailure = soapResultXml.SelectSingleNode("//pointOfFailure") == null ? "" : soapResultXml.SelectSingleNode("//pointOfFailure").InnerText;
                lNewRow.ResultCode = soapResultXml.SelectSingleNode("//resultCode") == null ? "" : soapResultXml.SelectSingleNode("//resultCode").InnerText;
                lNewRow.ResultMessage = soapResultXml.SelectSingleNode("//resultMessage") == null ? "" : soapResultXml.SelectSingleNode("//resultMessage").InnerText;
                lNewRow.Successful = bool.Parse(soapResultXml.SelectSingleNode("//successful") == null ? "" : soapResultXml.SelectSingleNode("//successful").InnerText);
                lNewRow.TransactionState = soapResultXml.SelectSingleNode("//transactionState") == null ? "" : soapResultXml.SelectSingleNode("//transactionState").InnerText;
                lNewRow.ModifiedOn = DateTime.Now;

                lStage = "Just before adding row";

                gPayUResultTable.AddPayUResultRow(lNewRow);

                lStage = "Just before updating database";


                gPayUResultAdapter.Update(gPayUResultTable);

                if (lNewRow.ResultMessage == "Successful")
                {
                    lStage = "Create PaymentRecord";
                    // Construct a PaymentRecord for more validation
                    PaymentData.PaymentRecord lRecord = new PaymentData.PaymentRecord();
                    lRecord.CustomerId = lNewRow.CustomerId;
                    lRecord.Amount = lNewRow.AmountPayed;
                    lRecord.Date = DateTime.Now;
                    lRecord.PaymentMethod = (int)PaymentMethod.Creditcard;
                    lRecord.ReferenceTypeId = (int)ReferenceType.AllocationNumber; 
                    lRecord.ReferenceTypeString = Enum.GetName(typeof(ReferenceType), (int)(int)ReferenceType.AllocationNumber);
                    lRecord.Reference = PayUReference;

                    lStage = "Register the payment in MIMS";
  
                    string lResult2 = "NoResult";
                    int lPaymentTransactionId = 0;                      

                    lResult2 = CustomerBiz.Pay(ref lRecord, out lPaymentTransactionId);
                    lStage = lResult2;

                    if (lResult2 != "OK")
                    {
                        lStage = "lResult = " + lResult2;
                        ViewBag.Message = lResult2;
                        return View(lNewRow);
                    }

                    lStage = "Just before ViewBag.Message";
                    ViewBag.Message = "Payment processed successfully. Electronic products are delivered immediately.";

                    // Clear the basket
                    Basket lBasket = SessionHelper.GetBasket(Session);
                    lBasket.BasketItems.Clear();
                    lBasket.InvoiceId = 0;
                    lBasket.TotalDiscount = 0;
                    lBasket.TotalDiscountedPrice = 0;
                    lBasket.TotalPrice = 0;
                }
                else
                {
                    ViewBag.Message = "Your bank could not process the payment. Please make alternate payment arrangements.";
                }
               
                return View(lNewRow);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "PayUTransactionResult", lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;
                return View(lNewRow);
            }
        }

        public ActionResult Cancelled(string PayUReference)
        {
            try {
                // Write to ledger.

                //LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
                //Basket lBasket = SessionHelper.GetBasket(Session);
                //if (lLoginRequest.CustomerId != null & lBasket != null)
                //{
                //    LedgerData.PayU((int)lLoginRequest.CustomerId, PayUReference, "Cancelled", lBasket.InvoiceId, lBasket.TotalDiscountedPrice);
                //}

                // Report back to user.

                TempData["Message"] = "Your payment was cancelled. The PayUReference is " + PayUReference;

                return RedirectToAction("Pay","Promotion");

            }
            catch(Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Index", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;
                return View(); 
            }
        }

        public ActionResult Notified(string pPayUReference)
        {
            // Write to ledger.

            //LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
            //Basket lBasket = SessionHelper.GetBasket(Session);
            //LedgerData.PayU((int)lLoginRequest.CustomerId, pPayUReference, "Notified", lBasket.InvoiceId, lBasket.TotalDiscountedPrice);

            // Report back to user.

            ViewBag.Message = "Your payment was notified. The PayUReference is " + pPayUReference;
            return RedirectToAction("Pay", "Order");
        }

    
        #region SOAP Helpers

 
        static string _soapEnvelope =
                     @"<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' 
                                    xmlns:ns1='http://soap.api.controller.web.payjar.com/' 
                                    xmlns:ns2='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
                                    <SOAP-ENV:Header>
                                        <wsse:Security SOAP-ENV:mustUnderstand='1' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
                                            <wsse:UsernameToken wsu:Id='UsernameToken-9' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                                                <wsse:Username></wsse:Username>
                                                <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'></wsse:Password>
                                            </wsse:UsernameToken>
                                        </wsse:Security>
                                    </SOAP-ENV:Header>
                                     <SOAP-ENV:Body>
                                     </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

        /// <summary>
        /// Creates the HttpWebRequest object
        /// </summary>
        /// <returns>webRequest</returns>        
        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }


        /// <summary>
        /// Creates the SOAP envelope object
        /// </summary>
        /// <returns>soapEnvelopeXml</returns> 
        private static XmlDocument CreateSoapEnvelope(string content)
        {
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            sb.Insert(sb.ToString().IndexOf("</SOAP-ENV:Body>"), content);

            // create an empty soap envelope
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(sb.ToString());

            return soapEnvelopeXml;
        }

        /// <summary>
        /// Insert soap envelope into web request
        /// </summary>
        /// <returns></returns>
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        #endregion
      
    }
}
