using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for CountryControl.xaml
    /// </summary>
    public partial class CountryControl : UserControl
    {
        private readonly Subs.Data.DeliveryAddressDoc gDoc;
        private readonly Subs.Data.DeliveryAddressDocTableAdapters.CountryTableAdapter gCountryAdapter = new Subs.Data.DeliveryAddressDocTableAdapters.CountryTableAdapter();
        private readonly CollectionViewSource gCountryViewSource;
        private CustomerData3 gCustomerData;
        public CountryControl()
        {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;

            gDoc = (DeliveryAddressDoc)this.Resources["deliveryAddressDoc"];
            gCountryAdapter.AttachConnection();
            gCountryAdapter.Fill(gDoc.Country);
            gCountryViewSource = (CollectionViewSource)this.Resources["countryViewSource"];
        }

        private void Select(int pCountryId)
        {
            try
            {
                ComboCountry.SelectedValue = pCountryId;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Select", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Select " + ex.Message);
            }
        }



        private void ComboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboCountry.SelectedValue != null)
                {
                    gCustomerData.CountryId = (int)ComboCountry.SelectedValue;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ComboCountry_SelectionChanged", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("ComboCountry_SelectionChanged " + ex.Message);
            }
        }

        public CustomerData3 CustomerData
        {
            get
            {
                return gCustomerData;
            }

            set
            {
                gCustomerData = (CustomerData3)value;
                Select(gCustomerData.CountryId);
            }
        }

    }
}
