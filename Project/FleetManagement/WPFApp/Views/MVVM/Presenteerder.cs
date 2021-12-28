using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

// Aangezien Fody als dependency geintroduceerd is zal het gebruik van Update() minder van toepassing zijn, er vindt namelijk bij die package automatisch emitten van property changed events plaats
// Enkele functies gebruiken nog Update

// Kan van overgeerft worden, draagt ook de INotifyPropertyChanged interface over waardoor Fody de overervende klasse zal behandelen en stelt de update functie ter beschikking
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


        // Wordt gebruikt door de view ApplicatieOverzicht, niet de beste plaats ervoor maar past onder de noemer "Presenteerder", en beter hier dan in ApplicatieOverzicht
        public SolidColorBrush TabbladTekstKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FF312F2F");
        public SolidColorBrush ActiefTabbladKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
        public SolidColorBrush InactiefTabbladKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE1E1E1");
        public SolidColorBrush TabbladOnderlijningKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#00000000");
    }
}
