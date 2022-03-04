using System;
using System.Globalization;
using System.Threading.Tasks;
using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Converters
{
    public class DateConverterTest
    {
        [Test]
        public void TestDateToYearConverter()
        {
            string date = "07/08/2021";
            var dateToYearConverter = new DateToYearConverter();
            string day = (string)dateToYearConverter.Convert(date, null, null, null);
            Assert.AreEqual("2021", day);
        }

        [Test]
        public void TestDateToYearConvertBack()
        {
            var dateToYearConverter = new DateToYearConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                dateToYearConverter.ConvertBack(null, null, null, null));
        }

        [Test]
        public void TestDateToMonthConverter()
        {
            string date = "08/07/2021";
            var dateToMonthConverter = new DateToMonthConverter();
            string month = (string)dateToMonthConverter.Convert(date, null, null, null);
            Assert.AreEqual("07月", month);
        }

        [Test]
        public void TestDateToMonthConvertBack()
        {
            var dateToMonthConverter = new DateToMonthConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                dateToMonthConverter.ConvertBack(null, null, null, null));
        }

        [Test]
        public void TestDateToDayConverter()
        {
            string date = "08/07/2021";
            var dateToDayConverter = new DateToDayConverter();
            string day = (string)dateToDayConverter.Convert(date, null, null, null);
            Assert.AreEqual("08日", day);
        }

        [Test]
        public void TestDateToDayConvertBack()
        {
            var dateToDayConverter = new DateToDayConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                dateToDayConverter.ConvertBack(null, null, null, null));
        }

        [Test]
        public void TestDayToDurationConverterWithNotString()
        {
            var dayToDurationConverter = new DayToDurationConverter();
            var result = dayToDurationConverter.Convert(null, null, null, null);
            Assert.IsNull(result);
        }

        [Test]
        public void TestDayToDurationConverterWithString()
        {
            string b = "08/07/2021";
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = "dd/MM/yyyy";
            DateTime old = System.Convert.ToDateTime(b, format);
            TimeSpan ts = DateTime.Now - old;
            var dayToDurationConverter = new DayToDurationConverter();
            var duration = dayToDurationConverter.Convert(b, null, null, null);
            Assert.AreEqual(ts.Days.ToString(), duration);
        }

        [Test]
        public void TestDayToDurationConvertBack()
        {
            var dayToDurationConverter = new DayToDurationConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                dayToDurationConverter.ConvertBack(null, null, null, null));
        }
    }
}