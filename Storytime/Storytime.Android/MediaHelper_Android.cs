using Storytime.Droid;
using Xamarin.Forms;
using Android.Media;
using Android.OS;
using System.Threading;
using System;
using Storytime.MVVM;
using Android.Content.Res;
using System.Threading.Tasks;

[assembly: Dependency(typeof(MediaHelper_Android))]
namespace Storytime.Droid
{
    public class MediaHelper_Android : IMediaHelper
    {
        private MediaPlayer mp;
        private MediaRecorder recorder;
        private bool _canResetLevel = true;
        private bool _recording = false;
        protected RecorderVM _rvm;

        


        public void Play(string file)
        {
            if(mp != null && mp.IsPlaying)
            {
                mp.Stop();
                mp.Release();
            }
            mp = MediaPlayer.Create(Android.App.Application.Context, Android.Net.Uri.Parse(file));
            mp.Start();
        }

        public void PlaySound(string fileInAssets)
        {
            if (mp != null && mp.IsPlaying)
            {
                mp.Stop();
                mp.Release();
            }
            AssetFileDescriptor descriptor = Android.App.Application.Context.Assets.OpenFd(fileInAssets);
            mp = new MediaPlayer();
            mp.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
            _canResetLevel = false;
            mp.Prepare();
            mp.Start();
            Task.Run(() => FinishWaiter());
        }

        private void FinishWaiter()
        {
            while (mp != null && mp.IsPlaying) { }
            _canResetLevel = true;
        }

        public void Record()
        {
            if (recorder != null)
            {
                _recording = true;
                recorder.Prepare();
                recorder.Start();
                new AsyncTaskTrack(this).Execute();
            }
        }

        public void SetupRecorder(string file)
        {
            recorder = new MediaRecorder();
            recorder.SetAudioSource(AudioSource.Mic);
            recorder.SetOutputFormat(OutputFormat.Mpeg4);
            recorder.SetAudioEncoder(AudioEncoder.Aac);
            recorder.SetAudioSamplingRate(48000); // 48 kHz
            recorder.SetAudioEncodingBitRate(128000); // 128 kbps
            recorder.SetOutputFile(file);
        }

        public void StopRecording()
        {
            recorder.Stop();
            recorder.Release();
            recorder = null;
            _recording = false;
        }

        public bool isRecording()
        {
            return _recording;
        }

        public void Pause()
        {
            if (mp != null)
            {
                if (mp.IsPlaying)
                    mp.Pause();
                else
                    mp.Start();
            }
        }

        public void Stop()
        {
            if (mp != null)
                mp.Release();
            mp = null;
        }

        public int GetPlayTime()
        {
            int pos = 0;
            try {
                pos = mp.CurrentPosition;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                Console.WriteLine("[Storytime/DEBUG] Minor (handled) playback error");
            }
            return pos;
        }

        public int GetFileDuration()
        {
            if (mp != null)
                return mp.Duration;
            return 1;
        }

        public bool IsPlaying()
        {
            if (mp != null)
                return mp.IsPlaying;
            return false;
        }

        public bool IsOpen()
        {
            return mp != null;
        }

        public void RegisterRecorderVM(RecorderVM rvm)
        {
            _rvm = rvm;
        }

        public RecorderVM GetRecorderVM()
        {
            return _rvm;
        }

        internal class SilenceTrackerHelper
        {
            public int level;

            public SilenceTrackerHelper()
            {
                level = 0;
            }

            public int getNotifyTime()
            {
                if (level == 0)
                    return 10;

                if (level == 1)
                    return 20;

                else
                    return 30;
            }

        }

        // Task that runs in the background while recording
        internal class AsyncTaskTrack : AsyncTask<Java.Lang.Void, int, Java.Lang.Void>
        {
            private MediaHelper_Android _p;
            private SilenceTrackerHelper _sth;

            public AsyncTaskTrack(MediaHelper_Android parent)
            {
                _p = parent;
                _sth = new SilenceTrackerHelper();
            }

            // Perform background task
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] native_parms)
            {
                int i;
                int tracker = 0;
                Random rnd;
                rnd = new Random();
                int random = rnd.Next(100);
                while (_p._recording)
                {
                    i = _p.recorder.MaxAmplitude; // Highest amp since the last time this function was called

                    _p.GetRecorderVM().RecordText = "geluidsniveau: " + i.ToString();

                    if (i < 10000)
                    {
                        tracker++;
                    }

                    _p.GetRecorderVM().Animationtracker++;

                    if(_p.GetRecorderVM().Animationtracker >= 3)
                    {
                        random = rnd.Next(100);
                        if (random > 50)
                        {
                            _p.GetRecorderVM().Character = "blinking.gif";
                        }
                        else
                        {
                            _p.GetRecorderVM().Character = "nodding.gif";
                        }

                        _p.GetRecorderVM().Animationtracker = 0;

                    }


                    else if(_p._canResetLevel)
                    {
                        tracker = 0;
                        _sth.level = 0;
                    }
                    if (tracker >= _sth.getNotifyTime())
                    {
                        PublishProgress(_sth.level);
                        _sth.level++;
                        tracker = 0;
                    }
                    Thread.Sleep(1000); //poll 1 times per second
                }
                return base.DoInBackground(native_parms);
            }

            // Provides updates on main thread (Some things such as toasts can only be sent through the main UI thread)
            protected override void OnProgressUpdate(params int[] values)
            {
                _p.GetRecorderVM().OnSilence(values[0]);
                base.OnProgressUpdate(values);
            }

            protected override Java.Lang.Void RunInBackground(params Java.Lang.Void[] @params)
            {
                return null;
            }
        }
    }
}