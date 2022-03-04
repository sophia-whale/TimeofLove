using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.XForms.RichTextEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageSource = FFImageLoading.Work.ImageSource;
using CustomItem = Syncfusion.XForms.Buttons;

namespace TabbedTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryDetailPage : ContentPage
    {

        CustomItem.SfButton readOnlyButton;

        public DiaryDetailPage()
        {
            InitializeComponent();
            AddCustomToolbarItems();
            //takePhoto.Clicked += async (sender, args) =>
            //{


            //};

            //pickPhoto.Clicked += async (sender, args) =>
            //{
            //    if (!CrossMedia.Current.IsPickPhotoSupported)
            //    {
            //        DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
            //        return;
            //    }
            //    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            //    {
            //        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            //    });


            //    if (file == null)
            //        return;

            //image.Source = Xamarin.Forms.ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});
            //};

            //takeVideo.Clicked += async (sender, args) =>
            //{
            //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            //    {
            //        DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //        return;
            //    }

            //    var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            //    {
            //        Name = "video.mp4",
            //        Directory = "DefaultVideos",
            //    });

            //    if (file == null)
            //        return;

            //    DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

            //    file.Dispose();
            //};

            //pickVideo.Clicked += async (sender, args) =>
            //{
            //    if (!CrossMedia.Current.IsPickVideoSupported)
            //    {
            //        DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
            //        return;
            //    }
            //    var file = await CrossMedia.Current.PickVideoAsync();

            //    if (file == null)
            //        return;

            //    DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
            //    file.Dispose();
            //};
        }


        protected void Button_Click(object sender, EventArgs e)
        {
            TitleEntry.Text = "";
            RTE.HtmlText = "";
        }

        private void AddCustomToolbarItems()
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();
            collection.Add(ToolbarOptions.Bold);
            collection.Add(ToolbarOptions.Italic);
            collection.Add(ToolbarOptions.Underline);
            collection.Add(ToolbarOptions.FontColor);
            collection.Add(ToolbarOptions.FontSize);
            collection.Add(ToolbarOptions.HighlightColor);
            collection.Add(ToolbarOptions.NumberList);
            collection.Add(ToolbarOptions.BulletList);

            if (Device.RuntimePlatform != Device.iOS)
            {
                readOnlyButton = new CustomItem.SfButton();
                readOnlyButton.BackgroundColor = Color.Transparent;
                readOnlyButton.TextColor = Color.FromHex("#DE333333");
                if (Device.RuntimePlatform == Device.Android)
                {
                    readOnlyButton.HeightRequest = 44;
                    //readOnlyButton.FontFamily = "Text edit 2.ttf#";
                    readOnlyButton.Text = "\ue702";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    //readOnlyButton.FontFamily = "Text edit 2.ttf#Text edit 2";
                    readOnlyButton.Text = "\ue700";
                }
                readOnlyButton.FontSize = 18;
                readOnlyButton.Clicked += ReadOnlyButton_Clicked;
                collection.Add(readOnlyButton);
            }
            else
            {
                Button readonlyButtonIOS = new Button();
                readonlyButtonIOS.FontFamily = "Text edit 2";
                readonlyButtonIOS.Text = "\ue700";
                readonlyButtonIOS.TextColor = Color.FromHex("#DE333333");
                readonlyButtonIOS.Clicked += ReadonlyButtonIOS_Clicked; ;
                collection.Add(readonlyButtonIOS);
            }
            RTE.ToolbarItems = collection;
        }

        CustomItem.SfButton CreateButton(string text)
        {
            CustomItem.SfButton button = new CustomItem.SfButton();
            if (Device.RuntimePlatform == Device.Android)
            {
                button.HeightRequest = 44;
            }
            button.BackgroundColor = Color.Transparent;
            button.TextColor = Color.FromHex("#DE333333");
            if (Device.RuntimePlatform == Device.Android)
            {
                button.FontFamily = "android";
                button.FontSize = 18;
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                button.FontFamily = "V3 Xamarin iOS uwp new.ttf#V3 Xamarin iOS uwp new";
            }
            button.Text = text;
            return button;
        }

        private void ReadonlyButtonIOS_Clicked(object sender, EventArgs e)
        {
            RTE.ReadOnly = !RTE.ReadOnly;
            if (RTE.ReadOnly)
            {
                (sender as Button).Text = "\ue701";
            }
            else
            {
                (sender as Button).Text = "\ue700";
            }
        }

        private void ReadOnlyButton_Clicked(object sender, EventArgs e)
        {
            RTE.ReadOnly = !RTE.ReadOnly;
            if (RTE.ReadOnly)
            {
                if (Device.RuntimePlatform == Device.Android)
                {

                    readOnlyButton.Text = "\ue703";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    readOnlyButton.Text = "\ue701";
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    readOnlyButton.Text = "\ue702";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    readOnlyButton.Text = "\ue700";
                }
            }
        }
    }
}