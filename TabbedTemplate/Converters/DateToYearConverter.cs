using System;
using System.Globalization;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.Converters
{
    public class DateToYearConverter : IValueConverter
    {
        /// <param name="value">要转换的值。</param>
        /// <param name="targetType">该值要转换为的类型。</param>
        /// <param name="parameter">转换期间使用的参数。</param>
        /// <param name="culture">转换期间使用的区域性。</param>
        /// <summary>实现此方法，以便通过使用 <paramref name="parameter" /> 和 <paramref name="culture" /> 将 <paramref name="value" /> 转换为 <paramref name="targetType" />。</summary>
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            string date = value.ToString();
            string year = date.Substring(6);
            return year;
        }

        /// <param name="value">要转换的值。</param>
        /// <param name="targetType">该值要转换为的类型。</param>
        /// <param name="parameter">转换期间使用的参数。</param>
        /// <param name="culture">转换期间使用的区域性。</param>
        /// <summary>实现此方法，以便通过使用 <paramref name="parameter" /> 和 <paramref name="culture" /> 将 <paramref name="value" /> 转换回 <paramref name="targetType" />。</summary>
        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new DoNotCallThisException();
        }
    }
}
