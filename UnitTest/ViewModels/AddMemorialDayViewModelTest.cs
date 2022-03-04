using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.ViewModels;

namespace TabbedTemplate.UnitTest.ViewModels
{
    public class AddMemorialDayViewModelTest
    {
        /// <summary>
        /// 测试添加纪念日方法
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestAdMemorialDayFunction()
        {
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var addMemorialDayViewModel = new AddMemorialDayViewModel(mockMemorialDayStorage, mockContentNavigationService);
            await addMemorialDayViewModel.AddMemorialDayFunction();
            memorialDayStorageMock.Verify(
                p => p.SaveMemorialDayAsync(It.IsAny<MemorialDay>(), 0, true), Times.Once);
            contentNavigationServiceMock.Verify(p => p.NavigateBack(ContentNavigationConstant
                .AddMemorialDayPage), Times.Once);

        }
    }
}
