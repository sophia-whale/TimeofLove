using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.Design.Widget;
using TabbedTemplate.Droid;
using Xamarin.Forms;
using Xamarin.Android;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyTabbedPageRenderer))]
namespace TabbedTemplate.Droid
{
    class MyTabbedPageRenderer : TabbedPageRenderer
    {
        private Context _context;
        public MyTabbedPageRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null && e.NewElement != null)
            {
                for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
                {
                    var childView = this.ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {
                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            var childRelativeLayoutView = viewGroup.GetChildAt(j);
                            if (childRelativeLayoutView is BottomNavigationView)
                            {
                                ((BottomNavigationView)childRelativeLayoutView).ItemTextAppearanceActive = Resource.Style.MyBottomNavigationView;
                                ((BottomNavigationView)childRelativeLayoutView).ItemTextAppearanceInactive = Resource.Style.MyBottomNavigationView;
                            }
                        }
                    }
                }
            }
        }
    }
}