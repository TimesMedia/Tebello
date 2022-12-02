using System;
using System.Collections.Generic;
using System.Data;

namespace Subs.Data
{
     public static class AdministrationData2
    {
        // This class is used only to display data. It does not cater for updating the database.

        private static readonly Subs.Data.AdministrationDoc gAdministrationDoc = new AdministrationDoc();
        private static readonly Subs.Data.AdministrationDocTableAdapters.CompanyTableAdapter gCompanyAdapter = new AdministrationDocTableAdapters.CompanyTableAdapter();
        private static readonly Subs.Data.DeliveryAddressDocTableAdapters.CountryTableAdapter gCountryAdapter = new DeliveryAddressDocTableAdapters.CountryTableAdapter();
        private static readonly Subs.Data.DeliveryAddressDocTableAdapters.DeliveryCostTableAdapter gDeliveryCostAdapter = new DeliveryAddressDocTableAdapters.DeliveryCostTableAdapter();
        private static readonly Subs.Data.SubscriptionDoc3TableAdapters.InvoiceTableAdapter gInvoiceTableAdapter = new SubscriptionDoc3TableAdapters.InvoiceTableAdapter();
        private static readonly Subs.Data.ClassificationDoc2TableAdapters.ClassificationTableAdapter gClassificationAdapter
              = new Subs.Data.ClassificationDoc2TableAdapters.ClassificationTableAdapter();

        public static Subs.Data.AdministrationDoc.CompanyDataTable Company = gAdministrationDoc.Company;
        public static DeliveryAddressDoc.CountryDataTable Country = new DeliveryAddressDoc.CountryDataTable();
        public static Subs.Data.ClassificationDoc2.ClassificationDataTable Classification = new ClassificationDoc2.ClassificationDataTable();
        public static DeliveryAddressDoc.DeliveryCostDataTable DeliveryCost = new DeliveryAddressDoc.DeliveryCostDataTable();
        public static Dictionary<int, string> gSubscriptionType = new Dictionary<int, string>();
        public static Dictionary<int, string> gSubscriptionMedium = new Dictionary<int, string>();

        #region Constructors

        static AdministrationData2()
        {
            try
            {
                gInvoiceTableAdapter.AttachConnection();
                Refresh();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "AdministrationData2", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        #endregion

        #region Utilities
        public static void Refresh()
        {
            try
            {
                RefreshCountry();
                RefreshCompany();
                RefreshClassification();
                RefreshDeliveryCost();

                gSubscriptionType.Clear();
                foreach (int lKey in Enum.GetValues(typeof(SubscriptionType)))
                {
                    gSubscriptionType.Add(lKey, Enum.GetName(typeof(SubscriptionType), lKey));
                }

                gSubscriptionMedium.Clear();
                foreach (int lKey in Enum.GetValues(typeof(SubscriptionMedium)))
                {
                    gSubscriptionMedium.Add(lKey, Enum.GetName(typeof(SubscriptionMedium), lKey));
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "Refresh", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        public static void RefreshCountry()
        {
            gCountryAdapter.AttachConnection();
            gCountryAdapter.ClearBeforeFill = true;
            gCountryAdapter.Fill(Country);

        }

        public static void RefreshClassification()
        {
            gClassificationAdapter.AttachConnection();
            gClassificationAdapter.ClearBeforeFill = true;
            gClassificationAdapter.Fill(Classification);
        }



        public static void RefreshCompany()
        {
            try
            {
                gCompanyAdapter.AttachConnection();
                gCompanyAdapter.ClearBeforeFill = true;
                gCompanyAdapter.Fill(Company);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "RefreshCompany", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        public static void RefreshDeliveryCost()
        {

            gDeliveryCostAdapter.AttachConnection();
            gDeliveryCostAdapter.ClearBeforeFill = true;
            gDeliveryCostAdapter.Fill(DeliveryCost);

        }

        public static Guid GetGuid(string pUsage, string pEmail)
        {
            AdministrationDoc.GUIDTableDataTable lTable = new AdministrationDoc.GUIDTableDataTable();
            AdministrationDocTableAdapters.GUIDTableTableAdapter lAdapter = new AdministrationDocTableAdapters.GUIDTableTableAdapter();

            try
            {
                lAdapter.AttachConnection();
                lAdapter.FillByEmail(lTable, pEmail);

                if (lTable.Count >= 1)
                {
                    return lTable[0].GUID;
                }
                else
                {
                    Guid lGuid = Guid.NewGuid();
                    lAdapter.Insert(lGuid, pUsage, pEmail, DateTime.Now, System.Environment.UserName);
                    return lGuid;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "GetGuid", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return Guid.Empty;
            }
            finally
            {
                if (lAdapter.Connection.State == ConnectionState.Open)
                {
                    lAdapter.Connection.Close();
                }
            }
        }

        public static bool FillGUIDByEmail(string pEmail, ref AdministrationDoc.GUIDTableDataTable pTable)
        {
            AdministrationDocTableAdapters.GUIDTableTableAdapter lAdapter = new AdministrationDocTableAdapters.GUIDTableTableAdapter();

            try
            {
                lAdapter.AttachConnection();
                lAdapter.FillByEmail(pTable, pEmail);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "FillGUIDByEmail", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                if (lAdapter.Connection.State == ConnectionState.Open)
                {
                    lAdapter.Connection.Close();
                }
            }
        }

        public static bool FillGUIDByGUID(Guid pGUID, ref AdministrationDoc.GUIDTableDataTable pTable)
        {
            AdministrationDocTableAdapters.GUIDTableTableAdapter lAdapter = new AdministrationDocTableAdapters.GUIDTableTableAdapter();

            try
            {
                lAdapter.AttachConnection();
                lAdapter.FillByGUID(pTable, pGUID);
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static AdministrationData2", "FillGUIDByGUID", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                if (lAdapter.Connection.State == ConnectionState.Open)
                {
                    lAdapter.Connection.Close();
                }
            }
        }

        public static int GetCompanyId(string pCompanyName)
        {
            try
            {
                foreach (AdministrationDoc.CompanyRow item in Company)
                {
                    if (item.CompanyName == pCompanyName)
                    {
                        return item.CompanyId;
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "AdministrationData2", "GetCompanyId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 1;
            }
        }

        public static int GetInvoiceId()
        {
            try
            {
                return (int)gInvoiceTableAdapter.GetInvoiceId();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "AdministrationData2", "GetInvoice", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        #endregion
     }
}
