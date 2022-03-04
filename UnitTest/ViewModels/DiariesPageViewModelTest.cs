using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels
{
    /// <summary>
    /// 日记预览页ViewModel测试
    /// </summary>
    public class DiariesPageViewModelTest
    {

        /// <summary>
        /// 测试页面显示命令
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestPageAppearingCommand()
        {
            var diariesStorage =
                await DiaryStorageHelpers.GetInitializedDiaryStorage();

            var honeyWordsServiceMock = new Mock<IHoneyWordsService>();
            honeyWordsServiceMock.Setup(p => p.GetHoneyWordsAsync())
                .ReturnsAsync("你好");
            var mockHoneyWordsService = honeyWordsServiceMock.Object;
            var diariesPageViewModel =
                new DiariesViewModel(diariesStorage, null, mockHoneyWordsService);
            var statusList = new List<string>();

            await diariesPageViewModel.PageAppearingCommandFunction();
            honeyWordsServiceMock.Verify(
                p => p.GetHoneyWordsAsync(), Times.Once);
            Assert.AreEqual("你好", diariesPageViewModel.HoneyWords);
            Assert.AreEqual(2, diariesPageViewModel.DiaryCollection.Count);
            var diaryStorageMock = new Mock<IDiaryStorage>();
            diaryStorageMock.Setup(p => p.Initialized())
                .Returns(false);
            var mockDiaryStorage = diaryStorageMock.Object;
            IList<Diary> diaries = new List<Diary>() {
                new Diary {Id = 1,}, new Diary {Id = 2,},
            };
            diariesPageViewModel =
                new DiariesViewModel(mockDiaryStorage, null, mockHoneyWordsService);
            diaryStorageMock
                .Setup(p =>
                    p.GetAllDiariesAsync(
                        diariesPageViewModel.DiaryCollection.Count, 20))
                .Returns(Task.FromResult(diaries));
            await diariesPageViewModel.PageAppearingCommandFunction();
            diaryStorageMock.Verify(
                p => p.Initialized(), Times.Once);
            Assert.AreEqual(2, diariesPageViewModel.DiaryCollection.Count);
        }

        /// <summary>
        /// 测试添加新日记命令。
        /// </summary>
        [Test]
        public async Task TestAddDiaryNavigationCommandFunction()
        {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var diariesViewModel = new DiariesViewModel(mockDiaryStorage, mockContentNavigationService, null);
            await diariesViewModel.AddDiaryNavigationCommandFunction();
            contentNavigationServiceMock.Verify(
                p => p.NavigateToAsync(
                    ContentNavigationConstant.DiaryDetailPage), Times.Once);
        }

        /// <summary>
        /// 测试编辑新日记命令。
        /// </summary>
        [Test]
        public async Task TestEditDiaryNavigationCommandFunction()
        {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var diariesViewModel = new DiariesViewModel(mockDiaryStorage, mockContentNavigationService, null);
            var DiaryToEdit = new Diary();
            await diariesViewModel.EditDiaryNavigationCommandFunction(
                DiaryToEdit);
            contentNavigationServiceMock.Verify(
                p => p.NavigateToAsync(
                    ContentNavigationConstant.EditDiaryPage, DiaryToEdit), Times.Once);

        }

        /// <summary>
        /// 测试删除日记
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestDeleteDiaryCommandFunction()
        {

            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var diaryList = new List<Diary>();
            for (int i = 0; i < 5; i++)
            {
                diaryList.Add(new Diary { Id = i, IsDeleted = 0, });
            }
            var diariesPageViewModel =
                new DiariesViewModel(mockDiaryStorage, null, null);
            Assert.AreEqual(0, diariesPageViewModel.DiaryCollection.Count);
            var diaryDeleted = new Diary { Id = 2, IsDeleted = 0, };
            diariesPageViewModel.DiaryCollection.AddRange(diaryList);
            await diariesPageViewModel.DeleteDiaryCommandFunction(diaryDeleted);
            diaryStorageMock.Verify(
                p => p.SaveDiaryAsync(diaryDeleted, 1, true), Times.Once);
        }

        /// <summary>
        /// 测试日记存储是否更新
        /// </summary>
        [Test]
        public void TestDiaryStorageUpdated()
        {
            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var diaryList = new List<Diary>();
            for (int i = 0; i < 5; i++)
            {
                diaryList.Add(new Diary { Id = i, IsDeleted = 0, });
            }
            var diariesPageViewModel =
                new DiariesViewModel(mockDiaryStorage, null, null);
            Assert.AreEqual(0, diariesPageViewModel.DiaryCollection.Count);
            var diaryDeleted = new Diary { Id = 2, IsDeleted = 1, };
            diariesPageViewModel.DiaryCollection.AddRange(diaryList);
            diaryStorageMock.Raise(p => p.Updated += null,
                mockDiaryStorage,
                new DiaryStorageUpdatedEventArgs(diaryDeleted, 1));
            Assert.AreEqual(diaryList.Count - 1,
                diariesPageViewModel.DiaryCollection.Count);
            Assert.IsFalse(diariesPageViewModel.DiaryCollection.Any(p => p.Id == diaryDeleted.Id));
            var diaryAdd = new Diary { Title = "啦啦啦啦" };
            diaryStorageMock.Raise(p => p.Updated += null,
                mockDiaryStorage,
                new DiaryStorageUpdatedEventArgs(diaryAdd, 0));
            Assert.AreEqual(diaryList.Count,
                diariesPageViewModel.DiaryCollection.Count);
            Assert.AreEqual("啦啦啦啦",
                diariesPageViewModel.DiaryCollection[0].Title);

            var diaryEdit = new Diary { Id = 1, IsDeleted = 0, Title = "修改" };
            diaryStorageMock.Raise(p => p.Updated += null,
                mockDiaryStorage,
                new DiaryStorageUpdatedEventArgs(diaryEdit, 1));
            Assert.AreEqual(diaryList.Count,
                diariesPageViewModel.DiaryCollection.Count);
            Assert.AreEqual("修改",
                diariesPageViewModel.DiaryCollection[0].Title);
        }
    }
}
