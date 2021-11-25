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
    /// Interaction logic for VoertuigOverzicht.xaml
    /// </summary>
    public partial class VoertuigOverzicht : UserControl {
        public VoertuigOverzicht() {
            InitializeComponent();
        }

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }

        private void _verbergAlleZoekfilters() {
            zoekveld.Visibility = Visibility.Hidden;
            // ..
        }

        private void zoekfilterbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
           // ..
           // Indien nodig hier switchen naar ander zoekveld (momenteel nvt)
        }

        private void VerwijderVoertuig_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Bent u zeker dat u dit voertuig wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                VerwijderenBevestigd.Command.Execute("");
            }
        }

    }
}
