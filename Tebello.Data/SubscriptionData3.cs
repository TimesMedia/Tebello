using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace Subs.Data
{
    #region Structs

    public class MimsValidationResult
    {
        public string Message;
        public bool Prompt = false;
    }

    #endregion

    public class SubscriptionData3 : BaseModel
    {
        #region Global variables

        //private Subs.Data.AdministrationData gAdministrationData;
        private readonly Subs.Data.SubscriptionDoc3 gDoc = new SubscriptionDoc3();
        private SubscriptionDoc3.SubscriptionDataTable gSubscriptionTable = new SubscriptionDoc3.SubscriptionDataTable();
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter gSubscriptionAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();
        private readonly Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter gSubscriptionIssuesAdapter = new SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
        private readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);
        private PayerDerived gPayerDerived;
        private ReceiverDerived gReceiverDerived;



        #endregion

        #region Constructors

        public SubscriptionData3()
        {
            if (!GeneralConstruction())
            {
                return;
            }

            gSubscriptionTable.Clear(); //Start with a clean slate
            gSubscriptionTable.AcceptChanges(); // Do not attempt to reconcile with the database

            Subs.Data.SubscriptionDoc3.SubscriptionRow lRow = gSubscriptionTable.NewSubscriptionRow();
            gSubscriptionTable.AddSubscriptionRow(lRow);
        }

        public SubscriptionData3(int pSubscriptionId, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                if (!GeneralConstruction())
                {
                    return;
                }

                gSubscriptionTable.Clear(); //Start with a clean slate
                gSubscriptionAdapter.FillById(gSubscriptionTable, "BySubscription", pSubscriptionId, 0);

                if (gSubscriptionTable.Rows.Count == 0)
                {
                    ExceptionData.WriteException(5, "Caller information", sourceFilePath, memberName, sourceLineNumber.ToString());
                    throw new Exception("There is no subscription with this Id");
                }

                gPayerDerived = gDataContext.MIMS_SubscriptionData_DerivedPayer_FillById(PayerId).Single();
                gReceiverDerived = gDataContext.MIMS_SubscriptionData_DerivedReceiver_FillById(ReceiverId).Single();

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SubscriptionData3(SubscriptionId)", "SubscriptionId = " + pSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return;
            }
        }

        private bool GeneralConstruction()
        {
            try
            {
                //gAdministrationData = new AdministrationData();
                gSubscriptionAdapter.AttachConnection();
                gSubscriptionIssuesAdapter.AttachConnection();
                gSubscriptionTable = gDoc.Subscription;

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GeneralConstruction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return false;
            }
        }

        #endregion

        #region Housekeeping methods

        public bool SetBaseRateVatPercentage(DateTime StartDate)
        {
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);

            try
            {
                SqlCommand Command = new SqlCommand();
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.SubscriptionData.SetBaseRateVatPercentage]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@PayerId"].Value = this.PayerId;
                Command.Parameters["@ReceiverId"].Value = this.ReceiverId;
                Command.Parameters["@ProductId"].Value = this.ProductId;
                Command.Parameters["@StartDate"].Value = StartDate;

                // Check for Base rate

                Command.Parameters["@Type"].Value = "BaseRate";
                decimal? Result = (decimal?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    BaseRate = (decimal)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }
                // Check for VAT

                List<int> lVatCountries = new List<int>() { 61, 47, 38, 70 }; //RSA, Namibia, Lesotho, Swaziland 

                if (lVatCountries.Contains((int)ReceiverCountryId))
                {
                    Command.Parameters["@Type"].Value = "Vat";
                    decimal? Result2 = (decimal?)Command.ExecuteScalar();
                    if (Result2 != null && Result2.HasValue)
                    {
                        VatPercentage = (decimal)Result2;
                    }
                    else
                    {
                        throw new Exception("The query did not return any scalar");
                    }
                }
                else
                {
                    VatPercentage = 0;
                }

                return true;

            }

            catch (Exception ex)
            {
                ExceptionData.WriteException(1, ex.Message, this.ToString(), "SetBaseRateVatPercentage", "");
                throw new Exception(this.ToString() + " : " + "SetBaseRateVatPercentage" + " : ", ex);
            }
            finally
            {
                lConnection.Close();
            }

        }



        public bool UpdateInTransaction(ref SqlTransaction myTransaction)
        {
            try
            {

                // We use the tool generated adapter, because it is easier, and because it automatically
                // updates the id of newly inserted rows in the dataset.
                // However, to get it to run in a transaction, we have to supplement the adapter via a partial 
                // class, in particular by adding the AttachTransaction method. Because it is an a partial class,
                // it is not interfered with by the design tool that generates the adapter.
                Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter myAdaptor = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();
                myAdaptor.AttachTransaction(ref myTransaction);

                string rowstate = gSubscriptionTable[0].RowState.ToString();
                gSubscriptionTable[0].ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                gSubscriptionTable[0].ModifiedOn = DateTime.Now;
                myAdaptor.Update(gSubscriptionTable[0]);
                gSubscriptionTable[0].AcceptChanges();
                return true;
            } // End try
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "UpdateInTransaction", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool Update()
        {
            try
            {
                //Mark the update status
                gSubscriptionTable[0].ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                gSubscriptionTable[0].ModifiedOn = DateTime.Now;


                //Ultimately we want can update only the subscription table.

                //SubscriptionDoc3.SubscriptionDataTable lSubscription = new SubscriptionDoc3.SubscriptionDataTable();
                Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter lSubscriptionTableAdapter = new SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();
                lSubscriptionTableAdapter.AttachConnection();

                //object[] lSourceArray = gSubscription[0].ItemArray;

                // Update the table

                try
                {
                    lSubscriptionTableAdapter.Update(gSubscriptionTable[0]);
                    gSubscriptionTable[0].AcceptChanges();
                }
                catch (System.Data.DBConcurrencyException ex)
                {
                    throw new Exception("Concurrency violation on subscriptionid = " + gSubscriptionTable[0].SubscriptionId.ToString() + ex.Message);
                }

                gSubscriptionTable[0].AcceptChanges();
                return true;
            } // End try

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        #endregion

        #region Properties updateable

        public int SubscriptionId
        {
            get
            { return gSubscriptionTable[0].SubscriptionId; }

            set
            { gSubscriptionTable[0].SubscriptionId = value; }
        }

        public int PayerId
        {
            get
            {
                return gSubscriptionTable[0].PayerId;
            }

            set
            {
                if (value == 0)
                {
                    // Allow for this value to be reset.
                    gSubscriptionTable[0].PayerId = 0;
                    return;
                }


                if (gSubscriptionTable[0].PayerId == 0)
                {
                    // OK this could be a new subscription
                    gSubscriptionTable[0].PayerId = value;
                    gPayerDerived = gDataContext.MIMS_SubscriptionData_DerivedPayer_FillById(PayerId).Single();

                }
                else
                {
                    if (gSubscriptionTable[0].PayerId != value)
                    {
                        if (Status != Data.SubStatus.Proposed)
                        {
                            // The payer is immutable, it cannot be changed once the subscription exists
                            string myMessage = "You cannot change the payer of existing subscription "
                                + gSubscriptionTable[0].SubscriptionId.ToString() + " to " + value.ToString();
                            throw new Exception(myMessage);
                        }
                        else
                        {
                            gSubscriptionTable[0].PayerId = value;
                            gPayerDerived = gDataContext.MIMS_SubscriptionData_DerivedPayer_FillById(PayerId).Single();
                            NotifyPropertyChanged("PayerSurname");
                            NotifyPropertyChanged("PayerCompany");
                            NotifyPropertyChanged("PayerCountryId");
                        }
                    }
                }
            }
        }

        public int ReceiverId
        {
            get
            {
                return gSubscriptionTable[0].ReceiverId;
            }

            set
            {
                gSubscriptionTable[0].ReceiverId = value;
 
                gReceiverDerived = gDataContext.MIMS_SubscriptionData_DerivedReceiver_FillById(ReceiverId).Single();
                NotifyPropertyChanged("ReceiverSurname");
                NotifyPropertyChanged("ReceiverCompany");
                NotifyPropertyChanged("ReceiverCountryId");
 

                //CustomerData3 lCustomerData = new CustomerData3(value);
                //gSubscriptionTable[0].ReceiverSurname = lCustomerData.Surname;
                //NotifyPropertyChanged("ReceiverSurname");
                //gSubscriptionTable[0].ReceiverCompany = lCustomerData.CompanyNaam;
                //NotifyPropertyChanged("ReceiverCompany");
                //gSubscriptionTable[0].ReceiverCountry = lCustomerData.CountryId;
                //NotifyPropertyChanged("ReceiverCountry");
            }
        }

        public int? InvoiceId
        {
            get
            {
                if (!gSubscriptionTable[0].IsInvoiceIdNull())
                {
                    return gSubscriptionTable[0].InvoiceId;
                }
                else
                {
                    return null;
                }

            }


            set
            {
                if (value == null)
                {
                    gSubscriptionTable[0].SetInvoiceIdNull();
                }
                else
                {
                    gSubscriptionTable[0].InvoiceId = (int)value;
                }
            }
        }

        public int? ProFormaId
        {
            get
            {
                if (!gSubscriptionTable[0].IsProFormaIdNull())
                {
                    return gSubscriptionTable[0].ProFormaId;
                }
                else
                {
                    return null;
                }

            }


            set
            {
                if (value == null)
                {
                    gSubscriptionTable[0].SetProFormaIdNull();
                }
                else
                {
                    gSubscriptionTable[0].ProFormaId = (int)value;
                }
            }
        }

        public SubStatus Status
        {
            get
            {
                return (SubStatus)Enum.ToObject(typeof(SubStatus), gSubscriptionTable[0].Status);
            }

            set
            {
                gSubscriptionTable[0].Status = (int)value;
            }
        }

        public DeliveryMethod DeliveryMethod
        {
            get
            {
                return (DeliveryMethod)Enum.ToObject(typeof(DeliveryMethod), gSubscriptionTable[0].DeliveryMethod);
            }

            set
            {
                gSubscriptionTable[0].DeliveryMethod = (int)value;
                NotifyPropertyChanged("DeliveryMethod");
                NotifyPropertyChanged("DeliveryMethodInt");
                NotifyPropertyChanged("DeliveryMethodString");
            }
        }

        public int DeliveryMethodInt
        {
            get
            {
                return gSubscriptionTable[0].DeliveryMethod;
            }

            set
            {
                gSubscriptionTable[0].DeliveryMethod = value;
                NotifyPropertyChanged("DeliveryMethod");
                NotifyPropertyChanged("DeliveryMethodInt");
                NotifyPropertyChanged("DeliveryMethodString");
            }
        }

        public int UnitsPerIssue
        {
            get
            { return gSubscriptionTable[0].UnitsPerIssue; }

            set
            {
                if (value <= 0)
                {
                    throw new Exception("There should be at least one unit per issue");
                }

                gSubscriptionTable[0].UnitsPerIssue = value;
                NotifyPropertyChanged("UnitsPerIssue");
            }
        }

        public int NumberOfIssues
        {
            get
            { return gSubscriptionTable[0].NumberOfIssues; }

            set
            {
                gSubscriptionTable[0].NumberOfIssues = value;
                NotifyPropertyChanged("NumberOfIssues");
            }
        }

        public int ProductId
        {
            get
            {
                return gSubscriptionTable[0].ProductId;
            }

            set
            {
                gSubscriptionTable[0].ProductId = value;
            }
        }

        public decimal BaseRate
        {
            get
            { return gSubscriptionTable[0].BaseRate; }

            set
            { gSubscriptionTable[0].BaseRate = value; }
        }

        public decimal DeliveryCost
        {
            get
            {
                if (gSubscriptionTable[0].IsDeliveryCostNull())
                    return 0;
                else
                    return gSubscriptionTable[0].DeliveryCost;
            }

            set
            {
                gSubscriptionTable[0].DeliveryCost = value;
                NotifyPropertyChanged("DeliveryCost");
            }
        }

        public decimal VatPercentage
        {
            get
            { return gSubscriptionTable[0].VatPercentage; }

            set
            { gSubscriptionTable[0].VatPercentage = value; }
        }

        public decimal Vat
        {
            get
            { return gSubscriptionTable[0].Vat; }

            set
            {
                gSubscriptionTable[0].Vat = value;
                NotifyPropertyChanged("Vat");
            }
        }

        public decimal UnitPrice
        {
            get
            { return gSubscriptionTable[0].UnitPrice; }

            set
            {
                gSubscriptionTable[0].UnitPrice = value;
                NotifyPropertyChanged("UnitPrice");
            }
        }

        public decimal DiscountMultiplier
        {
            get
            { return gSubscriptionTable[0].DiscountMultiplier; }

            set
            { gSubscriptionTable[0].DiscountMultiplier = value; }
        }

        public int? DeliveryAddressId
        {
            get
            {
                if (gSubscriptionTable[0].IsDeliveryAddressIdNull())
                {
                    return null;
                }
                else
                {
                    return gSubscriptionTable[0].DeliveryAddressId;
                }
            }

            set
            {
                if (value.HasValue)
                {
                    gSubscriptionTable[0].DeliveryAddressId = (int)value;
                }

            }
        }

        public string OrderNumber
        {
            get
            {
                if (gSubscriptionTable[0].IsOrderNumberNull())
                { return "No order number"; }
                else
                {
                    return gSubscriptionTable[0].OrderNumber;
                }
            }
            set
            { gSubscriptionTable[0].OrderNumber = value; }
        }

        public bool RenewalNotice
        {
            get
            { return gSubscriptionTable[0].RenewalNotice; }

            set
            { gSubscriptionTable[0].RenewalNotice = value; }
        }

        public bool FreeDelivery
        {
            get
            { return gSubscriptionTable[0].FreeDelivery; }

            set
            { gSubscriptionTable[0].FreeDelivery = value; }
        }

        public bool AutomaticRenewal
        {
            get
            { return gSubscriptionTable[0].AutomaticRenewal; }

            set
            { gSubscriptionTable[0].AutomaticRenewal = value; }
        }
        #endregion

        #region Non persistent fields
        // Additional information required to prime the capture screen controls
        // But this is not persistent data.
        public int ProposedStartIssue = 0;
        public int ProposedStartSequence = 0; // Used to calculate the initial LastIssue 
        public int ProposedLastSequence;
        public int ProposedLastIssue;
        public bool gReadyToSubmit = false;

        #endregion

        #region Properties - derived

        public string ProposedStartIssueName
        {
            get
            {
                return ProductDataStatic.GetIssueDescription(ProposedStartIssue);
            }
        }

        public string ProposedLastIssueName
        {
            get
            {
                return ProductDataStatic.GetIssueDescription(ProposedLastIssue);
            }
        }

        public decimal Weight { get; set; }

        public string DeliveryMethodString
        {
            get
            {
                return Enum.GetName(typeof(DeliveryMethod), DeliveryMethodInt);
            }
        }

        public string StatusString
        {
            get
            {
                return Enum.GetName(typeof(SubStatus), gSubscriptionTable[0].Status);
            }
        }



        public string PayerSurname
        {
            get
            {
                return gPayerDerived.PayerSurname;
            }
        }

        public string PayerCompany
        {
            get
            {
                return gPayerDerived.PayerCompany;
            }
        }

        public int PayerCountryId
        {
            get
            {
                return gPayerDerived.PayerCountryId;
            }
        }

        public string ReceiverSurname
        {
            get
            {
                return gReceiverDerived.ReceiverSurname;
            }
        }

        public string ReceiverCompany
        {
            get
            {
                return gReceiverDerived.ReceiverCompany;
            }
        }

        public int ReceiverCountryId
        {
            get
            {
                return gReceiverDerived.ReceiverCountryId;
            }
           
        }

        public string ProductName
        {
            get
            {
                return ProductDataStatic.GetProductName(ProductId);
            }
        }

        public int StartIssue
        {
            get
            {
                Subs.Data.SubscriptionDoc3.SubscriptionIssueDataTable lSubscriptionIssue = new SubscriptionDoc3.SubscriptionIssueDataTable();
                lSubscriptionIssue.Clear();
                gSubscriptionIssuesAdapter.FillById(lSubscriptionIssue, SubscriptionId, "BySubscription");
                return lSubscriptionIssue.First().IssueId;
            }
        }

        public string StartIssueDescription
        {
            get
            {
                return ProductDataStatic.GetIssueDescription(StartIssue);
            }
        }

        public int LastIssue
        {
            get
            {
                Subs.Data.SubscriptionDoc3.SubscriptionIssueDataTable lSubscriptionIssue = new SubscriptionDoc3.SubscriptionIssueDataTable();
                lSubscriptionIssue.Clear();
                gSubscriptionIssuesAdapter.FillById(lSubscriptionIssue, SubscriptionId, "BySubscription");
                return lSubscriptionIssue.Last().IssueId;
            }
        }

        public string LastIssueDescription
        {
            get
            {
                return ProductDataStatic.GetIssueDescription(LastIssue);
            }
        }


        public int NextIssue
        {
            get
            {
                // This is the issue for the next renewal of this subscription
                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                MIMS_SubscriptionData_NextIssueResult lResult = lContext.MIMS_SubscriptionData_NextIssue(LastIssue).Single();
                return (lResult.Column1);
            }
        }

        public decimal AdjustedBaseRate
        {
            get
            {
                return BaseRate * DiscountMultiplier;
            }
        }

        public decimal DiscountPerUnit
        {
            get
            {
                return BaseRate - AdjustedBaseRate;
            }
        }
        public int NumberOfUnits
        {
            get { return UnitsPerIssue * NumberOfIssues; }
        }

        public decimal TotalCost
        {
            get { return UnitPrice * NumberOfUnits; }
        }

        public string CreditNoteName
        {

            get
            {
                if (CreditNoteValue < 0)
                {
                    return gDataContext.MIMS_SubscriptionData_CreditNote(SubscriptionId).Select(p => p.CreditNoteName).Single();
                }
                else return "";
            }
        }


        public decimal CreditNoteValue
        {

            get
            {
                List<CreditNote> lCreditNote = gDataContext.MIMS_SubscriptionData_CreditNote(SubscriptionId).ToList();
                if (lCreditNote.Count() == 1)
                {
                    return lCreditNote[0].CreditNoteValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion


        #region Static Methods

        public static bool RegisterListDelivery(int pSubscriptionId, int pIssueId)
        {
            try
            {
                Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter lAdapter = new SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
                lAdapter.AttachConnection();
                lAdapter.RegisterDelivery(pSubscriptionId, pIssueId);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionData3", "RegisterListDelivery", "SubscriptionId = " + pSubscriptionId.ToString() + " IssueId = " + pIssueId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }


        }

        #endregion
    }
}
