using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.ViewModels;


namespace TabbedTemplate.UnitTest.ViewModels
{
    public class SyncPageViewModelTest
    {
        /// <summary>
        /// 测试页面显示命令。
        /// </summary>
        [Test]
        public async Task TestPageAppearingCommand()
        {
            var random = new Random();
            var oneDriveIsSignedIn = random.NextDouble() < 0.5;

            var oneDriveSyncServiceMock = new Mock<ISyncService>();
            oneDriveSyncServiceMock.Setup(p => p.IsSignedInAsync())
                .ReturnsAsync(oneDriveIsSignedIn);
            var mockOneDriveSyncService = oneDriveSyncServiceMock.Object;
            var mock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = mock.Object;
            var syncPageViewModel = new SyncPageViewModel()
            {
                _syncService = mockOneDriveSyncService,
                _preferenceStorage = mockPreferenceStorage,
            };
            var oneDriveLoadingList = new List<bool>();
            syncPageViewModel.PropertyChanged += (sender, args) =>
            {
                oneDriveLoadingList.Add(syncPageViewModel
                            .OneDriveLoading);
            };

            syncPageViewModel.PageAppearingCommandFunction();
            while (oneDriveLoadingList.Count != 2)
            {
                await Task.Delay(100);
            }

            Assert.IsTrue(oneDriveLoadingList.First());
            Assert.IsFalse(oneDriveLoadingList.Last());

            oneDriveSyncServiceMock.Verify(
                p => p.IsSignedInAsync(), Times.Once);
        }

        /// <summary>
        /// 测试OneDrive登录同步注销命令。
        /// </summary>
        [Test]
        public async Task TestOneDriveSignInSyncSignOutCommand()
        {
            var random = new Random();
            var oneDriveSignIn = random.NextDouble() < 0.5;
            var oneDriveSync = random.NextDouble() > 0.5;
            var oneDriveSyncServiceMock = new Mock<ISyncService>();
            oneDriveSyncServiceMock
                .Setup(p => p.SignInAsync())
                .ReturnsAsync(oneDriveSignIn);
            oneDriveSyncServiceMock.Setup(p => p.SyncAsync()).ReturnsAsync(
                new ServiceResult
                {
                    Status = oneDriveSync
                        ? ServiceResultStatus.Ok
                        : ServiceResultStatus.Exception
                });
            var mockOneDriveSyncService = oneDriveSyncServiceMock.Object;
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            DateTime savedDateTime = DateTime.MinValue;
            preferenceStorageMock
                .Setup(p => p.Set(SyncPageViewModel.LastOneDriveSyncTimeKey,
                    It.IsAny<DateTime>()))
                .Callback<string, DateTime>((s, d) => savedDateTime = d);
            preferenceStorageMock
                .Setup(p => p.Get(SyncPageViewModel.LastOneDriveSyncTimeKey,
                    DateTime.MinValue))
                .Returns((string s, DateTime d) => savedDateTime);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var syncPageViewModel = new SyncPageViewModel
            {
                _syncService = mockOneDriveSyncService,
                _preferenceStorage = mockPreferenceStorage
            };
            var oneDriveLoadingList = new List<bool>();
            syncPageViewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(SyncPageViewModel.OneDriveLoading):
                        oneDriveLoadingList.Add(syncPageViewModel
                            .OneDriveLoading);
                        break;
                }
            };
            await syncPageViewModel.OneDriveSignInCommandFunction();
            Assert.AreEqual(2, oneDriveLoadingList.Count);
            Assert.IsTrue(oneDriveLoadingList.First());
            Assert.IsFalse(oneDriveLoadingList.Last());
            oneDriveSyncServiceMock.Verify(
                p => p.SignInAsync(), Times.Once);
            Assert.AreEqual(oneDriveSignIn, syncPageViewModel.OneDriveSignedIn);

            await syncPageViewModel.OneDriveSyncCommandFunction();
            Assert.AreEqual(4, oneDriveLoadingList.Count);
            Assert.IsTrue(oneDriveLoadingList[2]);
            Assert.IsFalse(oneDriveLoadingList[3]);
            oneDriveSyncServiceMock.Verify(p => p.SyncAsync(), Times.Once);
            Assert.AreEqual(oneDriveSync,
                syncPageViewModel.LastOneDriveSyncTime > DateTime.Today);
            if (oneDriveSync)
            {
                preferenceStorageMock.Verify(
                    p => p.Set(SyncPageViewModel.LastOneDriveSyncTimeKey,
                        It.IsAny<DateTime>()), Times.Once);
            }
            else
            {
                preferenceStorageMock.Verify(
                    p => p.Set(SyncPageViewModel.LastOneDriveSyncTimeKey,
                        It.IsAny<DateTime>()), Times.Never);
            }
            await syncPageViewModel.OneDriveSignOutCommandFunction();
            Assert.AreEqual(6, oneDriveLoadingList.Count);
            Assert.IsTrue(oneDriveLoadingList[4]);
            Assert.IsFalse(oneDriveLoadingList[5]);
            oneDriveSyncServiceMock.Verify(
                p => p.SignOutAsync(), Times.Once);
            Assert.IsFalse(syncPageViewModel.OneDriveSignedIn);
        }

    }
}
