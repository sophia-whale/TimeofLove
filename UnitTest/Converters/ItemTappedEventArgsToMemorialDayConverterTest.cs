using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;
using Xamarin.Forms;

namespace TabbedTemplate.UnitTest.Converters {
    public class ItemTappedEventArgsToMemorialDayConverterTest {
        [Test]
        public void TestConvertBack()
        {
            var itemTappedEventArgsToMemorialDayConverter =
                new ItemTappedEventArgsToMemorialDayConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                itemTappedEventArgsToMemorialDayConverter.ConvertBack(null,
                    null, null, null));
        }

        [Test]
        public void TestConvertFailed()
        {
            var itemTappedEventArgsToMemorialDayConverter =
                new ItemTappedEventArgsToMemorialDayConverter();
            Assert.IsNull(
                itemTappedEventArgsToMemorialDayConverter.Convert(
                    new object(), null, null, null));

            var itemTappedEventArgsToConvert =
                new ItemTappedEventArgs(new object(), null, -1);
            Assert.IsNull(
                itemTappedEventArgsToMemorialDayConverter.Convert(
                    itemTappedEventArgsToConvert, null, null, null));
        }

        [Test]
        public void TestConvertSucceeded()
        {
            var itemTappedEventArgsToMemorialDayConverter =
                new ItemTappedEventArgsToMemorialDayConverter();
            var memorialDayToReturn = new MemorialDay();
            var itemTappedEventArgsToConvert =
                new ItemTappedEventArgs(null, memorialDayToReturn, -1);
            Assert.AreSame(memorialDayToReturn,
                itemTappedEventArgsToMemorialDayConverter.Convert(
                    itemTappedEventArgsToConvert, null, null, null));
        }
    }
}