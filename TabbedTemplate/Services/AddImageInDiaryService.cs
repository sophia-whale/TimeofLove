using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace TabbedTemplate.Services
{
    public class AddImageInDiaryService:IAddImageInDiaryService
    {
        public async Task<byte[]> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //Directory = "Test",
                CompressionQuality = 75,
                //SaveToAlbum = true,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return null;
            var stream = file.GetStream();
            file.Dispose();
            var imageSource = stream.ToByteArray();
            return imageSource;
        }

        public async Task<byte[]> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return null;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });


            if (file == null)
                return null;

            var stream = file.GetStream();
            file.Dispose();
            var imageSource = stream.ToByteArray();
            return imageSource;
        }
    }
}
