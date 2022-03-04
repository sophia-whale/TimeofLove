using System;
using System.Collections.Generic;
using System.Text;
using TabbedTemplate.Models;

namespace TabbedTemplate.Models
{
    public class DisplayMemorialDay
    {
        /// <summary>
        /// 纪念日
        /// </summary>
        public MemorialDay MemorialDay { get; set; }
        /// <summary>
        /// 存在的日长
        /// </summary>
        public string duration
        {
            get;
            set;
        }
    }
}
