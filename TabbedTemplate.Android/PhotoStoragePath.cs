using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TabbedTemplate.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(IPhotoStoragePath))]
namespace TabbedTemplate.Droid
{
    public class PhotoStoragePath : IPhotoStoragePath

    {
        public string getPath() { 
            string path= Path.Combine(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath, "ImageInDiary");
            return path;
        }
    }
}