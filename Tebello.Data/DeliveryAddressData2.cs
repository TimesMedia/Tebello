using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Subs.Data
{
    public class DeliveryAddressData2
    {
        private readonly Subs.Data.DeliveryAddressDoc.DeliveryAddressDataTable gTable = new DeliveryAddressDoc.DeliveryAddressDataTable();
        private readonly Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter gAdapter = new Subs.Data.DeliveryAddressDocTableAdapters.DeliveryAddressTableAdapter();
        private readonly string[] gFormattedAddress = new string[8];


        #region Constructors

        public DeliveryAddressData2()
        {
            try
            {
                gTable.Clear();
                DeliveryAddressDoc.DeliveryAddressRow lRow = gTable.NewDeliveryAddressRow();
                //gModified = false;
                lRow.CountryId = 61;
                lRow.PostCode = "0000";
                lRow.ModifiedBy = "Initial";
                lRow.ModifiedOn = DateTime.Now;
                gTable.AddDeliveryAddressRow(lRow);
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "DeliveryAddressData2 constructor", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }


        public DeliveryAddressData2(int pDeliveryAddressId)
        {
            try
            {
                // Initialise
                gAdapter.AttachConnection();

                if (!Load(pDeliveryAddressId))
                {
                    return;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "DeliveryAddressData2", "DeliveryAddressId = " + pDeliveryAddressId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        #endregion

        #region Static functions

        public static int CreateCustomersBySuburbId(int pSuburbId, string pSuburbName)
        {
            try
            {

                List<int> lResults = new List<int>();

                SqlConnection lConnection = new SqlConnection();
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[dbo].[MIMS.DeliveryAddressData2.GetCustomersBySuburbId]";

                SqlParameter lParameter1 = Command.CreateParameter();
                lParameter1.ParameterName = "@SuburbId";
                lParameter1.DbType = DbType.Int32;
                lParameter1.Value = pSuburbId;
                Command.Parameters.Add(lParameter1);

                SqlDataReader lReader = Command.ExecuteReader();

                while (lReader.Read())
                {
                    lResults.Add(lReader.GetInt32(0));
                }

                XmlSerializer lSerializer = new XmlSerializer(typeof(List<int>));
                FileStream lStream = new FileStream(@"c:\Subs\" + pSuburbName + ".xml", FileMode.Create);
                lSerializer.Serialize(lStream, lResults);
                lStream.Flush();
                lStream.Close();
                return lResults.Count;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressData2", nameof(CreateCustomersBySuburbId), pSuburbId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }


        public static int CreateCustomersByCityId(int pCityId, string pCityName)
        {
            try 
            { 

                List<int> lResults = new List<int>();

                SqlConnection lConnection = new SqlConnection();
                SqlCommand Command = new SqlCommand();
                SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.ConnectionString = Settings.ConnectionString;
                lConnection.Open();
                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[dbo].[MIMS.DeliveryAddressData2.GetCustomersByCityId]";

                SqlParameter lParameter1 = Command.CreateParameter();
                lParameter1.ParameterName = "@CityId";
                lParameter1.DbType = DbType.Int32;
                lParameter1.Value = pCityId;
                Command.Parameters.Add(lParameter1);

                SqlDataReader lReader = Command.ExecuteReader();

                while (lReader.Read())
                {
                    lResults.Add(lReader.GetInt32(0));
                }

                XmlSerializer lSerializer = new XmlSerializer(typeof(List<int>));
                FileStream lStream = new FileStream(@"c:\Subs\" + pCityName + ".xml", FileMode.Create);
                lSerializer.Serialize(lStream, lResults);
                lStream.Flush();
                lStream.Close();
                return lResults.Count;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryAddressData2", nameof(CreateCustomersByCityId), pCityId.ToString());
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }




        }


        #endregion

        #region Utilities


        private bool Load(int DeliveryAddressId)
        {
            try
            {
                gTable.Clear();
                gAdapter.FillBy(gTable, DeliveryAddressId, "ByDeliveryAddress");

                if (!gTable[0].IsStreetIdNull())
                {
                    var lAddress = DeliveryAddressStatic.AddressInStrings(gTable[0].StreetId);

                    // Get the verified data
                    gTable[0].Street =  lAddress.Street;
                    gTable[0].Suburb = lAddress.Suburb;
                    gTable[0].City = lAddress.City;
                    gTable[0].Province = lAddress.Province;
                    gTable[0].CountryId = lAddress.CountryId;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Load", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public bool Format()
        {
            try
            {
                gFormattedAddress[0] = Building + " " + Floor + " " + Room;
                gFormattedAddress[0] = gFormattedAddress[0].Trim();          
                // Use the data that was captured without verification
                gFormattedAddress[1] = StreetNo + " " + Street + " " + StreetExtension + " " + StreetSuffix;
                gFormattedAddress[2] = Suburb;
                gFormattedAddress[3] = City;
                gFormattedAddress[4] = Province + " " + PostCode;

                // Drop the empty lines

                for (int i = 0; i < 5; i++)
                {
                    if (gFormattedAddress[i].Trim() == "")
                    {
                        gFormattedAddress[i] = gFormattedAddress[i + 1];
                        gFormattedAddress[i + 1] = "";
                    }
                }

            }
            catch (Exception Ex)
            {
                ExceptionData.WriteException(1, Ex.Message, this.ToString(), "Format", "DeliveryAddressId = " + DeliveryAddressId.ToString());
                return false;

            }
            return true;
        }

        public string Update()
        {
            try
            {
                if (gTable[0].IsPostCodeNull())
                {
                    return "Postal code is compulsory.";
                }

                // Update some more fields
                if (gTable[0].RowState == DataRowState.Added | gTable[0].RowState == DataRowState.Modified)
                {
                    gTable[0].ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                    gTable[0].ModifiedOn = DateTime.Now;
                    gTable[0].EndEdit();

                }
                gAdapter.AttachConnection();
                gAdapter.Update(gTable);
                gTable.AcceptChanges();

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Error in Update = " + ex.Message;
            }
        }


        public static string Link(int pDeliveryAddressId, int pCustomerId)
        {
            try
            {
                if (pCustomerId == 0)
                {
                    return "0 is not a valid CustomerId";
                }


                SqlConnection lConnection = new SqlConnection();
                lConnection.ConnectionString = Settings.ConnectionString;
                SqlCommand Command = new SqlCommand();
                //SqlDataAdapter Adaptor = new SqlDataAdapter();
                lConnection.Open();

                Command.Connection = lConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[MIMS.DeliveryAddressData2.Link]";
                SqlCommandBuilder.DeriveParameters(Command);
                //Adaptor.SelectCommand = Command;
                Command.Parameters["@DeliveryAddressId"].Value = pDeliveryAddressId;
                Command.Parameters["@CustomerId"].Value = pCustomerId;
                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "DeliveryData2", "Link", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return " Error in Link: " + ex.Message;
            }
        }


        #endregion

        #region Properties

        public int DeliveryAddressId
        {
            get
            {
                return gTable[0].DeliveryAddressId;
            }

            set
            {
                try
                {
                    if (value != 0)
                    {
                        gTable[0].DeliveryAddressId = value;
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
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "DeliveryAddressId set", value.ToString());
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);
                }
            }
        }


        public string PhoneNumber
        {
            get
            {
                if (gTable[0].IsPhoneNumberNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].PhoneNumber;
                }
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    gTable[0].SetPhoneNumberNull();
                }
                else
                {
                    gTable[0].PhoneNumber = value;
                }
            }
        }


        public int CountryId
        {
            get
            {
                return gTable[0].CountryId;
            }

            set
            {
                gTable[0].CountryId = value;
            }
        }


        public string CountryName
        {
            get
            {
                return AdministrationData2.Country.Where(q => q.CountryId == CountryId).Select(s => s.CountryName).Single();
            }
        }

        public string Province
        {
            get
            {

                if (gTable[0].IsProvinceNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].Province;
                }
            }

            set
            {
                // Record the fact that the Province has been changed

                //gModified = true;
                if (value == null || value.Trim() == "")
                {
                    gTable[0].SetProvinceNull();
                }
                else
                {
                    gTable[0].Province = value;
                }
            }
        }

        public string City
        {
            get
            {
                if (gTable[0].IsCityNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].City;
                }
            }

            set
            {
                //gModified = true;
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetCityNull();
                }
                else
                {
                    gTable[0].City = value;
                }
            }
        }

        public string Suburb
        {
            get
            {
                if (gTable[0].IsSuburbNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].Suburb;
                }
            }

            set
            {
                //gModified = true;
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetSuburbNull();
                }
                else
                {
                    gTable[0].Suburb = value;
                }
            }
        }

        public string Street
        {
            get
            {
                if (gTable[0].IsStreetNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].Street;
                }
            }

            set
            {
                //gModified = true;
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetStreetNull();
                }
                else
                {
                    gTable[0].Street = value;
                }
            }
        }

        public string StreetExtension
        {
            get
            {
                if (gTable[0].IsStreetExtensionNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].StreetExtension;
                }
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetStreetExtensionNull();
                }
                else
                {
                    gTable[0].StreetExtension = value;
                }
            }


        }

        public string StreetSuffix
        {
            get
            {
                if (gTable[0].IsStreetSuffixNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].StreetSuffix;
                }
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetStreetSuffixNull();
                }
                else
                {
                    gTable[0].StreetSuffix = value;
                }
            }
        }

        public int StreetId
        {
            get
            {
                if (gTable[0].IsStreetIdNull())
                {
                    return 0;
                }
                else
                {
                    return gTable[0].StreetId;
                }
            }

            set
            {
                if (value == 0)
                {
                    gTable[0].SetStreetIdNull();
                }
                else
                { 
                    gTable[0].StreetId = value;
                }
            }
        }


        public string StreetNo
        {
            get
            {
                if (gTable[0].IsStreetNoNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].StreetNo;
                }
            }

            set
            {
                //gModified = true;
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetStreetNoNull();
                }
                else
                {
                    gTable[0].StreetNo = value;
                }
            }
        }

        public string Building
        {
            get
            {
                if (gTable[0].IsBuildingNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].Building;
                }
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetBuildingNull();
                }
                else
                {
                    gTable[0].Building = value;
                }
            }
        }

        public string Floor
        {
            get
            {
                if (gTable[0].IsFloorNoNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].FloorNo;
                }
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetFloorNoNull();
                }
                else
                {
                    gTable[0].FloorNo = value;
                }
            }
        }

        public string Room
        {
            get
            {
                if (gTable[0].IsRoomNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].Room;
                }
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetRoomNull();
                }
                else
                {
                    gTable[0].Room = value;
                }
            }
        }

        public string PostCode
        {
            get
            {
                if (gTable[0].IsPostCodeNull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].PostCode;
                }

            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    gTable[0].SetPostCodeNull();
                }
                else
                {
                    gTable[0].PostCode = value;
                }
            }
        }

        public string SDI
        {
            get
            {
                if (gTable[0].IsSDINull())
                {
                    return "";
                }
                else
                {
                    return gTable[0].SDI;
                }
            }

            set
            {

                gTable[0].SDI = value;
            }
        }

        public string ModifiedBy
        {
            get
            {
                return gTable[0].ModifiedBy;
            }

            set
            {

                gTable[0].ModifiedBy = value;
            }
        }

        public DateTime ModifiedOn
        {
            get
            {
                return gTable[0].ModifiedOn;
            }

            set
            {

                gTable[0].ModifiedOn = value;
            }
        }

        public string PAddress1
        {
            get
            {
                return gFormattedAddress[0];
            }
        }

        public string PAddress2
        {
            get
            {
                return gFormattedAddress[1];
            }
        }

        public string PAddress3
        {
            get
            {
                return gFormattedAddress[2];
            }
        }

        public string PAddress4
        {
            get
            {
                return gFormattedAddress[3];
            }
        }

        public string PAddress5
        {
            get
            {
                return gFormattedAddress[4];
            }
        }

        #endregion

    }
}
