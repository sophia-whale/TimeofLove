using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DeferredEvents;
using TabbedTemplate.Models;
using Xamarin.Essentials;

namespace TabbedTemplate.Services
{
    public class MemorialDayStorage : IMemorialDayStorage
    {

        public const string DbName = "memorialdays.sqlite3";

        public static readonly string MemorialDayDbPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), DbName);

        // ********* 私有变量
        /// <summary>
        /// 数据库连接影子变量
        /// </summary>
        private SQLiteAsyncConnection _connection;

        /// <summary>
        /// 数据库连接。
        /// </summary>
        private SQLiteAsyncConnection Connection =>
            _connection ??
            (_connection = new SQLiteAsyncConnection(MemorialDayDbPath));

        /// <summary>
        /// 偏好存储。
        /// </summary>
        private IPreferenceStorage _preferenceStorage;
        /// <summary>
        /// 笔记存储。
        /// </summary>
        /// <param name="preferenceStorage">偏好存储。</param>
        public MemorialDayStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }
        /// <summary>
        /// 是否初始化。
        /// </summary>
        /// <returns></returns>
        public bool Initialized() =>
            _preferenceStorage.Get(MemorialStorageConstants.VERSION_KEY, -1) ==
            MemorialStorageConstants.VERSION;

        /// <summary>
        /// 初始化。
        /// </summary>
        public async Task InitializeAsync()
        {
            using (var dbFileStream =
                new FileStream(MemorialDayDbPath, FileMode.OpenOrCreate))
            using (var dbAssetStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(DbName))
            {
                await dbAssetStream.CopyToAsync(dbFileStream);
            }
            _preferenceStorage.Set(MemorialStorageConstants.VERSION_KEY,
                MemorialStorageConstants.VERSION);
        }

        /// <summary>
        /// 获取一个纪念日
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MemorialDay> GetMemorialDayAsync(int id) =>
            await Connection.Table<MemorialDay>()
                .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// 获取满足给定条件的纪念日集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IList<MemorialDay>> GetMemorialDaysAsync(int skip,
            int take) => await Connection.Table<MemorialDay>()
                .Where(p => p.IsDeleted == 0).Take(take).Skip(skip).ToArrayAsync();

        /// <summary>
        /// 获取所有纪念日项，包括删除与未删除。
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MemorialDay>> GetMemorialDayItemsAsync() =>
            await Connection.Table<MemorialDay>().ToListAsync();



        public event EventHandler<MemorialStorageUpdatedEventArgs> Updated;



        /// <summary>
        /// 保存一个纪念日。
        /// </summary>
        /// <param name="memorialDay"></param>
        /// <returns></returns>
        public async Task SaveMemorialDayAsync(MemorialDay memorialDay, int flag, bool generateTimeStamp)
        {
            if (generateTimeStamp)
            {
                memorialDay.Timestamp = DateTime.Now.Ticks;
            }
            if (flag == 0)
            {
                await Connection.InsertAsync(memorialDay);
            }
            else
            {
                await Connection.InsertOrReplaceAsync(memorialDay);
            }
            Updated?.Invoke(this, new MemorialStorageUpdatedEventArgs(memorialDay, flag));
        }

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        /// <returns></returns>
        public async Task CloseDataFile() => await Connection.CloseAsync();


    }


}
