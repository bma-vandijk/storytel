namespace Storytime
{
    public interface IPlatformTools
    {
        void DisplayToast(string text, bool longDuration);

        string getAppFilePath();
    }
}
