using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Moq;
using NUnit.Framework;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using FileSystemInfo = System.IO.FileSystemInfo;

namespace TabbedTemplate.UnitTest.Services
{
    public class SyncServiceTaskRunnerTest
    {
        /// <summary>
        /// 本地纪念日同步
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestSyncMemorialDay()
        {
            GraphServiceClient graphServiceClient =
                new GraphServiceClient(new HttpClient());
            var localMemorialDayList = new List<MemorialDay>();
            for (int i = 2; i >= 0; i--)
            {
                localMemorialDayList.Add(new MemorialDay
                {
                    Id = i * 2,
                    IsDeleted = i % 2,
                    Timestamp = i * 2,
                });
            }

            var remoteMemorialDayList = new List<MemorialDay>();
            for (int i = 1; i >= 0; i--)
            {
                remoteMemorialDayList.Add(new MemorialDay
                {
                    Id = i * 2 + 1,
                    IsDeleted = i % 2,
                    Timestamp = i * 2 + 1
                });
            }

            var firstRemoteMemorialDay = new MemorialDay
            {
                Id = 3,
                IsDeleted = 1,
                Timestamp = int.MaxValue,
            };
            var firstLocalMemorialDay = new MemorialDay
            {
                Id = 4,
                IsDeleted = 0,
                Timestamp = int.MaxValue,
            };
            firstLocalMemorialDay.Timestamp = int.MaxValue;
            localMemorialDayList.Add(firstRemoteMemorialDay);

            remoteMemorialDayList.Add(firstLocalMemorialDay);

            Dictionary<int, MemorialDay> localMemorialDayDictionarySaved =
                new Dictionary<int, MemorialDay>();
            Dictionary<int, MemorialDay>
                remoteMemorialDayDictionarySaved = null;

            var localMemorialDayStorageMock = new Mock<IMemorialDayStorage>();
            localMemorialDayStorageMock
                .Setup(p => p.GetMemorialDayItemsAsync())
                .ReturnsAsync(localMemorialDayList);
            localMemorialDayStorageMock.Setup(p =>
                    p.SaveMemorialDayAsync(It.IsAny<MemorialDay>(), 0, false))
                .Callback<MemorialDay, int, bool>((p, q, r) =>
                      localMemorialDayDictionarySaved[p.Id] = p);
            localMemorialDayStorageMock.Setup(p =>
                    p.SaveMemorialDayAsync(It.IsAny<MemorialDay>(), 1, false))
                .Callback<MemorialDay, int, bool>((p, q, r) =>
                    localMemorialDayDictionarySaved[p.Id] = p);
            var mockLocalMemorialDayStorage =
                localMemorialDayStorageMock.Object;
            var remoteFavoriteStorageMock = new Mock<IRemoteMemorialDayStorage>();
            remoteFavoriteStorageMock.Setup(p => p.GetMemorialDayItemAsync(graphServiceClient))
                .ReturnsAsync(remoteMemorialDayList);
            var mockRemoteFavoriteStorage = remoteFavoriteStorageMock.Object;
            var remoteMemorialDayStorageMock =
                new Mock<IRemoteMemorialDayStorage>();
            remoteMemorialDayStorageMock.Setup(p => p.GetMemorialDayItemAsync(graphServiceClient)).ReturnsAsync(remoteMemorialDayList);
            var MockRemoteMemorialDayStorage =
                remoteMemorialDayStorageMock.Object;
            var alertServiceMock = new Mock<IAlertService>();
            var MockAlertService = alertServiceMock.Object;
            var syncServiceTaskRunner = new SyncServiceTaskRunner(
                MockAlertService, MockRemoteMemorialDayStorage,
                mockLocalMemorialDayStorage, null, null,null,null);
            await syncServiceTaskRunner.SyncMemorialDay(graphServiceClient);
            localMemorialDayStorageMock.Verify(p => p.GetMemorialDayItemsAsync(), Times.Once);
            remoteMemorialDayStorageMock.Verify(p => p.GetMemorialDayItemAsync(graphServiceClient), Times.Once);

            Assert.IsTrue(
                localMemorialDayDictionarySaved.ContainsKey(
                    firstLocalMemorialDay.Id));
            Assert.AreEqual(
                localMemorialDayDictionarySaved[
                    firstLocalMemorialDay.Id].IsDeleted,
                firstLocalMemorialDay.IsDeleted);
            Assert.AreEqual(
                localMemorialDayDictionarySaved[
                    firstLocalMemorialDay.Id].Timestamp,
                firstLocalMemorialDay.Timestamp);


        }


        /// <summary>
        /// 测试将远程图片同步到本地
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SyncImageTest() {
            GraphServiceClient graphServiceClient =
                new GraphServiceClient(new HttpClient());
            byte[] myByteArray = new byte[] { 0x01, 0x02, 0x03 };
            Tuple<byte[], string> resultRecord =
                new Tuple<byte[], string>(myByteArray, "test.txt");
            var photoStorageMock = new Mock<IPhotoStorage>();
            var localDirectory = new List<FileSystemInfo>();
            photoStorageMock.Setup(p => p.GetDirectoryInfo())
                .Returns(localDirectory.AsEnumerable());
            var remoteImageStorageMock = new Mock<IRemoteImageStorage>();
            var remoteDictionary = new Dictionary<string, bool>();
            remoteDictionary.Add("test.txt",true);
            remoteImageStorageMock
                .Setup(p => p.GetDirectoryInfoAsync(graphServiceClient))
                .Returns(Task.FromResult(remoteDictionary));
            ;
            var remoteDictionaryRecord = new List<String>();
            remoteImageStorageMock
                .Setup(p =>
                    p.GetImageAsync(graphServiceClient, It.IsAny<string>()))
                .Returns(Task.FromResult(resultRecord)).Callback<GraphServiceClient, string>((p, q) =>
                    remoteDictionaryRecord.Add(q));
            var remoteImageStorage = remoteImageStorageMock.Object;
            var saveToLocalDictionary = new List<Tuple<byte[], string>>();
            photoStorageMock
                .Setup(p => p.SavePhoto(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Callback<byte[], string>((p, q) =>
                    saveToLocalDictionary.Add(new Tuple<byte[], string>(p, q)));
            var photoStorage = photoStorageMock.Object;
            var syncServiceTaskRunner = new SyncServiceTaskRunner(null, null,
                null, null, null, photoStorage, remoteImageStorage);
            await syncServiceTaskRunner.SyncImage(graphServiceClient);
            photoStorageMock.Verify(p=>p.GetDirectoryInfo(),Times.Once);
            remoteImageStorageMock.Verify(p=>p.GetDirectoryInfoAsync(graphServiceClient),Times.Once);
            Assert.AreEqual(remoteDictionaryRecord.Count,1);
            Assert.AreEqual(remoteDictionaryRecord[0], "test.txt");
            remoteImageStorageMock.Verify(p=>p.GetImageAsync(graphServiceClient,It.IsAny<String>()),Times.Once);
            Assert.AreEqual(saveToLocalDictionary.Count,1);
            Assert.IsTrue(saveToLocalDictionary.Contains(resultRecord));

        }



        /// <summary>
        /// 测试将远程日记同步到本地
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SyncDiaryTest() {
            GraphServiceClient graphServiceClient =
                new GraphServiceClient(new HttpClient());
            var localDiaryList = new List<Diary>();
            for (int i = 2; i >= 0; i--)
            {
                localDiaryList.Add(new Diary
                {
                    Id = i * 2,
                    IsDeleted = i % 2,
                    timeStamp = i * 2,
                });
            }

            var remoteDiaryEntryList = new List<DiaryEntry>();
            for (int i = 1; i >= 0; i--)
            {
                remoteDiaryEntryList.Add(new DiaryEntry
                {
                    Id = i * 2 + 1,
                    IsDeleted = i % 2,
                    TimeStamp = i * 2 + 1
                });
            }

            var firstRemoteLocal = new Diary
            {
                Id = 3,
                IsDeleted = 0,
                timeStamp = int.MaxValue,
            };
            var firstLocalRemote = new DiaryEntry
            {
                Id = 4,
                IsDeleted = 1,
                TimeStamp = int.MaxValue,
            };
            localDiaryList.Add(firstRemoteLocal);

            remoteDiaryEntryList.Add(firstLocalRemote);

            Dictionary<int, Diary> localDiaryDictionarySaved =
                new Dictionary<int, Diary>();
            Dictionary<int, MemorialDay>
                remoteMemorialDayDictionarySaved = null;
            List<int> remoteDiaryFindList = new List<int>();

            var localDiaryStorageMock = new Mock<IDiaryStorage>();
            localDiaryStorageMock
                .Setup(p => p.GetDiaryItemsAsync())
                .ReturnsAsync(localDiaryList);

            localDiaryStorageMock.Setup(p =>
                    p.SaveDiaryAsync(It.IsAny<Diary>(), 0, false))
                .Callback<Diary, int, bool>((p, q, r) =>
                      localDiaryDictionarySaved[p.Id] = p);
            localDiaryStorageMock.Setup(p =>
                    p.SaveDiaryAsync(It.IsAny<Diary>(), 1, false))
                .Callback<Diary, int, bool>((p, q, r) =>
                    localDiaryDictionarySaved[p.Id] = p);

            var remoteDiaryStorageMock = new Mock<IRemoteDiaryStorage>();
            DiaryContent example = new DiaryContent {Title = "模板",};
            remoteDiaryStorageMock
                .Setup(p =>
                    p.GetDiaryItemAsync(graphServiceClient, It.IsAny<int>()))
                .Returns(Task.FromResult(example)).Callback<GraphServiceClient,int>((p,q)=>remoteDiaryFindList.Add(q));
            remoteDiaryStorageMock
                .Setup(p => p.GetDiariesAsync(graphServiceClient))
                .ReturnsAsync(remoteDiaryEntryList);
           
           
            var mockRemoteDiaryStorage = remoteDiaryStorageMock.Object;
            var mockLocalDiaryStorage = localDiaryStorageMock.Object;
            var syncServiceTaskRunner = new SyncServiceTaskRunner(
                null, null,
                null,mockLocalDiaryStorage, mockRemoteDiaryStorage, null, null);
            var result=await syncServiceTaskRunner.SyncDiary(graphServiceClient);
            localDiaryStorageMock.Verify(p => p.GetDiaryItemsAsync(), Times.Once);
            remoteDiaryStorageMock.Verify(p => p.GetDiariesAsync(graphServiceClient), Times.Once);
            Assert.AreEqual(result.Item2.Count,5);
            Assert.AreEqual(result.Item1.Count,3);
            Assert.AreEqual(remoteDiaryFindList.Count,2);
            Assert.IsTrue(remoteDiaryFindList.Contains(4));
           Assert.AreEqual(localDiaryDictionarySaved[0].Title, "模板");




        }



    }
}
