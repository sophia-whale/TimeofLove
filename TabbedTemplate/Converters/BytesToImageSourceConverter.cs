using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.Converters
{
    /// <summary>
    /// 比特数组到图片源转换器。
    /// </summary>
    public class BytesToImageFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return retSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new DoNotCallThisException();
        }
    }
}