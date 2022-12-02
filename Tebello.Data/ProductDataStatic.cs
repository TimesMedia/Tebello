using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace Subs.Data
{
    public static class ProductDataStatic
    {
        private readonly static SqlConnection gConnection = new SqlConnection();
        private static readonly MIMSDataContext gDataContext = new MIMSDataContext(Settings.ConnectionString);


        #region Globals
        public static ProductDoc gDoc = new ProductDoc();
        private readonly static ProductDocTableAdapters.Product2TableAdapter gProductAdapter = new ProductDocTableAdapters.Product2TableAdapter();
        private readonly static ProductDocTableAdapters.IssueTableAdapter gIssueAdapter = new ProductDocTableAdapters.IssueTableAdapter();
        #endregion

        #region Construction and housekeeping

        static ProductDataStatic()
        {
            try
            {
                // Set the connectionString for this object
                gConnection.ConnectionString = Settings.ConnectionString;

                // Attach connection to adapters

                gProductAdapter.AttachConnection();
                gIssueAdapter.AttachConnection();

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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "ProductData", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }
        public static void Refresh()
        {
            int lProgress = 1;
            try
            {
                gDoc.Issue.Clear();
                gDoc.Product2.Clear();
                lProgress = 2;
                gProductAdapter.Fill(gDoc.Product2);
                lProgress = 3;
                gIssueAdapter.Fill(gDoc.Issue);

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "Refresh", "Progress = " + lProgress.ToString());
                    throw new Exception("static ProductData" + " : " + "Refresh" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }
        #endregion

        #region Static methods

        public static bool AllowMultipleCopies(int pProductId)
        {
            try
            {
                if (gDoc.Product2.Count == 0)
                {
                    Refresh();
                }

                ProductDoc.Product2Row lRow = gDoc.Product2.FindByProductId(pProductId);

                if (lRow == null)
                {
                    throw new Exception("ProductId not found!");
                }
                else
                {
                    return lRow.AllowMultipleCopies;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "ProductData", "GetUnitsPerIssue", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return true;
            }
        }

        public static bool CheckStockBalance(int IssueId, ref int Balance)
        {
            try
            {
                gConnection.Open();

                SqlCommand Command = new SqlCommand();

                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.CheckStockBalance]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@IssueId"].Value = IssueId;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    Balance = (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "CheckStockBalance", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
            finally
            {
                gConnection.Close();
            }

        }

        public static List<int> CurrentIssues()
        {
            List<int> lIssues = gDoc.Issue.Where(p => p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now).Select(q => q.IssueId).ToList();
            return lIssues;
        }


        public static bool CurrentProduct(int pProductId)
        {
            try
            {
                // Note the a product is seen as being active, if it has future issues, even if the issues are not currently active.
                 System.Data.EnumerableRowCollection<ProductDoc.IssueRow> lIssues = gDoc.Issue.Where(p => p.ProductId == pProductId
                                                                                                          && p.EndDate >= DateTime.Now);
                int i = 0; // Count the number of rows in the result.
                foreach (ProductDoc.IssueRow lRow in lIssues)
                {
                    i++;
                }

                if (i > 0)
                {
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "CurrentProduct", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static decimal GetVatRate()
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[MIMS.ProductData.GetVatRate]";


                decimal? Result = (decimal?)Command.ExecuteScalar();
                if (Result != null)
                {
                    return (decimal)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "GetVatRate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0.0M;
            }
            finally
            {
                gConnection.Close();
            }

        }

        public static int GetDeliveryOptions(int pProductId)
        {
            EnumerableRowCollection<ProductDoc.Product2Row> lRows = gDoc.Product2.AsEnumerable<ProductDoc.Product2Row>();
            ProductDoc.Product2Row lRow = lRows.Where<ProductDoc.Product2Row>(p => p.ProductId == pProductId).Single();

            return lRow.DeliveryOptions;
        }

        public static string IssueId(int CurrentIssueId, int Offset, ref int NewIssueId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.IssueId]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@CurrentIssueId"].Value = CurrentIssueId;
                Command.Parameters["@Offset"].Value = Offset;

                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    NewIssueId = (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
                }
                if (NewIssueId == 0 & Offset < 0)
                {
                    return "You are trying to go backwards too far";
                }

                if (NewIssueId == 0)
                {
                    return "No appropriate issue has been defined yet";
                }
                else
                {
                    return "OK";
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "IssueId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed due to technical error";
            }
            finally
            {
                gConnection.Close();
            }
        }

        public static string GetIssueDescription(int IssueId)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.GetIssueDescription]";
                SqlCommandBuilder.DeriveParameters(Command);

                Command.Parameters["@IssueId"].Value = IssueId;

                string IssueName = (string)Command.ExecuteScalar();

                if (IssueName == "")
                {
                    throw new Exception("There is no IssueName for IssueId = " + IssueId.ToString());
                }

                return IssueName;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProdcutData", "IssueDescription", "");
                    throw new Exception("static ProdcutData" + " : " + "IssueDescription" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }

            finally
            {
                gConnection.Close();
            }
        }

        public static string GetIssueId(int pProductId, int pSequence, out int pIssueId)
        {
            pIssueId = 0;
            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.GetIssueId]";
                SqlCommandBuilder.DeriveParameters(Command);
                Command.Parameters["@ProductId"].Value = pProductId;
                Command.Parameters["@Sequence"].Value = pSequence;
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    pIssueId = (int)Result;
                    return "OK";
                }
                else
                {
                    return "Such an issue does not exist: ProductId = " + pProductId.ToString() + " Sequence = " + pSequence.ToString();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "GetIssueId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return "Failed due to technical error";
            }
            finally
            {
                gConnection.Close();
            }
        }

        public static bool GetNumberOfIssuesByIssue(int IssueId, out int NumberOfIssues)
        {
            NumberOfIssues = 0;

            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.GetNumberOfIssuesByIssue]";
                SqlCommandBuilder.DeriveParameters(Command);
                Command.Parameters["@Type"].Value = "Issue";
                Command.Parameters["@Id"].Value = IssueId;
                int? Result = (int?)Command.ExecuteScalar();
                if (Result != null && Result.HasValue)
                {
                    NumberOfIssues = (int)Result;
                }
                else
                {
                    throw new Exception("The query did not return any scalar");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "GetNumberOfIssuesByIssue", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }

            finally
            {
                gConnection.Close();
            }

        }

        public static int GetProductId(int IssueId)
        {
            try
            {
                if (gDoc.Issue.Count == 0)
                {
                    Refresh();
                }

                ProductDoc.IssueRow myRow;

                myRow = gDoc.Issue.FindByIssueId(IssueId);

                if (myRow.ProductId == 0)
                {
                    throw new Exception("ProductId not found!");
                }
                else
                {
                    return myRow.ProductId;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "GetProductId", "IssueId = " + IssueId.ToString());
                    throw new Exception("static ProductData" + " : " + "GetProductId" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }


        public static int GetPercentageLifeLeft(int IssueId)
        {
            try
            {
                if (IssueId == 0)
                {
                    throw new Exception("I cannot work with an IssueId of 0");
                }

                if (gDoc.Issue.Count == 0)
                {
                    Refresh();
                }

                ProductDoc.IssueRow lRow;

                lRow = gDoc.Issue.FindByIssueId(IssueId);

               System.TimeSpan lLifetime = lRow.EndDate - lRow.StartDate;
               System.TimeSpan lTimeLeft = lRow.EndDate - DateTime.Now;

                if (lLifetime.Days < 150)
                {
                    // Cater only for long lifetime products.
                    return 100;
                }

                int lDaysLeft = 0; // We do not cater for negative durations

                if (lTimeLeft.Days > 0)
                {
                    lDaysLeft = lTimeLeft.Days;
                }

                return lDaysLeft * 100/lLifetime.Days;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "GetPercentageLifeLeft", "IssueId = " + IssueId.ToString());
                    throw new Exception("static ProductData" + " : " + "GetPercentageLifeLeft" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }









        public static int GetSequence(int IssueId)
        {
            try
            {
                if (IssueId == 0)
                {
                    throw new Exception("I cannot work with an IssueId of 0");
                }

                if (gDoc.Issue.Count == 0)
                {
                    Refresh();
                }

                ProductDoc.IssueRow myRow;

                myRow = gDoc.Issue.FindByIssueId(IssueId);

                if (myRow.Sequence == 0)
                {
                    throw new Exception("Sequence not found!");
                }
                else
                {
                    return myRow.Sequence;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "GetSequence", "IssueId = " + IssueId.ToString());
                    throw new Exception("static ProductData" + " : " + "GetSequence" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }
        public static string GetProductName(int pProductId)
        {
            try
            {
                if (gDoc.Product2.Count == 0)
                {
                    Refresh();
                }

                ProductDoc.Product2Row lRow = gDoc.Product2.FindByProductId(pProductId);

                if (lRow == null)
                {
                    throw new Exception("ProductId not found!");
                }
                else
                {
                    return lRow.ProductName;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "GetProductName", "ProductId = " + pProductId.ToString());
                    throw new Exception("static ProductData" + " : " + "GetProductName" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
        }

        public static DateTime GetExpirationDate(int pProductId)
        {
            try
            {
                return gDoc.Issue.Where(p => p.ProductId == pProductId).Max(q => q.EndDate);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "GetExpirationDate", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }

        public static bool PostDelivery(ref SqlTransaction myTransaction, int IssueId, int Addition)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.Connection = myTransaction.Connection;
                Command.Transaction = myTransaction;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.PostDelivery]";
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@Addition", SqlDbType.Int);

                Command.Parameters["@IssueId"].Value = IssueId;
                Command.Parameters["@Addition"].Value = Addition;
                Command.ExecuteNonQuery();
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "static ProductData", "PostDelivery", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return false;
            }
        }

        public static void PostLoss(int IssueId, int Loss)
        {

            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.PostLoss]";
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@Loss", SqlDbType.Int);

                Command.Parameters["@IssueId"].Value = IssueId;
                Command.Parameters["@Loss"].Value = Loss;
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "Loose", "");
                    throw new Exception("static ProductData" + " : " + "Loose" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                gConnection.Close();
            }
        }

        public static void PostReturn(int IssueId, int Addition)
        {
            try
            {
                SqlCommand Command = new SqlCommand();
                gConnection.Open();
                Command.Connection = gConnection;

                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "dbo.[MIMS.ProductData.PostReturn]";
                Command.Parameters.Add("@IssueId", SqlDbType.Int);
                Command.Parameters.Add("@Addition", SqlDbType.Int);
                Command.Parameters["@IssueId"].Value = IssueId;
                Command.Parameters["@Addition"].Value = Addition;
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ExceptionData.WriteException(1, ex.Message, "static ProductData", "PostReturn", "");
                    throw new Exception("static ProductData" + " : " + "PostReturn" + " : ", ex);
                }
                else
                {
                    throw ex; // Just bubble it up
                }
            }
            finally
            {
                gConnection.Close();
            }
        }

        #endregion
    }
}
