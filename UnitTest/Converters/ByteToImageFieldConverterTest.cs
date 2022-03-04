using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FFImageLoading;
using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.UnitTest.Converters
{
    public class ByteToImageFieldConverterTest
    {
        [Test]
        public void TestConvertBack()
        {
            var bytesToImageSourceConverter = new BytesToImageFieldConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                bytesToImageSourceConverter.ConvertBack(null, null, null,
                    null));
        }

        [Test]
        public async Task TestBytesToImageFieldConverterWithNull()
        {
            var bytesToImageFieldConverter = new BytesToImageFieldConverter();
            var resultImageSource =
                bytesToImageFieldConverter.Convert(null, null, null,
                    null);
            Assert.IsNull(resultImageSource);
        }

        [Test]
        public async Task TestBytesToImageFieldConverterWithNotNull() {
           Stream imageStream = await LoadImageHelpers.LoadImageResourceAsync("","sea.jpg");
           byte[] imageBytes = imageStream.ToByteArray();
           var expectedImageSource = ImageSource.FromResource("TabbedTemplate.Resources.sea.jpg");
           var bytesToImageFieldConverter = new BytesToImageFieldConverter();
           var resultImageSource =
               (ImageSource)bytesToImageFieldConverter.Convert(imageBytes, null, null,
                   null);
            Assert.AreEqual(expectedImageSource.ToString(), resultImageSource.ToString());
        }
    }
}