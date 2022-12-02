using System.Windows;
using System.Windows.Documents;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for ElicitString.xaml
    /// </summary>
    public partial class ElicitRichText : Window
    {
        private string gStringAnswer = "";

        public ElicitRichText(string pQuestion)
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
            gAnswer.SelectAll();
            TextSelection lSelection = gAnswer.Selection;
            gStringAnswer = lSelection.Text;
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
