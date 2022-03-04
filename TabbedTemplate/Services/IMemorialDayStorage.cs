using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DeferredEvents;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public interface IMemorialDayStorage
    {

        /// <summary>
        /// 是否初始化。
        /// </summary>
        /// <returns></returns>
        bool Initialized();

        /// <summary>
        /// 初始化。
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取一个纪念日
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MemorialDay> GetMemorialDayAsync(int id);

        /// <summary>
        /// 获取所有纪念日
        /// </summary>
        /// <param name="where">Where条件</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IList<MemorialDay>> GetMemorialDaysAsync(int skip, int take);

        /// <summary>
        /// 获取所有纪念日项，包括删除与未删除。
        /// </summary>
        /// <returns></returns>
        Task<IList<MemorialDay>> GetMemorialDayItemsAsync();

        /// <summary>
        /// 保存一个纪念日。
        /// </summary>
        /// <param name="memorialDay"></param>
        /// <returns></returns>
        Task SaveMemorialDayAsync(MemorialDay memorialDay, int flag, bool generateTimeStamp);


        /// <summary>
        /// 收藏存储已更新事件。
        /// </summary>
        event EventHandler<MemorialStorageUpdatedEventArgs> Updated;

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        /// <returns></returns>
        Task CloseDataFile();

    }

    public static class MemorialStorageConstants
    {
        public const string VERSION_KEY =
            nameof(IMemorialDayStorage) + "." + nameof(VERSION);

        public const int VERSION = 1;
    }

    public class MemorialStorageUpdatedEventArgs : EventArgs
    {

        public MemorialDay UpdatedMemorialDay { get; }
        public int Flag { get; }

        public MemorialStorageUpdatedEventArgs(MemorialDay memorialDay, int flag)
        {
            UpdatedMemorialDay = memorialDay;
            Flag = flag;
        }
    }

}

