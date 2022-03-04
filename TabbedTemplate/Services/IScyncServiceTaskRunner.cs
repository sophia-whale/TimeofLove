using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public interface ISyncServiceTaskRunner
    {
        public Task<IList<MemorialDay>>
            SyncMemorialDay(GraphServiceClient graphClient);

        public Task<Tuple<Dictionary<int, Diary>, IList<DiaryEntry>>>
            SyncDiary(GraphServiceClient graphClient);

        public Task<ServiceResult> SaveMemorialDaysAsync(
            IList<MemorialDay> memorialDays, IList<DiaryEntry> diaryEntries,
            Dictionary<int, Diary> diaries, GraphServiceClient graphClient,
            string server);

        public Task SyncImage(GraphServiceClient graphClient);

        public Task<ServiceResult> SaveImageAsync(
            GraphServiceClient graphClient, string server);
    }
}
