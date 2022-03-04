using System;
using System.Collections.Generic;
using System.Text;

namespace TabbedTemplate.Utils
{
    /// <summary>
    /// 错误信息。
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// HttpClient错误标题。
        /// </summary>
        public const string HTTP_CLIENT_ERROR_TITLE = "连接错误";

        /// <summary>
        /// 获得HttpClient错误信息。
        /// </summary>
        /// <param name="server">服务器。</param>
        /// <param name="message">错误信息。</param>
        public static string HttpClientErrorMessage(string server,
            string message) =>
            string.Format($"与{server}连接时发生了错误：\n{message}");

        /// <summary>
        /// HttpClient错误按钮。
        /// </summary>
        public const string HTTP_CLIENT_ERROR_BUTTON = "确定";
    }
}
