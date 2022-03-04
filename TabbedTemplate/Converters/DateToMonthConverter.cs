using System;
using System.Globalization;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.Converters
{
    public class DateToMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string date = value.ToString();
            string month = date.Substring(0, 2) + "月";
            return month;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallThisException();
        }
    }
}
