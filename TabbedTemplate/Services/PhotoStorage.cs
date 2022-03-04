using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TabbedTemplate.Services
{
    public class PhotoStorage : IPhotoStorage {
        private IPhotoStoragePath _photoStoragePath;

        public string PicturesDirectory;
        public bool SavePhoto(byte[] data, string filename)
        {
            try
            {
                Directory.CreateDirectory(PicturesDirectory);
                filename = filename.Replace("/", "");
                string filePath = Path.Combine(PicturesDirectory, filename);

                using (FileStream fs =
                    new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return false;
            }

            return true;
        }

        public byte[] ReadPhoto(string filename)
        {
            Directory.CreateDirectory(PicturesDirectory);
            string filePath = Path.Combine(PicturesDirectory, filename);

            try
            {
                using (var fileStream = System.IO.File.OpenRead(filePath))
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception e)
            {
                var ex = e.ToString();

            }

            return null;
        }

        public IEnumerable<FileSystemInfo> GetDirectoryInfo()
        {
            Directory.CreateDirectory(PicturesDirectory);
            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(PicturesDirectory);
            IEnumerable<System.IO.FileSystemInfo> list1 = dir1.GetFileSystemInfos("*.*",
                System.IO.SearchOption.AllDirectories);
            return list1;
        }

        //public bool CreatePhotoFile(byte[] result, string photoName)
        //{

        //    Directory.CreateDirectory(PicturesDirectory);
        //    try
        //    {
        //        using (var fileStream = File.Create(Path.Combine(PicturesDirectory, photoName)))
        //        {
        //            //result.Seek(0, SeekOrigin.Begin);
        //            fileStream.Write(result, 0, result.Length);
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        var ex = e.ToString();
        //    }
        //    return false;
        //}

        public PhotoStorage(IPhotoStoragePath photoStoragePath) {
            _photoStoragePath = photoStoragePath;
            PicturesDirectory = _photoStoragePath.getPath();
        }
    }
}