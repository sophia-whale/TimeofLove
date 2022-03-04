using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.Converters
{
    public class LastSyncToStringConverter : IValueConverter
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
            if (!(value is DateTime time))
                return null;
            var timeSpan = DateTime.Now - time;
            if (timeSpan.Days == 0)
            {
                return "今天刚刚成功地同步过";
            }
            else if (timeSpan.Days < 100)
            {
                return $"距离上一次成功同步已经有{timeSpan.Days}天。";
            }
            else
            {
                return "你已经很久没有同步过了";
            }
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
