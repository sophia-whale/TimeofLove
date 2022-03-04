using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Services;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels
{
    /// <summary>
    /// ImageViewModel测试
    /// </summary>
    public class ImageViewModelTest
    {
        /// <summary>
        /// 测试添加命令
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestAddCommandFunction() {
            
            var alertServiceMock = new Mock<IAlertService>();
            var alertService = alertServiceMock.Object;
            var diaryDetailViewModel =
                new DiaryDetailViewModel(null, null, null,null, alertService,null);
            var imageViewModel = new ImageViewModel(diaryDetailViewModel);
            alertServiceMock
                .Setup(p => p.SelectAlert("选择", "取消", "再考虑一下", "从手机相册选择", "拍照"))
                .Returns(Task.FromResult("取消"));
            await imageViewModel.AddCommandFunction();
            alertServiceMock.Verify(p => p.SelectAlert("选择", "取消", "再考虑一下", "从手机相册选择", "拍照"),Times.Once);
        }

        /// <summary>
        /// 测试删除命令
        /// </summary>
        /// <returns></returns>
        [Test]
        public void TestDeleteCommandFunction() {

            var diaryDetailViewModelMock = new Mock<DiaryDetailViewModel>(null,null,null,null,null,null);
            var diaryDetailViewModel = diaryDetailViewModelMock.Object;

            var imageViewModel = new ImageViewModel(diaryDetailViewModel);
            imageViewModel.RemoveCommandFunction();
            diaryDetailViewModelMock.Setup(p =>
                p.RemoveImageViewModel(imageViewModel));
            diaryDetailViewModelMock.Verify(
                p => p.RemoveImageViewModel(imageViewModel), Times.Once);
        }
    }
}
