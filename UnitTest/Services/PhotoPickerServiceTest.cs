using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;

namespace TabbedTemplate.UnitTest.Services {
    public class PhotoPickerServiceTest {
        [Test]
        public async Task TestSavePhotoAsync() {
            //var photoPickerServiceMock = new Mock<IPhotoPickerService>();
            //var fileName = "sea.jpg";
            //Stream imageStream = await LoadImageHelpers.LoadImageResourceAsync(fileName);
            //byte[] imageBytes = imageStream.ToByteArray();
            //photoPickerServiceMock.Setup(p => p.SavePhotoAsync(imageBytes, fileName));
            //var mockPhotoPickerService = photoPickerServiceMock.Object;

            //mockPhotoPickerService.SavePhotoAsync(imageBytes, fileName);
            //var readPhotoAsync = LoadImageHelpers.LoadImageResourceAsync(fileName).Result;
            //byte[] resultBytes = readPhotoAsync.ToByteArray();
            //Assert.AreEqual(imageBytes, resultBytes);
        }

        //[Test]
        //public async Task TestReadPhotoAsync()
        //{
        //    var photoPickerServiceMock = new Mock<IPhotoPickerService>();
        //    var fileName = "sea.jpg";
        //    Stream imageStream = await LoadImageHelpers.LoadImageResourceAsync("",fileName);
        //    byte[] imageBytes = imageStream.ToByteArray();
        //    await LoadImageHelpers.SaveImageResourceAsync(imageBytes, fileName);
        //    photoPickerServiceMock.Setup(p => p.ReadPhotoA(fileName)).Returns(imageBytes);
        //    var mockPhotoPickerService = photoPickerServiceMock.Object;
        //    var resultBytes = mockPhotoPickerService.ReadPhotoAsync(fileName);
        //    Assert.AreSame(imageBytes, resultBytes);
        //}
    }
}