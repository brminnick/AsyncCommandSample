using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AsyncCommandSample
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T backingStore, in T value, in System.Action? onChanged = null, [CallerMemberName] in string propertyname = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        protected void OnPropertyChanged([CallerMemberName] in string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}