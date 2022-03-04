using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 诗词存储接口。
    /// </summary>
    public interface IDiaryStorage
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
        /// 获取一则日记。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Diary> GetDiaryAsync(int id);

        /// <summary>
        /// 获取所有日记。
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IList<Diary>> GetAllDiariesAsync(int skip, int take);

        /// <summary>
        /// 获取所有日记，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        public Task<IList<Diary>> GetDiaryItemsAsync();

        /// <summary>
        /// 保存一则日记。
        /// </summary>
        /// <param name="diary"></param>
        /// <returns></returns>
        Task SaveDiaryAsync(Diary diary, int flag, bool addTimeStamp);

        /// <summary>
        /// 收藏存储已更新事件。
        /// </summary>
        event EventHandler<DiaryStorageUpdatedEventArgs> Updated;

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        /// <returns></returns>
        Task CloseDataFile();
    }


    public static class DiaryStorageConstants
    {
        //name of 字符串常量充分反应字符串功能，方便重命名
        /// <summary>
        ///版本键
        /// </summary>
        public const string VersionKey = nameof(DiaryStorageConstants) + "." + nameof(Version);
        /// <summary>
        /// 版本号
        /// </summary>
        public const int Version = 1;
    }

    public class DiaryStorageUpdatedEventArgs : EventArgs
    {

        public Diary UpdatedDiary { get; }
        public int Flag { get; }

        public DiaryStorageUpdatedEventArgs(Diary diary, int flag)
        {
            UpdatedDiary = diary;
            Flag = flag;
        }
    }
}