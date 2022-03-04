using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TabbedTemplate.Services
{
    public interface IPhotoStorage
    {
        bool SavePhoto(byte[] data, string filename);
        byte[] ReadPhoto(string filename);
        public IEnumerable<FileSystemInfo> GetDirectoryInfo();
        //public bool CreatePhotoFile(byte[] data, string photoName);
    }
}
