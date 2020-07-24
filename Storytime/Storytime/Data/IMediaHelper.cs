using System.Threading.Tasks;
using Storytime.MVVM;

namespace Storytime
{
    public interface IMediaHelper
    {
        void Play(string file);
        void PlaySound(string fileInAssets);
        void Pause();
        void Stop();
        int GetPlayTime();
        int GetFileDuration();
        bool IsPlaying();
        bool IsOpen();

        void SetupRecorder(string file);
        void Record();
        bool isRecording();
        void StopRecording();

        void RegisterRecorderVM(RecorderVM rvm);
        RecorderVM GetRecorderVM();
    }
}
