using Storytime.Data;
using Storytime.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Storytime
{
    public partial class App : Application
    {
        private static App _current;
        internal ConnectionInterface connection { get => _con; }
        public static App CurrentApp { get => _current; set => _current = value; }

        private ConnectionInterface _con;

        public App()
        {
            InitializeComponent();
            DependencyService.Register<IFileActions>();
            DependencyService.Register<IMediaHelper>();
            DependencyService.Register<IPlatformTools>();
            _con = new ConnectionInterface();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Current = this;
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
