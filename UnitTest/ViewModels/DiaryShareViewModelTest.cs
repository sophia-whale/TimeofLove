using System;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using Moq;
using NUnit.Framework;
using SkiaSharp;
using TabbedTemplate.Models;
using TabbedTemplate.Renders;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels {
    public class DiaryShareViewModelTest {
        
        public async Task TestSaveShareImageCommandFunction() {
            var photoPickerServiceMock = new Mock<IPhotoPickerService>();
            var mockPhotoPickerService = photoPickerServiceMock.Object;
            var photoStorageMock = new Mock<IPhotoStorage>();
            var mockPhotoStorage = photoStorageMock.Object;
            var renderMock = new Mock<IRender>();
            var mockRender = renderMock.Object;
            var diaryShareViewModel = new DiaryShareViewModel(mockPhotoPickerService, mockPhotoStorage, null);
            Diary diary = new Diary {
                Title = "标题", Date = DateTime.Today.ToString("dd/MM/yyyy")
            };
            diaryShareViewModel.Diary = diary;
            var fileName = diaryShareViewModel.Diary.Title +
                diaryShareViewModel.Diary.Date + ".png";
            //设置RenderImage为"sea.jpg"
            Stream imageStream = await LoadImageHelpers.LoadImageResourceAsync("","sea.jpg");
            byte[] imageBytes = imageStream.ToByteArray();
            var image = SKImage.FromEncodedData(imageStream);
            diaryShareViewModel.Render = mockRender;
            //((ShareCanvasViewRender) diaryShareViewModel.Render).Image = image;
            diaryShareViewModel.SaveShareImageCommandFunction();
            var resultImageBytes = mockPhotoStorage.ReadPhoto(fileName);
            Assert.AreEqual(imageBytes, resultImageBytes);
        }
    }
}