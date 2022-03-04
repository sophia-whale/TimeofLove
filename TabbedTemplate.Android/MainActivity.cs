
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using GalaSoft.MvvmLight.Ioc;
using TabbedTemplate.Services;
using Microsoft.Identity.Client;
using TabbedTemplate.Views;
using Xamarin.Forms;

namespace TabbedTemplate.Droid
{
    [Activity(Label = "恋爱时光", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;

            ToolbarResource = Resource.Layout.Toolbar;
            Instance = this;

            base.OnCreate(savedInstanceState);

            SimpleIoc.Default.Register<IPhotoPickerService, PhotoPickerService>();
            SimpleIoc.Default.Register<IPhotoStoragePath, PhotoStoragePath>();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.SetFlags(new string[] { "SwipeView_Experimental", "Brush_Experimental", "Shapes_Experimental" });

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            LoadApplication(new App());
            App.AuthUIParent = this;
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }

            if (requestCode == 0)
            {
                AuthenticationContinuationHelper
                    .SetAuthenticationContinuationEventArgs(requestCode, resultCode,
                        intent);
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}