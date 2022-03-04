using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TabbedTemplate.Utils;

namespace TabbedTemplate.Services
{
    public class HoneyWordsService : IHoneyWordsService
    {

        // ******** 构造函数

        /// <summary>
        /// 警告服务
        /// </summary>
        private IAlertService _alertService;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public HoneyWordsService(IAlertService alertService)
        {
            _alertService = alertService;
        }


        /// <summary>
        /// 土味情话
        /// </summary>
        private const string Server = "一言服务器";
        public async Task<string> GetHoneyWordsAsync()
        {
            using var httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response =
                    await httpClient.GetAsync(
                        "https://v1.hitokoto.cn/");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.ShowAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return "连接接口失败";
            }

            var json = await response.Content.ReadAsStringAsync();
            var honeywords = JsonConvert.DeserializeObject<HoneyWordsData>(json);
            if (honeywords != null)
                return honeywords.Hitokoto;
            return "没有返回值";
        }
    }



    public class HoneyWordsData
    {
        public string Hitokoto { get; set; }
        public string From { get; set; }
        public string From_who { get; set; }
    }
}

