using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;
using TabbedTemplate.Services;

namespace TabbedTemplate.Models
{
    /// <summary>
    /// 可追踪纪念日
    /// </summary>
    [Table("memorialDays")]
    public class TrackableMemorialDay : MemorialDay
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
        public override long Timestamp
        {
            get => base.Timestamp;
            set
            {
                base.Timestamp = value;
                Updated = true;
            }
        }



        [Ignore]
        [JsonIgnore]
        public bool Updated { get; private set; }

        /// <summary>
        /// 可追踪纪念日。
        /// </summary>
        /// <param name="">收藏。</param>
        public TrackableMemorialDay(MemorialDay memorialDay)
        {
            Id = memorialDay.Id;
            StartDate = memorialDay.StartDate;
            Title = memorialDay.Title;
            CreateDate = memorialDay.CreateDate;
            base.Timestamp = memorialDay.Timestamp;
            base.IsDeleted = memorialDay.IsDeleted;
        }

        /// <summary>
        /// 克隆为已更新的可追踪纪念日
        /// </summary>
        /// <returns></returns>
        public TrackableMemorialDay CloneAsUpdated()
        {
            return new TrackableMemorialDay(this) { Updated = true };
        }
    }
}
