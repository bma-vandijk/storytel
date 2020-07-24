using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Storytime.MVVM
{
    public class ApplicationViewConverter : BaseConverter<ApplicationViewConverter>
    {
        /// <summary>
        /// Convert a given <see cref="ApplicationView"/> to an actual view
        /// </summary>
        public override object Convert(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            if (pValue is ApplicationView view)
            {
                switch (view)
                {
                    case ApplicationView.Home: return new HomeView();
                    case ApplicationView.Record: return new RecorderView();
                    case ApplicationView.Start: return new StartView();
                    case ApplicationView.FileList: return new FileListView();
                    case ApplicationView.Login: return new LoginView();
                }
            }
            throw new ArgumentException($"{nameof(pValue)} was not expected type of {pValue.GetType()}");
        }

        /// <summary>
        /// Unused
        /// </summary>
        public override object ConvertBack(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            throw new InvalidOperationException($"{nameof(ConvertBack)} is not supported by {nameof(ApplicationViewConverter)}");
        }
    }
}
