using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using FFImageLoading;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.DocIO.DLS;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using TabbedTemplate.Views;
using Xamarin.Forms;
using ImageSource = Xamarin.Forms.ImageSource;
using Xamarin.Essentials;

namespace TabbedTemplate.ViewModels
{
    public class DiaryDetailViewModel : ViewModelBase
    {

        /******** 绑定属性 ********/

        private string _title;
        private string _date;
        private string _editor;
        private Diary _diary;
        private byte[] _imageSource;
        private static  String[] initStrings = {"-1", "-1", "-1"};
        private List<string> _pathsSource = initStrings.ToList();

        public Diary Diary
        {
            get => _diary;
            set => Set(nameof(Diary), ref _diary, value);
        }

        public string Title
        {
            get => _title;
            set => Set(nameof(Title), ref _title, value);
        }

        public string Editor
        {
            get => _editor;
            set => Set(nameof(Editor), ref _editor, value);
        }

        public byte[] ImageSource {
            get => _imageSource;
            set => Set(nameof(ImageSource), ref _imageSource, value);
        }
        public ObservableCollection<ImageViewModel> ImageViewModelCollection { get; } =
        new ObservableCollection<ImageViewModel>();

        /// <summary>
        /// 日记存储。
        /// </summary>
        private IDiaryStorage _diaryStorage;

        private IContentNavigationService _contentNavigationService;

        private IPhotoStorage _photoStorage;

        private IPhotoPickerService _photoPickerService;

        private IAlertService _alertService;

        private IAddImageInDiaryService _addImageInDiaryService;

        public DiaryDetailViewModel(IPhotoPickerService photoPickerService,IPhotoStorage photoStorage, IDiaryStorage diaryStorage, IContentNavigationService contentNavigationService,IAlertService alertService,IAddImageInDiaryService addImageInDiaryService)
        {
            _photoPickerService = photoPickerService;
            _diaryStorage = diaryStorage;
            _contentNavigationService = contentNavigationService;
            _photoStorage = photoStorage;
            _alertService = alertService;
            _addImageInDiaryService = addImageInDiaryService;
        }


        /// <summary>
        /// 页面显示命令。
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                () => PageAppearingCommandFunction()));

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        public void PageAppearingCommandFunction()
        {
            
            if (ImageViewModelCollection.Count == 0)
            {
                ImageViewModelCollection.Add(new ImageViewModel(this));
            }

        }

        /// <summary>
        /// 日记添加命令。
        /// </summary>
        public RelayCommand AddDiaryCommand =>
            _addDiaryCommand ?? (_addDiaryCommand =
                new RelayCommand(async () =>
                    await AddDiaryCommandFunction()));

        /// <summary>
        /// 日记添加命令。
        /// </summary>
        private RelayCommand _addDiaryCommand;

        public async Task AddDiaryCommandFunction()
        {
            _date = DateTime.Now.ToString("MM/dd/yyyy");
            Diary diary = new Diary
            {
                Title = _title,
                Content = _editor,
                Date = _date,
                IsDeleted = 0,
                paths=_pathsSource
            };
            Diary = diary;
            ImageViewModelCollection.Clear();
            await _contentNavigationService.NavigateBack(ContentNavigationConstant
                .DiaryDetailPage);
            await _diaryStorage.SaveDiaryAsync(Diary, 0, true);
            
        }

        /// <summary>
        /// 日记分享页面导航。
        /// </summary>
        private RelayCommand _shareNavigationCommand;

        public RelayCommand ShareNavigationCommand =>
            _shareNavigationCommand ?? (_shareNavigationCommand =
                new RelayCommand(async () =>
                    await ShareNavigationCommandFunction()));

        public async Task ShareNavigationCommandFunction()
        {
            _date = DateTime.Now.ToString("MM/dd/yyyy");
            Diary diary = new Diary
            {
                Title = _title,
                Content = _editor,
                Date = _date,
                IsDeleted = 0,
                paths = _pathsSource
            };
            Diary = diary;
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.ShareDetailPage, Diary);
        }

        public async Task AddImageViewModel(ImageViewModel imageViewModel) {
            int index = ImageViewModelCollection.IndexOf(imageViewModel);
            int count = ImageViewModelCollection.Count;
            //if ((ImageViewModelCollection.Count == 3)&&(imageViewModel.ImageSource!=null))
            //{
            //     _alertService.ShowAlert("提示",
            //        "您只能添加三张图片", "好的，我知道了");
            //    return;
            //}
            if (imageViewModel.ImageSource == null) {
                var answer =
                    await _alertService.SelectAlert(
                        "选择", "取消", "再考虑一下", "从手机相册选择", "拍照");
                if (answer.Equals("拍照"))
                {
                    _imageSource = await _addImageInDiaryService.TakePhoto();
                    imageViewModel.ImageSource = _imageSource;

                }
                else if (answer.Equals("从手机相册选择")) {
                    _imageSource = await _addImageInDiaryService
                        .PickPhoto();
                    imageViewModel.ImageSource = _imageSource;
                }
                else
                {
                    return;
                }

            }

            var filepath = DateTime.Now.Ticks.ToString() + ".png"; 
            _photoStorage.SavePhoto(_imageSource, filepath);
            _pathsSource[index] = filepath;
            if (ImageViewModelCollection.Count < 3) {
                ImageViewModelCollection.Insert(
                    ImageViewModelCollection.IndexOf(imageViewModel) +
                    1, new ImageViewModel(this));
            }
        }

        public virtual void RemoveImageViewModel(ImageViewModel imageViewModel)
        {
            ImageViewModelCollection.Remove(imageViewModel);
            if (ImageViewModelCollection.Count == 0)
            {
                ImageViewModelCollection.Add(new ImageViewModel(this));
            }
        }
    }
}
