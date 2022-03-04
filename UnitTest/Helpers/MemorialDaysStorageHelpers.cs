using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TabbedTemplate.Services;

namespace TabbedTemplate.UnitTest.Helpers
{
    public static class MemorialDaysStorageHelpers
    {
        /// <summary>
        /// 日程数据库中日程的总数量
        /// </summary>
        public const int NumberNote = 4;
        /// <summary>
        /// 删除数据库文件。
        /// </summary>
        public static void RemoveDatabaseFile() =>
            File.Delete(MemorialDayStorage.MemorialDayDbPath);

        /// <summary>
        /// 获得已初始化的事项存储。
        /// </summary>
        public static async Task<MemorialDayStorage> GetInitializedMemorialDaysStorage()
        {
            var _memorialDayStorage =
                new MemorialDayStorage(new Mock<IPreferenceStorage>().Object);
            await _memorialDayStorage.InitializeAsync();
            return _memorialDayStorage;
        }
    }
}
