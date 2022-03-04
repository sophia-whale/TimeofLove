using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using Syncfusion.XForms.RichTextEditor;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using Xamarin.Forms;
using ImageSource = Xamarin.Forms.ImageSource;

namespace TabbedTemplate.ViewModels
{
    public class DiaryEditViewModel : ViewModelBase
    {
        // ******** 私有变量
        private Diary _diary;

        // ********** 绑定属性
        public Diary Diary
        {
            get => _diary;
            set => Set(nameof(Diary), ref _diary, value);
        }

        // ******** 构造函数
        private IDiaryStorage _diaryStorage;
        private IContentNavigationService _contentNavigationService;
        private IPhotoPickerService _photoPickerService;
        private IPhotoStorage _photoStorage;
        
        public DiaryEditViewModel(IPhotoPickerService photoPickerService, IDiaryStorage diaryStorage, IContentNavigationService contentNavigationService,IPhotoStorage photoStorage)
        {
            _photoPickerService = photoPickerService;
            _diaryStorage = diaryStorage;
            _contentNavigationService = contentNavigationService;
            _photoStorage = photoStorage;
            //ImageInsertCommand = new Command<object>(ImageLoad);
            
        }

        /// <summary>
        /// 页面显示命令
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(async () =>
                    await PageAppearingCommandFunction()));

        private RelayCommand _pageAppearingCommand;

        public async Task PageAppearingCommandFunction() {
            DiaryImage.Clear();
            if (Diary.paths == null) {
                return;
            }
            foreach (var imagePath in Diary.paths) {
                if (imagePath == "-1") {
                    continue;
                }
                var imageSource =
                     _photoStorage.ReadPhoto(imagePath);
                DiaryImage.Add(new ImagePath() {
                    ImageSource=imageSource,
                });
            }
        }

        // ******** 公开变量
        public ObservableRangeCollection<ImagePath> DiaryImage { get; } =
            new ObservableRangeCollection<ImagePath>();

        /// <summary>
        /// 日记添加命令。
        /// </summary>
        public RelayCommand EditDiaryCommand =>
            _editDiaryCommand ?? (_editDiaryCommand =
                new RelayCommand(async () =>
                    await EditDiaryCommandFunction()));

        /// <summary>
        /// 日记添加命令。
        /// </summary>
        private RelayCommand _editDiaryCommand;

        public async Task EditDiaryCommandFunction()
        {
            await _contentNavigationService.NavigateBack(ContentNavigationConstant
              .EditDiaryPage);
            await _diaryStorage.SaveDiaryAsync(Diary, 1, true);
        }
        /*
        /// <summary>
        /// Insert image command property
        /// </summary>
        public ICommand ImageInsertCommand { get; set; }
        public ICommand FontCommand { get; set; }

        /// <summary>
        /// Creates a event args for Image Insert
        /// </summary>
        void ImageLoad(object obj)
        {
            ImageInsertedEventArgs imageInsertedEventArgs = (obj as ImageInsertedEventArgs);
            this.GetImage(imageInsertedEventArgs);
        }
        /// <summary>
        /// Gets image stream from picker using dependency service.
        /// </summary>
        /// <param name="imageInsertedEventArgs">Event args to be passed for dependency service</param>
        async void GetImage(ImageInsertedEventArgs imageInsertedEventArgs)
        {
            Stream imageStream = await _photoPickerService.GetImageStreamAsync();
            Syncfusion.XForms.RichTextEditor.ImageSource imageSource = new Syncfusion.XForms.RichTextEditor.ImageSource();
            imageSource.ImageStream = imageStream;
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sig.png");
            using var OutFileStream = new FileStream(_fileName, FileMode.Create);
            using (new MemoryStream())
            {
                imageStream.CopyTo(OutFileStream);
            }
            imageInsertedEventArgs.ImageSourceCollection.Add(imageSource);
        }*/

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
            Diary diary = new Diary
            {
                Title = Diary.Title,
                Content = Diary.Content,
                Date = Diary.Content,
                paths = Diary.paths,
                IsDeleted = 0
            };
            Diary = diary;
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.ShareDetailPage, diary);
        }


       
    }
    public class ImagePath
    {
        public byte[] ImageSource { get; set; }
    }
}