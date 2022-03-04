using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TabbedTemplate.Models;
namespace TabbedTemplate.Utils
{
    public class HtmlToDairyContent
    {
        public static DiaryContent convertHtmlToDiaryContent(string str)
        {
            string title = "";

            string pattern = @"(?si)<title(?:\s+(?:""[^""]*""|'[^']*'|[^""'>])*)?>(?<title>.*?)</title>";
            title = Regex.Match(str, pattern).Groups["title"].Value.Trim();
            Regex reg = new Regex("(?is)<body[^>]*>(?<body>.*?)</body>");
            string bodyHtml = "";
            bodyHtml = reg.Match(str).Groups["body"].Value;
            bodyHtml = Regex.Replace(bodyHtml, "<p><h1 align=\"center\">(.*?)</h1></p>", "");
            var matches = Regex.Matches(bodyHtml, "<p><h5 align=\"right\">(.*?)</h5></p>", RegexOptions.Singleline);
            var date = matches[0].Value;
            bodyHtml = Regex.Replace(bodyHtml, "<p><h5 align=\"right\">(.*?)</h5></p>", "");
            string pattern1 = "<(.+?)>";
            string replacement = "";
            date = Regex.Replace(date, pattern1, replacement);
            return new DiaryContent
            {
                Title = title,
                Content = bodyHtml,
                Date = date,
            };

        }
    }
}
