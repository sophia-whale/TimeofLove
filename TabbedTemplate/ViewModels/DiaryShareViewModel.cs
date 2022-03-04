using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Graph;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaTextRenderer;
using TabbedTemplate.Extensions;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.Views;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.PlatformConfiguration;
using File = System.IO.File;
using Font = SkiaTextRenderer.Font;
using PropertyChangingEventHandler = Xamarin.Forms.PropertyChangingEventHandler;

namespace TabbedTemplate.ViewModels
{
    public class DiaryShareViewModel : ViewModelBase
    {
        private SKBitmap _bitmap = null;
        private Diary _diary;
        private SKPaint _titleTextPaint;
        private SKPaint _titleShadowPaint;
        private SKPaint _contentTextPaint;
        private SKPaint _contentShadowPaint;
        private SKImage _image;

        public Diary Diary
        {
            get => _diary;
            set => Set(nameof(Diary), ref _diary, value);
        }

        public SKBitmap Bitmap
        {
            get => _bitmap;
            set => Set(nameof(Bitmap), ref _bitmap, value);
        }

        public SKImage Image
        {
            get => _image;
            set => Set(nameof(Image), ref _image, value);
        }

        public SKPaint TitleTextPaint
        {
            get => _titleTextPaint;
            set => Set(nameof(TitleTextPaint), ref _titleTextPaint, value);
        }

        public SKPaint TitleShadowPaint
        {
            get => _titleShadowPaint;
            set => Set(nameof(TitleShadowPaint), ref _titleShadowPaint, value);
        }

        public SKPaint ContentTextPaint
        {
            get => _contentTextPaint;
            set => Set(nameof(ContentTextPaint), ref _contentTextPaint, value);
        }

        public SKPaint ContentShadowPaint
        {
            get => _contentShadowPaint;
            set => Set(nameof(ContentShadowPaint), ref _contentShadowPaint, value);
        }

        private IPhotoPickerService _photoPickerService;

        private IPhotoStorage _photoStorage;

        private IAlertService _alertService;

        public Renders.IRender Render { get; set; }

        public ObservableCollection<ImageSource> FontTypeImageCollection { get; } =
            new ObservableCollection<ImageSource>();

        public event EventHandler RefreshRequested;

        public DiaryShareViewModel(IPhotoPickerService photoPickerService, IPhotoStorage photoStorage, IAlertService alertService)
        {
            _photoPickerService = photoPickerService;
            _photoStorage = photoStorage;
            _alertService = alertService;
            _diary = new Diary { Title = "日记分享", Content = "可以选择自己喜欢的模板哦" };
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.hearts.png");
            _titleShadowPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                TextSize = 120,
                FakeBoldText = true,
                TextEncoding = SKTextEncoding.Utf32,
                Color = SKColors.Black
            };

            _titleTextPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                TextSize = 120,
                FakeBoldText = true,
                TextEncoding = SKTextEncoding.Utf32,
                Color = SKColors.Pink
            };

            _contentShadowPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                TextSize = 50,
                FakeBoldText = true,
                TextEncoding = SKTextEncoding.Utf32,
                Color = SKColors.Black
            };

            _contentTextPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                TextSize = 50,
                FakeBoldText = true,
                TextEncoding = SKTextEncoding.Utf32,
                Color = SKColors.White
            };
            Render = new Renders.ShareCanvasViewRender();
            ShareRender();
        }

        /// <summary>
        /// 选择图片Command。
        /// </summary>
        private RelayCommand _selectImageCommand;

        public RelayCommand SelectImageCommand =>
            new RelayCommand(async () => SelectImageCommandFunction());

        public async void SelectImageCommandFunction()
        {
            IPhotoPickerService photoLibrary = DependencyService.Get<IPhotoPickerService>();
            using Stream stream = await photoLibrary.GetImageStreamAsync();
            if (stream != null)
            {
                _bitmap = SKBitmap.Decode(stream);
            }
            ShareRender();
        }

        /// <summary>
        /// 粉白样式command。
        /// </summary>
        private RelayCommand _heartsPaintCommand;

        public RelayCommand HeartsPaintCommand =>
            _heartsPaintCommand ?? (_heartsPaintCommand =
                new RelayCommand(async () => await HeartsPaintCommandFunction()));

        public async Task HeartsPaintCommandFunction() {
            _titleTextPaint.Color = SKColors.Pink;
            _contentTextPaint.Color = SKColors.White;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.hearts.png");
            ShareRender();
        }

        /// <summary>
        /// 蓝粉样式command。
        /// </summary>
        private RelayCommand _snowPaintCommand;

        public RelayCommand SnowPaintCommand =>
            _snowPaintCommand ?? (_snowPaintCommand =
                new RelayCommand(async () => await SnowPaintCommandFunction()));

        public async Task SnowPaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.Blue;
            _contentTextPaint.Color = SKColors.Pink;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.snow.jpg");
            ShareRender();
        }

        /// <summary>
        /// 白黄样式command。
        /// </summary>
        private RelayCommand _treePaintCommand;

        public RelayCommand TreesPaintCommand =>
            _treePaintCommand ?? (_treePaintCommand =
                new RelayCommand(async () => await TreePaintCommandFunction()));

        public async Task TreePaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.White;
            _contentTextPaint.Color = SKColors.Yellow;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.trees.png");
            ShareRender();
        }

        private RelayCommand _whiteCloudPaintCommand;

        public RelayCommand WhiteCloudPaintCommand =>
            _whiteCloudPaintCommand ?? (_whiteCloudPaintCommand =
                new RelayCommand(async () => await WhiteCloudPaintCommandFunction()));

        public async Task WhiteCloudPaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.White;
            _contentTextPaint.Color = SKColors.LightBlue;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.whiteCloud.jpg");
            ShareRender();
        }

        private RelayCommand _bluePaintCommand;

        public RelayCommand BluePaintCommand =>
            _bluePaintCommand ?? (_bluePaintCommand =
                new RelayCommand(async () => await BluePaintCommandFunction()));

        public async Task BluePaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.LightBlue;
            _contentTextPaint.Color = SKColors.LightYellow;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.blue.jpg");
            ShareRender();
        }

        private RelayCommand _moonPaintCommand;

        public RelayCommand MoonPaintCommand =>
            _moonPaintCommand ?? (_moonPaintCommand =
                new RelayCommand(async () => await MoonPaintCommandFunction()));

        public async Task MoonPaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.LightSkyBlue;
            _contentTextPaint.Color = SKColors.MediumPurple;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.moon.jpg");
            ShareRender();
        }

        private RelayCommand _pinkCloudPaintCommand;

        public RelayCommand PinkCloudPaintCommand =>
            _pinkCloudPaintCommand ?? (_pinkCloudPaintCommand =
                new RelayCommand(async () => await PinkCloudPaintCommandFunction()));

        public async Task PinkCloudPaintCommandFunction()
        {
            _titleTextPaint.Color = SKColors.White;
            _contentTextPaint.Color = SKColors.LightYellow;
            _bitmap = BitmapExtensions.LoadBitmapResource(
                typeof(DiaryShareViewModel),
                "TabbedTemplate.Resources.pinkCloud.jpg");
            ShareRender();
        }

        /// <summary>
        /// ShareTemplateRender
        /// </summary>
        public void ShareRender()
        {
            ((Renders.ShareCanvasViewRender)Render).TitleShadowPaint = _titleShadowPaint;
            ((Renders.ShareCanvasViewRender)Render).TitleTextPaint = _titleTextPaint;
            ((Renders.ShareCanvasViewRender)Render).ContentShadowPaint = _contentShadowPaint;
            ((Renders.ShareCanvasViewRender)Render).ContentTextPaint = _contentTextPaint;
            ((Renders.ShareCanvasViewRender)Render).Bitmap = _bitmap;
            ((Renders.ShareCanvasViewRender)Render).Diary = _diary;
        }

        /// <summary>   /// 保存分享图到本地。
        /// </summary>
        private RelayCommand _saveShareImageCommand;

        public RelayCommand SaveShareImageCommand =>
            _saveShareImageCommand ?? (_saveShareImageCommand =
                new RelayCommand((() => SaveShareImageCommandFunction())));

        public void SaveShareImageCommandFunction()
        {
            var fileName = Diary.Title + Diary.Date + ".png";
            _image = ((Renders.ShareCanvasViewRender)Render).Image;
            var data = _image.Encode(SKEncodedImageFormat.Png, 80).ToArray();
            var isSavePhotoAsync = _photoPickerService.SavePhotoAsync(data, fileName);
            if (isSavePhotoAsync) {
                _alertService.ShowAlert("保存分享图片", "分享图片保存成功", "确定");
            } else {
                _alertService.ShowAlert("保存分享图片", "分享图片保存失败", "确定");
            }
            //for test
            
            _photoStorage.SavePhoto(data, fileName);
        }
        
    }

}