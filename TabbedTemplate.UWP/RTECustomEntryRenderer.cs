using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace TabbedTemplate.UWP
{
    public class RTECustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            }
        }
    }
}