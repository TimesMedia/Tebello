using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Subs.Presentation
{
    public partial class CorrespondenceControl : UserControl
    {
        public static readonly DependencyProperty CorrespondenceProperty;

        static CorrespondenceControl()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(CorrespondenceChanged));
            CorrespondenceControl.CorrespondenceProperty = DependencyProperty.Register("Correspondence", typeof(int), typeof(CorrespondenceControl), lMetaData);
        }

        private static void CorrespondenceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            CorrespondenceControl lControl = (CorrespondenceControl)o;
            lControl.SetCheckedList((int)e.NewValue);
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Correspondence = GetCheckedList();
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
                for (int i = 0; i <= 3; i++)
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

        private void SetCheckedList(int pCorrespondence)
        {
            if (pCorrespondence > (int)Math.Pow(2, 4) - 1)
            {
                throw new Exception("I cannot handle a number of that size.");
            }

            for (int i = 3; i >= 0; i--)
            {
                CheckBox lCheckBox = (CheckBox)CheckBoxStack.Children[i];

                if (pCorrespondence >= (int)Math.Pow(2, i))
                {
                    lCheckBox.IsChecked = true;
                    pCorrespondence = pCorrespondence - (int)Math.Pow(2, i);
                }
                else
                {
                    lCheckBox.IsChecked = false;
                }
            }
        }

        public CorrespondenceControl()
        {
            InitializeComponent();
        }

        public int Correspondence
        {
            get
            {
                return (int)GetValue(CorrespondenceControl.CorrespondenceProperty);
            }

            set
            {
                SetValue(CorrespondenceControl.CorrespondenceProperty, value);
            }

        }



    }
}