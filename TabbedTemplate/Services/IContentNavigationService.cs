using System.Threading.Tasks;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 内容导航服务接口
    /// </summary>
    public interface IContentNavigationService
    {
        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <returns></returns>
        Task NavigateToAsync(string pageKey);

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        Task NavigateToAsync(string pageKey, object parameter);

        /// <summary>
        /// 回退到前一个页面
        /// </summary>
        /// <param name="pageKey"></param>
        /// <returns></returns>
        Task NavigateBack(string pageKey);
    }
}
