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
    public class MemorialDetailViewModelTest
    {
        /// <summary>
        /// 编辑纪念日方法
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestEditMemorialDayFunction()
        {
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var memorialDayEditViewModel = new MemorialDetailViewModel(
                mockMemorialDayStorage, mockContentNavigationService);
            var memorialDay = new MemorialDay
            {
                Id = 1,
            };
            memorialDayEditViewModel.MemorialDay = memorialDay;
            await memorialDayEditViewModel.EditMemorialDayFunction();
            memorialDayStorageMock.Verify(
                p => p.SaveMemorialDayAsync(memorialDay, 1, true), Times.Once);
            contentNavigationServiceMock.Verify(p => p.NavigateBack(ContentNavigationConstant
                .MemorialDetailPage), Times.Once);
        }

        /// <summary>
        /// 删除纪念日方法
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestDeleteMemorialDayFunction()
        {
            var memorialDayStorageMock = new Mock<IMemorialDayStorage>();
            var mockMemorialDayStorage = memorialDayStorageMock.Object;
            var contentNavigationServiceMock =
                new Mock<IContentNavigationService>();
            var mockContentNavigationService =
                contentNavigationServiceMock.Object;
            var memorialDayEditViewModel = new MemorialDetailViewModel(
                mockMemorialDayStorage, mockContentNavigationService);
            var memorialDay = new MemorialDay
            {
                Id = 1,
            };
            memorialDay.IsDeleted = 1;
            memorialDayEditViewModel.MemorialDay = memorialDay;
            await memorialDayEditViewModel.DeleteMemorialDayFunction();
            memorialDayStorageMock.Verify(
                p => p.SaveMemorialDayAsync(memorialDay, 1, true), Times.Once);
            contentNavigationServiceMock.Verify(p => p.NavigateBack(ContentNavigationConstant
                .MemorialDetailPage), Times.Once);
        }
    }
}
