using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public interface ISyncService : INotifyStatusChanged
    {
        /// <summary>
        /// 同步
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult> SyncAsync();



        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        Task<bool> IsSignedInAsync();

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        Task<bool> SignInAsync();

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task SignOutAsync();
    }
}
