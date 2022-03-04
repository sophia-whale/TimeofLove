using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace TabbedTemplate.Droid {
    [Activity(MainLauncher = true, NoHistory = true, Label = "恋爱时光", Icon = "@drawable/icon", Theme = "@style/Theme.Splash",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}