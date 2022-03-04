using System;
using System.Collections.Generic;
using System.Text;
using TabbedTemplate.Models;

namespace TabbedTemplate.Utils
{
    /// <summary>
    /// 将日记内容转为html字符创
    /// </summary>
    public class DiaryContentToHtml
    {
        public static String ConvertDiaryContentToHtml(Diary diaryContent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "<!DOCTYPE html><head><meta charset = \"UTF-8\" ><title>");
            sb.Append(diaryContent.Title);
            sb.Append("</title></head>");
            sb.Append("<body><p><h1 align=\"center\">");
            sb.Append(diaryContent.Title);
            sb.Append("</h1></p>");
            sb.Append(diaryContent.Content);
            sb.Append("<p><h5 align=\"right\">");
            sb.Append(diaryContent.Date);
            sb.Append("</h5></p></body></ html>");
            return sb.ToString();
        }
    }
}
