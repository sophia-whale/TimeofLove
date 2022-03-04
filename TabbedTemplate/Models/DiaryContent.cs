using System;
using System.Collections.Generic;
using System.Text;

namespace TabbedTemplate.Models
{
    /// <summary>
    /// 日记内容类
    /// </summary>
    public class DiaryContent
    {
        /// <summary>
        /// 日记id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 日记主题
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日记标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 日记日期
        /// </summary>
        public string Date { get; set; }
    }
}
