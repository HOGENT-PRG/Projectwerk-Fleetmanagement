using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFApp.Model.Hosts;
using WPFApp.Views.Dialogs;
using FSharp.Control.Tasks;

namespace WPFApp.Views {
    /// <summary>
    /// Interaction logic for AdresOverzicht.xaml
    /// </summary>
    public partial class AdresOverzicht : UserControl {
        public AdresOverzicht() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }

        private void VerwijderAdres_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bent u zeker dat u dit adres wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VerwijderenBevestigd.Command.Execute("");
            }
        }

        private void AdresWijzigenDialogOpen_click(object sender, RoutedEventArgs e)
        {
            


        }
        }
    
}
