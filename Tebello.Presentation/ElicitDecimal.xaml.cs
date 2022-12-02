using Subs.Data;
using System;
using System.Windows;
using System.Windows.Input;

namespace Subs.Presentation
{

    public partial class ElicitDecimal : Window
    {
        public decimal Answer = 0M;
        public ElicitOptions gShortcut;

        public ElicitDecimal(string pQuestion)
        {
            InitializeComponent();
            gQuestion.Text = pQuestion;
            gAnswer.Focus();

            this.Title = "Request for information";

        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            Accept();
        }

        private void Accept()
        {
            if (decimal.TryParse(gAnswer.Text, out Answer))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("This is not a proper decimal number. Please try again");
                return;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;
        }

        private void CheckKeyStrokes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {

                if (Functions.IsInteger(e.Text) || e.Text == ".")
                {
                    return;
                }

                switch (e.Text)
                {
                    case "/":
                    case "i":
                        gShortcut = ElicitOptions.Invoice;
                        e.Handled = true;
                        Accept();
                        break;

                    case "*":
                    case "s":
                        gShortcut = ElicitOptions.Select;
                        e.Handled = true;
                        Accept();
                        break;

                    case "-":
                    case "u":
                        gShortcut = ElicitOptions.Update;
                        e.Handled = true;
                        Accept();
                        break;
                    default:
                        break;
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CheckKeyStrokes_PreviewTextInput", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in CheckKeyStrokes_PreviewTextInput " + ex.Message);
            }
        }
    }
}
