using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;

namespace TabbedTemplate.UnitTest.Services
{
    /// <summary>
    /// 纪念日存储类测试
    /// </summary>
    public class MemorialDaysStorageTest
    {
        /// <summary>
        /// 删除数据库文件
        /// </summary>
        [SetUp, TearDown]
        public static void RemoveDatabaseFile() =>
            MemorialDaysStorageHelpers.RemoveDatabaseFile();

        /// <summary>
        /// 测试初始化。
        /// </summary>
        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(MemorialDayStorage.MemorialDayDbPath));
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;

            var memorialDayStorage = new MemorialDayStorage(mockPreferenceStorage);
            await memorialDayStorage.InitializeAsync();
            Assert.IsTrue(File.Exists(MemorialDayStorage.MemorialDayDbPath));
        }
        /// <summary>
        /// 测试未初始化
        /// </summary>
        [Test]
        public void TestNotInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock.Setup(p => p.Get(MemorialStorageConstants.VERSION_KEY, -1)).Returns(MemorialStorageConstants.VERSION - 1);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var memorialDayStorage = new MemorialDayStorage(mockPreferenceStorage);
            Assert.IsFalse(memorialDayStorage.Initialized());
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
        /// 测试读取所有纪念日。
        /// </summary>
        [Test]
        public async Task TestGetMemorialDaysAsync()
        {
            var memorialDayStorage = await MemorialDaysStorageHelpers
                .GetInitializedMemorialDaysStorage();
            var where = Expression.Lambda<Func<MemorialDay, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(MemorialDay), "p"));
            var days = await memorialDayStorage.GetMemorialDaysAsync(0, int.MaxValue);
            Assert.AreEqual(MemorialDaysStorageHelpers.NumberNote, days.Count);
            await memorialDayStorage.CloseDataFile();
        }

        /// <summary>
        /// 测试获取所有纪念日，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetMemorialDayItemsAsync() {
            var memorialDayStorage = await MemorialDaysStorageHelpers
                .GetInitializedMemorialDaysStorage();
            var days = await memorialDayStorage.GetMemorialDayItemsAsync();
            Assert.AreEqual(MemorialDaysStorageHelpers.NumberNote,days.Count);
            await memorialDayStorage.CloseDataFile();
        }

        /// <summary>
        /// 测试获取一个纪念日。
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetMemorialDayAsync()
        {
            var memorialDayStorage = await MemorialDaysStorageHelpers
                .GetInitializedMemorialDaysStorage();
            var memorialDay = await memorialDayStorage.GetMemorialDayAsync(2);
            Assert.AreEqual("星期六还有", memorialDay.Title);
            await memorialDayStorage.CloseDataFile();
        }

        //还未完成。
        [Test]
        public async Task TestSaveMemorialDayAsync() {
            int flag = 0;
            var updated = false;
            MemorialDay updatedMemorialDay = null;
            var memorialDayStorage = await MemorialDaysStorageHelpers
                .GetInitializedMemorialDaysStorage();
            var oldMemorialDays =
                await memorialDayStorage.GetMemorialDaysAsync(0, int.MaxValue);
            var num = oldMemorialDays.Count;
            MemorialDay memorialDay = new MemorialDay
            {
                Title = "新的纪念日",
                StartDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            memorialDayStorage.Updated += (sender, args) => {
                updated = true;
                updatedMemorialDay = args.UpdatedMemorialDay;
                flag = args.Flag;
            };
            await memorialDayStorage.SaveMemorialDayAsync(memorialDay, 0, true);
            var currentMemorialDays =
                await memorialDayStorage.GetMemorialDaysAsync(0, int.MaxValue);
            Assert.AreEqual(currentMemorialDays.Count,num+1);
            Assert.IsTrue(updated);
            Assert.AreEqual(updatedMemorialDay,memorialDay);
            Assert.AreEqual(flag,0);
            var memorialDayUpdated = new MemorialDay {
                Id = 4, 
                Title = "纪念日呀",
            };
            await memorialDayStorage.SaveMemorialDayAsync(memorialDayUpdated, 1,
                true);
            currentMemorialDays =
                await memorialDayStorage.GetMemorialDaysAsync(0, int.MaxValue);
            Assert.AreEqual(currentMemorialDays.Count,num+1);
            Assert.AreEqual(flag,1);
            Assert.AreEqual(updatedMemorialDay,memorialDayUpdated);
            await memorialDayStorage.CloseDataFile();
        }


    }
}
