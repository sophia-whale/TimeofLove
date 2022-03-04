using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Utils
{
   
    public class DiaryContentToHtmlTest
    {
        [Test]
        public void ConvertDiaryContentToHtmlTest() {
            Diary diary = new Diary{
                Title = "日记1", Content = "今天下雨", Date = "02/06/2021",
            };
            string str = "<!DOCTYPE html><head><meta charset = \"UTF-8\" ><title>日记1</title></head><body><p><h1 align=\"center\">日记1</h1></p>今天下雨<p><h5 align=\"right\">02/06/2021</h5></p></body></ html>";
            String result = DiaryContentToHtml.ConvertDiaryContentToHtml(diary);
            Assert.AreEqual(str,result);
        }
    }
}
