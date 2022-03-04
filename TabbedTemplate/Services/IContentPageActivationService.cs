using Xamarin.Forms;

namespace DailyPoetryX.Services
{
    /// <summary>
    /// 内容页面激活服务。
    /// </summary>
    public interface IContentPageActivationService
    {
        /// <summary>
        /// 激活页面。
        /// </summary>
        /// <returns>页面键。</returns>
        /// <param name="pageKey">激活的内容页面。</param>
        ContentPage Activate(string pageKey);
    }
}