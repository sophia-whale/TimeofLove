using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using TabbedTemplate.Models;
using TimeOfLove.Confidential;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TabbedTemplate.Utils;

namespace TabbedTemplate.Services
{
    public class OneDriveMemorialDayStorage : IRemoteMemorialDayStorage
    {
        //// ******** 公开变量
        ///// <summary>
        ///// 状态
        ///// </summary>
        //public string Status {
        //    get => _status;
        //    set {
        //        _status = value;
        //        StatusChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}

        //private string _status;

        ///// <summary>
        ///// 状态改变事件
        ///// </summary>
        //public event EventHandler StatusChanged;

        //// ******** 私有变量
        //private IAlertService _alertService;



        // ******** 继承方法

        /// <summary>
        /// 获得所有的纪念日，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MemorialDay>> GetMemorialDayItemAsync(
            GraphServiceClient graphClient)
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == "LoveOfTime.zip"))
            {
                return new List<MemorialDay>();
            }

            var fileStream = await graphClient.Me.Drive.Root
                .ItemWithPath("/LoveOfTime.zip").Content.Request().GetAsync();
            ZipInputStream zipStream = new ZipInputStream(fileStream);
            ZipEntry zipEntry = null;
            using (var zf = new ZipFile(fileStream))
            {
                foreach (ZipEntry ze in zf)
                {
                    if (ze.IsDirectory)
                        continue;
                    if (ze.Name.Equals("MemorialDay.json"))
                    {
                        byte[] buffer = new byte[1024];
                        using (Stream s = zf.GetInputStream(ze))
                        using (MemoryStream ms = new MemoryStream())
                        {

                            StreamUtils.Copy(s, ms, buffer);
                            ms.Position = 0;
                            var jsonReader = new StreamReader(ms);
                            var memmorialDayList =
                                JsonConvert
                                    .DeserializeObject<IList<MemorialDay>>(
                                        await jsonReader.ReadToEndAsync());
                            jsonReader.Close();
                            return memmorialDayList?.ToList() ??
                                new List<MemorialDay>();
                        }

                    }
                }
            }
            return null;
        }



        //// ******** 构造函数
        ///******** 公开方法 ********/

        ///// <summary>
        ///// 远程收藏存储。
        ///// </summary>
        ///// <param name="alertService">警告服务。</param>
        //public OneDriveMemorialDayStorage(IAlertService alertService)
        //{
        //    this._alertService = alertService;

        //}

    }
}
