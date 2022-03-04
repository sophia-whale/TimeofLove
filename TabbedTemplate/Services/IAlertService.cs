using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TabbedTemplate.Services
{
    public interface IAlertService
    {
        /// <summary>
        /// 显示警告
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">信息</param>
        /// <param name="button">按钮</param>
        void ShowAlert(string title, string message, string button);

        public  Task<string> SelectAlert(string title, string cancel,
            string destruction, params string[] buttons);


    }
}
