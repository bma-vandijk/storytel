using Android.Widget;
using Java.IO;
using Storytime.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileActions_Android))]
namespace Storytime.Droid
{
    public class FileActions_Android : IFileActions
    {
        public bool Delete(string path)
        {
            File file = new File(path);
            if (file.Delete())
            {
                Toast.MakeText(Android.App.Application.Context, "Deleted", ToastLength.Short).Show();
                return true;
            }
            else
            {
                Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Short).Show();
                return false;
            }

        }
    }
}