using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using TabbedTemplate.Models;
using TimeOfLove.Confidential;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 同步服务
    /// </summary>
    public class SyncService : ISyncService
    {
        // ******** 私有变量
        /// <summary>
        /// 本地收藏存储
        /// </summary>
        private IMemorialDayStorage _localMemorialDayStorage;

        /// <summary>
        ///本地存储的日记
        /// </summary>
        private IDiaryStorage _diaryStorage;

        /// <summary>
        /// 远程纪念日存储
        /// </summary>
        private IRemoteMemorialDayStorage _remoteMemorialDayStorage;

        /// <summary>
        /// 远程日记存储
        /// </summary>
        private IRemoteDiaryStorage _remoteDiaryStorage;

        /// <summary>
        /// 服务器常量
        /// </summary>
        private const string Server = "OneDrive服务器";

        /// <summary>
        /// Miscroft Graph服务范围
        /// </summary>
        private string[] _scopes = OneDriveOAuthSettings.Scopes.Split(' ');

        /// <summary>
        /// 客户端验证范围
        /// </summary>
        private IPublicClientApplication _pca;

        /// <summary>
        /// Microsoft Graph 客户端
        /// </summary>
        private GraphServiceClient _graphClient;

        private ISyncServiceTaskRunner _syncServiceTaskRunner;


        // ******** 公开变量
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _status;

        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// 远程纪念日存储。
        /// </summary>
        public IRemoteMemorialDayStorage RemoteMemorialDayStorage =>
            _remoteMemorialDayStorage;

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsSignedInAsync()
        {
            Status = "正在检查OneDrive登录状态";

            string accessToken = string.Empty;

            try
            {
                var accounts = await _pca.GetAccountsAsync();
                if (accounts.Any())
                {
                    var silentAuthResult = await _pca
                        .AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                    accessToken = silentAuthResult.AccessToken;
                }
            }
            catch (MsalUiRequiredException)
            {
                return false;
            }

            return !string.IsNullOrEmpty(accessToken);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SignInAsync()
        {
            Status = "正在登录OneDrive";
            try
            {
                var interactiveRequest = _pca.AcquireTokenInteractive(_scopes);
                if (App.AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(App.AuthUIParent);
                }

                await interactiveRequest.ExecuteAsync();
            }
            catch (MsalClientException e)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            Status = "正在退出OneDrive登录";

            var accounts = await _pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await _pca.RemoveAsync(accounts.First());
                accounts = await _pca.GetAccountsAsync();
            }
        }


        ///// <summary>
        ///// 同步
        ///// </summary>
        ///// <returns></returns>
        public async Task<ServiceResult> SyncAsync()
        {
            IList<DiaryEntry> diaryEntris = new List<DiaryEntry>();
            Dictionary<int, Diary> diaries = new Dictionary<int, Diary>();
            IList<MemorialDay> memorialDays = new List<MemorialDay>();
            Status = "正在将远程的纪念日同步到本地";
            memorialDays = await _syncServiceTaskRunner.SyncMemorialDay(_graphClient);
            Status = "正在将远程的日记同步到本地";
            (diaries, diaryEntris) = await _syncServiceTaskRunner.SyncDiary(_graphClient);
            Status = "正在将数据同步到远程";
            var saveResult =
                await _syncServiceTaskRunner.SaveMemorialDaysAsync(memorialDays,
                    diaryEntris, diaries, _graphClient, Server);
            if (saveResult.Status != ServiceResultStatus.Ok)
            {
                return saveResult;
            }

            await _syncServiceTaskRunner.SyncImage(_graphClient);
            var saveResult2 = await _syncServiceTaskRunner.SaveImageAsync(_graphClient, Server);
            if (saveResult2.Status != ServiceResultStatus.Ok)
            {
                return saveResult2;
            }
            return new ServiceResult { Status = ServiceResultStatus.Ok };
        }

        // ******** 公开方法
        /// <summary>
        /// 同步服务
        /// </summary>
        /// <param name="remoteMemorialDayStorage"></param>
        /// <param name="localMemorialDayStorage"></param>
        public SyncService(SyncServiceTaskRunner scyncServiceTaskRunner)
        {
            _syncServiceTaskRunner = scyncServiceTaskRunner;

            var builder = PublicClientApplicationBuilder
                .Create(OneDriveOAuthSettings.ApplicationId)
                .WithRedirectUri(OneDriveOAuthSettings.RedirectUri);

            if (!string.IsNullOrEmpty(App.iOSKeychainSecurityGroup))
            {
                builder =
                    builder.WithIosKeychainSecurityGroup(
                        App.iOSKeychainSecurityGroup);
            }

            _pca = builder.Build();

            _graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    var accounts = await _pca.GetAccountsAsync();

                    var result = await _pca
                        .AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer",
                            result.AccessToken);
                }));
        }
    }
}
