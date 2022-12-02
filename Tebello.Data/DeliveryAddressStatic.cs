using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Subs.Data
{
    public static class DeliveryAddressStatic
    {
        #region Globals

        public static DeliveryAddressDoc DeliveryAddresses = new DeliveryAddressDoc();
        public static bool Loaded = false;
        private static long gDeliveryAddressTableVersion = 800;
        private static readonly Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter gDeliveryTableAdapter = new Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter();
        private static readonly Data.DeliveryAddressDocTableAdapters.CountryTableAdapter gCountryTableAdapter = new Data.DeliveryAddressDocTableAdapters.CountryTableAdapter();
        private static readonly Data.DeliveryAddressDocTableAdapters.ProvinceTableAdapter gProvinceTableAdapter = new Data.DeliveryAddressDocTableAdapters.ProvinceTableAdapter();
        private static readonly Data.DeliveryAddressDocTableAdapters.CityTableAdapter gCityTableAdapter = new Data.DeliveryAddressDocTableAdapters.CityTableAdapter();
        private static readonly Data.DeliveryAddressDocTableAdapters.SuburbTableAdapter gSuburbTableAdapter = new Data.DeliveryAddressDocTableAdapters.SuburbTableAdapter();
        private static readonly Data.DeliveryAddressDocTableAdapters.StreetTableAdapter gStreetTableAdapter = new Data.DeliveryAddressDocTableAdapters.StreetTableAdapter();

        #endregion

        public static bool Refresh()
        {
            string lState = "Start";
            try
            {
                Loaded = false;

                DeliveryAddresses.DeliveryAddress.Clear();
                DeliveryAddresses.Street.Clear();
                DeliveryAddresses.Suburb.Clear();
                DeliveryAddresses.City.Clear();
                DeliveryAddresses.Province.Clear();
                DeliveryAddresses.Country.Clear();

                lState = "Fill";

                gCountryTableAdapter.Fill(DeliveryAddresses.Country);
                gProvinceTableAdapter.Fill(DeliveryAddresses.Province);
                gCityTableAdapter.Fill(DeliveryAddresses.City);

                lState = "Suburb";

                gSuburbTableAdapter.Fill(DeliveryAddresses.Suburb);
                gStreetTableAdapter.Fill(DeliveryAddresses.Street);
                gDeliveryTableAdapter.Fill(DeliveryAddresses.DeliveryAddress);

                lState = "Version";

                //gDeliveryAddressTableVersion = (long)gDeliveryTableAdapter.Version(800);  // This can be fixed in Dotnet Core 3.1

                Loaded = true;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressStatic", "Refresh", "State = " + lState);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                Loaded = false;
                return false;
            }

        }

        public static bool Exists(int pDeliveryAddressId)
        {
            //long lCurrentVersion = (long)gDeliveryTableAdapter.Version(62);

            //if (gDeliveryAddressTableVersion != lCurrentVersion)
            //{
            //    Refresh();
            //    gDeliveryAddressTableVersion = lCurrentVersion;
            //}

            int lCount = DeliveryAddresses.DeliveryAddress.Where(p => p.DeliveryAddressId == pDeliveryAddressId).Count<DeliveryAddressDoc.DeliveryAddressRow>();
            if (lCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static (int CountryId, string Province, string City, string Suburb, string Street) AddressInStrings(int pStreetId)
        {
            try 
            {
                if (!DeliveryAddressStatic.Loaded) {
                    Thread.Sleep(5000);
                }

                if (!DeliveryAddressStatic.Loaded)
                {
                    throw new Exception("DeliveryAddresses not loaded");
                }

                (string, int) lStreet = DeliveryAddresses.Street.Where(p => p.StreetId == pStreetId).Select(q => (q.StreetName, q.SuburbId)).Single();
                (string, int) lSuburb = DeliveryAddresses.Suburb.Where(p => p.SuburbId == lStreet.Item2).Select(q => (q.SuburbName, q.CityId)).Single();
                (string, int) lCity = DeliveryAddresses.City.Where(p => p.CityId == lSuburb.Item2).Select(q => (q.CityName, q.ProvinceId)).Single();
                (string, int) lProvince = DeliveryAddresses.Province.Where(p => p.ProvinceId == lCity.Item2).Select(q => (q.ProvinceName, q.CountryId)).Single();
                int lCountryId = lProvince.Item2;

                return (lProvince.Item2, lProvince.Item1, lCity.Item1, lSuburb.Item1,lStreet.Item1);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressStatic", "AddressInStrings", "StreetId = " + pStreetId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return (0,"","","","");
            }
        }

        public static int GetStreetId(int CountryId, string Province, string City, string Suburb, string Street, string Extension = "", string Suffix = "")
        {
            try
            {
                if (!DeliveryAddressStatic.Loaded)
                {
                    Thread.Sleep(5000);
                }

                if (!DeliveryAddressStatic.Loaded)
                {
                    throw new Exception("DeliveryAddresses not loaded");
                }

                List<int> lProvinces  = DeliveryAddresses.Province.Where(p => p.CountryId == CountryId).Select(q => q.ProvinceId).ToList();
                List<int> lCities = DeliveryAddresses.City.Where(p => lProvinces.Contains(p.ProvinceId) && p.CityName.ToUpper() == City.ToUpper()).Select(q => q.CityId).ToList();
                List<int> lSuburbs = DeliveryAddresses.Suburb.Where(p => lCities.Contains(p.CityId) && p.SuburbName.ToUpper() == Suburb.ToUpper()).Select(q => q.SuburbId).ToList();
                List<int> lStreets = DeliveryAddresses.Street.Where(p => lSuburbs.Contains(p.SuburbId) && p.StreetName.ToUpper() == Street.ToUpper()
                                     && (p.IsStreetExtensionNull() ? "": p.StreetExtension).ToUpper() == ((Extension == "") ? "": Extension.ToUpper())
                                     && (p.IsStreetSuffixNull()? "": p.StreetSuffix).ToUpper() == ((Suffix == "")? "": Suffix.ToUpper())).Select(q => q.StreetId).ToList();
                if (lStreets == null)
                {
                    return 0;
                }

                if (lStreets.Count == 1)
                {
                    return lStreets[0];
                }
                else
                { 
                    // Not found
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressStatic", "GetStreetId",
                        CountryId.ToString() + " " + Province + " " + City + " " + Suburb + " " + Street);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }




        static DeliveryAddressStatic()
        {
            try
            {
                gDeliveryTableAdapter.AttachConnection();
                gCountryTableAdapter.AttachConnection();
                gProvinceTableAdapter.AttachConnection();
                gCityTableAdapter.AttachConnection();
                gSuburbTableAdapter.AttachConnection();
                gStreetTableAdapter.AttachConnection();

                //DateTime gStartTime;
                //DateTime gEndTime;
                //TimeSpan gInterval;

                //gStartTime = DateTime.Now;

                Refresh();

                //gEndTime = DateTime.Now;
                //gInterval = gEndTime - gStartTime;

                //ExceptionData.WriteException(5, "It took " + gInterval.ToString() + " to load the deliveryaddresses", "DeliveryAddressStatic", "DeliveryAddressStatic", "");
            }


            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressStatic", "DeliveryAddressStatic", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }
    }
}
