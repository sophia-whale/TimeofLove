using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.Converters
{
    /// <summary>
    /// ItemTappedEventArgs到展示纪念日转换器
    /// </summary>
    public class DayToDurationConverter : IValueConverter
    {
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <summary>Implement this method to convert <paramref name="value" /> to <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (!(value is string b))
            {
                return null;
            }
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = "MM/dd/yyyy HH:mm:ss";
            DateTime old = System.Convert.ToDateTime(b, format);
            TimeSpan ts = DateTime.Now - old;
            var duration = ts.Days.ToString();
            return duration;
        }


        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <summary>Implement this method to convert <paramref name="value" /> back from <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new DoNotCallThisException();
        }
    }
}

