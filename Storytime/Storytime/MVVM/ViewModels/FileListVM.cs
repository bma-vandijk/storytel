using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Storytime.AssetModels;
using Xamarin.Forms;

namespace Storytime.MVVM
{
    public class FileListVM : BaseVM
    {
        private FileListView content;
        private bool running = false;

        public FileListVM()
        {
            Refresh = new Command(() => ListFiles());
            Pause = new Command(() => {
                DependencyService.Get<IMediaHelper>().Pause();
                if (DependencyService.Get<IMediaHelper>().IsPlaying())
                    PImage = "pause.png";
                else
                    PImage = "play.png";
            });
            Stop = new Command(() => {
                running = false;
                DependencyService.Get<IMediaHelper>().Stop();
                content.hidePopupPlayer();
                PImage = "pause.png";
            });

            ListFiles();
        }

        private string _progress = "0:00";

        public string Progress
        {
            get => _progress;
            set => setProperty(ref _progress, value);
        }

        private double _sliderMax = 1;

        public double SliderMax {
            get => _sliderMax;
            set => setProperty(ref _sliderMax, value);
        }

        private double _sliderVal = 0;
        
        public double SliderVal
        {
            get => _sliderVal;
            set => setProperty(ref _sliderVal, value);
        }

        private string _pImage = "pause.png";

        public string PImage
        {
            get => _pImage;
            set => setProperty(ref _pImage, value);
        }

        public void setContentPage(FileListView cp)
        {
            content = cp;
        }

        private FileCell[] _files;

        public FileCell[] Files
        {
            get => _files;
            set => setProperty(ref _files, value);
        }

        public ICommand Refresh { get; set; }

        private void ListFiles()
        {
            FileListView mp = content as FileListView;

            if (mp != null)
            {
                mp.GetFileList().IsRefreshing = true;
            }
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(UpdateFileList);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkFinished);
            bw.RunWorkerAsync();
        }

        internal void PlayTask()
        {
            new Thread(() => PlayTickLoop()).Start();
        }

        private void UpdateFileList(object sender, DoWorkEventArgs e)
        {
            List<FileCell> fileList = new List<FileCell>();
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    string dir = DependencyService.Get<IPlatformTools>().getAppFilePath();
                    Console.WriteLine("{0}", dir);
                    try
                    {
                        foreach (string d in System.IO.Directory.GetFiles(dir))
                        {
                            FileCell fc = new FileCell();
                            fc.title = System.IO.File.GetCreationTime(d).ToString("yyyy-MM-dd HH:mm:ss");
                            fc.file = d;
                            fileList.Add(fc);
                        }
                        fileList.Sort((x, y) => y.title.CompareTo(x.title));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}", ex.Message);
                    }
                    break;
                default:
                    break;
            }
            Files = fileList.ToArray();
        }

        private void OnWorkFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            FileListView mp = content as FileListView;
            if (mp != null)
            {
                mp.GetFileList().IsRefreshing = false;
            }
        } 

        private void PlayTickLoop()
        {
            IMediaHelper mp = DependencyService.Get<IMediaHelper>();
            SliderMax = mp.GetFileDuration();
            running = true;
            while (mp.IsOpen() && running)
            {
                TimeSpan t = TimeSpan.FromMilliseconds(mp.GetPlayTime());
                Progress = string.Format("{0}:{1:D2}", (int)t.TotalMinutes, t.Seconds);
                SliderVal = mp.GetPlayTime();
                Thread.Sleep(100); //poll 20 times per second
            }
        }   

        public ICommand Pause { get; set; }

        public ICommand Stop { get; set; }
    }
}
