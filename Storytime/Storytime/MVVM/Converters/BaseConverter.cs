using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Storytime.MVVM
{
    /// <summary>
    /// <see langword="abstract"/> template for converters, makes XAML declaration easier
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseConverter<T> : IMarkupExtension, IValueConverter where T : class, new()
    {
        /// <summary>
        /// The value to return for the implementation of <see cref="MarkupExtension"/>
        /// </summary>
        private T _instance;

#pragma warning disable RCS1168, RCS1141

        /// <summary>
        /// Converts source to target
        /// </summary>
        public abstract object Convert(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture);

        /// <summary>
        /// Converts target to source
        /// </summary>
        public abstract object ConvertBack(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture);

        /// <summary>
        /// Provides the converter through <see cref="MarkupExtension"/>
        /// </summary>
        public object ProvideValue(IServiceProvider pServiceProvider)
        {
            return _instance ?? (_instance = new T());
        }

#pragma warning restore RCS1141, RCS1168
    }
}