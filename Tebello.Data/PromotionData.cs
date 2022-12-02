using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Subs.Data
{

    public class PromotionData: BaseModel
    {
        #region Globals
            private SubscriptionDoc3.PromotionDataTable gPromotionTable = new SubscriptionDoc3.PromotionDataTable();
            private Data.SubscriptionDoc3TableAdapters.PromotionTableAdapter gPromotionAdapter = new SubscriptionDoc3TableAdapters.PromotionTableAdapter();
        #endregion

        #region Housekeeping
        public PromotionData()
        {
            gPromotionAdapter.AttachConnection();
            SubscriptionDoc3.PromotionRow lNewPromotionRow = gPromotionTable.NewPromotionRow();
            lNewPromotionRow.StartDate = DateTime.Now;
            lNewPromotionRow.EndDate = DateTime.Now.AddMonths(1);
            gPromotionTable.AddPromotionRow(lNewPromotionRow);
        }

        public PromotionData(int pPromotionId)
        {
            gPromotionAdapter.AttachConnection();
            gPromotionAdapter.FillById(gPromotionTable, pPromotionId );
        }


        #endregion

        #region Properties

        public int PromotionId
        {
            get 
            {
                return gPromotionTable[0].PromotionId;
            }

            set 
            {
                gPromotionTable[0].PromotionId = value;
            }
        }

        public int? PayerId
        {
            get
            {
                if (gPromotionTable[0].IsPayerIdNull())
                {
                    return null;
                }
                else return gPromotionTable[0].PayerId;
            }

            set
            {
                if (value == null)
                {
                    gPromotionTable[0].SetPayerIdNull();
                }
                else
                {
                    gPromotionTable[0].PayerId = (int)value;
                }

                NotifyPropertyChanged("PayerSurname");
            }
        }
        public int ProductId
        {
            get
            {
                return gPromotionTable[0].ProductId;
            }

            set
            {
                gPromotionTable[0].ProductId = value;
                NotifyPropertyChanged("ProductName");
            }
        }
        public DateTime StartDate
        {
            get
            {
                return gPromotionTable[0].StartDate;
            }

            set
            {
                gPromotionTable[0].StartDate = value;
            }
        }
        public DateTime EndDate
        {
            get
            {
                return gPromotionTable[0].EndDate;
            }

            set
            {
                if (value > ProductDataStatic.GetExpirationDate(ProductId))
                {
                    throw new Exception("A promotion cannot expire after the product it refers to: " + PayerSurname  + " | " + ProductName);
                }

                gPromotionTable[0].EndDate = value;
            }
        }
        public decimal DiscountPercentage
        {
            get
            {
                return gPromotionTable[0].DiscountPercentage;
            }

            set
            {
                gPromotionTable[0].DiscountPercentage = value;
            }
        }

        public string ProductName
        {
            get
            {
                return ProductDataStatic.GetProductName(ProductId);
            }

        }
        public string PayerSurname
        {
            // In C# 8.0, use a nullable string.

            get
            {
                if (PayerId != null)
                {
                    CustomerData3 lCustomerData = new CustomerData3((int)PayerId);
                    return lCustomerData.Surname;
                }
                else
                    return "";
            }
        }

        public bool Unchanged
        {
            get 
            {
                try
                {
                    if (gPromotionTable.Count == 0 )
                    {
                        return true;
                    }

                    return gPromotionTable[0].RowState == DataRowState.Unchanged;
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Unchanged", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    return true;
                }
            }
        }


        #endregion

        #region Methods

        public string Update()
        {
            try
            {
                if (gPromotionTable[0].RowState == DataRowState.Added || gPromotionTable[0].RowState == DataRowState.Modified)
                {
                    gPromotionTable[0].ModifiedBy = Environment.UserDomainName;
                    gPromotionTable[0].ModifiedOn = DateTime.Now;
                }

                gPromotionAdapter.Update(gPromotionTable[0]);
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

                return "Update failed due to technical error";
            }
        }
        #endregion
    }
}
