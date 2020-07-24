using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Storytime.MVVM;
using System.Globalization;

namespace Storytime
{
    /// <summary>
    /// The view container that swaps between views
    /// </summary>
	public partial class ViewHost : ContentView
    {
        /// <summary>
        /// Identifies the <see cref="ActiveView"/> bindable property
        /// </summary>
        public static BindableProperty ActiveViewProperty = BindableProperty.Create(nameof(ActiveView), typeof(ApplicationView), typeof(ViewHost), defaultValue: ApplicationView.Home, propertyChanged: OnActiveViewChanged);

        /// <summary>
        /// Static callback that routes the callback to the actual instance
        /// </summary>
        /// <param name="pSender">The viewhost whose property changed</param>
        /// <param name="pOldValue">The original value before it changed</param>
        /// <param name="pNewValue">The new value after it changed</param>
        public static async void OnActiveViewChanged(BindableObject pSender, object pOldValue, object pNewValue)
        {
            if (pSender is ViewHost host && pOldValue is ApplicationView oldval && pNewValue is ApplicationView newval)
                await host.OnActiveViewChanged(oldval, newval).ConfigureAwait(false);
        }

        /// <summary>
        /// Callback for when the view changes
        /// </summary>
        /// <param name="pOldView">The old view</param>
        /// <param name="pNewView">The new view</param>
        public async Task OnActiveViewChanged(ApplicationView pOldView, ApplicationView pNewView)
        {
            // Don't do anything if the view didn't actually change
            if (pOldView == pNewView)
                return;

            // Prepare the fades
            OldView.Content = new ApplicationViewConverter().Convert(pOldView, typeof(View), null, CultureInfo.CurrentCulture) as View;
            NewView.Content = new ApplicationViewConverter().Convert(pNewView, typeof(View), null, CultureInfo.CurrentCulture) as View;

            OldView.Opacity = 1;
            NewView.Opacity = 0;

            // Wait for the fades to finish
            await Task.WhenAll(OldView.FadeTo(0, 100), NewView.FadeTo(1, 100)).ConfigureAwait(true);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
		public ViewHost()
        {
            InitializeComponent();

            ActiveView = ApplicationView.Home;
            NewView.Content = new ApplicationViewConverter().Convert(ActiveView, typeof(View), null, CultureInfo.CurrentCulture) as View;
            OldView.Opacity = 1;
        }

        /// <summary>
        /// The currently active view in the <see cref="ViewHost"/>
        /// </summary>
        public ApplicationView ActiveView
        {
            get => (ApplicationView)GetValue(ActiveViewProperty);
            set => SetValue(ActiveViewProperty, value);
        }

    }

    /// <summary>
    /// The different views or screen that the app has available
    /// </summary>
    public enum ApplicationView
    {
        /// <summary>
        /// Undefined screen, a fallback value
        /// </summary>
        Undefined,

        /// <summary>
        /// The loading splashscreen
        /// </summary>
        Splash,

        /// <summary>
        /// The homescreen
        /// </summary>
        Home,

        /// <summary>
        /// The recording screen
        /// </summary>
        Record,

        /// <summary>
        /// The writing screen
        /// </summary>
        Write,

        /// <summary>
        /// The reading screen
        /// </summary>
        Read,

        /// <summary>
        /// The audio file browsing screen
        /// </summary>
        Browse,

        Start,
        FileList,
        Login,
    }
}