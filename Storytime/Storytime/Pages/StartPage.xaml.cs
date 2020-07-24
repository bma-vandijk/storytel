using Storytime.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Storytime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartView : ContentView
    {
        public StartView()
        {
            InitializeComponent();
        }

        private void OnClicked(object sender, ItemTappedEventArgs e)
        {
            MainPageVM.MPVM.Navigate(ApplicationView.Record);
            return;
        }

        private void OnParentClicked(object sender, ItemTappedEventArgs e)
        {
            MainPageVM.MPVM.Navigate(ApplicationView.Login);
        }
    }
}