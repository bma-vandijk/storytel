using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Storytime.MVVM
{
    public class MainPageVM : BaseVM
    {
        public static MainPageVM MPVM;
        private Stack<ApplicationView> _viewStack;

        public MainPageVM()
        {
            MPVM = this;
            _viewStack = new Stack<ApplicationView>();
            SwapCommand = new Command(() =>
            {
                if (ActiveView == ApplicationView.Home)
                    ActiveView = ApplicationView.Record;
                else if (ActiveView == ApplicationView.Record)
                    ActiveView = ApplicationView.Start;
                else if (ActiveView == ApplicationView.Start)
                    ActiveView = ApplicationView.FileList;
                else if (ActiveView == ApplicationView.FileList)
                    ActiveView = ApplicationView.Login;
                else
                    ActiveView = ApplicationView.Home;
            });
            
        }

        public ICommand SwapCommand { get; set; }

        private ApplicationView _activeView = ApplicationView.Home;

        public ApplicationView ActiveView
        {
            get => _activeView;
            set => setProperty(ref _activeView, value);
        }

        public void PopView()
        {
            if (_viewStack.Count != 0)
            {
                ApplicationView v = _viewStack.Pop();
                ActiveView = v;
            }
        }

        public void Navigate(ApplicationView v)
        {
            _viewStack.Push(_activeView);
            ActiveView = v;
        }

        
    }
}
