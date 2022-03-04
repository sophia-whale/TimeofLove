using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;
using TabbedTemplate.Services;

namespace TabbedTemplate.Models
{
    [Table("diaries")]
    public class TrackableDiary : Diary
    {
        /// <summary>
        /// 是否删除。
        /// </summary>
        public override int IsDeleted
        {
            get => base.IsDeleted;
            set
            {
                base.IsDeleted = value;
                Updated = true;
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public override long timeStamp
        {
            get => base.timeStamp;
            set
            {
                base.timeStamp = value;
                Updated = true;
            }
        }

        [Ignore]
        [JsonIgnore]
        public bool Updated { get; set; }

        /// <summary>
        /// 标记是否编辑
        /// </summary>
        [Ignore]
        [JsonIgnore]
        public bool Edit { get; set; }

        public TrackableDiary(Diary diary)
        {
            Id = diary.Id;
            Date = diary.Date;
            Title = diary.Title;
            Content = diary.Content;
            pathsBlobbed = diary.pathsBlobbed;
            base.IsDeleted = diary.IsDeleted;
            base.timeStamp = diary.timeStamp;

        }
    }
}
