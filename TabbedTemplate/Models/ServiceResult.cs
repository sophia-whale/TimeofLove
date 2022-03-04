using System;
using System.Collections.Generic;
using System.Text;

namespace TabbedTemplate.Models
{
    /// <summary>
    /// 服务结果
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 服务结果状态。
        /// </summary>
        public ServiceResultStatus Status { get; set; }

        /// <summary>
        /// 信息。
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 服务结果状态。
    /// </summary>
    public enum ServiceResultStatus
    {
        Ok,
        Exception
    }
}
