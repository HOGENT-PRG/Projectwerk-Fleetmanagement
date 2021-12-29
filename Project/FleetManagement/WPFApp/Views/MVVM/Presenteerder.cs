using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

// Aangezien Fody als dependency geintroduceerd is is het rechtstreeks gebruik van Update() niet meer van toepassing, tijdens de eerste fases werd deze functie nog gebruikt.
// De package faciliteert het automatisch emitten van PropertyChanged events door at compilation de properties om te vormen naar properties met full bodies, private velden aan te maken en in de setter OnPropertyChanged aan te roepen.
// Deze omvormingen zijn niet zichtbaar in de IDE maar overerving van INotifyPropertyChanged impliceert dit gedrag

// De meeste ViewModels erven hier van over (hetzij direct of via overerving van FilterDialogs of NotificatieModule) aangezien het daar aangenamer ontwikkelen is om geen private velden aan te maken en full property bodies te schrijven.
namespace WPFApp.Views.MVVM {
    internal abstract class Presenteerder : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Deze zou normaal gebruikt worden om een PropertyChanged event te emitten in de setter van een property,
        // Fody neemt die taak over
        //
        // Om nog steeds te aan de implementatievoorwaarden te voldoen en geen errors te hebben in de IDE
        // worden deze functies niet ontbonden
        protected void Update<T>(ref T veld, T waarde, [CallerMemberName] string? propertyNaam = null) {
            if (EqualityComparer<T>.Default.Equals(veld, waarde)) return;

            veld = waarde;
            OnPropertyChanged(propertyNaam);
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyNaam = null) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyNaam));
        }


        // Wordt gebruikt door de view ApplicatieOverzicht, niet de beste plaats ervoor maar past onder de noemer "Presenteerder", en beter hier dan in ApplicatieOverzichtViewModel
        public SolidColorBrush TabbladTekstKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FF312F2F");
        public SolidColorBrush ActiefTabbladKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
        public SolidColorBrush InactiefTabbladKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE1E1E1");
        public SolidColorBrush TabbladOnderlijningKleur => (SolidColorBrush)new BrushConverter().ConvertFrom("#00000000");
    }
}
