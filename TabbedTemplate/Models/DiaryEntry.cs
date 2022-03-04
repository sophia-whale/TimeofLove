using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;

namespace TabbedTemplate.Models
{
    public class DiaryEntry
    {
        /// <summary>
        /// 日记的主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 上次更改的时间戳
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string pathsBlobbed { get; set; }

        /// <summary>
        /// 是否需要更新
        /// </summary>
        [Ignore]
        [JsonIgnore]
        public bool Updated { get; private set; }

    }
}
