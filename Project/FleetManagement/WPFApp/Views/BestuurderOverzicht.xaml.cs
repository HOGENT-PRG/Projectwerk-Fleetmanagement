using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp.Views {
    /// <summary>
    /// Interaction logic for BestuurderOverzicht.xaml
    /// </summary>
    public partial class BestuurderOverzicht : UserControl {
        public BestuurderOverzicht() {
            InitializeComponent();
        }


        private void LMB_VoegBestuurderToe(object sender, MouseButtonEventArgs e) {

        }

        private void _verbergAlleZoekfilters() {
            zoekveld.Visibility = Visibility.Hidden;
            zoekdate.Visibility = Visibility.Hidden;
        }

        private void zoekfilterbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                _verbergAlleZoekfilters();

                if (zoekfilterbox?.SelectedItem is not null) {
                    if (zoekfilterbox.SelectedItem.ToString().Contains("GeboorteDatum")
                        || zoekfilterbox.SelectedItem.ToString().Contains("Vervaldatum")) {
                        zoekdate.Visibility = Visibility.Visible;
                    } else {
                        zoekveld.Visibility = Visibility.Visible;
                    }
                } else {
                    zoekveld.Visibility = Visibility.Visible;
                }
            } catch { /* Durft al eens klagen bij switchen van tabs */ }
        }

        private void VerwijderBestuurder_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Bent u zeker dat u deze bestuurder wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                VerwijderenBevestigd.Command.Execute("");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }
	}

    
}
