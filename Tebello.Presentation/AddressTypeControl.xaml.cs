using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Subs.Presentation
{
    /// <summary>
    /// Interaction logic for AddressTypeControl.xaml
    /// </summary>
    public partial class AddressTypeControl : UserControl
    {
        #region Addresstype property - static and public

        public static readonly DependencyProperty AddressTypeProperty;

        public AddressType AddressType
        {
            get
            {
                return (AddressType)GetValue(AddressTypeControl.AddressTypeProperty);
            }

            set
            {
                SetValue(AddressTypeControl.AddressTypeProperty, value);
            }

        }

        #endregion

        #region AddressTypeChanged event

        public static readonly RoutedEvent RadioButtonChangedEvent = EventManager.RegisterRoutedEvent("RadioButtonChanged",
                                                                                                       RoutingStrategy.Bubble,
                                                                                                       typeof(RoutedEventHandler),
                                                                                                       typeof(AddressTypeControl));

        public event RoutedEventHandler RadioButtonChanged  // Wrapper
        {
            add
            {
                this.AddHandler(RadioButtonChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(RadioButtonChangedEvent, value);
            }
        }

        #endregion


        #region Constructors - static and instance

        static AddressTypeControl()
        {
            FrameworkPropertyMetadata lMetaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(AddressTypeChanged));
            AddressTypeControl.AddressTypeProperty = DependencyProperty.Register("AddressType", typeof(AddressType), typeof(AddressTypeControl), lMetaData);

        }


        public AddressTypeControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Change from dependency property to embedded controls

        private static void AddressTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AddressTypeControl lControl = (AddressTypeControl)o;
            lControl.SetControl((AddressType)e.NewValue);
        }

        public void SetControl(AddressType pAddressType)
        {
            try
            {
                switch (pAddressType)
                {
                    case AddressType.PostBox:
                        radioBox.IsChecked = true;
                        break;
                    case AddressType.PostStreet:
                        radioStreet.IsChecked = true;
                        break;
                    case AddressType.International:
                        radioInternational.IsChecked = true;
                        break;
                    case AddressType.UnAssigned:
                        radioUnassigned.IsChecked = true;
                        break;
                    default:
                        throw new Exception("Unknown Address Type");
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
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SetControl", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("SetControl error; " + ex.Message);
            }
        }

        #endregion

        #region Change from embedded controls to dependency property

        private void Click_RadioButton(object sender, RoutedEventArgs e)
        {
            RadioButton lButton = (RadioButton)e.OriginalSource;
            string lName = lButton.Name;

            switch (lName)
            {
                case "radioBox":
                    AddressType = AddressType.PostBox;
                    break;
                case "radioStreet":
                    AddressType = AddressType.PostStreet;
                    break;
                case "radioInternational":
                    AddressType = AddressType.International;
                    break;
                case "radioUnassigned":
                    AddressType = AddressType.UnAssigned;
                    break;
                default:
                    throw new Exception("Unknown radio button");
            }

            // Raise an event to let the container know that the address type changed.

            RoutedEventArgs lEventArg = new RoutedEventArgs(RadioButtonChangedEvent);
            RaiseEvent(lEventArg);
        }
        #endregion

    }
}
