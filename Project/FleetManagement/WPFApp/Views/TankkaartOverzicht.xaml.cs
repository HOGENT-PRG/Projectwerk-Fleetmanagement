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

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for TankkaartOverzicht.xaml
    /// </summary>
    public partial class TankkaartOverzicht : UserControl
    {
        public TankkaartOverzicht()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }

        private void _verbergAlleZoekfilters()
        {
            zoekveld.Visibility = Visibility.Hidden;
            zoekdate.Visibility = Visibility.Hidden;
        }

        private void VerwijderTankkaart_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bent u zeker dat u deze tankkaart wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VerwijderBevestigd.Command.Execute("");
            }
        }

        private void zoekfilterbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _verbergAlleZoekfilters();

                if (zoekfilterbox?.SelectedItem is not null)
                {
                    if (zoekfilterbox.SelectedItem.ToString().Contains("Vervaldatum")
                        || zoekfilterbox.SelectedItem.ToString().Contains("GeboorteDatum"))
                    {
                        zoekdate.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        zoekveld.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    zoekveld.Visibility = Visibility.Visible;
                }
            }
            catch { /* Durft al eens klagen bij switchen van tabs */ }
        }
    }
}
