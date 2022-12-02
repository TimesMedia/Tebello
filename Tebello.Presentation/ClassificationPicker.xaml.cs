using System.Windows;
using System.Windows.Input;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for ClassificationPicker.xaml
    /// </summary>
    public partial class ClassificationPicker : Window
    {
        private ClassificationControl gClassificationControl = new ClassificationControl();

        public ClassificationPicker()
        {
            InitializeComponent();
            gClassificationControl.MouseDoubleClick += ClassificationControl_MouseDoubleClick; 
            gGrid.Children.Add(gClassificationControl);
        }

         public int ClassificationId
        {
            get
            {
                return gClassificationControl.SelectedClassificationId;
            }
        }

        private void ClassificationControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Bubbles up till here.
            this.Close();
        }
    }
}
