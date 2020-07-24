using System;
using System.Collections.Generic;
using System.Text;
using Storytime.MVVM;
using Xamarin.Forms;

namespace Storytime.AssetModels
{
    public class FileCell : ViewCell
    {
        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
            }
        }

        private string _subtitle;
        private string _file;
        public string file
        {
            get => _file;
            set
            {
                _subtitle = value.Substring(value.LastIndexOf('/') + 1);
                _file = value;
            }
        }

        public string subtitle
        {
            get => _subtitle;
            set => _subtitle = value;
        }

        public FileCell()
        {
            StackLayout cellWrapper = new StackLayout();
            StackLayout verticalLayout = new StackLayout();
            Label top = new Label();
            Label bottom = new Label();

            top.SetBinding(Label.TextProperty, "title");
            bottom.SetBinding(Label.TextProperty, "subtitle");

            cellWrapper.BackgroundColor = Color.FromHex("#eee");
            verticalLayout.Orientation = StackOrientation.Vertical;
            top.TextColor = Color.FromHex("#f35e20");
            bottom.TextColor = Color.FromHex("503026");

            verticalLayout.Children.Add(top);
            verticalLayout.Children.Add(bottom);
            cellWrapper.Children.Add(verticalLayout);
            View = cellWrapper;
        }
    }
}
