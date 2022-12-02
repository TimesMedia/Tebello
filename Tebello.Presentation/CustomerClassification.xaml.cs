using Subs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Subs.Presentation
{
    public partial class CustomerClassification : Window
    {

        private ClassificationPathModelView gViewModel;
        private readonly ClassificationData2 gClassificationData = new ClassificationData2();

        public CustomerClassification()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Settings.CurrentCustomerId != 0)
                {
                    CustomerData3 lCustomerData = new CustomerData3(Settings.CurrentCustomerId);
                    if (lCustomerData.CustomerId == 0)
                    {
                        MessageBox.Show("There is no customer with such an id.");
                        return;
                    }
                    CustomerId.Content = lCustomerData.CustomerId;
                    Surname.Content = lCustomerData.Surname;
                    Company.Content = lCustomerData.CompanyName;

                    gViewModel = new ClassificationPathModelView(lCustomerData.CustomerId);

                    Classifications.ItemsSource = gViewModel;
                }
                else
                {
                    MessageBox.Show("You have not selected a customer that I can work with.");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "MainWindow_Loaded", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in MainWindow_Loaded: " + ex.Message);
            }
        }


        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ClassificationData2.ClassificationPath lClassificationPath = (ClassificationData2.ClassificationPath)Classifications.Items.CurrentItem;

                gViewModel.Remove(lClassificationPath);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "MenuItemRemove_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in MenuItemRemove_Click: " + ex.Message);
            }
        }

        private void ButtonAddClassification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassificationPicker lPicker = new ClassificationPicker();
                lPicker.ShowDialog();

                if (lPicker.ClassificationId == 0)
                {
                    MessageBox.Show("You have not selected a category. Please try again.");
                    return;
                }


                var lQuery = gViewModel.Where(a => a.ClassificationId == lPicker.ClassificationId);
                List<ClassificationData2.ClassificationPath> lFound = lQuery.ToList<ClassificationData2.ClassificationPath>();
                if (lFound.Count > 0)
                {
                    MessageBox.Show("This is a duplicate categorisation");
                    return;
                }

                // Test the existing mappings

                {
                    string lResult;

                    if ((lResult = TestAClassificationForViewModel(Settings.CurrentCustomerId, lPicker.ClassificationId, gViewModel)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }

                // If you get here, you have passed the test.
                ClassificationData2.ClassificationPath lNew = gClassificationData.GetClassificationPath(lPicker.ClassificationId);
                gViewModel.Add(lNew);
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonAddClassification_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in ButtonAddClassification_Click: " + ex.Message);
            }
        }

        private void ButtonPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassificationPicker lPicker = new ClassificationPicker();
                lPicker.ShowDialog();

                if (lPicker.ClassificationId == 0)
                {
                    MessageBox.Show("You have not selected a category. Please try again.");
                    return;
                }

                MessageBox.Show("Under construction!");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonPick_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in ButtonPick_Click: " + ex.Message);
            }
        }

        private string TestAClassificationForViewModel(int pCustomerId, int pClassificationId, ClassificationPathModelView pViewModel)
        {
            // This applies to the current customer

            ClassificationData2 lClassificationData = new ClassificationData2();
            List<int> lResult = lClassificationData.GetFamily(pClassificationId);

            // Ensure that you do not already have a mapping to one of the family  members
            // Go through all existing mappings, and ensure that they are not family

            foreach (ClassificationData2.ClassificationPath lPath in pViewModel)
            {
                if (lResult.Contains(lPath.ClassificationId))
                {
                    return "Customer " + pCustomerId.ToString() + " is already mapped to a family member, i.e. " + lPath.ClassificationId.ToString();
                }
            }
            return "OK";
        }

        private string TestAllClassificationsInViewModel(int pCustomerId, ClassificationPathModelView pViewModel)
        {
            foreach (ClassificationData2.ClassificationPath lPath in pViewModel)
            {

                {
                    string lResult;

                    if ((lResult = TestAClassificationForViewModel(pCustomerId, lPath.ClassificationId, pViewModel)) != "OK")
                    {
                        return lResult;

                    }
                }
            }
            return "OK";
        }

        private void ButtonSingeTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TestAllClassificationsInViewModel(Settings.CurrentCustomerId, gViewModel));
        }

        private void ButtonAllTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Subs.Data.ClassificationDoc2TableAdapters.CustomerClassificationTableAdapter lCustomerClassificationAdapter = new Subs.Data.ClassificationDoc2TableAdapters.CustomerClassificationTableAdapter();
                ClassificationDoc2.CustomerClassificationDataTable lCustomerClassification = new ClassificationDoc2.CustomerClassificationDataTable();
                lCustomerClassificationAdapter.AttachConnection();
                lCustomerClassificationAdapter.Fill(lCustomerClassification);


                List<int> lCustomerList = new List<int>();

                //************************************ This  works

                //foreach (ClassificationDoc2.CustomerClassificationRow lRow in lCustomerClassification)
                //{
                //    lCustomerList.Add(lRow.CustomerId);
                //}

                //IEnumerable<int> lCustomerListDistinct = lCustomerList.Distinct<int>();
                //**********************************

                ///***************************************************
                //IEnumerable<int> lCustomerListDistinct =
                //      lCustomerClassification.ToList<ClassificationDoc2.CustomerClassificationRow>().Select(a => a.CustomerId).Distinct();

                ///****************************************************

                ////*******************

                var lCustomerListDistinct = lCustomerClassification.ToList().Select(a => a.CustomerId).Distinct();


                ////**************




                List<string> lErrors = new List<string>();


                foreach (int c in lCustomerListDistinct)
                {
                    ClassificationPathModelView lViewModel = new ClassificationPathModelView(c);

                    {
                        string lResult;

                        if ((lResult = TestAllClassificationsInViewModel(c, lViewModel)) != "OK")
                        {
                            lErrors.Add(lResult);
                        }
                    }
                }

                MessageBox.Show("There are " + lErrors.Count.ToString() + " customers in error.");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonAllTest_Click", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!gViewModel.Classified())
            {
                MessageBox.Show("You have to classify the customer!");

                // Prevent the window from closing
                e.Cancel = true;
            }
        }
    }
}
