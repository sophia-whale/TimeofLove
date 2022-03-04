using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TabbedTemplate.Views;
using Xamarin.Forms;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 警告服务。
    /// </summary>
    public class AlertService : IAlertService
    {
        // ******** 私有变量

        /// <summary>
        /// MainPage。
        /// </summary>
        private MainPage MainPage => (MainPage)Application.Current.MainPage;

        // ******** 继承方法

        /// <summary>
        /// 显示警告。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="message">信息。</param>
        /// <param name="button">按钮文字。</param>
        public void ShowAlert(string title, string message, string button) =>
            Device.BeginInvokeOnMainThread(() =>
                MainPage.DisplayAlert(title, message, button));

        public async Task<string> SelectAlert(string title, string cancel,
            string destruction, params string[] buttons) {
            var answer =
                await MainPage.DisplayActionSheet(
                    title, cancel, destruction, buttons);
            return answer;
        }


    }
}
