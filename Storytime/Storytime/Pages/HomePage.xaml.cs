using Storytime.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Storytime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, ItemTappedEventArgs e)
        {
            MainPageVM.MPVM.ActiveView = ApplicationView.Start;
        }
    }
}