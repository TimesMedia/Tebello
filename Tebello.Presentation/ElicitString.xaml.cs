using System.Windows;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for ElicitString.xaml
    /// </summary>
    public partial class ElicitString : Window
    {
        private string gStringAnswer = "";

        public ElicitString(string pQuestion)
        {
            InitializeComponent();
            gQuestion.Content = pQuestion;
            gAnswer.Focus();
            this.Title = "Request for information";

        }

        public string Answer
        {
            get
            {
                if (string.IsNullOrWhiteSpace(gStringAnswer))
                {
                    return "";
                }
                else
                {
                    return (string)gStringAnswer;
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Request for information";
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            gStringAnswer = gAnswer.Text.Trim();
            if (gStringAnswer.Length == 0)
            {
                MessageBox.Show("You did not supply me with any data.");
                return;
            }
            
            this.Close();
            return;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            gStringAnswer = "";
            this.Close();
            return;
        }
    }
}
