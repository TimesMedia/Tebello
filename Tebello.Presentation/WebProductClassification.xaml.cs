using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Subs.Data;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for WebProductClassification.xaml
    /// </summary>
    public partial class WebProductClassification : UserControl
    {
        public static readonly DependencyProperty WebProductClassificationProperty;

        private static readonly string[] EnumArray = Enum.GetNames(typeof(WebProductClassifications));

        static WebProductClassification()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(WebProductClassificationChanged));
            WebProductClassification.WebProductClassificationProperty = DependencyProperty.Register("Classification", typeof(int), typeof(WebProductClassification), lMetaData);
        }

        private static void WebProductClassificationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            // Propogate change from dependency property to embedded controls.

            WebProductClassification lControl = (WebProductClassification)o;
            lControl.SetCheckedList((int)e.NewValue);
        }

        private void SetCheckedList(int pWebProductClassification)
        {
            try
            {
                if (pWebProductClassification > (int)Math.Pow(2, EnumArray.Length) - 1)
                {
                    throw new Exception("I cannot handle a number of that size.");
                }

                for (int i = (EnumArray.Length - 1); i >= 0; i--)
                {
                    CheckBox lCheckBox = (CheckBox)WebProduct.Children[i];

                    //if ((string)lCheckBox.Content != EnumArray[i])
                    //{
                    //    throw new Exception("Mismatch between enum and checkboxes!");
                    //}

                    if (pWebProductClassification >= (int)Math.Pow(2, i))
                    {
                        lCheckBox.IsChecked = true;
                        pWebProductClassification = pWebProductClassification - (int)Math.Pow(2, i);
                    }
                    else
                    {
                        lCheckBox.IsChecked = false;
                    }
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SetCheckedList", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            // Propogate change from embedded controls to dependecy property. 

            try
            {
                Classification = GetCheckedList();
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "CheckBoxChanged", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("CheckBoxChanged" + ex.Message);
            }
        }

        private int GetCheckedList()
        {
            int Number = 0;

            try
            {
                for (int i = 0; i <= (EnumArray.Length - 1); i++)
                {
                    CheckBox lCheckBox = (CheckBox)WebProduct.Children[i];
                    if ((bool)lCheckBox.IsChecked)
                    {
                        Number = Number + (int)Math.Pow(2, i);
                    }

                }
                return Number;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetCheckedList", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("GetCheckedList " + ex.Message);
                return 0;
            }
        }

        public WebProductClassification()
        {
            InitializeComponent();
        }

        public int Classification
        {
            get
            {
                return (int)GetValue(WebProductClassification.WebProductClassificationProperty);
            }

            set
            {
                SetValue(WebProductClassification.WebProductClassificationProperty, value);
            }
        }
    }
}
