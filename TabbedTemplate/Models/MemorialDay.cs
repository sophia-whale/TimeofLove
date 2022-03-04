using GalaSoft.MvvmLight;
using SQLite;

namespace TabbedTemplate.Models
{

    [SQLite.Table("memorialDays")]
    public class MemorialDay : ObservableObject
    {
        /// <summary>
        /// 主键。
        /// </summary>
        [PrimaryKey, AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 纪念日标题。
        /// </summary>
        [SQLite.Column("title")]
        public string Title { get; set; }

        /// <summary>
        /// 纪念日开始日期。
        /// </summary>
        [SQLite.Column("startDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// 创建纪念项日期。
        /// </summary>
        [SQLite.Column("createDate")]
        public string CreateDate { get; set; }

        /// <summary>
        /// 是否删除。
        /// </summary>
        [SQLite.Column("isDeleted")]
        public virtual int IsDeleted { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [SQLite.Column("timeStamp")]
        public virtual long Timestamp { get; set; }
    }
}
