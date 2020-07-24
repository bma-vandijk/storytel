using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Storytime.MVVM
{
    public class RecorderVM : BaseVM
    {
        private DateTime startTime;
        Random rnd;

        public RecorderVM()
        {
            Record = new Command(() => runRecord());
            DependencyService.Get<IMediaHelper>().RegisterRecorderVM(this);
            rnd = new Random();
        }

        private string _recordText = "Klik op de knop en begin je verhaal!";

        public string RecordText
        {
            get => _recordText;
            set => setProperty(ref _recordText, value);
        }

        private string _micIcon = "mic_off.png";

        public string MicIcon
        {
            get => _micIcon;
            set => setProperty(ref _micIcon, value);
        }

        private string _character = "idle.gif";

        public string Character
        {
            get => _character;
            set => setProperty(ref _character, value);
        }

        private int _animationtracker = 0;

        public int Animationtracker
        {
            get => _animationtracker;
            set => setProperty(ref _animationtracker, value);
        }

        public ICommand Record { get; set; }

        private void runRecord()
        {
            try
            {
                IPlatformTools pt = DependencyService.Get<IPlatformTools>();
                IMediaHelper mh = DependencyService.Get<IMediaHelper>();
                if (!mh.isRecording())
                {
                    RecordText = "Ik luister";
                    MicIcon = "mic_on.png";
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            string dir = pt.getAppFilePath();
                            if (!System.IO.Directory.Exists(dir))
                            {
                                System.IO.Directory.CreateDirectory(dir);
                            }
                            string pathSave = dir + '/' + Guid.NewGuid().ToString() + "_audio.m4a";
                            Console.WriteLine("Directory: {0}", pathSave);
                            pt.DisplayToast("Opname Begonnen!", false);
                            startTime = DateTime.Now;

                            mh.SetupRecorder(pathSave);
                            //setupMediaRecorder();

                            Animationtracker = -2;
                            Character = "speaking.gif";
                            int i = rnd.Next(2);
                            if (i == 0)
                                DependencyService.Get<IMediaHelper>().PlaySound("ikluister.ogg");
                            else
                                DependencyService.Get<IMediaHelper>().PlaySound("vertelmaar.ogg");
                            mh.Record();
                            //recorder.Prepare();
                            //recorder.Start();
                            //new AsyncTaskTrack(this).Execute();

                            //new Thread(onRecordTick).Start();
                            break;
                        case Device.iOS:
                            throw new NotImplementedException();
                        default:
                            break;
                    }
                }
                else
                {
                    Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }
        }

        public void Stop()
        {
            RecordText = "Bedankt voor het verhaal!";
            MicIcon = "mic_off.png";
            Character = "idle.gif";
            DependencyService.Get<IPlatformTools>().DisplayToast("Opname voltooid!", false);
            DependencyService.Get<IMediaHelper>().StopRecording();
        }

        public void OnSilence(int level)
        {
            switch(level)
            {
                case 0:
                    int i = rnd.Next(2);

                    Animationtracker = -2;
                    Character = "speaking.gif";
                    
                    
                    
                    if (i == 0)
                    {
                       
                        DependencyService.Get<IMediaHelper>().PlaySound("ja.ogg");
                    }
                    else
                        DependencyService.Get<IMediaHelper>().PlaySound("juist.ogg");
                    break;
                case 1:
                    Animationtracker = -2;
                    Character = "speaking.gif";
                    
                                 
                    DependencyService.Get<IMediaHelper>().PlaySound("uhum.ogg");
                    break;
                case 2:
                    Stop();
                    break;
                default:
                    break;
            }
        }

        public void OnLevelChange(int newLevel)
        {
            if(newLevel == 0)
            {

            }
        }
    }
}
