using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BurdashevCalc.Infrastructure
{
    public class ViewModel : INotifyPropertyChanged
    {
        public Action Close { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(PropertyName);
            return true;
        }
    }
}
