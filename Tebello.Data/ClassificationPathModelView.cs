using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Subs.Data
{
    public class ClassificationPathModelView : ObservableCollection<ClassificationData2.ClassificationPath>
    {
        #region Globals
        private readonly Subs.Data.ClassificationDoc2TableAdapters.CustomerClassificationTableAdapter gCustomerClassificationAdapter = new Subs.Data.ClassificationDoc2TableAdapters.CustomerClassificationTableAdapter();
        private readonly ClassificationDoc2.CustomerClassificationDataTable gCustomerClassification = new ClassificationDoc2.CustomerClassificationDataTable();

        private readonly int gCustomerId = 0;
        private readonly ClassificationData2 gClassificationData;

        #endregion

        public ClassificationPathModelView(int pCustomerId)
        {
            try
            {
                gCustomerId = pCustomerId;
                gCustomerClassificationAdapter.AttachConnection();
                gClassificationData = new ClassificationData2();
                Fill(gCustomerId);

                this.CollectionChanged += GViewModel_CollectionChanged;
            }


            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ClassificationPathModelView", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        private void Fill(int pCustomerId)
        {
            try
            {
                // Fill the classifications

                gCustomerClassification.Clear();
                gCustomerClassificationAdapter.FillBy(gCustomerClassification, pCustomerId);

                this.Clear();

                foreach (ClassificationDoc2.CustomerClassificationRow lRow in gCustomerClassification)
                {
                    this.Add(gClassificationData.GetClassificationPath(lRow.ClassificationId));
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Fill", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
        }

        private void GViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                // Get the current status
                gCustomerClassificationAdapter.FillBy(gCustomerClassification, gCustomerId);

                // Test for new items

                if (e.NewItems != null)
                {

                    System.Collections.IList NewItems = e.NewItems;
                    if (NewItems.Count == 1)
                    {
                        ClassificationData2.ClassificationPath lNewPath = (ClassificationData2.ClassificationPath)NewItems[0];
                        ClassificationDoc2.CustomerClassificationRow lNewRow = gCustomerClassification.NewCustomerClassificationRow();

                        lNewRow.ClassificationId = lNewPath.ClassificationId;
                        lNewRow.CustomerId = gCustomerId;
                        lNewRow.ModifiedBy = System.Environment.UserName;
                        lNewRow.ModifiedOn = DateTime.Now;

                        gCustomerClassification.AddCustomerClassificationRow(lNewRow);
                    }

                }

                // Test for removals

                if (e.OldItems != null)
                {
                    System.Collections.IList lOldItems = e.OldItems;
                    if (lOldItems.Count == 1)
                    {
                        ClassificationData2.ClassificationPath lOldPath = (ClassificationData2.ClassificationPath)lOldItems[0];
                        ClassificationDoc2.CustomerClassificationRow lOldRow = gCustomerClassification.FindByCustomerIdClassificationId(gCustomerId, lOldPath.ClassificationId);
                        lOldRow.Delete();
                    }
                }

                // Persis the result
                gCustomerClassificationAdapter.Update(gCustomerClassification);

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GViewModel_CollectionChanged", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return;
            }
        }

        public bool Classified()
        {
            if (gCustomerClassification.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
