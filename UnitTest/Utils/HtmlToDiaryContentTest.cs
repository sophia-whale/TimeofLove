using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Helpers
{
    public class HtmlToDiaryContentTest
    {
        [Test]
        public void convertHtmlToDiaryContentTest() {
            string str =
                "<!DOCTYPE html><head><meta charset = \"UTF-8\" ><title>日记1</title></head><body><p><h1 align=\"center\">日记1</h1></p>今天下雨<p><h5 align=\"right\">02/06/2021</h5></p></body></ html>";
            DiaryContent newDiary =
                HtmlToDairyContent.convertHtmlToDiaryContent(str);
            DiaryContent fact = new DiaryContent {
                Title = "日记1", Content = "今天下雨", Date = "02/06/2021",
            };
            Assert.AreEqual(newDiary.Title,fact.Title);
            Assert.AreEqual(newDiary.Content, fact.Content);
            Assert.AreEqual(newDiary.Date, fact.Date);

        }


    }
}
