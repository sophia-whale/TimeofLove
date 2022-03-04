using Moq;
using System.IO;
using System.Threading.Tasks;
using TabbedTemplate.Services;

namespace TabbedTemplate.UnitTest.Helpers
{
    public static class DiaryStorageHelpers
    {
        /// <summary>
        /// 日程数据库中日程的总数量
        /// </summary>
        public const int NumberNote = 2;

        /// <summary>
        /// 删除数据库文件。
        /// </summary>
        public static void RemoveDatabaseFile() =>
            File.Delete(DiaryStorage.DiaryDbPath);

        /// <summary>
        /// 获得已初始化的事项存储。
        /// </summary>
        public static async Task<DiaryStorage> GetInitializedDiaryStorage()
        {
            var mockPreferenceStorage = new Mock<IPreferenceStorage>();
            mockPreferenceStorage
                .Setup(p => p.Get(DiaryStorageConstants.VersionKey, -1))
                .Returns(1);
            var _diaryStorage =
                 new DiaryStorage(mockPreferenceStorage.Object);
            await _diaryStorage.InitializeAsync();
            return _diaryStorage;

        }
    }
}
