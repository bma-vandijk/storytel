using Storytime.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Storytime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentView
	{
		public LoginView()
		{
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, ItemTappedEventArgs e)
        {
            if(doLogin())
                MainPageVM.MPVM.Navigate(ApplicationView.FileList);
        }

        private bool doLogin()
        {
            return true; //dummy
        }
    }
}