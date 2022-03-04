using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;

namespace TabbedTemplate.UnitTest.Services
{
    /// <summary>
    /// 日记存储测试类。
    /// </summary>
    public class DiaryStorageTest
    {
        /// <summary>
        /// 删除数据库文件
        /// </summary>
        [SetUp, TearDown]
        public static void RemoveDatabaseFile() =>
            DiaryStorageHelpers.RemoveDatabaseFile();

        /// <summary>
        /// 测试初始化。
        /// </summary>
        [Test]
        public async Task TextInitializeAsync()
        {
            Assert.IsFalse(File.Exists(DiaryStorage.DiaryDbPath));
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;

            var diaryStorage = new DiaryStorage(mockPreferenceStorage);
            await diaryStorage.InitializeAsync();
            Assert.IsTrue(File.Exists(DiaryStorage.DiaryDbPath));
        }

        /// <summary>
        /// 测试未初始化
        /// </summary>
        [Test]
        public void TestNoInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock.Setup(p => p.Get(DiaryStorageConstants.VersionKey, -1)).Returns(DiaryStorageConstants.Version - 1);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var diaryStorage = new DiaryStorage(mockPreferenceStorage);
            Assert.IsFalse(diaryStorage.Initialized());
        }

        /// <summary>
        /// 测试已初始化。
        /// </summary>
        [Test]
        public void TestInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock
                .Setup(p => p.Get(DiaryStorageConstants.VersionKey, -1))
                .Returns(DiaryStorageConstants.Version);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var diaryStorage = new DiaryStorage(mockPreferenceStorage);
            Assert.IsTrue(diaryStorage.Initialized());
            preferenceStorageMock.Verify(p =>
                p.Get(DiaryStorageConstants.VersionKey, -1), Times.Once);
        }

        /// <summary>
        /// 获取一则日记。
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetDiaryAsync()
        {
            var diaryStorage =
                await DiaryStorageHelpers.GetInitializedDiaryStorage();
            var diary = await diaryStorage.GetDiaryAsync(1);
            Assert.AreEqual("日记1", diary.Title);
            await diaryStorage.CloseDataFile();
        }

        /// <summary>
        /// 测试读取所有日记。
        /// </summary>
        [Test]
        public async Task TestAllNoteAsync()
        {
            var diaryStorage = await DiaryStorageHelpers
                .GetInitializedDiaryStorage();
            var where = Expression.Lambda<Func<Diary, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Diary), "p"));
            var diaries = await diaryStorage.GetAllDiariesAsync(0, int.MaxValue);
            Assert.AreEqual(DiaryStorageHelpers.NumberNote, diaries.Count);
            await diaryStorage.CloseDataFile();
        }

        /// <summary>
        /// 测试读取所有日记包括删除和未删除
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetDiaryItemsAsync() {
            var diaryStorage =
                await DiaryStorageHelpers.GetInitializedDiaryStorage();
            var diaries = await diaryStorage.GetDiaryItemsAsync();
            Assert.AreEqual(DiaryStorageHelpers.NumberNote,diaries.Count);
            await diaryStorage.CloseDataFile();
        }


        /// <summary>
        /// 测试插入数据
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestSaveNoteAsync()
        {
            var updated = false;
            Diary updatedDiary = null;
            int flag = 0;
            var diaryStorage = await DiaryStorageHelpers
                .GetInitializedDiaryStorage();
            var where = Expression.Lambda<Func<Diary, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Diary), "p"));
            var oldDiaries = await diaryStorage.GetAllDiariesAsync(0, int.MaxValue);
            var num = oldDiaries.Count;
            var diary = new Diary
            {
                Title = "日记2",
                Content = "胜利在即",
                Date = "12/09/2021"
            };
            diaryStorage.Updated += (sender, args) =>
            {
                updated = true;
                updatedDiary = args.UpdatedDiary;
                flag = args.Flag;
            };
            await diaryStorage.SaveDiaryAsync(diary, 0, true);
            var currentDiaies = await diaryStorage.GetAllDiariesAsync(0, int.MaxValue);
            Assert.AreEqual(currentDiaies[num].Content, diary.Content);
            Assert.AreEqual(currentDiaies[num].Title, diary.Title);
            Assert.AreEqual(currentDiaies[num].Date, diary.Date);
            Assert.AreEqual(currentDiaies.Count, num + 1);
            Assert.IsTrue(updated);
            Assert.AreSame(diary, updatedDiary);
            Assert.AreEqual(flag, 0);
            var diaryUpdated = new Diary {
                Id=2,
                Title = "日记2", 
                Content = "继续加油", 
                Date = "12/09/2021",
            };
            await diaryStorage.SaveDiaryAsync(diaryUpdated, 1, true);
            currentDiaies =
                await diaryStorage.GetAllDiariesAsync(0, int.MaxValue);
            Assert.AreEqual(currentDiaies.Count,num+1);
            //Assert.AreEqual(currentDiaies[num].Content,diaryUpdated.Content);
            Assert.AreEqual(flag,1);
            Assert.AreEqual(updatedDiary,diaryUpdated);
            await diaryStorage.CloseDataFile();
        }
    }
}
