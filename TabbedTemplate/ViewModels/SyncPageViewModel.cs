using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TabbedTemplate.Models;
using TabbedTemplate.Services;

namespace TabbedTemplate.ViewModels
{
    public class SyncPageViewModel : ViewModelBase
    {
        // ******** 构造函数
        /// <summary>
        /// 使用oneDrive进行同步的服务
        /// </summary>
        public ISyncService _syncService;

        /// <summary>
        /// 偏好存储
        /// </summary>
        public IPreferenceStorage _preferenceStorage;


        // ********** 绑定属性


        /// <summary>
        /// OneDrive状态
        /// </summary>
        public string OneDriveStatus
        {
            get => _oneDriveStatus;
            set => Set(nameof(OneDriveStatus), ref _oneDriveStatus, value);
        }

        /// <summary>
        /// OneDrive状态。
        /// </summary>
        private string _oneDriveStatus;

        /// <summary>
        /// oneDrive已登录
        /// </summary>
        private bool _oneDriveSignedIn;

        public bool OneDriveSignedIn
        {
            get => _oneDriveSignedIn;
            set => Set(nameof(OneDriveSignedIn), ref _oneDriveSignedIn, value);
        }

        /// <summary>
        /// OneDrive正在载入
        /// </summary>
        private bool _oneDriveLoading;

        public bool OneDriveLoading
        {
            get => _oneDriveLoading;
            set => Set(nameof(OneDriveLoading), ref _oneDriveLoading, value);
        }

        // ******** 构造函数
        [PreferredConstructor]
        public SyncPageViewModel(IPreferenceStorage preferenceStorage) {
            _syncService = new SyncService(new SyncServiceTaskRunner(
                SimpleIoc.Default.GetInstance<IAlertService>(),
                SimpleIoc.Default.GetInstance<IRemoteMemorialDayStorage>(),
                SimpleIoc.Default.GetInstance<IMemorialDayStorage>(),
                SimpleIoc.Default.GetInstance<IDiaryStorage>(),
                SimpleIoc.Default.GetInstance<IRemoteDiaryStorage>(),SimpleIoc.Default.GetInstance<IPhotoStorage>(),SimpleIoc.Default.GetInstance<IRemoteImageStorage>() ));
            _syncService.StatusChanged += (sender, args) =>
                OneDriveStatus = _syncService.Status;
            _preferenceStorage = preferenceStorage;
        }

        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand _pageAppearingCommand;
        // ******** 绑定命令
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));


        public async Task PageAppearingCommandFunction()
        {
            Task.Run(async () =>
            {
                OneDriveLoading = true;
                OneDriveSignedIn = await _syncService.IsSignedInAsync();
                OneDriveLoading = false;
            });
        }
        /// <summary>
        /// oneDrive登录命令
        /// </summary>
        private RelayCommand _oneDriveSignInCommand;

        public RelayCommand OneDriveSignInCommand =>
            _oneDriveSignInCommand ?? (_oneDriveSignInCommand =
                new RelayCommand(async () =>
                    await OneDriveSignInCommandFunction()));

        public async Task OneDriveSignInCommandFunction()
        {
            OneDriveLoading = true;
            OneDriveSignedIn =
                await _syncService.SignInAsync();
            OneDriveLoading = false;
        }
        /// <summary>
        /// 上一次OneDrive同步时间。
        /// </summary>
        public DateTime LastOneDriveSyncTime
        {
            get =>
                _preferenceStorage.Get(LastOneDriveSyncTimeKey,
                    DateTime.MinValue);
            set
            {
                var oldValue = _preferenceStorage.Get(LastOneDriveSyncTimeKey,
                    DateTime.MinValue);
                if (oldValue == value)
                {
                    return;
                }

                _preferenceStorage.Set(LastOneDriveSyncTimeKey, value);
                RaisePropertyChanged(nameof(LastOneDriveSyncTime), oldValue,
                    value, false);
            }
        }

        public const string LastOneDriveSyncTimeKey = nameof(SyncPageViewModel) + "." +
            nameof(LastOneDriveSyncTime);

        /// <summary>
        /// 同步命令
        /// </summary>
        private RelayCommand _oneDriveSyncCommand;

        public RelayCommand OneDriveSyncCommand =>
            _oneDriveSyncCommand ??
            (_oneDriveSyncCommand = new RelayCommand(async () =>
                await OneDriveSyncCommandFunction()));

        public async Task OneDriveSyncCommandFunction()
        {
            OneDriveLoading = true;
            var syncResult = await _syncService.SyncAsync();
            if (syncResult.Status == ServiceResultStatus.Ok)
            {
                LastOneDriveSyncTime = DateTime.Now;
            }

            OneDriveLoading = false;
        }

        /// <summary>
        /// OneDrive注销命令。
        /// </summary>
        public RelayCommand OneDriveSignOutCommand =>
            _oneDriveSignOutCommand ?? (_oneDriveSignOutCommand =
                new RelayCommand(async () =>
                    await OneDriveSignOutCommandFunction()));

        /// <summary>
        /// OneDrive注销命令。
        /// </summary>
        private RelayCommand _oneDriveSignOutCommand;

        public SyncPageViewModel()
        {

        }

        public async Task OneDriveSignOutCommandFunction()
        {
            OneDriveLoading = true;
            await _syncService.SignOutAsync();
            OneDriveSignedIn = false;
            OneDriveLoading = false;
        }


    }
}
