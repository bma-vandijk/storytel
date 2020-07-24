using Storytime.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileActions_iOS))]
namespace Storytime.iOS
{
    public class FileActions_iOS : IFileActions
    {
        public bool Delete(string file)
        {
            throw new System.NotImplementedException();
        }
    }
}