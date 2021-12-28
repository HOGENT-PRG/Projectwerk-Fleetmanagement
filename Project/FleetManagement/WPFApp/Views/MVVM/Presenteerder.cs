﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// Aangezien Fody als dependency geintroduceerd is zal het gebruik van Update() minder van toepassing zijn, er vindt namelijk bij die package automatisch emitten van property changed events plaats
// Enkele functies gebruiken nog Update

<<<<<<< HEAD
// Kan van overgeerft worden, draagt ook de INotifyPropertyChanged interface over waardoor Fody die klasse zal behandelen en stelt de update functie ter beschikking
=======
// Kan van overgeerft worden, draagt ook de INotifyPropertyChanged interface over waardoor Fody de overervende klasse zal behandelen en stelt de update functie ter beschikking
>>>>>>> parent of 87a59f3 (Fix requestDTOnaarDomein enum parsing, verplaatsen interface, RRNValideerder soft error, extra check bestuurdermgr, overbodige vpp files weg)
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
