using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Graph;

namespace TabbedTemplate.Services
{
    public class OneDriveImageStorage:IRemoteImageStorage
    {
        public async Task<Tuple<byte[],String>> GetImageAsync(
            GraphServiceClient graphClient, String photoName)
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == "LoveOfTime-image.zip"))
            {
                return null;
            }
            var fileStream = await graphClient.Me.Drive.Root
                .ItemWithPath("/LoveOfTime-image.zip").Content.Request().GetAsync();
            ZipInputStream zipStream = new ZipInputStream(fileStream);
            ZipEntry zipEntry = null;
            using (var zf = new ZipFile(fileStream))
            {
                foreach (ZipEntry ze in zf)
                {
                    if (ze.IsDirectory)
                        continue;
                    var name = ze.Name;
                    if (name.Equals(photoName))
                    {
                        byte[] buffer = new byte[1024];
                        using (Stream s = zf.GetInputStream(ze)){
                            var bytes= s.ToByteArray();
                            return new Tuple<byte[],String>(bytes,name);
                        }
                    }
                }

            }

            return null;
        }


        public async Task<Dictionary<string,bool>> GetDirectoryInfoAsync(GraphServiceClient graphClient) {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            Dictionary<String, bool> remoteDictionary =
                new Dictionary<string, bool>();
            if (!rootChildren.Any(p => p.Name == "LoveOfTime-image.zip")) {
                return remoteDictionary;
            }
            var fileStream = await graphClient.Me.Drive.Root
                .ItemWithPath("/LoveOfTime-image.zip").Content.Request()
                .GetAsync();
            ZipInputStream zipStream = new ZipInputStream(fileStream);
            using (var zf = new ZipFile(fileStream))
            {
                foreach (ZipEntry ze in zf)
                {
                    if (ze.IsDirectory)
                        continue;
                    remoteDictionary[ze.Name] = true;
                }
            }

            return remoteDictionary;
        }
    }
}
