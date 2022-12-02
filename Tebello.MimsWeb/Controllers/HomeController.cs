using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net.Mail;
using Subs.Data;
using Subs.Business;
using Tebello.MimsWeb.Models;
using System.Collections.Specialized;
using System.Text;

namespace Tebello.MimsWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Globals

        #endregion

        #region Constructor
        public HomeController()
        {
            //gCompanyAdapter.AttachConnection();
        }
        #endregion

        // GET: Home
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        // Contact *********************************************************************************************

        [HttpGet]
        public ViewResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Contact(ContactRequest pContactRequest)
        {
            try
            {
                string lSubject = "Contact request from " + pContactRequest.Name;
                string lBody = pContactRequest.ContactNumber + " " + pContactRequest.Message;

                if (CustomerBiz.SendEmail("", "vandermerwer@mims.co.za", lSubject, lBody) != "OK")
                {
                    ViewBag.Message = "There was a problem emailing the contact message";
                    return View("Empty");
                }

                ViewBag.Message = "Your message has been emailed to vandermerwer@mims.co.za.";

                return View();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Contact", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }



        // About *********************************************************************************************

        [HttpGet]
        public ViewResult About()
        {
            return View();
        }



        // Register a user *********************************************************************************************

        [HttpGet]
        public ActionResult RegisterEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterEmail(LoginRequest pLoginRequest)
        {
            try
            {
                LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);

                if (lLoginRequest.CustomerId != null)
                {
                    ViewBag.Message = "This session is already logged in, not to mention registered.";
                    return View();
                }

                if (Regex.IsMatch(pLoginRequest.Email, @"\.@|@\."))
                {
                    ViewBag.Message = "This is not a valid Email address.";
                    return View();
                }

                if (!Regex.IsMatch(pLoginRequest.Email, @"^([&\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$"))
                {
                    ViewBag.Message = "This is not a valid Email address.";
                    return View();
                }

                // See if there is not already a user on that Email Address

                if (CustomerData3.ExistsByEmail(pLoginRequest.Email) > 0)
                {
                    ViewBag.Message = "There are already users registered on that email address. Please contact MIMS to create your account for you.";
                    return View();
                }

                // Ask person to check his email to set or reset his password

                Guid lGuid = AdministrationData2.GetGuid("ActivationToken", pLoginRequest.Email);

                // Send Email. 

                if (lGuid == Guid.Empty)
                {
                    ViewBag.Message = "There was an error in generating an activation token.";
                    return View();
                }

                if (!SendPromptForAccountDetails(lGuid, pLoginRequest.Email))
                {
                    ViewBag.Message = "There was an error in sending an activation token.";
                    return View();
                }

                ViewBag.Message = "Please check your email to complete the registration.";
                return View();

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "RegisterEmail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = "Error in RegisterEmail" + "" + ex.Message;
                return View();
            }
        }

        private bool SendPromptForAccountDetails(Guid pGuid, string pDestination)
        {
            try
            {
                string lBody = "Please follow this link:" + "172.15.87.252:8003/home/InitialAccount?pGuid=" + pGuid;

                if (CustomerBiz.SendEmail("", pDestination, "EMail registration", lBody) != "OK")
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SendEmailRegisterEmail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message(ex.Message);
                return false;
            }
        }


        public ActionResult Test()
        {
            LoginRequest lLoginRequest = new LoginRequest() { Email = "heinreitmann@gmail.com" };
            SessionHelper.Set(Session, SessionKey.LoginRequest, lLoginRequest);
            return RedirectToAction("Account");
        }


        [HttpGet]
        public ActionResult InitialAccount(Guid pGuid)
        {
            try
            {
                //  Validate the RegisterToken

                AdministrationDoc.GUIDTableDataTable lGUIDTable = new AdministrationDoc.GUIDTableDataTable();
                if (!AdministrationData2.FillGUIDByGUID(pGuid, ref lGUIDTable))
                {
                    throw new Exception("FillGUIDbyGUID failed.");
                }

                if (lGUIDTable.Count() <= 0)
                {
                    throw new Exception("Invalid GUID");
                }

                // Capture the account details

                LoginRequest lLoginRequest = new LoginRequest() { Email = lGUIDTable[0].Email };
                SessionHelper.Set(Session, SessionKey.LoginRequest, lLoginRequest);

                return RedirectToAction("Account");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "InitialAccount", "Guid = " + pGuid.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);


                ViewBag.Message = ex;
                return View("Error");
            }
        }


        private bool PrimeValidValues(UserTemplate pUserTemplate, int pClassificationId1 = 9, int? pClassificationId2 = 23)
        {
            try
            {
                List<ListItem> lListItems = new List<ListItem>();

                foreach (int i in Enum.GetValues(typeof(Title)))
                {
                    ListItem lNewItem = new ListItem();
                    lNewItem.Key = i;
                    lNewItem.Value = Enum.GetName(typeof(Title), i);
                    lListItems.Add(lNewItem);
                }

                SelectList lTitleList = new SelectList(lListItems, "Key", "Value", pUserTemplate.TitleId);
                pUserTemplate.TitleSelectList = lTitleList;

                var lCountryQuery = from lRow in AdministrationData2.Country
                                    select new ListItem { Key = lRow.CountryId, Value = lRow.CountryName };

                SelectList lCountryList = new SelectList((IEnumerable<ListItem>)lCountryQuery.ToList(), "Key", "Value", pUserTemplate.CountryId);

                pUserTemplate.CountrySelectList = lCountryList;

                List<ListItem> lClassification1Items = new List<ListItem>();
                lClassification1Items = AdministrationData2.Classification.Where(p => p.ParentId == 1).Select(q => new ListItem() { Key = q.ClassificationId, Value = q.Classification }).ToList();
                pUserTemplate.Classification1SelectList = new SelectList(lClassification1Items, "Key", "Value", 1);

                List<ListItem> lClassification2Items = new List<ListItem>();
                lClassification2Items = AdministrationData2.Classification.Where(p => p.ParentId == pClassificationId1).Select(q => new ListItem()
                {
                    Key = q.ClassificationId,
                    Value = q.Classification
                }).ToList();

                pUserTemplate.Classification2SelectList = new SelectList(lClassification2Items, "Key", "Value", 1);

                pUserTemplate.ClassificationId1 = pClassificationId1;
                if (pClassificationId2.HasValue)
                {
                    pUserTemplate.ClassificationId2 = pClassificationId2.Value;
                }

                return true;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "PrimeValidValues", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }


        [HttpGet]
        public ActionResult Account()
        {
            UserTemplate lUserTemplate = new UserTemplate();
            CustomerData3 lCustomerData = new CustomerData3();
            string lStage = "Start";

            try
            {
                // Populate initial values

                LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
                if (lLoginRequest.CustomerId == null)
                {
                    // This s a new customer 
                    if (!PrimeValidValues(lUserTemplate))
                    {
                        ViewBag.Message = "Something went wrong with the initialisation of the UserTemplate.";
                        return View("Account");
                    }

                    lUserTemplate.EmailAddress = lLoginRequest.Email;
                    lUserTemplate.CountryId = 61; // Default
                }
                else
                {
                    // This is an existing customer

                    lCustomerData = new CustomerData3((int)lLoginRequest.CustomerId);

                    // Map the values

                    if (!PrimeValidValues(lUserTemplate, lCustomerData.ClassificationId1, lCustomerData.ClassificationId2))
                    {
                        ViewBag.Message = "Something went wrong with the initialisation of the UserTemplate.";
                        return View("Account");
                    }

                    // lUserTemplate.Password = lCustomerData.Password;
                    lUserTemplate.CustomerId = lCustomerData.CustomerId;
                    lUserTemplate.TitleId = lCustomerData.TitleId;
                    lUserTemplate.Initials = lCustomerData.Initials;
                    lUserTemplate.FirstName = lCustomerData.FirstName;
                    lUserTemplate.Surname = lCustomerData.Surname;
                    lUserTemplate.EmailAddress = lCustomerData.EmailAddress;
                    lUserTemplate.PhoneNumber = lCustomerData.PhoneNumber;
                    lUserTemplate.CellPhoneNumber = lCustomerData.CellPhoneNumber;

                    lStage = "Company";

                    lUserTemplate.CompanyName = lCustomerData.CompanyName == "" ? lCustomerData.CompanyNameUnverified : lCustomerData.CompanyName;

                    lUserTemplate.CountryId = lCustomerData.CountryId;

                    lUserTemplate.CouncilNumber = lCustomerData.CouncilNumber;

                    lStage = "Physical Address";

                    if (lCustomerData.PhysicalAddressId != null)
                    {
                        DeliveryAddressData2 lDeliveryAddressData = new DeliveryAddressData2((int)lCustomerData.PhysicalAddressId);
                        lUserTemplate.CountryId = lDeliveryAddressData.CountryId;
                        lUserTemplate.Province = lDeliveryAddressData.Province;
                        lUserTemplate.City = lDeliveryAddressData.City;
                        lUserTemplate.Suburb = lDeliveryAddressData.Suburb;
                        lUserTemplate.Street = lDeliveryAddressData.Street;
                        lUserTemplate.StreetExtension = lDeliveryAddressData.StreetExtension;
                        lUserTemplate.StreetSuffix = lDeliveryAddressData.StreetSuffix;
                        lUserTemplate.StreetNo = lDeliveryAddressData.StreetNo;
                        lUserTemplate.PostCode = lDeliveryAddressData.PostCode;
                        lUserTemplate.Building = lDeliveryAddressData.Building;
                        lUserTemplate.Floor = lDeliveryAddressData.FloorNo;
                        lUserTemplate.Room = lDeliveryAddressData.Room;
                    }
                }
                return View("Account", lUserTemplate);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Account get", "Stage = " + lStage + " " + lCustomerData.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Account(UserTemplate UserTemplate)
        {
            // Ensure, first of all, that all required fields have benn filled.
            if (!ModelState.IsValid)
            {
                // Give the user another chance to improve on his capture

                if (!PrimeValidValues(UserTemplate, UserTemplate.ClassificationId1, UserTemplate.ClassificationId2))
                {
                    ViewBag.Message = "Something went wrong with the initialisation of the UserTemplate.";
                    return View("Account", UserTemplate);
                }

                ViewBag.Message = "Sorry, some required fields have not been filled.";
                return View("Account", UserTemplate);
            }

            LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
            CustomerData3 lCustomerData = new CustomerData3();
            DeliveryAddressData2 lDeliveryAddress = new DeliveryAddressData2();

            try
            {
                // Initialise objects
                if (lLoginRequest.CustomerId.HasValue)
                {
                    lCustomerData = new CustomerData3((int)lLoginRequest.CustomerId);
                    if (lCustomerData.PhysicalAddressId != null)
                    {
                        lDeliveryAddress = new DeliveryAddressData2((int)lCustomerData.PhysicalAddressId);
                    }
                }

                // Capture DeliveryAddress.

                if (!CaptureDeliveryAddress())
                {
                    ViewBag.Message = "Something went wrong with the capture of the DeliveryAddress";
                    return View("Account", UserTemplate);
                }

                // Capture Customer info

                if (!CaptureCustomerData())
                {
                    ViewBag.Message = "Something went wrong with the capture of the CustomerData";
                    return View("Account", UserTemplate);
                }

                if (!ModelState.IsValid)
                {
                    //Record what is going wrong
                    foreach (ModelState item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            ExceptionData.WriteException(3, UserTemplate.EmailAddress, this.ToString(), "Account post", item.Value.AttemptedValue + " " + item.Errors[0].ErrorMessage);
                        }
                    }

                    // Give the user another chance to improve on his capture

                    if (!PrimeValidValues(UserTemplate, UserTemplate.ClassificationId1, UserTemplate.ClassificationId2))
                    {
                        ViewBag.Message = "Something went wrong with the initialisation of the UserTemplate.";
                        return View("Account", UserTemplate);
                    }

                    ViewBag.Message = "Sorry, some of the data are in error.";

                    return View("Account", UserTemplate);   // Here I want to invoke the Account view with the pUserTemplate again, plus the ModelState 
                }
                else
                {
                    // See if this is a duplicate entry, i.e. whether the customer exists already

                    if (lLoginRequest.CustomerId == null)
                    {
                        {
                            string lResult4;

                            if ((lResult4 = CustomerBiz.ValidateDuplicate(lCustomerData)) != "OK")
                            {
                                ViewBag.Message = lResult4;

                                return View("Empty", UserTemplate);
                            }
                        }
                    }
                }

                // Update the database

                string lResult;
                if ((lResult = lDeliveryAddress.Update()) != "OK")
                {
                    ViewBag.Message = lResult;

                    return View("Empty", UserTemplate);
                }


                lCustomerData.PhysicalAddressId = lDeliveryAddress.DeliveryAddressId;

                string lResult5;
                if ((lResult5 = lCustomerData.Update()) != "OK")
                {
                    ViewBag.Message = lResult5;
                    return View("Account", UserTemplate);
                }

                if (UserTemplate.ClassificationId2.HasValue)
                {
                    // I am assuming that if the second one changes, the first one will change automatically.

                    try
                    {

                        // You cannot do the next step if you do not have a custyomerid already
                        lCustomerData.ClassificationId2 = UserTemplate.ClassificationId2.Value;
                        // Once you have set it, you have to repeat the update
                        string lResult6;
                        if ((lResult6 = lCustomerData.Update()) != "OK")
                        {
                            ViewBag.Message = lResult6;
                            return View("Account", UserTemplate);
                        }

                        // Also update the many to many mapping 

                        {
                            string lResult7;

                            if ((lResult7 = DeliveryAddressData2.Link(lDeliveryAddress.DeliveryAddressId, lCustomerData.CustomerId)) != "OK")
                            {
                                ViewBag.Message = lResult7;
                                return View("Account", UserTemplate);
                            }
                        }
                    }
                    catch (Exception Ex) { ModelState.AddModelError("ClassificationId2", Ex.Message); }
                }

                if (lLoginRequest.CustomerId == null)
                {
                    //This is a new user, so log him on automatically

                    lLoginRequest.CustomerId = lCustomerData.CustomerId;
                    lLoginRequest.Email = lCustomerData.EmailAddress;
                    SessionHelper.Set(Session, SessionKey.LoginRequest, lLoginRequest);
                    ViewBag.Message = "Welcome " + lCustomerData.Title + " " + lCustomerData.Surname + ".You are now logged in. Please proceed.";

                    // Keep track of the creation of a new customer
                    LedgerData.InitialiseCustomer(lCustomerData);

                    return View("Empty");
                }

                ViewBag.Message = "Update of account data for customer " + lCustomerData.CustomerId.ToString() + " was successful.";
                return View("Empty");

                //***********************************************************************************************************************************************


                bool CaptureCustomerData()
                {
                    try
                    {
                        //Map all the fields, as applicable

                        if (UserTemplate.TitleId != 0)
                        {
                            try { lCustomerData.TitleId = UserTemplate.TitleId; }
                            catch (Exception Ex) { ModelState.AddModelError("TitleId", Ex.Message); }
                        }

                        if (UserTemplate.Initials != "")
                        {
                            try { lCustomerData.Initials = UserTemplate.Initials; }
                            catch (Exception Ex) { ModelState.AddModelError("Initials", Ex.Message); }
                        }

                        if (UserTemplate.FirstName != "")
                        {
                            try { lCustomerData.FirstName = UserTemplate.FirstName; }
                            catch (Exception Ex) { ModelState.AddModelError("FirstName", Ex.Message); }
                        }

                        if (UserTemplate.Surname != "")
                        {
                            try { lCustomerData.Surname = UserTemplate.Surname; }
                            catch (Exception Ex) { ModelState.AddModelError(nameof(lCustomerData.Surname), Ex.Message); }
                        }

                        if (UserTemplate.EmailAddress != "")
                        {
                            try { lCustomerData.EmailAddress = UserTemplate.EmailAddress; }
                            catch (Exception Ex) { ModelState.AddModelError("EmailAddress", Ex.Message); }
                        }

                        if (!String.IsNullOrWhiteSpace(UserTemplate.PhoneNumber))
                        {
                            try { lCustomerData.PhoneNumber = UserTemplate.PhoneNumber; }
                            catch (Exception Ex) { ModelState.AddModelError("PhoneNumber", Ex.Message); }
                        }

                        if (UserTemplate.CellPhoneNumber != "")
                        {
                            try { lCustomerData.CellPhoneNumber = UserTemplate.CellPhoneNumber; }
                            catch (Exception Ex) { ModelState.AddModelError("CellPhoneNumber", Ex.Message); }
                        }

                        if (UserTemplate.CompanyName != lCustomerData.CompanyName)
                        {
                            // The user changed the CompanyName
                            try
                            {
                                int lCompanyId = AdministrationData2.GetCompanyId(UserTemplate.CompanyName);

                                if (lCompanyId != 1)
                                {
                                    lCustomerData.CompanyId = lCompanyId;
                                    lCustomerData.CompanyNameUnverified = "";
                                }
                                else
                                {
                                    lCustomerData.CompanyId = 1;
                                    lCustomerData.CompanyNameUnverified = UserTemplate.CompanyName;
                                }
                            }
                            catch (Exception Ex) { ModelState.AddModelError("CompanyName", Ex.Message); }
                        }


                        if (UserTemplate.CountryId != 0)
                        {
                            try { lCustomerData.CountryId = UserTemplate.CountryId; }
                            catch (Exception Ex) { ModelState.AddModelError("CountryId", Ex.Message); }
                        }

                        if (!String.IsNullOrWhiteSpace(UserTemplate.Password))
                        {
                            if (UserTemplate.ConfirmedPassword != null)
                            {
                                if (UserTemplate.Password == UserTemplate.ConfirmedPassword)
                                {
                                    lCustomerData.Password1 = UserTemplate.Password;
                                }
                                else
                                {
                                    ModelState.AddModelError("Password", "The confirmed password does not match the original password.");
                                }
                            }
                        }

                        if (UserTemplate.CouncilNumber != "")
                        {
                            try { lCustomerData.CouncilNumber = UserTemplate.CouncilNumber; }
                            catch (Exception Ex) { ModelState.AddModelError("CouncilNumber", Ex.Message); }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        //Display all the exceptions

                        Exception CurrentException = ex;
                        int ExceptionLevel = 0;
                        do
                        {
                            ExceptionLevel++;
                            ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "local CaptureCustomerData", lCustomerData.CustomerId.ToString());
                            CurrentException = CurrentException.InnerException;
                        } while (CurrentException != null);

                        return false;
                    }

                }

                bool CaptureDeliveryAddress()
                {
                    string lStage = "PhoneNumber";
                    try
                    {

                        if (!String.IsNullOrWhiteSpace(UserTemplate.PhoneNumber))
                        {
                            try { lDeliveryAddress.PhoneNumber = UserTemplate.PhoneNumber; }
                            catch (Exception Ex) { ModelState.AddModelError("PhoneNumber", Ex.Message); }
                        }

                        lStage = "GetStreetId";

                        int? lStreetId = DeliveryAddressStatic.GetStreetId(UserTemplate.CountryId,
                            UserTemplate.Province,
                            UserTemplate.City,
                            UserTemplate.Suburb,
                            UserTemplate.Street,
                            UserTemplate.StreetExtension,
                            UserTemplate.StreetSuffix);

                        if (lStreetId != 0)
                        {
                            lDeliveryAddress.StreetId = lStreetId;
                        }

                        // If you have a StreetId, some of this is redundant, but I capture is anyway, for now.
                        lStage = "Country";

                        if (UserTemplate.CountryId != 0)
                        {
                            lDeliveryAddress.CountryId = UserTemplate.CountryId;
                        }

                        lStage = "Before Province";

                        if (UserTemplate.Province != "")
                        {
                            try { lDeliveryAddress.Province = UserTemplate.Province; }
                            catch (Exception Ex) { ModelState.AddModelError("Province", Ex.Message); }
                        }

                        if (UserTemplate.City != "")
                        {
                            try { lDeliveryAddress.City = UserTemplate.City; }
                            catch (Exception Ex) { ModelState.AddModelError("City", Ex.Message); }
                        }

                        if (UserTemplate.Suburb != "")
                        {
                            try { lDeliveryAddress.Suburb = UserTemplate.Suburb; }
                            catch (Exception Ex) { ModelState.AddModelError("Suburb", Ex.Message); }
                        }

                        if (UserTemplate.Street != "")
                        {
                            try { lDeliveryAddress.Street = UserTemplate.Street; }
                            catch (Exception Ex) { ModelState.AddModelError("Street", Ex.Message); }
                        }

                        lStage = "Before StreetExtension";
                        if (UserTemplate.StreetExtension != "")
                        {
                            try { lDeliveryAddress.StreetExtension = UserTemplate.StreetExtension; }
                            catch (Exception Ex) { ModelState.AddModelError("StreetExtension", Ex.Message); }
                        }

                        if (UserTemplate.StreetSuffix != "")
                        {
                            try { lDeliveryAddress.StreetSuffix = UserTemplate.StreetSuffix; }
                            catch (Exception Ex) { ModelState.AddModelError("StreetSuffix", Ex.Message); }
                        }

                        if (UserTemplate.StreetNo != "")
                        {
                            try { lDeliveryAddress.StreetNo = UserTemplate.StreetNo; }
                            catch (Exception Ex) { ModelState.AddModelError("StreetNo", Ex.Message); }
                        }

                        if (UserTemplate.PostCode != "")
                        {
                            try { lDeliveryAddress.PostCode = UserTemplate.PostCode; }
                            catch (Exception Ex) { ModelState.AddModelError("PostCode", Ex.Message); }
                        }

                        lStage = "Before Building";

                        if (UserTemplate.Building != "")
                        {
                            try { lDeliveryAddress.Building = UserTemplate.Building; }
                            catch (Exception Ex) { ModelState.AddModelError("Building", Ex.Message); }
                        }

                        if (UserTemplate.Floor != "")
                        {
                            try { lDeliveryAddress.FloorNo = UserTemplate.Floor; }
                            catch (Exception Ex) { ModelState.AddModelError("Floor", Ex.Message); }
                        }
                        if (UserTemplate.Room != "")
                        {
                            try { lDeliveryAddress.Room = UserTemplate.Room; }
                            catch (Exception Ex) { ModelState.AddModelError("Room", Ex.Message); }
                        }

                        return true;

                    }
                    catch (Exception ex)
                    {
                        //Display all the exceptions

                        Exception CurrentException = ex;
                        int ExceptionLevel = 0;
                        do
                        {
                            ExceptionLevel++;
                            ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "local CaptureDeliveryAddress", "Stage = " + lStage + " " + UserTemplate.EmailAddress);
                            CurrentException = CurrentException.InnerException;
                        } while (CurrentException != null);

                        return false;
                    }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Account post", " CustomerId = " + lCustomerData.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return View("Empty");
            }
        }

        public JsonResult Companies()
        {
            List<ListItem> lCompanyItems = new List<ListItem>();
            lCompanyItems = AdministrationData2.Company.Select(q => new ListItem()
            {
                Key = q.CompanyId,
                Value = q.CompanyName
            }).ToList();

            return Json(lCompanyItems, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Classifications(int? ParentClassificationId = 2)
        {
            List<ListItem> lClassification2Items = new List<ListItem>();
            lClassification2Items = AdministrationData2.Classification.Where(p => p.ParentId == ParentClassificationId).Select(q => new ListItem()
            {
                Key = q.ClassificationId,
                Value = q.Classification
            }).ToList();

            return Json(lClassification2Items, JsonRequestBehavior.AllowGet);
        }



        // Login **************************************************************************************************************

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest pLoginRequest)
        {
            CustomerData3 lCustomerData;
            LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);
            Subs.Data.CustomerDoc2TableAdapters.CustomerTableAdapter lAdapter = new Subs.Data.CustomerDoc2TableAdapters.CustomerTableAdapter();
            CustomerDoc2.CustomerDataTable lCustomerDataTable = new CustomerDoc2.CustomerDataTable();

            try
            {
                // See if the user is logged in already for this session

                if (lLoginRequest.CustomerId != null)
                {
                    ViewBag.Message = "You are logged in already. Welcome once again.";
                    return View();
                }

                if (!String.IsNullOrWhiteSpace(pLoginRequest.Email))
                {
                    lAdapter.AttachConnection();
                    lAdapter.FillById(lCustomerDataTable, "Email", 0, pLoginRequest.Email);
                    if (lCustomerDataTable.Count == 0)
                    {
                        ViewBag.Message = "There is no customer with that Email Address.";
                        return View();
                    }
                    else
                    {
                        StringBuilder lCustomers = new StringBuilder();
                        foreach (CustomerDoc2.CustomerRow item in lCustomerDataTable)
                        {
                            lCustomers.Append(item.FirstName + " " + item.Surname + ": Customer Number = " + item.CustomerId + "     ");
                        }
                        ViewBag.Message = "Enter one of these customer numbers: " + lCustomers;
                        return View();
                    }
                }

                if (pLoginRequest.CustomerId == null)
                {
                    ViewBag.Message = "If you cannot supply a Customer id. you might have to register first.";
                    return View();
                }

                if (!CustomerData3.Exists((int)pLoginRequest.CustomerId))
                {
                    ViewBag.Message = "You are not a registered as a customer on our database. Please register.";
                    return View();
                }

                lCustomerData = new CustomerData3((int)pLoginRequest.CustomerId);



                if (pLoginRequest.ResetFlag)
                {
                    if (SendResetPassword(lCustomerData))
                    {
                        ViewBag.Message = "Please check your email for a generated password.";
                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong with your request.";
                    }

                    return View("Login");
                }


                if (String.IsNullOrWhiteSpace(lCustomerData.Password1))
                {
                    if (SendResetPassword(lCustomerData))
                    {
                        ViewBag.Message = "You are an existing customer, but your password has not yet been set. Please check your email for a generated password.";
                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong with your request.";
                    }

                    return View("Login");
                }


                if (lCustomerData.Password1 == pLoginRequest.Password)
                {
                    pLoginRequest.Email = lCustomerData.EmailAddress;
                    pLoginRequest.CountryId = lCustomerData.CountryId;
                    SessionHelper.Set(Session, SessionKey.LoginRequest, pLoginRequest);

                    //ViewBag.User = lCustomerData.FirstName;
                    ViewBag.Message = "Welcome, " + lCustomerData.Title + " " + lCustomerData.Surname + ", you are now logged in.";
                    return View("Welcome");
                }
                else
                {
                    ViewBag.Message = "Unfortunately, your password did not match. Please try again.";
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Login post",
                                                 lLoginRequest.CustomerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = "Error in RequestLogin" + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            //if (SendResetPassword(lCustomerData))
            //{
            //    ViewBag.Message = "Please check your email for a generated password.";
            //}
            //else
            //{
            //    ViewBag.Message = "Something went wrong with your request.";
            //}
            return View("Login");
        }
        private bool SendResetPassword(CustomerData3 pCustomerData)
        {

            try
            {
                string lGeneratedPassword = "";
                int lKey = (3 * DateTime.Now.Second) + (4 * DateTime.Now.Millisecond);
                if (lKey > 500)
                {
                    lGeneratedPassword = pCustomerData.EmailAddress.Substring(2, 1).ToUpper() + lKey.ToString() + pCustomerData.EmailAddress.Substring(5, 1).ToLower();
                }
                else
                {
                    lGeneratedPassword = pCustomerData.EmailAddress.Substring(6, 1).ToUpper() + lKey.ToString() + pCustomerData.EmailAddress.Substring(4, 1).ToLower();
                }

                pCustomerData.Password1 = lGeneratedPassword;
                pCustomerData.Update();

                string lBody = "As requested, your password is: " + lGeneratedPassword + "   Once logged in, you can change it to something of closer to your preference. \nIn order to avoid typing errors, it is a good idea to cut and paste the password.";
                if (CustomerBiz.SendEmail("", pCustomerData.EmailAddress, "Mims", lBody) != "OK")
                {
                    return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                // Record the event in the Exception table
                ExceptionData.WriteException(1, Ex.Message, this.ToString(), "SendResetPassword", pCustomerData.CustomerId.ToString());
                return false;
            }
        }


        [HttpGet]
        public ActionResult CPD()
        {
            try
            {
                // Ensure that the person is logged in

                LoginRequest lLoginRequest = SessionHelper.GetLoginRequest(Session);

                if (lLoginRequest.CustomerId == null)
                {
                    ViewBag.Message = "Sorry, I cannot take you to CPD unless you are first logged in.";
                    return View("Empty");
                }


                return Redirect("https://www.mimscpd.co.za");

                //return View("Empty");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CPD", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                ViewBag.Message = ex.Message;
                return new ViewResult();
            }
        }


        // Signout **************************************************************************************************************

        [HttpGet]
        public void LogOut()
        {
            Session.Abandon();
            ViewBag.Message = "Thank you for shopping with us.";
        }
    }
}
