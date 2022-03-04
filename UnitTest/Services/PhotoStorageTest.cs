using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Services;
using TabbedTemplate.UnitTest.Helpers;

namespace TabbedTemplate.UnitTest.Services
{
    /// <summary>
    /// 测试图片存储类
    /// </summary>
    public class PhotoStorageTest
    {
        [Test]
        public async Task TestSaveAndReadPhotoAsync() {
            var photoStorageMock = new Mock<IPhotoStorage>();
            var filename = "sea.jpg";
            Stream imageStream =
                await LoadImageHelpers.LoadImageResourceAsync("",filename);
            byte[] imageBytes = imageStream.ToByteArray();
            var photoStoragePathMock = new Mock<IPhotoStoragePath>();
            photoStoragePathMock.Setup(p => p.getPath()).Returns(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), "sharePhoto"));
            var photoStoragePath = photoStoragePathMock.Object;
            PhotoStorage photoStorage = new PhotoStorage(photoStoragePath);
            photoStorage.SavePhoto(imageBytes, filename);
            var readPhotoAsync = LoadImageHelpers.LoadImageResourceAsync("sharePhoto",filename).Result;
            byte[] resultBytes = readPhotoAsync.ToByteArray();
            Assert.AreEqual(imageBytes, resultBytes);
            byte[] readByPhotoStorage=photoStorage.ReadPhoto(filename);
            Assert.AreEqual(imageBytes,readByPhotoStorage);
            IEnumerable<FileSystemInfo> directoryInfos= photoStorage.GetDirectoryInfo();
            Assert.AreEqual(directoryInfos.Count(),1);
            foreach (var file in directoryInfos) {
                Assert.AreEqual("sea.jpg",file.Name);
                
            }
            LoadImageHelpers.deleteDirectory("sharePhoto");
        }






    }
}
