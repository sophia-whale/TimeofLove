using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TabbedTemplate.Models;
using TabbedTemplate.Services;
namespace TabbedTemplate.ViewModels
{
    public class MemorialDetailViewModel : ViewModelBase
    {
        // ********** 绑定属性
        public MemorialDay MemorialDay
        {
            get => _memorialDay;
            set => Set(nameof(MemorialDay), ref _memorialDay, value);
        }

        // ******** 私有变量
        private IMemorialDayStorage _memorialDayStorage;

        /// <summary>
        /// 纪念日
        /// </summary>
        private MemorialDay _memorialDay;

        private IContentNavigationService _contentNavigationService;

        public MemorialDetailViewModel(IMemorialDayStorage memorialDayStorage, IContentNavigationService contentNavigationService)
        {
            _memorialDayStorage = memorialDayStorage;
            _contentNavigationService = contentNavigationService;
        }

        // ******** 公开方法

        /// <summary>
        /// 编辑纪念日。
        /// </summary>
        public RelayCommand EditMemorialDayCommand =>
            new RelayCommand(async () =>
                await EditMemorialDayFunction());

        public async Task EditMemorialDayFunction()
        {
            await _contentNavigationService.NavigateBack(ContentNavigationConstant
                .MemorialDetailPage);
            await _memorialDayStorage.SaveMemorialDayAsync(MemorialDay, 1, true);
        }

        public RelayCommand DeleteMemorialDayCommand =>
            new RelayCommand(async () => await DeleteMemorialDayFunction());

        public async Task DeleteMemorialDayFunction()
        {
            MemorialDay.IsDeleted = 1;
            await _contentNavigationService.NavigateBack(ContentNavigationConstant
                .MemorialDetailPage);
            await _memorialDayStorage.SaveMemorialDayAsync(MemorialDay, 1, true);
        }
    }
}
