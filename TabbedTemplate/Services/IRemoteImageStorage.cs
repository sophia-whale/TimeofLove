using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace TabbedTemplate.Services
{
    public interface IRemoteImageStorage {
        public Task<Tuple<byte[],string>> GetImageAsync(
            GraphServiceClient graphClient, String photoName);

        /// <summary>
        /// 获取图片压缩包的文件列表
        /// </summary>
        /// <param name="graphClient"></param>
        /// <returns></returns>
        public Task<Dictionary<string, bool>> GetDirectoryInfoAsync(
            GraphServiceClient graphClient);
    }
}
