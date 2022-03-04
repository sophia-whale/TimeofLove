using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using TabbedTemplate.Models;

namespace TabbedTemplate.Services
{
    public interface IRemoteMemorialDayStorage
    {
        /// <summary>
        /// 获得所有的纪念日，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        Task<IList<MemorialDay>> GetMemorialDayItemAsync(GraphServiceClient graphClient);

        ///// <summary>
        ///// 保存所有的纪念日，包括删除和未删除
        ///// </summary>
        ///// <param name="memorialDays">所有纪念日包括删除和未删除</param>
        ///// <returns></returns>
        //Task<ServiceResult> SaveMemorialDaysAsync(
        //    IList<MemorialDay> memorialDays,IList<DiaryEntry> diaryEntries,Dictionary<int,Diary> diaries,GraphServiceClient graphClient,string server);

    }
}
