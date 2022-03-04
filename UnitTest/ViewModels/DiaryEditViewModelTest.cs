using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels
{
    public class DiaryEditViewModelTest
    {
        [Test]
        public async Task TestPageAppearingCommandFunction() {
            var photoStorageMock = new Mock<IPhotoStorage>();
            var mockPhotoStorage = photoStorageMock.Object;
            var diaryEditViewModel =
                new DiaryEditViewModel(null, null, null, mockPhotoStorage);
            Diary diary = new Diary {paths = null};
            var diaryImage = diaryEditViewModel.DiaryImage;

            //null
            diaryEditViewModel.Diary = diary;
            await diaryEditViewModel.PageAppearingCommandFunction();
            Assert.AreEqual(0, diaryImage.Count);

            //add
            diaryEditViewModel.Diary.paths = new List<string> { "sea.jpg", "-1", "-1"};
            Stream imageStream = await LoadImageHelpers.LoadImageResourceAsync("","sea.jpg");
            byte[] imageBytes = imageStream.ToByteArray();
            mockPhotoStorage.SavePhoto(imageBytes, "sea.jpg");
            await diaryEditViewModel.PageAppearingCommandFunction();
            Assert.AreEqual(1, diaryImage.Count);
        }

        /// <summary>
        /// 测试编辑日记方法
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestEditDiaryCommandFunction()
        {
            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryEditViewModel = new DiaryEditViewModel(null,
                mockDiaryStorage, mockContentNavigationService,null);
            var diary = new Diary
            {
                Content = "内容",
                Title = "标题",
                Id = 1,
            };
            diaryEditViewModel.Diary = diary;
            await diaryEditViewModel.EditDiaryCommandFunction();
            diaryStorageMock.Verify(
                p => p.SaveDiaryAsync(diary, 1, true), Times.Once);
            contentNavigationServiceMock.Verify(p => p.NavigateBack(ContentNavigationConstant
                .EditDiaryPage), Times.Once);
        }

        [Test]
        public async Task TestShareNavigationCommandFunction()
        {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryEditViewModel = new DiaryEditViewModel(null, null,
                mockContentNavigationService, null);
            var title = "标题";
            var editor = "内容";
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            String[] initStrings = { "-1", "-1", "-1" };
            List<string> pathsSource = initStrings.ToList();
            Diary diary = new Diary
            {
                Title = title,
                Content = editor,
                Date = date,
                IsDeleted = 0,
                paths = pathsSource,
            };
            diaryEditViewModel.Diary = diary;
            await diaryEditViewModel.ShareNavigationCommandFunction();
            contentNavigationServiceMock.Verify(p => p.NavigateToAsync(ContentNavigationConstant
                .ShareDetailPage, diaryEditViewModel.Diary), Times.Once);
        }
    }
}
