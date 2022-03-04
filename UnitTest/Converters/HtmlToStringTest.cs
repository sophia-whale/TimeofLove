using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Converters
{
    public class HtmlToStringTest
    {
        [Test]
        public void TestHtmlToStringConverter()
        {
            string html = "<p>今天<strong>​很好<em>​</em></strong></p><p><strong><em>明天<span style=\"text-decoration: underline;\">​也很好</span></em></strong></p><p><strong><span style=\"text-decoration: underline;\">后天</span></strong>​​<span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\">​也不错</span></p><p><span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\">昨天<span style=\"font-size: 18pt;\">​还可以</span></span></p><p><span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\"><span style=\"background-color: rgb(255, 255, 254);\">​那么</span>​<span style=\"background-color: rgb(255, 217, 0);\">​未来会</span></span></p><ol><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">﻿第一天</span></li><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">第二天</span></li><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">第三天﻿</span><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\"><span style=\"background-color: rgb(255, 255, 254);\"></span></span></li></ol>";
            var htmlStringToStringConverter = new HtmlStringToStringConverter();
            string resultString =
                (string)htmlStringToStringConverter.Convert(html, null, null, null);
            string expectedResult = "今天​很好​明天​也很好后天​​​也不错昨天​还可以​那么​​未来会﻿第一天第二天第三天﻿";
            Assert.AreEqual(expectedResult, resultString);
        }

        [Test]
        public void TestConvertBack()
        {
            var htmlStringToStringConverter = new HtmlStringToStringConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                htmlStringToStringConverter.ConvertBack(null, null, null, null));
        }
    }
}