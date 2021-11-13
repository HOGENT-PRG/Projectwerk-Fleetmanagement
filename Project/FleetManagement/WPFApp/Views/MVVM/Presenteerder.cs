using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Views.MVVM {
    internal abstract class Presenteerder : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Update<T>(ref T veld, T waarde, [CallerMemberName] string? propertyNaam = null) {
            if (EqualityComparer<T>.Default.Equals(veld, waarde)) return;

            veld = waarde;
            OnPropertyChanged(propertyNaam);
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyNaam = null) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyNaam));
        }
    }
}
