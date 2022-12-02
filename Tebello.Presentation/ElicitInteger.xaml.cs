using Subs.Data;
using System;
using System.Windows;
using System.Windows.Input;

namespace Subs.Presentation
{


    public enum ElicitOptions
    {
        Select = 1,
        Invoice = 2,
        Update = 3
    }

    public partial class ElicitInteger : Window
    {
        public int Answer = 0;
        public ElicitOptions gShortcut;

        public ElicitInteger(string pQuestion)
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
            if (int.TryParse(gAnswer.Text, out Answer))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("This is not a proper integer number. Please try again");
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
                    case "r":
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
