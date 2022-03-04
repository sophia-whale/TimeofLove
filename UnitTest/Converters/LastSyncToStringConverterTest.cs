using System;
using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Converters {
    public class LastSyncToStringConverterTest {
        [Test]
        public void TestLastSyncToStringConverter()
        {
            var lastSyncToStringConverter = new LastSyncToStringConverter();
            //not dateTime
            var resultNull =
                lastSyncToStringConverter.Convert("time", null, null, null);
            Assert.IsNull(resultNull);

            //0
            var resultZero =
                lastSyncToStringConverter.Convert(DateTime.Now, null, null,
                    null);
            Assert.AreEqual("今天刚刚成功地同步过", resultZero);

            //<100
            DateTime downDateTime = DateTime.Today.AddDays(-20);
            var resultDownHundred =
                lastSyncToStringConverter.Convert(downDateTime, null, null,
                    null);
            Assert.AreEqual("距离上一次成功同步已经有20天。", resultDownHundred);

            //>100
            DateTime upDateTime = DateTime.Today.AddDays(-100);
            var resultUpHundred =
                lastSyncToStringConverter.Convert(upDateTime, null, null, null);
            Assert.AreEqual("你已经很久没有同步过了", resultUpHundred);
        }

        [Test]
        public void TestConvertBack()
        {
            var lastSyncToStringConverter = new LastSyncToStringConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                lastSyncToStringConverter.ConvertBack(null, null, null, null));
        }
    }
}