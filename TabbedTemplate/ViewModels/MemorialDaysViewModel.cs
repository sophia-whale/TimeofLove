using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MvvmHelpers;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
using Xamarin.Forms.Extended;

namespace TabbedTemplate.ViewModels
{
    public class MemorialDaysViewModel : ViewModelBase {
        
        // ******** 构造函数

        /// <summary>
        /// 纪念日服务
        /// </summary>
        private IMemorialDayStorage _memorialDayStorage;

        /// <summary>
        /// 内容导航服务
        /// </summary>
        private IContentNavigationService _contentNavigationService;

        /// <summary>
        /// 每日情话服务
        /// </summary>
        private IHoneyWordsService _honeyWordsService;
        /// <summary>
        /// 记事内容集合
        /// </summary>
        //在构造函数里赋初始值
        public MemorialDaysViewModel(IMemorialDayStorage memorialDayStorage, IContentNavigationService contentNavigationService)
        {
            _memorialDayStorage = memorialDayStorage;
            _contentNavigationService = contentNavigationService;
            memorialDayStorage.Updated += MemorialDayStorageOnUpdated;
            MemorialDaysCollection.OnCanLoadMore = () => _canLoadMore;
            //返回每次新添加的Poetry列表
            MemorialDaysCollection.OnLoadMore = async () =>
            {
                Status = Loading;
                var days = await memorialDayStorage.GetMemorialDaysAsync(
                    MemorialDaysCollection.Count, PageSize);
                if (days.Count < PageSize)
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }
                else
                {
                    Status = "";
                }

                if (days.Count == 0 && days.Count == 0)
                {
                    Status = NoMoreResult;
                }
                if (days.Count == 0)
                {
                    return null;
                }

                return days;

            };
        }

        // ********** 绑定属性

        private MemorialDay _memorialDay;

        public MemorialDay MemorialDay
        {
            get => _memorialDay;
            set => Set(nameof(MemorialDay), ref _memorialDay, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => Set(nameof(Title), ref _title, value);
        }

        private string _startDate;
        public string StartDate
        {
            get => _startDate;
            set => Set(nameof(StartDate), ref _startDate, value);
        }

        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        /// <summary>
        /// 加载状态。
        /// </summary>
        /// 
        private string _status;

        public InfiniteScrollCollection<MemorialDay>
            MemorialDaysCollection
        { get; } =
            new InfiniteScrollCollection<MemorialDay>();


        public ObservableRangeCollection<MemorialDay> ThatYearMemorialDay { get; } =
        new ObservableRangeCollection<MemorialDay>();
        // ******** 公开变量

        /// <summary>
        /// 一页显示的记事项的数量。
        /// </summary>

        public const int PageSize = 20;

        /// <summary>
        /// 正在载入。
        /// </summary>
        public const string Loading = "正在载入";

        /// <summary>
        /// 没有满足条件的结果。
        /// </summary>
        public const string NoMoreResult = "没有更多结果";

        // ******绑定命令

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(async () => PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            if (pageLoaded)
            {
                return;
            } else {
                lock (pageLoadedLock) {
                    if (pageLoaded) {
                        return;
                    }

                    pageLoaded = true;
                }

            }

            if (!_memorialDayStorage.Initialized())
            {
                await _memorialDayStorage.InitializeAsync();
                Status = "正在初始化纪念日数据库";
            }

            _canLoadMore = true;
            await MemorialDaysCollection.LoadMoreAsync();
            _canLoadMore = false;
            var a = await _memorialDayStorage.GetMemorialDaysAsync(0,
                    int.MaxValue);
            ThatYearMemorialDay.AddRange(a.Where(p => 
                DateTime.Now.ToString("MM/dd").Equals((p.StartDate)
                    .ToString().Substring(0, 5))).ToList());
            
        }

        /// <summary>
        /// 纪念日点击命令
        /// </summary>
        private RelayCommand<MemorialDay> _memorialDayTappedCommand;

        public RelayCommand<MemorialDay> MemorialDayTappedCommand =>
            _memorialDayTappedCommand ?? (_memorialDayTappedCommand =
                new RelayCommand<MemorialDay>(async memorialDay =>
                    await MemorialDayTappedCommandFunction(memorialDay)));

        public async Task MemorialDayTappedCommandFunction(MemorialDay memorialDay)
        {
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.MemorialDetailPage, memorialDay);

        }

        private RelayCommand _addMemorialDayNavigationCommand;
        /// <summary>
        /// 添加页面导航。
        /// </summary>
        public RelayCommand AddMemorialDayNavigationCommand =>
            _addMemorialDayNavigationCommand ??
            (_addMemorialDayNavigationCommand = new RelayCommand(async () =>
                await AddMemorialDayNavigationCommandFunction()));

        public async Task AddMemorialDayNavigationCommandFunction()
        {
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationConstant.AddMemorialDayPage);
        }

        // ****** 私有变量
        /// <summary>
        /// 能否加载更多记事项。
        /// </summary>
        public bool _canLoadMore;

        // ********私有方法
        /// <summary>
        /// 数据同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemorialDayStorageOnUpdated(object sender,
            MemorialStorageUpdatedEventArgs e)
        {
            var memorialDay = e.UpdatedMemorialDay;
            if (e.Flag == 1)
            {
                MemorialDaysCollection.Remove(
                    MemorialDaysCollection.FirstOrDefault(m =>
                        m.Id == memorialDay.Id));
            }
            if (memorialDay.IsDeleted == 0)
            {
                MemorialDaysCollection.Insert(0, memorialDay);
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
