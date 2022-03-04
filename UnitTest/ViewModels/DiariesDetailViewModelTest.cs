using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.ViewModels;
using Xamarin.Forms;

namespace TabbedTemplate.UnitTest.ViewModels
{
    public class DiariesDetailViewModelTest
    {
        [Test]
        public void TestPageAppearingCommandFunction() {
            var diaryDetailViewModel = new DiaryDetailViewModel(null, null,
                null, null, null,null);
            var imageViewModelCollection = diaryDetailViewModel.ImageViewModelCollection;
            var imageViewModel = new ImageViewModel(diaryDetailViewModel);
            diaryDetailViewModel.PageAppearingCommandFunction();
            Assert.AreEqual(1, imageViewModelCollection.Count);
        }

        /// <summary>
        /// 测试添加日记
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestAddDiaryCommandFunction()
        {
            var diaryStorageMock = new Mock<IDiaryStorage>();
            var mockDiaryStorage = diaryStorageMock.Object;
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryDetailViewModel = new DiaryDetailViewModel(null,null,
                mockDiaryStorage, mockContentNavigationService,null,null);
            diaryDetailViewModel.Title = "标题";
            diaryDetailViewModel.Editor = "内容";
            await diaryDetailViewModel.AddDiaryCommandFunction();
            diaryStorageMock.Verify(
                p => p.SaveDiaryAsync(It.IsAny<Diary>(), 0, true), Times.Once);
            contentNavigationServiceMock.Verify(p => p.NavigateBack(ContentNavigationConstant
                    .DiaryDetailPage), Times.Once);
        }

        [Test]
        public async Task TestShareNavigationCommandFunction() {
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var diaryDetailViewModel = new DiaryDetailViewModel(null, null,
                null, mockContentNavigationService, null,null);
            var title =  diaryDetailViewModel.Title = "标题";
            var editor = diaryDetailViewModel.Editor = "内容";
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
            diaryDetailViewModel.Diary = diary;
            await diaryDetailViewModel.ShareNavigationCommandFunction();
            contentNavigationServiceMock.Verify(p => p.NavigateToAsync(ContentNavigationConstant
                .ShareDetailPage, diaryDetailViewModel.Diary), Times.Once);
        }

        [Test]
        public async Task AddImageViewModelTest() {
            var addImageInDiaryServiceMock = new Mock<IAddImageInDiaryService>();
            var addImageInDiaryService = addImageInDiaryServiceMock.Object;
            var alertServiceMock = new Mock<IAlertService>();
            var alertService = alertServiceMock.Object;
            var photoStorageMock = new Mock<IPhotoStorage>();
            var photoStorage = photoStorageMock.Object;
            var diaryDetailViewModel = new DiaryDetailViewModel(null,
                photoStorage, null, null, alertService, addImageInDiaryService);
            alertServiceMock
                .Setup(p => p.SelectAlert("选择", "取消", "再考虑一下", "从手机相册选择", "拍照"))
                .Returns(Task.FromResult("拍照"));
            byte[] imageSource= Enumerable.Repeat((byte)0x08, 10).ToArray();
            addImageInDiaryServiceMock.Setup(p => p.TakePhoto())
                .Returns(Task.FromResult(imageSource));
            diaryDetailViewModel.PageAppearingCommandFunction();
            await diaryDetailViewModel.AddImageViewModel(
                diaryDetailViewModel.ImageViewModelCollection[0]);
            alertServiceMock.Verify(p => p.SelectAlert("选择", "取消", "再考虑一下", "从手机相册选择", "拍照"), Times.Once);
            photoStorageMock.Verify(p=>p.SavePhoto(imageSource,It.IsAny<string>()));

        }

        [Test]
        public async Task TestRemoveImageViewModel()
        {
            var diaryDetailViewModel = new DiaryDetailViewModel(null, null,
                null, null, null,null);
            var imageViewModelCollection = diaryDetailViewModel.ImageViewModelCollection;

            var imageViewModel = new ImageViewModel(diaryDetailViewModel);
            diaryDetailViewModel.RemoveImageViewModel(imageViewModel);
            Assert.AreEqual(1, imageViewModelCollection.Count);
            imageViewModelCollection.Add(imageViewModel);
            imageViewModelCollection.Add(imageViewModel);
            diaryDetailViewModel.RemoveImageViewModel(imageViewModel);
            Assert.AreEqual(2, imageViewModelCollection.Count);
        }
    }
}
