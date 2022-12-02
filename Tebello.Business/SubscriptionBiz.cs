using Subs.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Subs.Business
{
    public class SeatResult
    {
        public int Seats;
        public string Reason;
        public DateTime ExpirationDate;

    }

    public class AuthorizationResult
    {
        public int Year;
        public int Month;
        public int CustomerId;
        public int ProductId;
        public int Seats;
        public DateTime ExpirationDate;
    }

    public class PromotionCriteria
    {
        public int PayerId;
        public int ProductId;
        public int IssueId;
        public DateTime StartDate;
        public decimal DiscountPercentage;
    }



    public static class SubscriptionBiz
    {
        public static MimsValidationResult Validate(SubscriptionData3 pSubscriptionData)
        {
            SqlConnection lConnection = new SqlConnection(Settings.ConnectionString);

            try
            {
                if (string.IsNullOrWhiteSpace(pSubscriptionData.OrderNumber) && pSubscriptionData.Status == SubStatus.Deliverable)
                {
                    return new MimsValidationResult() { Message = "I cannot do anything without an order number!" };
                }

                if (pSubscriptionData.ReceiverId == 0 | pSubscriptionData.PayerId == 0)
                {
                    return new MimsValidationResult() { Message = "I need both a receiver and a payer id." };
                }

                if (pSubscriptionData.DeliveryCost < 0)
                {
                    return new MimsValidationResult() { Message = "Delivery cost cannot be a negative number" };
                }

                if (pSubscriptionData.ProductId == 0)
                {
                    return new MimsValidationResult() { Message = "I need a product id." };
                }

                if (pSubscriptionData.NumberOfIssues <= 0)
                {
                    return new MimsValidationResult() { Message = "I cannot accept number of issues to be less than one." };
                }

                if (pSubscriptionData.UnitsPerIssue <= 0)
                {
                    return new MimsValidationResult() { Message = "I cannot accept number of copies to be less than one." };
                }


                if (!ProductDataStatic.AllowMultipleCopies((int)pSubscriptionData.ProductId) && pSubscriptionData.UnitsPerIssue > 1)
                {
                    return new MimsValidationResult() { Message = "I allow only ONE copy per subscription on this product. Create a SEPERATE subscription for each receiver." };
                }


                if (pSubscriptionData.BaseRate == 0)
                {
                    return new MimsValidationResult() { Message = "I cannot accept a base rate to be equal to zero." };
                }


                if (pSubscriptionData.ProposedStartIssue == 0)
                {
                    return new MimsValidationResult() { Message = "I cannot accept a Start issue equal to zero." };
                }


                if (pSubscriptionData.ProposedLastIssue == 0)
                {
                    return new MimsValidationResult() { Message = "I cannot accept a Last issue equal to zero." };
                }


                if (pSubscriptionData.Status != Subs.Data.SubStatus.Deliverable & pSubscriptionData.Status != Subs.Data.SubStatus.Proposed)
                {
                    return new MimsValidationResult() { Message = "The SubStatus is incorrect for a new subscription." };
                }

                // Check deliverymethods

                DeliveryMethod[] lMethodsRequiringAddress = { DeliveryMethod.Courier, DeliveryMethod.Mail, DeliveryMethod.RegisteredMail };
                if (lMethodsRequiringAddress.Contains(pSubscriptionData.DeliveryMethod) && pSubscriptionData.DeliveryAddressId == null)
                {
                    return new MimsValidationResult() { Message = "I need a delivery address." };
                }

                if (pSubscriptionData.ProductId == 17 && pSubscriptionData.DeliveryMethod != DeliveryMethod.ElectronicSingle)
                {
                    return new MimsValidationResult() { Message = "The deliverymethod for E-Mims has to be ElectronicSingle." };
                }


                // Check for overlaps

                var lContext = new MIMSDataContext(Settings.ConnectionString);

                var lOverLap = from lValues1 in lContext.MIMS_SubscriptionData_SubscriptionOverlapPrime() select lValues1;


                var lOverlapValues = from lValues2 in lContext.MIMS_SubscriptionData_SubscriptionOverlap(pSubscriptionData.ReceiverId, pSubscriptionData.ProposedStartIssue, pSubscriptionData.ProposedLastIssue)
                                     select lValues2.SubscriptionId;

                IEnumerable<int> lOverlaps = (IEnumerable<int>)lOverlapValues.ToList<int>();

                if (lOverlaps.Count() > 0)
                {
                    string lList = "";

                    foreach (int lSubscription in lOverlaps)
                    {
                        lList = lList + lSubscription.ToString() + " ";
                    }

                    return new MimsValidationResult() { Message = "This subscription overlaps with subscription " + lList.ToString(), Prompt = true };
                }

                lContext.Dispose();

                // Check for applicability of issue

                if (ProductDataStatic.GetPercentageLifeLeft(pSubscriptionData.ProposedStartIssue) < 33)
                {
                    return new MimsValidationResult() { Message = "There is less than 33% life left on the start issue. Change the basket to start on the next issue.", Prompt = true };
                }

                // Check for suspensions

                SqlCommand Command = new SqlCommand();
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.SubscriptionData.Validate]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@Receiver"].Value = pSubscriptionData.ReceiverId;
                Command.Parameters["@ProductId"].Value = pSubscriptionData.ProductId;

                int? Result = (int?)Command.ExecuteScalar();

                lConnection.Close();


                if (Result != null && Result.HasValue)
                {
                    if ((int)Result > (int)0)
                    {
                        return new MimsValidationResult() { Message = "There are " + Result.ToString() + " suspended subscriptions for this person for this product.", Prompt = true };
                    }
                }
                else
                {
                    return new MimsValidationResult() { Message = "The suspension query did not return any scalar" };
                }
                return new MimsValidationResult() { Message = "OK" };

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBiz", "Validate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return new MimsValidationResult() { Message = "Validate failed due to a technical error" };
            }
            finally
            {
                if (lConnection.State == ConnectionState.Open)
                {
                    lConnection.Close();
                }
            }
        }

        public static string NextDate(int pReceiverId, int pProductId, out DateTime pNextDate)
        {
         
            Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter lAdapter = new Data.SubscriptionDoc3TableAdapters.SubscriptionIssueTableAdapter();
            lAdapter.AttachConnection();

            DateTime lNextDate = (DateTime)lAdapter.NextDate((int)pReceiverId, (int)pProductId);

            if (lNextDate == null)
            {
                pNextDate = DateTime.Now;
                return "You already have the latest version of this product.";
            }
           
            pNextDate = lNextDate;
            return "OK";
        }

          

        private static string NullifyWithCreditNote(ref SqlTransaction pTransaction, Subs.Data.SubscriptionData3 pSubscriptionData, string pReason)
        {
            try
            {
                //Create a Credit Note if you can

                if (IssueBiz.GetUnitsLeft(pSubscriptionData.SubscriptionId) <= 0)
                {
                    // Everything has been delivered, so there is no point in issuing a creditnote.
                    return "OK";
                }

                // Generate an creditnote number

                int CreditNoteNumber = 0;
                if (!CounterData.GetUniqueNumber("CreditNote", ref CreditNoteNumber))
                {
                    return "Error getting a unique credit note number";
                }
                string CreditNoteNumberString = "CRE" + CreditNoteNumber.ToString("000000#");

                //We record the credit note in the transaction table. The credit note batch job will generate it at a later stage.

                LedgerData.CreditNote(pSubscriptionData.SubscriptionId, IssueBiz.GetUnitsLeft(pSubscriptionData.SubscriptionId),
                       CreditNoteNumberString, pReason, IssueBiz.GetUnitsLeft(pSubscriptionData.SubscriptionId) * pSubscriptionData.UnitPrice);

                //Remove the units left to be in  line with an empty subscription
                // If it fails, it will throw an exception.

                if (!IssueBiz.Cancel(ref pTransaction, pSubscriptionData.SubscriptionId)) { return "Error in IssueBiz.Cancel"; }

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "NullifyWithCreditNote", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        public static string Cancel(Data.SubscriptionData3 pSubscriptionData, string pReason)
        {

            SqlConnection lConnection = new SqlConnection();
            SqlTransaction lTransaction;

            // Prepare for a transaction

            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            lTransaction = lConnection.BeginTransaction("Cancel");

            try
            {
                // See if the subscription has been invoiced          

                if (pSubscriptionData.InvoiceId == null)
                {
                    // Destroy all evidence of the subscription

                    lTransaction.Rollback("Cancel");

                    MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                    lContext.MIMS_SubscriptionData_DestroySubscription(pSubscriptionData.SubscriptionId);

                    // Leave a trace of destruction
                    ExceptionData.WriteException(5, "Subscription " + pSubscriptionData.SubscriptionId.ToString() + " destroyed.", "SubscriptionBiz static", "Cancel", pReason + "CustomerId = " + pSubscriptionData.PayerId.ToString());

                    {
                        string lResult;

                        if ((lResult = CustomerBiz.SynchronizeLiability(new CustomerData3(pSubscriptionData.PayerId))) != "OK")
                        {
                            if (!lResult.Contains("Nothing"))
                            {
                                return lResult;
                            }
                        }
                    }

                    return "OK";
                }

                // Check current status

                if (pSubscriptionData.Status == SubStatus.Cancelled)
                {
                    return "Subscription " + pSubscriptionData.SubscriptionId.ToString() + " is already cancelled and cannot be cancelled again.";
                }



                if (IssueBiz.GetUnitsLeft(pSubscriptionData.SubscriptionId) > 0)
                {
                    string lResult;

                    if ((lResult = NullifyWithCreditNote(ref lTransaction, pSubscriptionData, pReason)) != "OK")
                    {

                        return lResult;
                    }
                }

                // Make the modifications

                pSubscriptionData.Status = SubStatus.Cancelled;

                // Phase 1 of persistence

                if (!pSubscriptionData.UpdateInTransaction(ref lTransaction))
                {
                    lTransaction.Rollback("Cancel");
                    return "Failed to update subscription";
                }

                //Phase 2 or persistence  Write this to the transaction log
                // If it fails, it will throw an exception.
                LedgerData.CancelSubscription(ref lTransaction, pSubscriptionData.SubscriptionId, pReason);

                lTransaction.Commit();
                return "OK";
            } // end of try


            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Cancel", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                lTransaction.Rollback("Cancel");
                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static MimsValidationResult SetInitialValues(SubscriptionData3 pSubscriptionData, DateTime pStartDate)
        {
            try
            {
                // Assume that you already have the PayerId and the ProductId in the SubscriptionData3 object DateFrom.
                // Infer:
                // ReceiverId
                // NumberOfIssues

                // Infer, but allow for modifications - after which TotalPrice has to be run.
                // ProductWeight
                // StartIssue  - Current or next issue
                // StartSequence
                // EndIssue
                // EndSequence
                // UnitsPerIssue
                // DeliveryMethod
                // DeliveryCost  - initially 0
                // DeliveryAddress
                // OrderNo
                // Multiplier

                // Get initial values from the Linq to SQL database, using a dataContext object

                var lContext = new MIMSDataContext(Settings.ConnectionString);
                var lInitialValues = from lValues in lContext.MIMS_SubscriptionBiz_SetInitialValues(pSubscriptionData.PayerId,
                    pSubscriptionData.ReceiverId, pSubscriptionData.ProductId, pStartDate)
                                     select lValues;

                MIMS_SubscriptionBiz_SetInitialValuesResult lResult = lInitialValues.Single();

                pSubscriptionData.Weight = (decimal)lResult.Weight;
                pSubscriptionData.NumberOfIssues = (int)lResult.NumberOfIssues;
                pSubscriptionData.UnitsPerIssue = 1;  // Can vary

                if (pSubscriptionData.DeliveryMethod == 0)
                {
                    pSubscriptionData.DeliveryMethod = (DeliveryMethod)Enum.ToObject(typeof(DeliveryMethod), lResult.DeliveryMethod);
                }


                if (pSubscriptionData.DeliveryAddressId == null)
                {
                    if (lResult.DeliveryAddressId != null)
                    {
                        pSubscriptionData.DeliveryAddressId = (int)lResult.DeliveryAddressId;
                    }
                }

                pSubscriptionData.BaseRate = (decimal)lResult.BaseRate;
                pSubscriptionData.VatPercentage = (decimal)lResult.VatRate;

                if (pSubscriptionData.ProposedLastIssue == 0)
                {
                    if ((int)lResult.LastIssue == 0)
                    {
                        return new MimsValidationResult() { Message = "The pre-defined issues do not allow for an automatic proposal of a last issue for a subscription.", Prompt = false };
                    }

                    pSubscriptionData.ProposedLastIssue = (int)lResult.LastIssue;
                    pSubscriptionData.ProposedLastSequence = (int)lResult.LastSequence;
                }


                if (pSubscriptionData.ProposedStartIssue == 0)
                {
                    pSubscriptionData.ProposedStartIssue = (int)lResult.StartIssue;
                    pSubscriptionData.ProposedStartSequence = (int)lResult.StartSequence;
                }


                // If the startissue is almost spent, rather propose the next issue, if one is defined.
                
                if (ProductDataStatic.GetPercentageLifeLeft(pSubscriptionData.ProposedStartIssue) < 33)
                {
                    int lNextIssue = IssueBiz.GetIssueId(pSubscriptionData.ProductId, pSubscriptionData.ProposedLastSequence + 1);
                    if (lNextIssue != 0)
                    {
                        pSubscriptionData.ProposedLastIssue = lNextIssue;
                        pSubscriptionData.ProposedLastSequence = IssueBiz.GetSequenceNumber(pSubscriptionData.ProposedLastIssue);
  
                        lNextIssue = IssueBiz.GetIssueId(pSubscriptionData.ProductId, pSubscriptionData.ProposedStartSequence + 1);
                        if (lNextIssue != 0)
                        {
                            pSubscriptionData.ProposedStartIssue = lNextIssue;
                            pSubscriptionData.ProposedStartSequence = IssueBiz.GetSequenceNumber(pSubscriptionData.ProposedStartIssue);
                        }
                    }
                    else
                    {
                         return new MimsValidationResult() { Message = "The pre-defined issues do not allow for an automatic proposal of an alternate issue for an almost spent issue.\nDo you want to keep the current issue?", Prompt=true };
                    }
                }

                return new MimsValidationResult() { Message = "OK", Prompt=false };

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBiz static", "SetInitialValues", "PayerId= " + pSubscriptionData.PayerId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return new MimsValidationResult() { Message = ex.Message, Prompt=false };
            }
        }

        public static string SetUnitPriceAndVat(Data.SubscriptionData3 pSubscriptionData)
        {
            try
            {
                decimal UnitPrice = 0;

                if (pSubscriptionData.BaseRate == 0)
                {
                    // No deliverycost on voucher copies.
                    pSubscriptionData.DeliveryCost = 0;
                }


                UnitPrice = (pSubscriptionData.BaseRate * pSubscriptionData.DiscountMultiplier) + pSubscriptionData.DeliveryCost;

                List<int> lVatCountries = new List<int>() { 61 }; //RSA

                if (!lVatCountries.Contains((int)pSubscriptionData.PayerCountryId))
                {
                    pSubscriptionData.VatPercentage = 0;
                }

                pSubscriptionData.Vat = (pSubscriptionData.VatPercentage * UnitPrice) / 100M;

                pSubscriptionData.UnitPrice = UnitPrice + pSubscriptionData.Vat;

                if (pSubscriptionData.UnitPrice < 0)
                {
                    throw new Exception("You cannot have a unit price of less than 0 cents on productid  " + pSubscriptionData.ProductId.ToString());
                }

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "SetUnitPriceAndVat", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        public static decimal GetDeliveryCost(int pCountryId, decimal pWeight, DeliveryMethod pDeliveryMethod)
        {
            try
            {
                EnumerableRowCollection<DeliveryAddressDoc.DeliveryCostRow> lCostData = AdministrationData2.DeliveryCost.AsEnumerable<DeliveryAddressDoc.DeliveryCostRow>();

                DeliveryAddressDoc.DeliveryCostRow lCostRow = lCostData.Where(r => r.CountryId == pCountryId && r.DateFrom <= DateTime.Now)
                   .OrderByDescending(o => o.DateFrom)
                   .ElementAt(0);

                //DeliveryAddressDoc.DeliveryCostRow lCostRow = (DeliveryAddressDoc.DeliveryCostRow)lCostData.Where<DeliveryAddressDoc.DeliveryCostRow>
                //    (r => r.CountryId == pCountryId && DateTime.Now >= r.DateFrom && r.DateFrom >= DateTime.Now ).Single();

                if (pDeliveryMethod == DeliveryMethod.Mail)
                {
                    if (pWeight < 1)
                    {
                        return lCostRow.Mail1;
                    }
                    else if (pWeight < 5)
                    {
                        return lCostRow.Mail2;
                    }
                    else
                    {
                        return lCostRow.Mail3;
                    }
                }

                if (pDeliveryMethod == DeliveryMethod.RegisteredMail)
                {
                    return lCostRow.Mail3;
                }

                if (pDeliveryMethod == DeliveryMethod.Courier)
                {
                    if (pWeight < 1)
                    {
                        return lCostRow.Courier1;
                    }
                    else if (pWeight < 5)
                    {
                        return lCostRow.Courier2;
                    }
                    else return lCostRow.Courier3;
                }

                if (pDeliveryMethod == DeliveryMethod.Collect
                    || pDeliveryMethod == DeliveryMethod.ElectronicSingle
                    || pDeliveryMethod == DeliveryMethod.ElectronicMultiple
                    )
                {
                    return 0;
                }

                return 0;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionBiz", "GetDeliveryCost", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0M;
            }
        }

        public static decimal FullPrice(Data.SubscriptionData3 pSubscriptionData)
        {
            try
            {
                //return pSubscriptionData.UnitPrice * pSubscriptionData.UnitsPerIssue * pSubscriptionData.NumberOfIssues;



                return (pSubscriptionData.BaseRate + pSubscriptionData.DeliveryCost)
                  * ((pSubscriptionData.VatPercentage + 100) / 100)
                  * pSubscriptionData.UnitsPerIssue * pSubscriptionData.NumberOfIssues;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "FullPrice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        public static decimal DiscountedPrice(Data.SubscriptionData3 pSubscriptionData)
        {
            try
            {
                return (pSubscriptionData.UnitPrice * pSubscriptionData.UnitsPerIssue * pSubscriptionData.NumberOfIssues);

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "DiscountedPrice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        private static decimal PromotionDiscountPercentage(PromotionCriteria pCriteria)
        {
            try
            {
                SubscriptionDoc3.PromotionDataTable lTable = new SubscriptionDoc3.PromotionDataTable();
                Data.SubscriptionDoc3TableAdapters.PromotionTableAdapter lAdapter = new Data.SubscriptionDoc3TableAdapters.PromotionTableAdapter();
                lAdapter.AttachConnection();
                lAdapter.Fill(lTable);

                List<SubscriptionDoc3.PromotionRow> lProducts = lTable.Where(p => p.ProductId == pCriteria.ProductId).ToList();
                if (lProducts.Count == 0)
                {
                    //There are no promotions for this product
                    return 0;
                }

                List<SubscriptionDoc3.PromotionRow> lPayers = lProducts.Where(p => !p.IsPayerIdNull() && p.PayerId == pCriteria.PayerId).ToList();
                if (lPayers.Count == 1)
                {
                    // You have a match on the payer as well
                    return lPayers[0].DiscountPercentage;
                }


                List<SubscriptionDoc3.PromotionRow> lProductWide = lProducts.Where(p => p.IsPayerIdNull()).ToList();
                if (lProductWide.Count == 1)
                {
                    // You have a match on the product alone
                    return lProductWide[0].DiscountPercentage;
                }

                return 0; // No match
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionBiz", "PromotionDiscountPercentage", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0M;
            }
        }

        public static decimal StandardProductPrice(int pProductId, int pCustomerId)
        {
            try 
            { 
                SubscriptionData3 lSubscriptionData = new SubscriptionData3();
                lSubscriptionData.PayerId = pCustomerId;
                lSubscriptionData.ReceiverId = pCustomerId;
                lSubscriptionData.ProductId = pProductId;
                SubscriptionBiz.SetInitialValues(lSubscriptionData, DateTime.Now);

                if (lSubscriptionData.FreeDelivery)
                {
                    lSubscriptionData.DeliveryCost = 0;
                }
                else
                {
                    lSubscriptionData.DeliveryCost = SubscriptionBiz.GetDeliveryCost(lSubscriptionData.ReceiverCountryId,
                                                                                     lSubscriptionData.Weight,
                                                                                     lSubscriptionData.DeliveryMethod);
                }

                SubscriptionBiz.SetUnitPriceAndVat(lSubscriptionData);

                return SubscriptionBiz.FullPrice(lSubscriptionData);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionBiz", "StandardProductPrice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0M;
            }
        }

        public static bool CalculateBasket(ObservableCollection<BasketItem> pBasket)
        {
            try
            {
                int lUnits = 0;
                decimal lInternallyCalculatedDiscountFraction;


                //************************************************* Cater for the delivery cost. 

                foreach (BasketItem lBasketItem in pBasket)
                {
                    lBasketItem.DeliveryOptions = ProductBiz.GetDeliveryOptions(lBasketItem.Subscription.ProductId);

                    if (lBasketItem.Subscription.FreeDelivery)
                    {
                        lBasketItem.Subscription.DeliveryCost = 0;
                    }
                    else
                    {
                        lBasketItem.Subscription.DeliveryCost = SubscriptionBiz.GetDeliveryCost(lBasketItem.Subscription.ReceiverCountryId,
                                                                                         lBasketItem.Subscription.Weight * lBasketItem.Subscription.UnitsPerIssue,
                                                                                         lBasketItem.Subscription.DeliveryMethod)
                                                         / lBasketItem.Subscription.UnitsPerIssue;
                    }
                }  // End of foreach loop


                // *****************************************Cater for products with overlapping content

                List<int> lMimsProducts = new List<int>() { 1, 8, 17, 32, 47, 49 };
                List<int> lPromotionProducts = new List<int>() {  8, 10 };

                int lNumberOfMimsProducts = pBasket.Where(p => lMimsProducts.Contains(p.Subscription.ProductId)).Count();
                int lNumberOfPromotionProducts = pBasket.Where(p => lPromotionProducts.Contains(p.Subscription.ProductId)).Count();


                //***********************************************************************************************

                if (DateTime.Now < new DateTime(2022,08,28))
                {
                    if (lNumberOfPromotionProducts > 1)
                    {
                        // Boost the unitcount if you have more than one product that covers more or less the same information.

                        lUnits = 50;
                    }
                }





                if (lNumberOfMimsProducts > 1)
                {
                    // Boost the unitcount if you have more than one product that covers more or less the same information.

                    lUnits = 50;
                }

                lUnits = lUnits + pBasket.Sum(p => p.Subscription.UnitsPerIssue);

                // ************************************** Give additional discount on mass purchases

                if (lUnits == 1)
                {
                    lInternallyCalculatedDiscountFraction = 0;
                }
                else
                {
                    lInternallyCalculatedDiscountFraction = (decimal)(0.006 * Math.Pow(lUnits, 2) + (1.0 * lUnits) - 1) / 100;
                }


                // *************************************  Apply a ceiling on overall discount.


                if (lInternallyCalculatedDiscountFraction > 0.3M)
                {
                    lInternallyCalculatedDiscountFraction = 0.3M;
                }


                // ************************************  Override calculated discount on per basket item basis


                foreach (BasketItem lBasketItem in pBasket)
                {

                    // Cater for discount via promotion.

                    if (lBasketItem.ExplicitDiscountPercentage == 0)
                    {

                        // Check for explicit discount via promotion price.

                        PromotionCriteria lPromotionCriteria = new PromotionCriteria()
                        {
                            PayerId = lBasketItem.Subscription.PayerId,
                            ProductId = lBasketItem.Subscription.ProductId,
                            IssueId = lBasketItem.Subscription.ProposedStartIssue,
                            StartDate = DateTime.Now
                        };

                        decimal lDiscountPercentage = SubscriptionBiz.PromotionDiscountPercentage(lPromotionCriteria);

                        if (lDiscountPercentage > 0)
                        {
                            // Display it on the form as explicit discount percentage
                            lBasketItem.ExplicitDiscountPercentage = lDiscountPercentage;
                        }
                    }


                    // Cater for discount via last minute override at at item level. Give the customer the benefit of the most discount.

                    if (lBasketItem.ExplicitDiscountPercentage > 0)
                    {
                        lBasketItem.Subscription.DiscountMultiplier = (1 - (decimal)(lBasketItem.ExplicitDiscountPercentage / 100));
                        SubscriptionBiz.SetUnitPriceAndVat(lBasketItem.Subscription);
                    }
                    else
                    {
                        // Cater for absolute price override

                        if (lBasketItem.FinalPriceOverride > 0)
                        {
                            {
                                string lResult;

                                if ((lResult = OverrideUnitPriceAndVat(lBasketItem.Subscription, lBasketItem.FinalPriceOverride)) != "OK")
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            lBasketItem.Subscription.DiscountMultiplier = 1 * (1 - lInternallyCalculatedDiscountFraction);
                            SubscriptionBiz.SetUnitPriceAndVat(lBasketItem.Subscription);
                        }
                    }
                    // Calculate the totals

                    lBasketItem.Price = SubscriptionBiz.FullPrice(lBasketItem.Subscription);
                    lBasketItem.DiscountedPrice = SubscriptionBiz.DiscountedPrice(lBasketItem.Subscription);

                }   // End of for loop

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBiz", "CalculateBasket", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static string DeleteQuoteExpired()
        {
            try
            {
                Subs.Data.SubscriptionDoc3.SubscriptionDataTable lSubscriptionTable = new Subs.Data.SubscriptionDoc3.SubscriptionDataTable();
                Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter lSubscriptionAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();

                lSubscriptionAdapter.AttachConnection();

                // Get all the subscriptions that qualify
                lSubscriptionAdapter.ClearBeforeFill = true;
                lSubscriptionAdapter.FillById(lSubscriptionTable, "ByQuoteDuration", 1, 1);

                //SubscriptionData3 mySubscriptionData = new SubscriptionData3();


                foreach (SubscriptionDoc3.SubscriptionRow myRow in lSubscriptionTable)
                {
                    lSubscriptionAdapter.Delete1(myRow.SubscriptionId);
                }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "DeleteQuoteExpired", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in DeleteQuoteExpired";
            }
        }

        public static string Suspend(Data.SubscriptionData3 pSubscriptionData, string pReference)
        {
            SqlConnection lConnection = new SqlConnection();
            int CurrentSubscriptionId = pSubscriptionData.SubscriptionId;

            // Start the transaction
            SqlTransaction myTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("Suspend");

            try
            {
                // Check current status

                if (pSubscriptionData.Status != SubStatus.Deliverable)
                {
                    return "Subscription not in a suspendable state.";
                }


                // Suspend it
                pSubscriptionData.Status = SubStatus.Suspended;

                // Remember the changes
                if (!pSubscriptionData.UpdateInTransaction(ref myTransaction))
                {
                    myTransaction.Rollback("Suspend");
                    return "Failed to update subscription";
                }

                LedgerData.Suspend(ref myTransaction, pSubscriptionData.SubscriptionId, pReference);


                myTransaction.Commit();
                return "OK";
            } // end of try

            catch (Exception ex)
            {
                myTransaction.Rollback("Suspend");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Suspend", "SubscriptionId = " + CurrentSubscriptionId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return "Error in Suspend";
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static string ReverseDelivery(Data.SubscriptionData3 pSubscriptionData,
               int IssueId, int UnitsToReturn, string Reason)
        {
            SqlConnection lConnection = new SqlConnection();

            // Start the transaction
            SqlTransaction lTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            lTransaction = lConnection.BeginTransaction("ReverseDelivery");

            try
            {
                // Update the Liability
                decimal Money = UnitsToReturn * pSubscriptionData.UnitPrice;
                if (!CustomerData3.AddToLiability(ref lTransaction, pSubscriptionData.PayerId, Money))
                {
                    throw new WarningException("CustomerData.UpdateLiability failed.");
                }

                // Update the subscriptionissue

                if (pSubscriptionData.Status != SubStatus.Cancelled)
                {
                    if (!IssueBiz.ReverseDelivery(ref lTransaction, pSubscriptionData.SubscriptionId, IssueId, UnitsToReturn, pSubscriptionData.UnitsPerIssue))
                    {
                        return "Error in IssueBiz.ReverseDelivery";
                    }

                }



                // Resume if expired

                if (pSubscriptionData.Status == SubStatus.Expired)
                {
                    pSubscriptionData.Status = SubStatus.Deliverable;
                    pSubscriptionData.UpdateInTransaction(ref lTransaction);
                }

                lTransaction.Commit();
                return "OK";
            }
            catch (Exception ex)
            {
                lTransaction.Rollback("ReverseDelivery");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "ReverseDelivery", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool Expire(Data.SubscriptionData3 pSubscriptionData)
        {
            SqlConnection lConnection = new SqlConnection();
            // Start the transaction
            SqlTransaction lTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            lTransaction = lConnection.BeginTransaction("Expire");

            try
            {
                pSubscriptionData.Status = SubStatus.Expired;

                if (!pSubscriptionData.UpdateInTransaction(ref lTransaction))
                {
                    lTransaction.Rollback("Expire");
                    return false;
                }

                LedgerData.Expire(ref lTransaction, pSubscriptionData.SubscriptionId);

                lTransaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                lTransaction.Rollback("Expire");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Expire", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool Activate(Data.SubscriptionData3 mySubscriptionData)
        {
            SqlConnection lConnection = new SqlConnection();
            // Start the transaction
            SqlTransaction myTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("Activate");

            try
            {
                // Change the status to deliverable

                mySubscriptionData.Status = SubStatus.Deliverable;

                // Remember the changes in subscriptiondata
                if (!mySubscriptionData.UpdateInTransaction(ref myTransaction))
                {
                    myTransaction.Rollback("Activate");
                    return false;
                }

                // Create a record in the transaction log 
                if (!LedgerData.InitialiseSubscription(ref myTransaction,
                    mySubscriptionData.SubscriptionId, mySubscriptionData.UnitPrice * mySubscriptionData.UnitsPerIssue * mySubscriptionData.NumberOfIssues))
                {
                    myTransaction.Rollback("Activate");
                    return false;
                }

                myTransaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                myTransaction.Rollback("Activate");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Activate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool CancelExpiredTooLong()
        {
            SubscriptionData3 lSubscriptionData;
            Subs.Data.SubscriptionDoc3.SubscriptionDataTable lSubscriptionTable = new Subs.Data.SubscriptionDoc3.SubscriptionDataTable();
            Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter lSubscriptionAdapter = new Subs.Data.SubscriptionDoc3TableAdapters.SubscriptionTableAdapter();

            try
            {
                // Get all the subscriptions that qualify

                lSubscriptionAdapter.AttachConnection();
                lSubscriptionAdapter.FillById(lSubscriptionTable, "ByExpirationDuration", 1, 1);

                foreach (SubscriptionDoc3.SubscriptionRow lRow in lSubscriptionTable)
                {
                    lSubscriptionData = null;
                    lSubscriptionData = new SubscriptionData3(lRow.SubscriptionId);

                    {
                        string lResult;

                        if ((lResult = Cancel(lSubscriptionData, "Expired for too long")) != "OK")
                        {
                            return false;
                        }
                    }
                } // End of for loop
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "CancelExpiredTooLong", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

        }

        public static string OverrideUnitPriceAndVat(Data.SubscriptionData3 pSubscriptionData, decimal pDesiredPrice)
        {
            try
            {
                // Reverse engineer the unit price and the VAT.

                pSubscriptionData.UnitPrice = pDesiredPrice / (pSubscriptionData.UnitsPerIssue * pSubscriptionData.NumberOfIssues);

                //pSubscriptionData.BaseRate = ((pSubscriptionData.UnitPrice * (Convert.ToDecimal(100) / (Convert.ToDecimal(100) + Convert.ToDecimal(pSubscriptionData.VatPercentage)))) - pSubscriptionData.DeliveryCost)
                //    / pSubscriptionData.DiscountMultiplier;

                //if (pSubscriptionData.BaseRate < 0)
                //{
                //    return "You cannot use such a low price, because that makes your base rate negative";
                //}

                pSubscriptionData.Vat = (pSubscriptionData.VatPercentage * pSubscriptionData.UnitPrice) / (Convert.ToDecimal(100) + pSubscriptionData.VatPercentage);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "OverrideUnitPriceAndVat", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        public static string ChangeReceiver(SubscriptionData3 pSubscriptionData, int pNewReceiverId)
        {
            SqlConnection lConnection = new SqlConnection();
            // Start the transaction
            SqlTransaction myTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;
            lConnection.Open();
            myTransaction = lConnection.BeginTransaction("ChangeReceiver");

            try
            {
                // Check current status

                if (pSubscriptionData.Status != SubStatus.Deliverable)
                {
                    return "There is no point in changing the receiver if you are not in a deliverable state.";
                }

                int lPreviousReceiverId = pSubscriptionData.ReceiverId;
                pSubscriptionData.ReceiverId = pNewReceiverId;


                if (!pSubscriptionData.UpdateInTransaction(ref myTransaction))
                {
                    myTransaction.Rollback("ChangeReceiver");
                    return "Failure in updating subscriptionData.";
                }

                // Write this to the transaction log
                LedgerData.ChangeReceiver(ref myTransaction, pSubscriptionData, lPreviousReceiverId);

                myTransaction.Commit();
                return "OK";
            }
            catch (Exception ex)
            {
                myTransaction.Rollback("ChangeReceiver");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscritionBizStatic", "ChangeReceiver", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        public static bool RenewalNotice(Data.SubscriptionData3 pSubscriptionData, bool pRenew)
        {
            SqlConnection lConnection = new SqlConnection();
            SqlTransaction lTransaction;

            // Start the transaction

            lConnection.ConnectionString = Settings.ConnectionString;
            if (lConnection.State != ConnectionState.Open)
            {
                lConnection.Open();
            }

            // This could be a nested transaction, if called from an
            // existing transaction
            lTransaction = lConnection.BeginTransaction("Renewal");
            try
            {


                // Untick the renewal flag
                pSubscriptionData.RenewalNotice = pRenew;

                // Remember the changes
                if (!pSubscriptionData.UpdateInTransaction(ref lTransaction))
                {
                    lTransaction.Rollback("Renewal");
                    return false;
                }

                // Write this to the transaction log
                LedgerData.ChangeRenewalNotice(ref lTransaction, pSubscriptionData, pRenew);

                lTransaction.Commit();

                return true;
            } // end of try

            catch (Exception ex)
            {
                lTransaction.Rollback("Renewal");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscritionBizStatic", "ChangeReceiver", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static bool AutomaticRenewal(ref Data.SubscriptionData3 pSubscriptionData, bool pAutomaticRenew)
        {
            SqlConnection lConnection = new SqlConnection();
            SqlTransaction lTransaction;

            // Start the transaction

            lConnection.ConnectionString = Settings.ConnectionString;
            if (lConnection.State != ConnectionState.Open)
            {
                lConnection.Open();
            }

            // This could be a nested transaction, if called from an
            // existing transaction
            lTransaction = lConnection.BeginTransaction("AutomaticRenewal");
            try
            {
                if (pAutomaticRenew)
                {
                    pSubscriptionData.RenewalNotice = false;
                    pSubscriptionData.AutomaticRenewal = true;
                }
                else
                {
                    pSubscriptionData.AutomaticRenewal = true;
                }

                // Remember the changes
                if (!pSubscriptionData.UpdateInTransaction(ref lTransaction))
                {
                    lTransaction.Rollback("AutomaticRenewal");
                    return false;
                }

                // Write this to the transaction log
                LedgerData.ChangeAutomaticRenewal(ref lTransaction, pSubscriptionData, pAutomaticRenew);

                lTransaction.Commit();

                return true;
            } // end of try

            catch (Exception ex)
            {
                lTransaction.Rollback("AutomaticRenewal");

                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscritionBizStatic",
                        "AutomaticRenewal", "AutomaticRenew = " + pAutomaticRenew.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                lConnection.Close();
            }

        }

        public static string Initialise(SqlTransaction pTransaction, Subs.Data.SubscriptionData3 pSubscriptionData)
        {

            // Initialise the issues for this subscription

            try
            {
                // Persist the changes in subscriptiondata
                if (!pSubscriptionData.UpdateInTransaction(ref pTransaction))
                {
                    return "UpdateInTransaction failed on SubscriptionData";
                }

                // Create the issue level entries.

                {
                    string lResult;

                    if ((lResult = IssueBiz.Create(ref pTransaction,
                                    pSubscriptionData.SubscriptionId,
                                    pSubscriptionData.UnitsPerIssue,
                                    pSubscriptionData.ProposedStartIssue,
                                    pSubscriptionData.ProposedLastIssue)) != "OK")
                    {
                        return lResult;
                    }
                }

                // Record the new subscription capture in the ledger, unless it is only a proposal

                if (pSubscriptionData.Status == SubStatus.Deliverable)
                {
                    if (!LedgerData.InitialiseSubscription(ref pTransaction,
                    pSubscriptionData.SubscriptionId, pSubscriptionData.UnitPrice * pSubscriptionData.UnitsPerIssue * pSubscriptionData.NumberOfIssues))
                    {
                        return "Failed to create a transaction of operation 16.";
                    }
                }

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Initialise", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        public static string Resume(Data.SubscriptionData3 pSubscriptionData)
        {
            if (pSubscriptionData.Status != SubStatus.Suspended)
            {
                return "You can resume only suspended subscriptions.";
            }

            SqlConnection lConnection = new SqlConnection();
            // Start the transaction
            SqlTransaction myTransaction;
            lConnection.ConnectionString = Settings.ConnectionString;

            if (lConnection.State != ConnectionState.Open)
            {
                lConnection.Open();
            }

            // This could be a nested transaction, if called from an
            // existing transaction
            myTransaction = lConnection.BeginTransaction("Resume");

            try
            {
                // Check current status

                if (pSubscriptionData.Status == SubStatus.Deliverable)
                {
                    return "OK";
                }

                // Resume it
                pSubscriptionData.Status = SubStatus.Deliverable;

                // Remember the changes
                if (!pSubscriptionData.UpdateInTransaction(ref myTransaction))
                {
                    myTransaction.Rollback("Resume");
                    return "SubscriptionData.UpdateInTransaction failed.";
                }

                LedgerData.Resume(ref myTransaction, pSubscriptionData.SubscriptionId);

                myTransaction.Commit();
                return "OK";
            } // end of try

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "SubscriptionBizStatic", "Resume", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
            finally
            {
                lConnection.Close();
            }
        }

        #region MobiMims

        public static SeatResult Authorize(int pProductId, int pReceiverId)
        {
            SeatResult lSeatResult = new SeatResult();
            try
            {
                //SubscriptionDoc3.AuthorizeDataTable lAuthorize = new SubscriptionDoc3.AuthorizeDataTable();
                // Data.SubscriptionDoc3TableAdapters.AuthorizeTableAdapter lAuthorizeAdapter = new Data.SubscriptionDoc3TableAdapters.AuthorizeTableAdapter();
                // lAuthorizeAdapter.AttachConnection();
                // lAuthorizeAdapter.FillBy(lAuthorize, pProductId, pReceiverId);

                // if (lAuthorize.Count != 1)
                // {
                //     throw new Exception("FillBy did not return exactly one row");
                // }

                MIMSDataContext lContext = new MIMSDataContext(Settings.ConnectionString);
                MIMS_SubscriptionBiz_AuthorizeResult lResult = (MIMS_SubscriptionBiz_AuthorizeResult)lContext.MIMS_SubscriptionBiz_Authorize(pProductId, pReceiverId).Single();

                lSeatResult.Seats = (int)lResult.Seats;
                lSeatResult.Reason = lResult.Reason;
                if (lResult.ExpirationDate.HasValue)
                {
                    lSeatResult.ExpirationDate = (DateTime)lResult.ExpirationDate;
                }
                else
                {
                    lSeatResult.ExpirationDate = DateTime.Now.AddYears(-10);
                }

                return lSeatResult;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionBiz", "Authorize", "ProductId = " + pProductId.ToString() + " ReceiverId= " + pReceiverId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                lSeatResult.Seats = 0;
                lSeatResult.Reason = "Failed due to technical error";

                return lSeatResult;
            }
        }

        public static List<AuthorizationResult> Authorizations()
        {
            List<AuthorizationResult> lResult = new List<AuthorizationResult>();

            try
            {
                SubscriptionDoc3.AuthorizationsDataTable lAuthorizations = new SubscriptionDoc3.AuthorizationsDataTable();
                Data.SubscriptionDoc3TableAdapters.AuthorizationsTableAdapter lAuthorizationsAdapter = new Data.SubscriptionDoc3TableAdapters.AuthorizationsTableAdapter();
                lAuthorizationsAdapter.AttachConnection();
                lAuthorizationsAdapter.Fill(lAuthorizations);

                foreach (Data.SubscriptionDoc3.AuthorizationsRow lRow in lAuthorizations)
                {

                    lResult.Add(new AuthorizationResult
                    {
                        Year = lRow.Year,
                        Month = lRow.Month,
                        CustomerId = lRow.CustomerId,
                        ProductId = lRow.ProductId,
                        Seats = lRow.Seats,
                        ExpirationDate = lRow.ExpirationDate
                    });
                }

                return lResult;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static SubscriptionBiz", "Authorizations", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return lResult;
            }
        }

        #endregion
    }
}
