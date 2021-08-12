using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;

namespace ERP.ValueConverters
{
    class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceDictionary resources = (ResourceDictionary)parameter;

            if ((bool)value == true)
                return resources["1"];
            else
                return resources["2"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class BoolToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceDictionary resources = (ResourceDictionary)parameter;

            if ((bool)value == true)
                return resources["1"];
            else
                return resources["2"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class NumericToColorConveerter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceDictionary resources = (ResourceDictionary)parameter;

            if (value == null || (int)value < 0)
                return resources["1"];
            else
                return resources["2"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class DateTimeToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceDictionary resources = (ResourceDictionary)parameter;

            if ((DateTime)value < DateTime.Now)
                return resources["1"];
            else
                return resources["2"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class MultipleBoolToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            ResourceDictionary resources = (ResourceDictionary)parameter;
            var Results = Array.ConvertAll(values, item => item == DependencyProperty.UnsetValue ? null : (bool?)item);

            if (Results[0] == true && Results[1] == true)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(resources["1"].ToString()));
            else if (Results[0] == true && Results[1] == false)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(resources["2"].ToString()));
            else if (Results[0] == false && Results[1] == true)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(resources["3"].ToString()));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(resources["4"].ToString()));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class NumericToStringConverter : IValueConverter 
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 1)
                return "Address";
            else if ((int)value == 2)
                return "Designation";
            else if ((int)value == 3)
                return "Branch";
            else if ((int)value == 4)
                return "Section";
            else if ((int)value == 5)
                return "Department";
            else if ((int)value == 6)
                return "Grade";
            else if ((int)value == 7)
                return "Pay Method";
            else if ((int)value == 8)
                return "Basic Salary";
            else if ((int)value == 16)
                return "Town";
            else if ((int)value == 18)
                return "NIC";
            else if ((int)value == 19)
                return "Civil Status";
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class GuidToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (((Guid)value) == Guid.Empty)
                    return "Deduction";
                else
                    return "Benefit";
            }
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class MultipleBoolToStringConverterSubtask : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            ResourceDictionary resources = (ResourceDictionary)parameter;
            var Results = Array.ConvertAll(values, item => item == DependencyProperty.UnsetValue ? null : (bool?)item);

            if (Results[0] == false && Results[1] == true)
                return "Finished";
            else if (Results[0] == true && Results[1] == false)
                return "Active";
            else
                return "Inactive";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class MultipleBoolToStringConverterSubtaskEmployee : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            ResourceDictionary resources = (ResourceDictionary)parameter;
            var Results = Array.ConvertAll(values, item => item == DependencyProperty.UnsetValue ? null : (bool?)item);

            if (Results[0] == true && Results[1] == false && Results[2] == false && Results[3] == false && Results[4] == false && Results[5] == false && Results[6] == false)
                return "Active";
            else if (Results[0] == true && Results[1] == true && Results[2] == false && Results[3] == false && Results[4] == false && Results[5] == false && Results[6] == false)
                return "Accepted";
            else if (Results[0] == true && Results[1] == false && Results[2] == true && Results[3] == false && Results[4] == false && Results[5] == false && Results[6] == false)
                return "Rejected";
            else if (Results[3] == true)
                return "Awaiting";
            else if (Results[4] == true && Results[5] == false)
                return "Expired";
            else if (Results[4] == true && Results[5] == true)
                return "Removed";
            else if (Results[4] == false && Results[5] == true)
                return "Removed";
            else if (Results[6] == true)
                return "Finished";
            else
                return "Active";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
