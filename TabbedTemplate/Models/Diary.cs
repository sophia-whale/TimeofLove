using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TabbedTemplate.Models
{
    [SQLite.Table("diaries")]
    public class Diary : ObservableObject
    {
        /// <summary>
        /// 主键。
        /// </summary>
        [PrimaryKey, AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 日记标题。
        /// </summary>
        [SQLite.Column("title")]
        public string Title { get; set; }

        /// <summary>
        /// 文字内容。
        /// </summary>
        [SQLite.Column("content")]
        public string Content { get; set; }

        /// <summary>
        /// 日期。
        /// </summary>
        [SQLite.Column("date")]
        public string Date { get; set; }

        /// <summary>
        /// 是否删除。
        /// </summary>
        [SQLite.Column("isDeleted")]
        public virtual int IsDeleted { get; set; }

        [SQLite.Column("timeStamp")]
        public virtual long timeStamp { get; set; }

        ///// <summary>
        ///// 存储图片路径
        ///// </summary>
        //[TextBlob("pathsBlobbed")]
        //public List<String> paths { get; set; }

        ///// <summary>
        ///// 存储序列化对象
        ///// </summary>
        //public string pathsBlobbed { get; set; }

        [SQLite.Column("pathsBlobbed")]
        public string pathsBlobbed { get; set; }

        [Ignore]
        public List<string> paths
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(pathsBlobbed);
            }
            set
            {
                pathsBlobbed = JsonConvert.SerializeObject(value);
            }
        }


    }
}
