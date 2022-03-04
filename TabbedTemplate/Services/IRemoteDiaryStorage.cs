using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public interface IRemoteDiaryStorage
    {
        /// <summary>
        /// 获得所有的日记条目，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        Task<DiaryContent> GetDiaryItemAsync(GraphServiceClient graphClient, int id);

        ///// <summary>
        ///// 保存日记和日记条目
        ///// </summary>
        ///// <param name="memorialDays">所有纪念日包括删除和未删除</param>
        ///// <returns></returns>
        //Task<ServiceResult> SaveDiariesAsync(
        //    IList<MemorialDay> memorialDays, GraphServiceClient graphClient, string server);

        Task<IList<DiaryEntry>> GetDiariesAsync(GraphServiceClient graphClient);
    }
}
