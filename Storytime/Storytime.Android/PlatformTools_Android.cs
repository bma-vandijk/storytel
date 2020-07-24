using Storytime.Droid;
using Xamarin.Forms;
using Android.Widget;

[assembly: Dependency(typeof(PlatformTools_Android))]
namespace Storytime.Droid
{
    class PlatformTools_Android : IPlatformTools
    {
        public void DisplayToast(string text, bool longDuration)
        {
            ToastLength tl = longDuration ? ToastLength.Long : ToastLength.Short;
            Toast.MakeText(Android.App.Application.Context, text, tl).Show();
        }

        public string getAppFilePath()
        {
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/StoryTime";
        }
    }
}