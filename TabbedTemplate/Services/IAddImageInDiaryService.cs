using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TabbedTemplate.Services
{
    public interface IAddImageInDiaryService {
        public  Task<byte[]> TakePhoto();
        public Task<byte[]> PickPhoto();

    }
}
