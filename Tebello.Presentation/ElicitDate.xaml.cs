using System;
using System.Windows;

namespace Subs.Presentation
{

    public partial class ElicitDate : Window
    {
        public DateTime Answer;
        public ElicitOptions gShortcut;

        public ElicitDate(string pQuestion)
        {
            InitializeComponent();
            gQuestion.Text = pQuestion;
            gDatePicker.Focus();

            this.Title = "Request for information";
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            Accept();
        }

        private void Accept()
        {
            if (gDatePicker.SelectedDate != null)
            {
                Answer = (DateTime)gDatePicker.SelectedDate;
                this.Close();
            }
            else
            {
                MessageBox.Show("You did not select a date. Please try again");
                return;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;
        }
    }
}
