using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public class DiaryStorage : IDiaryStorage
    {

        public const string DbName = "kuwords.sqlite3";
        ///<summary>
        ///数据库路径
        /// </summary>
        public static readonly string DiaryDbPath =
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
            (_connection = new SQLiteAsyncConnection(DiaryDbPath));

        /// <summary>
        /// 偏好存储。
        /// </summary>
        private IPreferenceStorage _preferenceStorage;
        /// <summary>
        /// 笔记存储。
        /// </summary>
        /// <param name="preferenceStorage">偏好存储。</param>
        public DiaryStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }

        ///<summary>
        ///是否初始化。
        /// </summary>
        public bool Initialized()
        {
            var b = DiaryStorageConstants.VersionKey;
            var a = _preferenceStorage.Get(DiaryStorageConstants.VersionKey, -1) ==
                 DiaryStorageConstants.Version;
            return a;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// 
        public async Task InitializeAsync()
        {
            //if (!File.Exists(DiaryDbPath)) {
            using (var dbFileStream =
                new FileStream(DiaryDbPath, FileMode.OpenOrCreate))
            using (var dbAssetStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(DbName))
            {
                await dbAssetStream.CopyToAsync(dbFileStream);
            }
            //}

            _preferenceStorage.Set(DiaryStorageConstants.VersionKey, DiaryStorageConstants.Version);
        }

        /// <summary>
        /// 获取一个日记。
        /// </summary>
        /// <param name="id">日记id。</param>
        public async Task<Diary> GetDiaryAsync(int id) =>
            await Connection.Table<Diary>()
                .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// 获取所有日记。
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IList<Diary>> GetAllDiariesAsync(int skip, int take) =>
            await Connection.Table<Diary>().Where(p => p.IsDeleted == 0).Skip(skip).Take(take)
            .ToListAsync();


        public event EventHandler<DiaryStorageUpdatedEventArgs> Updated;

        /// <summary>
        /// 保存一则日记。
        /// </summary>
        /// <param name="diary"></param>
        public async Task SaveDiaryAsync(Diary diary, int flag, bool generateTimeStamp)
        {
            if (generateTimeStamp)
            {
                diary.timeStamp = DateTime.Now.Ticks;
            }

            if (flag == 0)
            {
                await Connection.InsertAsync(diary);
            }
            else
            {
                await Connection.InsertOrReplaceAsync(diary);
            }
            Updated?.Invoke(this, new DiaryStorageUpdatedEventArgs(diary, flag));
        }

        /// <summary>
        /// 获取所有日记（包括删除和未删除）
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Diary>> GetDiaryItemsAsync() =>
            await Connection.Table<Diary>().ToListAsync();

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        /// <returns></returns>
        public async Task CloseDataFile() => await Connection.CloseAsync();
    }
}

