using System.Windows;

namespace Subs.Presentation
{
      public partial class AdministrationCountry : Window
    {
        private Subs.Data.DeliveryAddressDoc gDeliveryAddressDoc;
        private readonly Subs.Data.DeliveryAddressDocTableAdapters.CountryTableAdapter gCountryTableAdapter = new Subs.Data.DeliveryAddressDocTableAdapters.CountryTableAdapter();
        private readonly Subs.Data.DeliveryAddressDocTableAdapters.DeliveryCostTableAdapter gDeliveryCostTableAdapter = new Subs.Data.DeliveryAddressDocTableAdapters.DeliveryCostTableAdapter();

        public AdministrationCountry()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gDeliveryAddressDoc = ((Subs.Data.DeliveryAddressDoc)(this.FindResource("deliveryAddressDoc")));
            gCountryTableAdapter.AttachConnection();
            gDeliveryCostTableAdapter.AttachConnection();

            gCountryTableAdapter.Fill(gDeliveryAddressDoc.Country);
            gDeliveryCostTableAdapter.Fill(gDeliveryAddressDoc.DeliveryCost);
        }

        private void buttonSubmitDeliveryCost_Click(object sender, RoutedEventArgs e)
        {
            gDeliveryCostTableAdapter.Update(gDeliveryAddressDoc.DeliveryCost);
        }

        private void buttonCountry_Click(object sender, RoutedEventArgs e)
        {
            gCountryTableAdapter.Update(gDeliveryAddressDoc.Country);
            MessageBox.Show("Done");
        }
    }
}
