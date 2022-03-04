using DailyPoetryX.Services;
using GalaSoft.MvvmLight.Ioc;
using TabbedTemplate.Renders;
using TabbedTemplate.Services;

namespace TabbedTemplate.ViewModels
{
    public class ViewModelLocator
    {

        public DiariesViewModel DiariesViewModel =>
            SimpleIoc.Default.GetInstance<DiariesViewModel>();

        public DiaryDetailViewModel DiaryDetailViewModel =>
            SimpleIoc.Default.GetInstance<DiaryDetailViewModel>();

        public MemorialDaysViewModel MemorialDaysViewModel =>
            SimpleIoc.Default.GetInstance<MemorialDaysViewModel>();

        public MemorialDetailViewModel MemorialDetailViewModel =>
            SimpleIoc.Default.GetInstance<MemorialDetailViewModel>();

        public AddMemorialDayViewModel AddMemorialDayViewModel =>
            SimpleIoc.Default.GetInstance<AddMemorialDayViewModel>();

        public DiaryEditViewModel DiaryEditViewModel =>
            SimpleIoc.Default.GetInstance<DiaryEditViewModel>();

        public SyncPageViewModel SyncPageViewModel =>
            SimpleIoc.Default.GetInstance<SyncPageViewModel>();

        public DiaryShareViewModel DiaryShareViewModel =>
            SimpleIoc.Default.GetInstance<DiaryShareViewModel>();

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DiariesViewModel>();
            SimpleIoc.Default.Register<DiaryDetailViewModel>();
            SimpleIoc.Default.Register<MemorialDaysViewModel>();
            SimpleIoc.Default.Register<MemorialDetailViewModel>();
            SimpleIoc.Default.Register<AddMemorialDayViewModel>();
            SimpleIoc.Default.Register<DiaryEditViewModel>();
            SimpleIoc.Default.Register<SyncPageViewModel>();
            SimpleIoc.Default.Register<DiaryShareViewModel>();
            SimpleIoc.Default.Register<IMemorialDayStorage, MemorialDayStorage>();
            SimpleIoc.Default.Register<IDiaryStorage, DiaryStorage>();
            SimpleIoc.Default.Register<IPreferenceStorage, PreferenceStorage>();
            SimpleIoc.Default.Register<IContentPageActivationService, ContentPageActivationService>();
            SimpleIoc.Default.Register<IContentNavigationService, ContentNavigationService>();
            SimpleIoc.Default.Register<IHoneyWordsService, HoneyWordsService>();
            SimpleIoc.Default.Register<ISyncServiceTaskRunner, SyncServiceTaskRunner>();
            SimpleIoc.Default.Register<IAlertService, AlertService>();
            SimpleIoc.Default.Register<ISyncService, SyncService>();
            SimpleIoc.Default.Register<IRender, ShareCanvasViewRender>();
            SimpleIoc.Default.Register<IRemoteMemorialDayStorage, OneDriveMemorialDayStorage>();
            SimpleIoc.Default.Register<IRemoteDiaryStorage, OneDriveDiaryStorage>();
            SimpleIoc.Default.Register<IRemoteImageStorage,OneDriveImageStorage>();
            SimpleIoc.Default.Register<IAddImageInDiaryService,AddImageInDiaryService>();
            SimpleIoc.Default.Register<IPhotoStorage,PhotoStorage>();

        }
    }
}
