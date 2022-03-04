using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Graph;
using Newtonsoft.Json;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;
using TabbedTemplate.Utils;
using Xamarin.Forms.Internals;


namespace TabbedTemplate.Services
{
    public class SyncServiceTaskRunner : ISyncServiceTaskRunner {
        // ******** 私有变量
        private IAlertService _alertService;

        /// <summary>
        /// 本地收藏存储
        /// </summary>
        private IMemorialDayStorage _localMemorialDayStorage;

        /// <summary>
        ///本地存储的日记
        /// </summary>
        private IDiaryStorage _diaryStorage;

        /// <summary>
        /// 远程纪念日存储
        /// </summary>
        private IRemoteMemorialDayStorage _remoteMemorialDayStorage;

        /// <summary>
        /// 远程日记存储
        /// </summary>
        private IRemoteDiaryStorage _remoteDiaryStorage;

        private IPhotoStorage _photoStorage;

        private IRemoteImageStorage _remoteImageStorage;

        private IEnumerable<System.IO.FileSystemInfo> _todayDictionary;



        public async Task<IList<MemorialDay>> SyncMemorialDay(
            GraphServiceClient graphClient) {
            var localList =
                (await _localMemorialDayStorage.GetMemorialDayItemsAsync())
                .Select(p => new TrackableMemorialDay(p)).ToList();
            var remoteList =
                (await _remoteMemorialDayStorage.GetMemorialDayItemAsync(
                    graphClient)).Select(p => new TrackableMemorialDay(p))
                .ToList();
            Dictionary<int, TrackableMemorialDay> localDictionary =
                new Dictionary<int, TrackableMemorialDay>();
            localDictionary = localList.ToDictionary(p => p.Id, p => p);
            foreach (var remoteMemorialDay in remoteList) {
                if (localDictionary.TryGetValue(remoteMemorialDay.Id,
                    out var localMemorialDay)) {
                    if (remoteMemorialDay.Timestamp >
                        localMemorialDay.Timestamp) {
                        var tmp = remoteMemorialDay.CloneAsUpdated();
                        localDictionary[remoteMemorialDay.Id] = tmp;
                        await _localMemorialDayStorage.SaveMemorialDayAsync(tmp,
                            1, false);
                    }
                } else {
                    var tmp = remoteMemorialDay.CloneAsUpdated();
                    localDictionary[remoteMemorialDay.Id] = tmp;
                    await _localMemorialDayStorage.SaveMemorialDayAsync(tmp, 0,
                        false);

                }
            }

            Dictionary<int, TrackableMemorialDay> remoteDictionary =
                new Dictionary<int, TrackableMemorialDay>();
            remoteDictionary = remoteList.ToDictionary(p => p.Id, p => p);

            foreach (var localMemorialDay in localList) {
                if (remoteDictionary.TryGetValue(localMemorialDay.Id,
                    out var remoteFavorite)) {
                    if (localMemorialDay.Timestamp > remoteFavorite.Timestamp) {
                        remoteDictionary[localMemorialDay.Id] =
                            localMemorialDay.CloneAsUpdated();
                    }
                } else {
                    remoteDictionary[localMemorialDay.Id] =
                        localMemorialDay.CloneAsUpdated();
                }
            }

            return remoteDictionary.Values.Select(p => p as MemorialDay)
                .ToList();
        }

        public async Task SyncImage(GraphServiceClient graphClient) {
            var todayDirectory = _todayDictionary ??
                (_todayDictionary = _photoStorage.GetDirectoryInfo());
            string[] files = todayDirectory.Select(o => o.Name).ToArray();
            var remoteDictionary =
                await _remoteImageStorage.GetDirectoryInfoAsync(graphClient);
            var remoteToLocal = new List<string>();
            //远程到本地
            foreach (var file in remoteDictionary.Keys) {
                if (files.IndexOf(file) == -1) {
                    remoteToLocal.Add(file);
                }
            }

            IList<Task> readTaskList = new List<Task>();
            foreach (var remotePhoto in remoteToLocal) {
                Task b =
                    _remoteImageStorage.GetImageAsync(graphClient, remotePhoto);
                readTaskList.Add(b);

            }

            await Task.WhenAll(readTaskList);

            List<Tuple<byte[], string>> results =
                new List<Tuple<byte[], string>>();
            foreach (var task in readTaskList) {
                var result = ((Task<Tuple<byte[], string>>) task).Result;
                if (_photoStorage.SavePhoto(result.Item1, result.Item2)) {
                    continue;
                }
            }

        }

        public async Task<Tuple<Dictionary<int, Diary>, IList<DiaryEntry>>>
            SyncDiary(GraphServiceClient graphClient) {
            var localList2 = (await _diaryStorage.GetDiaryItemsAsync())
                .Select(p => new TrackableDiary(p)).ToList();
            var remoteDiaryEntryList =
                await _remoteDiaryStorage.GetDiariesAsync(graphClient) ??
                new List<DiaryEntry>();
            Dictionary<int, TrackableDiary> localDiaryDictionary =
                new Dictionary<int, TrackableDiary>();
            localDiaryDictionary = localList2.ToDictionary(p => p.Id, p => p);

            IList<Task> ReadTaskList = new List<Task>();
            try {
                foreach (var remoteDiaryItem in remoteDiaryEntryList) {
                    if (localDiaryDictionary.TryGetValue(remoteDiaryItem.Id,
                        out var localDiary)) {
                        if (remoteDiaryItem.TimeStamp > localDiary.timeStamp) {
                            Task b =
                                _remoteDiaryStorage.GetDiaryItemAsync(
                                    graphClient, remoteDiaryItem.Id);
                            ReadTaskList.Add(b);

                            //var diaryContent = await _remoteDiaryStorage.GetDiaryItemAsync(graphClient, remoteDiaryItem.Id);

                            var tmp = new TrackableDiary(new Diary {
                                IsDeleted = remoteDiaryItem.IsDeleted,
                                timeStamp = remoteDiaryItem.TimeStamp,
                                Id = remoteDiaryItem.Id,
                                pathsBlobbed = remoteDiaryItem.pathsBlobbed,
                            });
                            tmp.Updated = true;
                            tmp.Edit = true;
                            localDiaryDictionary[remoteDiaryItem.Id] = tmp;
                            //localDiaryDictionary[remoteDiaryItem.Id] = tmp;
                            //await _diaryStorage.SaveDiaryAsync(
                            //    tmp, 1, false);
                        }
                    } else {
                        Task b =
                            _remoteDiaryStorage.GetDiaryItemAsync(graphClient,
                                remoteDiaryItem.Id);
                        ReadTaskList.Add(b);
                        var tmp = new TrackableDiary(new Diary {
                            IsDeleted = remoteDiaryItem.IsDeleted,
                            timeStamp = remoteDiaryItem.TimeStamp,
                            Id = remoteDiaryItem.Id,
                            pathsBlobbed = remoteDiaryItem.pathsBlobbed,
                        });
                        tmp.Updated = true;
                        tmp.Edit = false;
                        localDiaryDictionary[remoteDiaryItem.Id] = tmp;


                        //var diaryContent = await _remoteDiaryStorage.GetDiaryItemAsync(graphClient, remoteDiaryItem.Id);
                        //var tmp = new TrackableDiary(new Diary
                        //{
                        //    Content = diaryContent.Content,
                        //    Title = diaryContent.Title,
                        //    Date = diaryContent.Date,
                        //    IsDeleted = remoteDiaryItem.IsDeleted,
                        //    timeStamp = remoteDiaryItem.TimeStamp,
                        //    Id = remoteDiaryItem.Id,
                        //});
                        //tmp.Updated = true;
                        //localDiaryDictionary[remoteDiaryItem.Id] = tmp;
                        //await _diaryStorage.SaveDiaryAsync(
                        //    tmp, 0, false);

                    }
                }

                await Task.WhenAll(ReadTaskList);
            } finally {
                List<DiaryContent> results = new List<DiaryContent>();
                foreach (var task in ReadTaskList) {
                    var result = ((Task<DiaryContent>) task).Result;
                    if (localDiaryDictionary[result.Id].Edit) {
                        await _diaryStorage.SaveDiaryAsync(
                            new Diary {
                                Content = result.Content,
                                Title = result.Title,
                                Date = result.Date,
                                IsDeleted =
                                    localDiaryDictionary[result.Id].IsDeleted,
                                pathsBlobbed =
                                    localDiaryDictionary[result.Id]
                                        .pathsBlobbed,
                                Id = result.Id,
                                timeStamp = localDiaryDictionary[result.Id]
                                    .timeStamp,
                            }, 1, false);
                    } else {
                        await _diaryStorage.SaveDiaryAsync(
                            new Diary {
                                Content = result.Content,
                                Title = result.Title,
                                Date = result.Date,
                                IsDeleted =
                                    localDiaryDictionary[result.Id].IsDeleted,
                                pathsBlobbed =
                                    localDiaryDictionary[result.Id]
                                        .pathsBlobbed,
                                Id = result.Id,
                                timeStamp = localDiaryDictionary[result.Id]
                                    .timeStamp,
                            }, 0, false);
                    }
                }
            }

            Dictionary<int, DiaryEntry> remoteDiaryEntryDictionary =
                new Dictionary<int, DiaryEntry>();
            Dictionary<int, Diary> remoteDiary = new Dictionary<int, Diary>();
            remoteDiaryEntryDictionary =
                remoteDiaryEntryList.ToDictionary(p => p.Id, p => p);
            foreach (var localDiary in localList2) {
                if (remoteDiaryEntryDictionary.TryGetValue(localDiary.Id,
                    out var remoteDiaryEntry)) {
                    if (localDiary.timeStamp > remoteDiaryEntry.TimeStamp) {
                        remoteDiaryEntry.IsDeleted = localDiary.IsDeleted;
                        remoteDiaryEntry.TimeStamp = localDiary.timeStamp;
                        remoteDiaryEntry.pathsBlobbed = localDiary.pathsBlobbed;
                        remoteDiary[localDiary.Id] = localDiary;
                    }
                } else {
                    remoteDiary[localDiary.Id] = localDiary;
                    remoteDiaryEntryDictionary[localDiary.Id] = new DiaryEntry {
                        Id = localDiary.Id,
                        IsDeleted = localDiary.IsDeleted,
                        TimeStamp = localDiary.timeStamp,
                        pathsBlobbed = localDiary.pathsBlobbed,
                    };
                }
            }

            return new Tuple<Dictionary<int, Diary>, IList<DiaryEntry>>(
                remoteDiary,
                remoteDiaryEntryDictionary.Values.Select(p => p as DiaryEntry)
                    .ToList());
        }


        public async Task<ServiceResult> SaveImageAsync(GraphServiceClient graphClient, string server)
        {

            var todayDirectory =
                _todayDictionary ?? (_todayDictionary = _photoStorage.GetDirectoryInfo());
            string[] files = todayDirectory.Select(o => o.Name).ToArray();
            var remoteDictionary =
               await _remoteImageStorage.GetDirectoryInfoAsync(graphClient);
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            var fileStream = new MemoryStream();
            if (rootChildren.Any(p => p.Name == "LoveOfTime-image.zip"))
            {
                var fileStream1 = await graphClient.Me.Drive.Root
                    .ItemWithPath("/LoveOfTime-image.zip").Content.Request().GetAsync();
                StreamUtils.Copy(fileStream1, fileStream, new byte[1024]);
            }
            Dictionary<String, bool> localDictionary =
                new Dictionary<string, bool>();
            using (var zf = new ZipFile(fileStream))
            {
                zf.BeginUpdate();
                foreach (System.IO.FileInfo s in todayDirectory)
                {
                    if (!remoteDictionary.TryGetValue(s.Name,
                        out var remoteImage))
                    {
                        using (FileStream fs = s.OpenRead())
                        {
                            MemoryStream ms = new MemoryStream();
                            byte[] buffer = new byte[1024];
                            StreamUtils.Copy(fs, ms, buffer);
                            ms.Position = 0;
                            var dataSource = new CustomStaticDataSource();
                            dataSource.SetStream(ms);
                            zf.Add(dataSource, s.Name);

                        }
                    }
                }
                zf.CommitUpdate();
                fileStream.Position = 0;
                try
                {
                    await graphClient.Me.Drive.Root
                        .ItemWithPath("/LoveOfTime-image.zip").Content.Request()
                        .PutAsync<DriveItem>(fileStream);
                }
                catch (ServiceException e)
                {
                    _alertService.ShowAlert(ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                        ErrorMessages.HttpClientErrorMessage(server, e.Message),
                        ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                    return new ServiceResult
                    {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }
                finally
                {
                    fileStream.Close();
                }
                return new ServiceResult { Status = ServiceResultStatus.Ok };
            }

        }





        /// <summary>
        /// 保存所有的纪念日，包括删除和未删除
        /// </summary>
        /// <param name="memorialDays">所有纪念日包括删除和未删除</param>
        /// <returns></returns>
        public async Task<ServiceResult> SaveMemorialDaysAsync(
            IList<MemorialDay> memorialDays, IList<DiaryEntry> diaryEntries,
            Dictionary<int, Diary> diaries, GraphServiceClient graphClient,
            string server)
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            var fileStream = new MemoryStream();
            if (rootChildren.Any(p => p.Name == "LoveOfTime.zip"))
            {
                var fileStream1 = await graphClient.Me.Drive.Root
                    .ItemWithPath("/LoveOfTime.zip").Content.Request().GetAsync();
                StreamUtils.Copy(fileStream1, fileStream, new byte[1024]);
            }
            var json = JsonConvert.SerializeObject(memorialDays);
            var json2 = JsonConvert.SerializeObject(diaryEntries);
            using (ZipFile zf = new ZipFile(fileStream))
            {
                zf.BeginUpdate();
                if (zf.GetEntry("MemorialDay.json") != null)
                {
                    zf.Delete("MemorialDay.json");
                }
                if (zf.GetEntry("loveDiaries.json") != null)
                {
                    zf.Delete("loveDiaries.json");
                }
                var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var jsonStream2 = new MemoryStream(Encoding.UTF8.GetBytes(json2));
                var dataSource = new CustomStaticDataSource();
                dataSource.SetStream(jsonStream);
                zf.Add(dataSource, "MemorialDay.json");
                var dataSource2 = new CustomStaticDataSource();
                dataSource2.SetStream(jsonStream2);
                zf.Add(dataSource2, "loveDiaries.json");

                foreach (var diaryname in diaries.Keys)
                {
                    var zipEntryName = diaryname.ToString() + ".html";
                    if (zf.GetEntry(zipEntryName) != null)
                    {
                        zf.Delete(zipEntryName);
                    }
                    var dataSource3 = new CustomStaticDataSource();
                    var diaryContent =
                        DiaryContentToHtml.ConvertDiaryContentToHtml(
                            diaries[diaryname]);
                    dataSource3.SetStream(new MemoryStream(Encoding.UTF8.GetBytes(diaryContent)));
                    zf.Add(dataSource3, zipEntryName);
                }
                zf.CommitUpdate();
                jsonStream.Close();
                fileStream.Position = 0;
                try
                {
                    await graphClient.Me.Drive.Root
                        .ItemWithPath("/LoveOfTime.zip").Content.Request()
                        .PutAsync<DriveItem>(fileStream);
                }
                catch (ServiceException e)
                {
                    _alertService.ShowAlert(ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                        ErrorMessages.HttpClientErrorMessage(server, e.Message),
                        ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                    return new ServiceResult
                    {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }
                finally
                {
                    fileStream.Close();
                }
                return new ServiceResult { Status = ServiceResultStatus.Ok };
            }
        }

        public SyncServiceTaskRunner(IAlertService alertService, IRemoteMemorialDayStorage remoteMemorialDayStorage,
            IMemorialDayStorage localMemorialDayStorage, IDiaryStorage localDiaryStorage, IRemoteDiaryStorage remoteDiaryStorage,IPhotoStorage photoStorage,IRemoteImageStorage remoteImageStorage)
        {
            _remoteMemorialDayStorage = remoteMemorialDayStorage;
            _localMemorialDayStorage = localMemorialDayStorage;
            _diaryStorage = localDiaryStorage;
            _remoteDiaryStorage = remoteDiaryStorage;
            _alertService = alertService;
            _photoStorage = photoStorage;
            _remoteImageStorage = remoteImageStorage;
        }


    }
}
