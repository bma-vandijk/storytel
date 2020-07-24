using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/*  
 *  author: Severin Fitriyadi (1417398)
 */

namespace Storytime.MVVM
{
    /// <summary>
    /// Base class for viewmodels, implements <see cref="INotifyPropertyChanged"/>
    /// and provides the <see cref="setProperty{T}(ref T, T, string)"/> to reduce boilerplate.
    /// </summary>
    public abstract class BaseVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that gets hooked into by bindings, should not be used manually.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event with the given name, or automatically grabs the caller name.
        /// </summary>
        /// <param name="pPropertyName">Name of the property</param>
        protected void onPropertyChanged([CallerMemberName] string pPropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pPropertyName));
        }

        /// <summary>
        /// Sets a property to the given value, if they are not equal in value.
        /// If the property was set, <see cref="PropertyChanged"/> is invoked for that property.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="pProperty">Reference to the property</param>
        /// <param name="pValue">New value for the property</param>
        /// <param name="pPropertyName">Name of the property</param>
        /// <returns><see langword="true"/> if the property was set, <see langword="false"/> otherwise</returns>
        protected bool setProperty<T>(ref T pProperty, T pValue, [CallerMemberName] string pPropertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(pProperty, pValue))
                return false;

            pProperty = pValue;
            onPropertyChanged(pPropertyName);
            return true;
        }
    }
}
