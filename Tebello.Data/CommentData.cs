using System;

namespace Subs.Data
{
    public class CommentData
    {
        private readonly Subs.Data.CustomerDoc2TableAdapters.Comment2TableAdapter gAdapter = new CustomerDoc2TableAdapters.Comment2TableAdapter();
        public Subs.Data.CustomerDoc2 gDoc = new CustomerDoc2();
        private readonly int gCustomerId;

        public CommentData()
        {
            gAdapter.AttachConnection();
        }

        public CommentData(int pCustomerId)
        {
            try
            {
                gCustomerId = pCustomerId;
                gAdapter.AttachConnection();
                gAdapter.FillByCustomerId(gDoc.Comment2, pCustomerId);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CommentData constructor", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        public string Update(int pCustomerId)
        {
            try
            {
                foreach (Data.CustomerDoc2.Comment2Row lRow in gDoc.Comment2)
                {
                    if (lRow.RowState == System.Data.DataRowState.Added || lRow.RowState == System.Data.DataRowState.Modified)
                    {
                        lRow.CustomerId = pCustomerId;
                        lRow.ModifiedBy = Environment.UserDomainName.ToString() + "\\" + Environment.UserName.ToString();
                        lRow.ModifiedOn = DateTime.Now;
                    }
                }

                gAdapter.Update(gDoc.Comment2);
                gDoc.Comment2.AcceptChanges();
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

                return ex.Message;
            }
        }
    }
}
