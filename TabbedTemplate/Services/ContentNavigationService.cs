using DailyPoetryX.Services;
using System.Threading.Tasks;
using TabbedTemplate.Models;
using TabbedTemplate.Views;
using Xamarin.Forms;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 内容页导航服务。
    /// </summary>
    public class ContentNavigationService : IContentNavigationService
    {
        // ******** 构造函数

        /// <summary>
        /// 内容页面激活服务。
        /// </summary>
        private IContentPageActivationService _contentPageActivationService;

        /// <summary>
        /// 内容页导航服务。
        /// </summary>
        /// <param name="contentPageActivationService">内容页导航服务。</param>
        public ContentNavigationService(
            IContentPageActivationService contentPageActivationService)
        {
            _contentPageActivationService = contentPageActivationService;
        }

        // ******** 私有变量

        /// <summary>
        /// MainPage。
        /// </summary>
        private MainPage MainPage => (MainPage)Application.Current.MainPage;

        // ******** 继承方法

        /// <summary>
        /// 导航。
        /// </summary>
        /// <param name="pageKey">页面键。</param>
        public async Task NavigateToAsync(string pageKey)
        {
            if (pageKey.Equals(ContentNavigationConstant.DiaryDetailPage))
            {
                await MainPage.CurrentPage.Navigation.PushAsync(new DiaryDetailPage());
            }
            else if (pageKey.Equals(ContentNavigationConstant
              .AddMemorialDayPage))
            {
                await MainPage.CurrentPage.Navigation
                    .PushAsync(_contentPageActivationService.Activate(pageKey));
            }
        }

        public async Task NavigateBack(string pageKey)
        {
            await MainPage.CurrentPage.Navigation.PopAsync();
        }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public async Task NavigateToAsync(string pageKey, object parameter)
        {
            if (pageKey.Equals(ContentNavigationConstant.EditDiaryPage))
            {
                var page1 = new EditDiaryPage();
                NavigationContext.SetParameter(page1, parameter);
                await MainPage.CurrentPage.Navigation.PushAsync(page1);
                return;
            }
            var page = _contentPageActivationService.Activate(pageKey);
            NavigationContext.SetParameter(page, parameter);
            await MainPage.CurrentPage.Navigation.PushAsync(page);
           
        }
    }
    /// <summary>
    /// 内容导航常量
    /// </summary>
    public static class ContentNavigationConstant
    {
        /// <summary>
        /// 日记详情页键
        /// </summary>
        public const string DiaryDetailPage =
            nameof(Views.DiaryDetailPage);

        /// <summary>
        /// 纪念日详情页键
        /// </summary>
        public const string MemorialDetailPage =
            nameof(Views.MemorialDetailPage);

        /// <summary>
        /// 添加纪念日页键
        /// </summary>
        public const string AddMemorialDayPage =
            nameof(Views.AddMemorialDayPage);

        /// <summary>
        /// 日记编辑页键。
        /// </summary>
        public const string EditDiaryPage = nameof(Views.EditDiaryPage);

        /// <summary>
        /// 日记分享页键。
        /// </summary>
        public const string ShareDetailPage = nameof(Views.ShareDetailPage);
    }
}
