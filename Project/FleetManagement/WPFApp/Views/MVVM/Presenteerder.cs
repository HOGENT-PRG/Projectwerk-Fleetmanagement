using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// Aangezien Fody als dependency geintroduceerd is zal het gebruik van Update() minder van toepassing zijn, er vindt namelijk bij die package automatisch emitten van property changed events plaats
// Enkele functies gebruik nog Update

// Kan van overgeerft worden, houdt de INotifyPropertyChanged interface in waardoor Fody die klasse zal behandelen en stelt een functie ter beschikking
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
