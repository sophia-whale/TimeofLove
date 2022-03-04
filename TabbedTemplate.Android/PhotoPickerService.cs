using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Java.IO;
using TabbedTemplate.Droid;
using TabbedTemplate.Services;
using Xamarin.Forms;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using PhotoPickerService = TabbedTemplate.Droid.PhotoPickerService;
using Stream = System.IO.Stream;

[assembly: Dependency(typeof(PhotoPickerService))]

namespace TabbedTemplate.Droid {
    public class PhotoPickerService : IPhotoPickerService {
        public Task<System.IO.Stream> GetImageStreamAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource =
                new TaskCompletionSource<System.IO.Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }

        // Saving photos requires android.permission.WRITE_EXTERNAL_STORAGE in AndroidManifest.xml

        public bool SavePhotoAsync(byte[] data, string filename) {
            try {
                string picturesDirectory = Path.Combine(
                    Environment
                        .GetExternalStoragePublicDirectory(Environment
                            .DirectoryPictures).Path, "TimeOfLove");
                Directory.CreateDirectory(picturesDirectory);
                filename = filename.Replace("/", "");
                string filePath = Path.Combine(picturesDirectory, filename);

                using (FileStream fs =
                    new FileStream(filePath, FileMode.OpenOrCreate)) {
                    fs.Write(data, 0, data.Length);
                }
            } catch (Exception e) {
                var ex = e.ToString();
                return false;
            }

            return true;
        }

        public byte[] ReadPhotoAsync(string filename) {
            throw new NotImplementedException();
        }
    }
}