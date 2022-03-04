using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using SQLite;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using Xamarin.Forms.Extended;

namespace TabbedTemplate.ViewModels
{
    public class DiariesViewModel : ViewModelBase
    {

        /// <summary>
        /// 加载状态。
        /// </summary>
        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }


        public string Title
        {
            get => _title;
            set => Set(nameof(Title), ref _title, value);
        }

        public string Content
        {
            get => _content;
            set => Set(nameof(Content), ref _content, value);
        }

        private string _status;
        private string _title;
        private string _content;

        private string _honeyWords;

        public string HoneyWords
        {
            get => _honeyWords;
            set => Set(nameof(HoneyWords), ref _honeyWords, value);
        }

        /// <summary>
        /// 日记存储服务
        /// </summary>
        private IDiaryStorage _diaryStorage;

        /// <summary>
        /// 内容导航服务
        /// </summary>
        private IContentNavigationService _contentNavigationService;

        /// <summary>
        /// 每日情话服务
        /// </summary>
        private IHoneyWordsService _honeyWordsService;

        /// <summary>
        /// 日记显示ViewModel。
        /// </summary>
        /// <param name="diaryStorage">日记存储。</param>
        /// <param name="contentNavigationService">内容导航服务</param>
        public DiariesViewModel(IDiaryStorage diaryStorage, IContentNavigationService contentNavigationService, IHoneyWordsService honeyWordsService)
        {
            // TODO 删除供测试的初始化代码
            _diaryStorage = diaryStorage;
            _contentNavigationService = contentNavigationService;
            _honeyWordsService = honeyWordsService;
            _diaryStorage.Updated += DiaryStorageOnUpdated;

            DiaryCollection.OnCanLoadMore = () => _canLoadMore;
            DiaryCollection.OnLoadMore = async () =>
            {
                Status = Loading;
                var diaries =
                    await diaryStorage.GetAllDiariesAsync(DiaryCollection.Count, PageSize);

                if (diaries.Count < PageSize)
                {
                    Status = NoMoreResult;
                    _canLoadMore = false;
                }
                else
                {
                    Status = "";
                }

                if (diaries.Count == 0 && DiaryCollection.Count == 0)
                {
                    Status = NoMoreResult;
                }

                if (diaries.Count == 0)
                {
                    return null;
                }
                return diaries;
            };
        }

        public InfiniteScrollCollection<Diary> DiaryCollection { get; } =
        new InfiniteScrollCollection<Diary>();

        // ******** 公开变量

        public IDiaryStorage diaryStorage;

        /// <summary>
        /// 一页显示的日记的数量。
        /// </summary>

        public const int PageSize = 20;

        /// <summary>
        /// 正在载入。
        /// </summary>
        public const string Loading = "正在载入";


        /// <summary>
        /// 没有更多结果。
        /// </summary>
        public const string NoMoreResult = "没有更多结果";
        // ******绑定命令

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        //页面初始状态
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(async () =>
                    await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            if (pageLoaded)
            {
                return;
            }
            else
            {
                lock (pageLoadedLock)
                {
                    if (pageLoaded)
                    {
                        return;
                    }
                    pageLoaded = true;
                }
            }

            if (!_diaryStorage.Initialized())
            {
                await _diaryStorage.InitializeAsync();
            }

            Task.Run(async () =>
            {
                HoneyWords = await _honeyWordsService.GetHoneyWordsAsync();
            });
            _canLoadMore = true;
            await DiaryCollection.LoadMoreAsync();
        }

        /// <summary>
        /// 导航至日记添加页面命令。
        /// </summary>
        public RelayCommand AddDiaryNavigationCommand =>
            _addDiaryNavigationCommand ?? (_addDiaryNavigationCommand =
                new RelayCommand(async () =>
                    await AddDiaryNavigationCommandFunction()));

        /// <summary>
        /// 导航至日记添加页面命令。
        /// </summary>
        private RelayCommand _addDiaryNavigationCommand;

        public async Task AddDiaryNavigationCommandFunction() =>
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.DiaryDetailPage);


        /// <summary>
        /// 导航至日记添加页面命令。
        /// </summary>
        public RelayCommand<Diary> EditDiaryNavigationCommand =>
            _editDiaryNavigationCommand ?? (_editDiaryNavigationCommand =
                new RelayCommand<Diary>(async diary =>
                    await EditDiaryNavigationCommandFunction(diary)));

        /// <summary>
        /// 导航至日记添加页面命令。
        /// </summary>
        private RelayCommand<Diary> _editDiaryNavigationCommand;

        public async Task EditDiaryNavigationCommandFunction(Diary diary) =>
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.EditDiaryPage, diary);

        /// <summary>
        /// 删除某条日记
        /// </summary>
        private RelayCommand<Diary> _deleteDiaryCommand;

        public RelayCommand<Diary> DeleteDiaryCommand =>
            _deleteDiaryCommand ?? (_deleteDiaryCommand =
                new RelayCommand<Diary>(async diary =>
                    await DeleteDiaryCommandFunction(diary)));

        public async Task DeleteDiaryCommandFunction(Diary diary)
        {
            diary.IsDeleted = 1;
            await _diaryStorage.SaveDiaryAsync(diary, 1, true);
        }


        public event EventHandler<DiaryStorageUpdatedEventArgs> Updated;
        // ****** 私有变量
        /// <summary>
        /// 能否加载更多日记。
        /// </summary>
        public bool _canLoadMore;


        // ********私有方法
        /// <summary>
        /// 数据同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DiaryStorageOnUpdated(object sender,
            DiaryStorageUpdatedEventArgs e)
        {
            var diary = e.UpdatedDiary;
            if (e.Flag == 1)
            {
                DiaryCollection.Remove(
                    DiaryCollection.FirstOrDefault(m =>
                        m.Id == diary.Id));
            }
            if (diary.IsDeleted == 0)
            {
                DiaryCollection.Insert(0, diary);
            }

        }

        // ******** 私有变量
        /// <summary>
        /// 页面已加载
        /// </summary>
        private volatile bool pageLoaded;

        //页面已加载锁
        private readonly object pageLoadedLock = new object();
    }
}
