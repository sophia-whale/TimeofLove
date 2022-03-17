using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels
{
    /// <summary>
    /// 纪念日ViewModel测试
    /// </summary>
    public class MemorialDaysViewModelTest
    {
        /// <summary>
        ///删除数据库文件
        /// </summary>
        [SetUp, TearDown]
        public static void RemoveDatabaseFile() =>
            MemorialDaysStorageHelpers.RemoveDatabaseFile();

        [Test]
        public async Task TestPageAppearingCommandFunction() {
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            IList<MemorialDay> memorialDaysList = new List<MemorialDay>();
            for (int i = 0; i < 5; i++)
            {
                memorialDaysList.Add(new MemorialDay() { Id = i, IsDeleted = 0,StartDate = "05/07/2021"});
            }

            string todayDate = DateTime.Now.ToString("MM/dd/yyyy");
            var todayMemorialDay = new MemorialDay() {
                Id = 2, IsDeleted = 0, StartDate = todayDate,
            };
            memorialDaysList.Add(todayMemorialDay);
            var memorialDaysViewModel =
                new MemorialDaysViewModel(mockMemorialDayStorage, null);
            Assert.AreEqual(0, memorialDaysViewModel.ThatYearMemorialDay.Count);
            Assert.AreEqual(0, memorialDaysViewModel.MemorialDaysCollection.Count);
            memorialDayStorageMock.Setup(p => p.GetMemorialDaysAsync(0, It.IsAny<int>()))
                .Returns(Task.FromResult(memorialDaysList));
            await memorialDaysViewModel.PageAppearingCommandFunction();
            memorialDayStorageMock.Verify(
                p => p.Initialized(), Times.Once);
            Assert.AreEqual(6, memorialDaysViewModel.MemorialDaysCollection.Count);
            Assert.AreEqual(1,memorialDaysViewModel.ThatYearMemorialDay.Count);

        }

        [Test]
        public async Task TestMemorialDayTappedCommandFunction()
        {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            var MemorialDayToTap = new MemorialDay();
            var memorialDaysViewModel =
                new MemorialDaysViewModel(mockMemorialDayStorage, mockContentNavigationService);
            await memorialDaysViewModel.MemorialDayTappedCommandFunction(
                MemorialDayToTap);
            contentNavigationServiceMock.Verify(p => p.NavigateToAsync(
                ContentNavigationConstant.MemorialDetailPage, MemorialDayToTap), Times.Once);
        }

        [Test]
        public async Task TestAddMemorialDayNavigationCommandFunction()
        {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            var memorialDaysViewModel =
                new MemorialDaysViewModel(mockMemorialDayStorage, mockContentNavigationService);
            await memorialDaysViewModel.AddMemorialDayNavigationCommandFunction();
            contentNavigationServiceMock.Verify(p => p.NavigateToAsync(
                ContentNavigationConstant.AddMemorialDayPage), Times.Once);
        }

        [Test]
        public void TestMemorialDayStorageOnUpdated() {
            var memoryDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemoryDayStorage = memoryDayStorageMock.Object;
            var memoryDayList = new List<MemorialDay>();
            for (int i = 0; i < 5; i++)
            {
                memoryDayList.Add(new MemorialDay { Id = i, IsDeleted = 0});
            }
            var memorialDaysViewModel =
                new MemorialDaysViewModel(mockMemoryDayStorage, null);
            Assert.AreEqual(0, memorialDaysViewModel.MemorialDaysCollection.Count);
            var memorialDayDeleted = new MemorialDay { Id = 2, IsDeleted = 1, };
            memorialDaysViewModel.MemorialDaysCollection.AddRange(memoryDayList);
            memoryDayStorageMock.Raise(p => p.Updated += null,
                mockMemoryDayStorage,
                new MemorialStorageUpdatedEventArgs(memorialDayDeleted, 1));
            Assert.AreEqual(memoryDayList.Count - 1,
                memorialDaysViewModel.MemorialDaysCollection.Count);
            Assert.IsFalse(memorialDaysViewModel.MemorialDaysCollection.Any(p => p.Id == memorialDayDeleted.Id));
            var memorialDayAdd = new MemorialDay { Title = "恋爱时光诞生已经" };
            memoryDayStorageMock.Raise(p => p.Updated += null,
                mockMemoryDayStorage,
                new MemorialStorageUpdatedEventArgs(memorialDayAdd, 0));
            Assert.AreEqual(memoryDayList.Count,
                memorialDaysViewModel.MemorialDaysCollection.Count);
            Assert.AreEqual("恋爱时光诞生已经",
                memorialDaysViewModel.MemorialDaysCollection[0].Title);

            var memorialDayEdit = new MemorialDay { Id = 1, IsDeleted = 0, Title = "修改纪念日" };
            memoryDayStorageMock.Raise(p => p.Updated += null,
                memoryDayStorageMock,
                new MemorialStorageUpdatedEventArgs(memorialDayEdit, 1));
            Assert.AreEqual(memoryDayList.Count,
                memorialDaysViewModel.MemorialDaysCollection.Count);
            Assert.AreEqual("修改纪念日",
                memorialDaysViewModel.MemorialDaysCollection[0].Title);
        }
    }
}
