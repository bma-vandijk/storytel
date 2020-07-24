using System.Diagnostics;
using Storytime.AssetModels;
using Storytime.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Storytime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListView : ContentView
    {
        public FileListView()
        {
            InitializeComponent();
            FileList.ItemTemplate = new DataTemplate(typeof(FileCell));
            ((FileListVM)BindingContext).setContentPage(this);
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            FileCell fc = e.Item as FileCell;
            if (fc != null)
            {
                var action = await App.Current.MainPage.DisplayActionSheet("Wat wil je doen met opname " + fc.subtitle, "Annuleren", null, "Afspelen", "Versturen", "Verwijderen");
                Debug.WriteLine("Response: " + action);
                if (action != null && action != "" && action != "Annuleren")
                {
                    var action2 = await App.Current.MainPage.DisplayActionSheet("Weet je zeker dat je deze file wilt " + action, "Nee", "Ja");
                    Debug.WriteLine("Response: " + action2);
                    if (action2 == "Ja")
                    {
                        string file = fc.file;
                        switch (action)
                        {
                            case "Afspelen":
                                DependencyService.Get<IMediaHelper>().Play(file);
                                popupPlayerView.IsVisible = true;
                                ((FileListVM)BindingContext).PlayTask();
                                break;
                            case "Vesturen":
                                await App.CurrentApp.connection.Upload(11, fc.file);
                                break;
                            case "Verwijderen":
                                DependencyService.Get<IFileActions>().Delete(file);
                                var viewModel = (FileListVM)BindingContext;
                                if (viewModel.Refresh.CanExecute(null))
                                    viewModel.Refresh.Execute(null);
                                break;
                            case "Annuleren":
                                break;
                            default:
                                break;
                        }
                    }
                }
                //await App.CurrentApp.connection.Upload(11, fc.file);
            }
        }

        public ListView GetFileList()
        {
            return FileList;
        }

        public void hidePopupPlayer()
        {
            popupPlayerView.IsVisible = false;
        }
    }
}