using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TabbedTemplate.ViewModels
{
    /// <summary>
    /// 轮播图片ViewModel
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        /******** 构造函数 ********/

        /// <summary>
        ///添加页面ViewModel。
        /// </summary>
        private DiaryDetailViewModel diaryDetailViewModel;

        /// <summary>
        /// 搜索条件ViewModel。
        /// </summary>
        /// <param name="queryPageViewModel">ViewModel。</param>
        public ImageViewModel(DiaryDetailViewModel diaryDetailViewModel)
        {
            this.diaryDetailViewModel = diaryDetailViewModel;

        }

        // ******** 绑定属性

        public byte[] ImageSource
        {
            get => _imageSource;
            set => Set(nameof(ImageSource), ref _imageSource, value);
        }
        private byte[] _imageSource;


        /******** 绑定命令 ********/

        /// <summary>
        /// 添加命令。
        /// </summary>
        public RelayCommand AddCommand =>
            _addCommand ?? (_addCommand =
                new RelayCommand(async () => await AddCommandFunction()));

        /// <summary>
        /// 添加命令。
        /// </summary>
        private RelayCommand _addCommand;

        public async Task AddCommandFunction() =>
            await diaryDetailViewModel.AddImageViewModel(this);

        /// <summary>
        /// 删除命令。
        /// </summary>
        public RelayCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand =
                new RelayCommand(() => RemoveCommandFunction()));

        /// <summary>
        /// 删除命令。
        /// </summary>
        private RelayCommand _removeCommand;

        public void RemoveCommandFunction() =>
            diaryDetailViewModel.RemoveImageViewModel(this);
    }
}
