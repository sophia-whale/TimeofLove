using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TabbedTemplate.Models;
using TabbedTemplate.Utils;

namespace TabbedTemplate.Services
{
    public class OneDriveDiaryStorage : IRemoteDiaryStorage
    {
        /// <summary>
        /// 获得所有的日记条目，包括删除和未删除
        /// </summary>
        /// <returns></returns>
        public async Task<DiaryContent> GetDiaryItemAsync(
            GraphServiceClient graphClient, int id)
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == "LoveOfTime.zip"))
            {
                return null;
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
                    var zipEntryName = id.ToString() + ".html";
                    if (ze.Name.Equals(zipEntryName))
                    {
                        byte[] buffer = new byte[1024];
                        using (Stream s = zf.GetInputStream(ze))
                        using (MemoryStream ms = new MemoryStream())
                        {
                            StreamUtils.Copy(s, ms, buffer);
                            ms.Position = 0;
                            var jsonReader = new StreamReader(ms);
                            var diaryContent =
                                HtmlToDairyContent.convertHtmlToDiaryContent(
                                    jsonReader.ReadToEnd());
                            diaryContent.Id = id;
                            jsonReader.Close();
                            return diaryContent;
                        }
                    }
                }

            }

            return null;
        }

        /// <summary>
        /// 保存日记和日记条目
        /// </summary>
        /// <param name="memorialDays">所有纪念日包括删除和未删除</param>
        /// <returns></returns>
        public async Task<ServiceResult> SaveDiariesAsync(IList<MemorialDay> memorialDays, GraphServiceClient graphClient,
            string server)
        {
            throw new NotImplementedException();
        }


        public async Task<IList<DiaryEntry>> GetDiariesAsync(GraphServiceClient graphClient)
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == "LoveOfTime.zip"))
            {
                return null;
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
                    if (ze.Name.Equals("loveDiaries.json"))
                    {
                        byte[] buffer = new byte[1024];
                        using (Stream s = zf.GetInputStream(ze))
                        using (MemoryStream ms = new MemoryStream())
                        {

                            StreamUtils.Copy(s, ms, buffer);
                            ms.Position = 0;
                            var jsonReader = new StreamReader(ms);
                            var diaryEntryList =
                                JsonConvert.DeserializeObject<IList<DiaryEntry>>(
                                    await jsonReader.ReadToEndAsync());
                            jsonReader.Close();
                            return diaryEntryList?.ToList() ?? new List<DiaryEntry>();
                        }

                    }
                }
            }
            return null;
        }


    }
}
