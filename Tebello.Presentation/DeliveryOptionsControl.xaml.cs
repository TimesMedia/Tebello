using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for DeliveryOptionsControl.xaml
    /// </summary>
    public partial class DeliveryOptionsControl : UserControl
    {
        public static readonly DependencyProperty DeliveryOptionsProperty;

        private static readonly string[] EnumArray = Enum.GetNames(typeof(DeliveryMethod));

        static DeliveryOptionsControl()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(DeliveryOptionsChanged));
            DeliveryOptionsControl.DeliveryOptionsProperty = DependencyProperty.Register("DeliveryOptions", typeof(int), typeof(DeliveryOptionsControl), lMetaData);
        }

        private static void DeliveryOptionsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            // Propogate change from dependency property to embedded controls.

            DeliveryOptionsControl lControl = (DeliveryOptionsControl)o;
            lControl.SetCheckedList((int)e.NewValue);
        }

        private void SetCheckedList(int pDeliveryOptions)
        {
            try
            {
                if (pDeliveryOptions > (int)Math.Pow(2, EnumArray.Length) - 1)
                {
                    throw new Exception("I cannot handle a number of that size.");
                }

                for (int i = (EnumArray.Length - 1); i >= 0; i--)
                {
                    CheckBox lCheckBox = (CheckBox)CheckBoxStack.Children[i];

                    if ((string)lCheckBox.Content != EnumArray[i])
                    {
                        throw new Exception("Mismatch between enum and checkboxes!");
                    }

                    if (pDeliveryOptions >= (int)Math.Pow(2, i))
                    {
                        lCheckBox.IsChecked = true;
                        pDeliveryOptions = pDeliveryOptions - (int)Math.Pow(2, i);
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
                DeliveryOptions = GetCheckedList();
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
                    CheckBox lCheckBox = (CheckBox)CheckBoxStack.Children[i];
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

        public DeliveryOptionsControl()
        {
            InitializeComponent();
        }

        public int DeliveryOptions
        {
            get
            {
                return (int)GetValue(DeliveryOptionsControl.DeliveryOptionsProperty);
            }

            set
            {
                SetValue(DeliveryOptionsControl.DeliveryOptionsProperty, value);
            }
        }
    }
}
