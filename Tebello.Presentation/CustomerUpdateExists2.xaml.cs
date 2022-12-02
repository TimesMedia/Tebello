using Subs.Data;
using System;
using System.Data;
using System.Windows;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for CustomerUpdate.xaml
    /// </summary>
    public partial class CustomerUpdateExists2 : Window
    {
        public CustomerData3 gCustomerData = new CustomerData3();


        public CustomerUpdateExists2()
        {
            try
            { 
                InitializeComponent();
                this.comboCompany.DataSource = AdministrationData2.Company; // this.bindingSourceCompany;
                this.comboCompany.DisplayMember = "CompanyName";
                this.comboCompany.ValueMember = "CompanyId";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return;
            }

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             this.comboCompany.SelectedIndex = this.comboCompany.FindString("[None]");
        }


        public void Resume()
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.buttonCheckExisting.Visibility = System.Windows.Visibility.Hidden;
        }


        private void CheckExisting(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxInitials.Text))
                {
                    MessageBox.Show("Initials are compulsory");
                    return;
                }


                if (string.IsNullOrWhiteSpace(textBoxSurname.Text))
                {
                    MessageBox.Show("Surname is compulsory");
                    return;
                }
                               

                if (string.IsNullOrWhiteSpace(textBoxCellPhoneNumber.Text))
                {
                    MessageBox.Show("Cell phone number is compulsory");
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxEmailAddress.Text))
                {
                    MessageBox.Show("Email address number is compulsory");
                    return;
                }

                gCustomerData.Initials = textBoxInitials.Text;
                gCustomerData.Surname = textBoxSurname.Text;
                //gCustomerData.PhoneNumber = textBoxPhoneNumber.Text;
                gCustomerData.CellPhoneNumber = textBoxCellPhoneNumber.Text;
                gCustomerData.EmailAddress = textBoxEmailAddress.Text;

                int.TryParse(comboCompany.SelectedValue.ToString(), out int CompanyId);

                if (CompanyId == 0)
                {
                    MessageBox.Show("CompanyId could not be parsed");
                    return;
                }

                gCustomerData.CompanyId = CompanyId;  // [None]

                this.Hide();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            gCustomerData = null;
            this.Close();
            return;
        }
    
    }
}
