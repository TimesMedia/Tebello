using Subs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Tebello.Presentation
{
    internal class Base
    {
    }

    public class SubscriptionCategory
    {
        public Dictionary<int, string> SubscriptionMedium
        {
            get
            {
                return AdministrationData2.gSubscriptionMedium;
            }
        }

    }


    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(DateTime))
            {
                return String.Empty;
            }
            DateTime lDate;
            lDate = (DateTime)value;
            return lDate.ToString("dd MMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(int))]
    public class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int lint;
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    lint = 0;
                }
                else
                {
                    lint = int.Parse(value.ToString());
                }

                return lint;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Convert", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return 0;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string lResult = value.ToString();
                //lResult = lResult.ToString();
                return lResult;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ConvertBack", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
                return "";
            }
        }
    }




    [ValueConversion(typeof(decimal), typeof(string))]
    public class RandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal lRand;
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                lRand = 0;
            }
            else
            {
                lRand = (decimal)value;
            }

            return lRand.ToString("########0.00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string lValue = value.ToString();
            if (lValue.Length == 0)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(value.ToString());
            }
        }
    }

    [ValueConversion(typeof(int), typeof(Subs.Data.AddressType))]

    public class AddressTypeInt2Type : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (AddressType)Enum.Parse(typeof(AddressType), Enum.GetName(typeof(AddressType), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value;
        }
    }

    [ValueConversion(typeof(int), typeof(Subs.Data.SubStatus))]

    public class SubStatusInt2Type : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (SubStatus)Enum.Parse(typeof(SubStatus), Enum.GetName(typeof(SubStatus), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value;
        }
    }


    [ValueConversion(typeof(int), typeof(string))]

    public class PaymentState2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.GetName(typeof(PaymentData.PaymentState), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [ValueConversion(typeof(int), typeof(Subs.Data.SubscriptionMedium))]

    public class SubscriptionMediumInt2Type : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (SubscriptionMedium)Enum.Parse(typeof(SubscriptionMedium), Enum.GetName(typeof(SubscriptionMedium), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value;
        }
    }

    //[ValueConversion(typeof(int), typeof(string))]

    //public class AddressTypeInt2String : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        string lTest = (Enum.GetName(typeof(AddressType), value));
    //        return Enum.GetName(typeof(AddressType), value);

    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        string lString = (string)value;
    //        AddressType lAddressType = (AddressType)Enum.Parse(typeof(AddressType), lString);
    //        int lInt = (int)lAddressType;
    //        return (int)lAddressType;
    //    }
    //}


    [ValueConversion(typeof(int), typeof(Dictionary<int, string>))]

    public class DeliveryOptionsToDictionary : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string[] EnumArray = Enum.GetNames(typeof(DeliveryMethod));

                Dictionary<int, string> lDictionary = new Dictionary<int, string>();

                int lBitArray = (int)value;
                int lDigits = EnumArray.Length;

                if (lBitArray > (int)Math.Pow(2, EnumArray.Length) - 1)
                {
                    throw new Exception("I cannot handle a number with more than " + lDigits.ToString() + " digits.");
                }

                int lKeyValue = 0;

                while (lBitArray > 0)
                {
                    lKeyValue = (int)Math.Pow(2, lDigits);

                    if (lBitArray >= lKeyValue)
                    {
                        lDictionary.Add(lKeyValue, Enum.GetName(typeof(DeliveryMethod), lKeyValue));
                        lBitArray = lBitArray - lKeyValue;
                    }
                    lDigits--;
                }

                return lDictionary;

            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Convert", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                System.Windows.MessageBox.Show(ex.Message);
                return new Dictionary<int, string>();
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
            //string lString = (string)value;
            //AddressType lAddressType = (AddressType)Enum.Parse(typeof(AddressType), lString);
            //int lInt = (int)lAddressType;
            //return (int)lAddressType;
        }
    }


}
