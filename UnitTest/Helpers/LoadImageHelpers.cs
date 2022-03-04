using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TabbedTemplate.UnitTest.Helpers {
    public static class LoadImageHelpers {
        public static async Task<Stream> LoadImageResourceAsync(string directory,string imageName) {
            String imagePath= Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData);
            if (!directory.Equals("")) {
                imagePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), directory);
            }
            imagePath = Path.Combine(imagePath, imageName);
            var assembly = Assembly.GetExecutingAssembly();
            await using (var imageFileStream =
                new FileStream(imagePath, FileMode.OpenOrCreate))
            await using (var imageAssetStream =
                assembly.GetManifestResourceStream(imageName)) {
                await imageAssetStream.CopyToAsync(imageFileStream);
            }

            Stream imageMemoryStream = new MemoryStream();
            using (var imageFileStream =
                new FileStream(imagePath, FileMode.Open)) {
                await imageFileStream.CopyToAsync(imageMemoryStream);
            }

            imageMemoryStream.Position = 0;
            return imageMemoryStream;
        }

        public static void  deleteDirectory(string directory) {
            String imagePath = Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData);
            if (!directory.Equals(""))
            {
                imagePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), directory);
            }
            DirectoryInfo di = new DirectoryInfo(imagePath);
            di.Delete(true);
        }
        

        public static async Task<bool> SaveImageResourceAsync(byte[] data, string imageName) {
            try
            {
                var picturesDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), imageName);
                Directory.CreateDirectory(picturesDirectory);
                imageName = imageName.Replace("/", "");
                string filePath = Path.Combine(picturesDirectory, imageName);

                await using FileStream fs =
                    new FileStream(filePath, FileMode.OpenOrCreate);
                fs.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return false;
            }

            return true;
        }
    }
}