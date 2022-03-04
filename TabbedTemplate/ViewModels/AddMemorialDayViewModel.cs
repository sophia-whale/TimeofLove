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
    public class AddMemorialDayViewModel : ViewModelBase
    {
        // ********** 绑定属性
        public MemorialDay MemorialDay
        {
            get => _memorialDay;
            set => Set(nameof(MemorialDay), ref _memorialDay, value);
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set => Set(nameof(DateTime), ref _dateTime, value);
        }

        public string Title
        {
            get => _title;
            set => Set(nameof(Title), ref _title, value);
        }

        // ******** 私有变量
        private IMemorialDayStorage _memorialDayStorage;

        /// <summary>
        /// 纪念日
        /// </summary>
        private MemorialDay _memorialDay;

        private DateTime _dateTime;

        private string _title;
        private IContentNavigationService _contentNavigationService;

        public AddMemorialDayViewModel(IMemorialDayStorage memorialDayStorage, IContentNavigationService contentNavigationService)
        {
            _memorialDayStorage = memorialDayStorage;
            _contentNavigationService = contentNavigationService;
        }

        // ******** 公开方法
        /// <summary>
        /// 添加纪念日。
        /// </summary>
        private RelayCommand _addMemorialDayCommand;

        /// <summary>
        /// 添加纪念日。
        /// </summary>
        public RelayCommand AddMemorialDayCommand =>
            _addMemorialDayCommand ?? (_addMemorialDayCommand =
                new RelayCommand(async () => await AddMemorialDayFunction()));

        public async Task AddMemorialDayFunction()
        {

            MemorialDay memorialDay = new MemorialDay
            {
                Title = Title,
                StartDate = DateTime.ToString("MM/dd/yyyy"),
                CreateDate = DateTime.Now.ToString("MM/dd/yyyy"),
            };
            await _contentNavigationService.NavigateBack(
                ContentNavigationConstant.AddMemorialDayPage);
            await _memorialDayStorage.SaveMemorialDayAsync(memorialDay, 0, true);
        }
    }
}
