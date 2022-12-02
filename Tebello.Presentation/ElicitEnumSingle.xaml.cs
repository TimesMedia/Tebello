using System;
using System.Windows;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for ElicitEnumSingle.xaml
    /// </summary>
    public partial class ElicitEnumSingle : Window
    {
        private int _Answer = 0;
        private readonly Type gEnumType;

        public ElicitEnumSingle(Type pType, string pQuestion)
        {
            this.InitializeComponent();

            gEnumType = pType;

            gQuestion.Text = pQuestion;
            foreach (string lOption in Enum.GetNames(pType))
            {
                OptionList.Items.Add(lOption);
            }
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (OptionList.SelectedItem == null)
            {
                MessageBox.Show("Please select one of the items.");
                return;
            }
            else
            {
                _Answer = (int)Enum.Parse(gEnumType, OptionList.SelectedItem.ToString());
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public int Answer
        {
            get { return _Answer; }
        }
    }
}
