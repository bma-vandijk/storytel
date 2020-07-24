using Android.App;
using Android.Content.PM;
using Android.OS;
using Android;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Storytime.MVVM;

namespace Storytime.Droid
{
    [Activity(Label = "@string/AppName", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {

        View layout;

        static readonly int REQUEST_APP = 0;
        static string[] PERMISSIONS_APP = {
            Manifest.Permission.ModifyAudioSettings,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Internet,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.MediaContentControl,
            Manifest.Permission.WakeLock
        };

        void getPermissions()
        {
            if (CheckSelfPermission(Manifest.Permission.ModifyAudioSettings) != (int)Permission.Granted || CheckSelfPermission(Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted ||
                CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted || CheckSelfPermission(Manifest.Permission.RecordAudio) != (int)Permission.Granted)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ModifyAudioSettings) || ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadExternalStorage) ||
                    ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage) || ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.RecordAudio))
                    Snackbar.Make(layout, "need.modify.audio", Snackbar.LengthIndefinite).SetAction("OK", v => ActivityCompat.RequestPermissions(this, PERMISSIONS_APP, REQUEST_APP)).Show();
                else
                {
                    ActivityCompat.RequestPermissions(this, PERMISSIONS_APP, REQUEST_APP);
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            layout = FindViewById(Resource.Id.sample_main_layout);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                getPermissions();
            }


            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }


        public override void OnBackPressed() {
            if (MainPageVM.MPVM.ActiveView == ApplicationView.Home) {
                base.OnBackPressed();
                return;
            }
            MainPageVM.MPVM.PopView();
        }
    }
}