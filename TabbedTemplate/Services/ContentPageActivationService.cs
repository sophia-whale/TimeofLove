using DailyPoetryX.Services;
using System;
using System.Collections.Generic;
using TabbedTemplate.Views;
using Xamarin.Forms;
using EditDiaryPage = TabbedTemplate.Views.EditDiaryPage;

namespace TabbedTemplate.Services
{
    public class ContentPageActivationService : IContentPageActivationService
    {
        private Dictionary<string, Type> pageKeyTypeDictionary =
            new Dictionary<string, Type>
            {
                [ContentNavigationConstant.DiaryDetailPage] = typeof(DiaryDetailPage),
                [ContentNavigationConstant.MemorialDetailPage] = typeof(MemorialDetailPage),
                [ContentNavigationConstant.AddMemorialDayPage] = typeof(AddMemorialDayPage),
                [ContentNavigationConstant.EditDiaryPage] = typeof(EditDiaryPage),
                [ContentNavigationConstant.ShareDetailPage] = typeof(ShareDetailPage)
            };
        /// <summary>
        /// 缓存
        /// </summary>
        private Dictionary<string, ContentPage> cache =
            new Dictionary<string, ContentPage>();
        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="pageKey"></param>
        /// <returns></returns>
        public ContentPage Activate(string pageKey) => cache.ContainsKey(pageKey)
            ? cache[pageKey]
            : cache[pageKey] =
                (ContentPage)Activator.CreateInstance(
                    pageKeyTypeDictionary[pageKey]);

    }
}

