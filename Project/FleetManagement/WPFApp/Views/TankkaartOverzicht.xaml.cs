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
    /// Interaction logic for TankkaartOverzicht.xaml
    /// </summary>
    public partial class TankkaartOverzicht : UserControl {
        public TankkaartOverzicht() {
            InitializeComponent();
        }

        private void LMB_VoegTankkaartToe(object sender, MouseButtonEventArgs e) {

        }

        private void zoekterm_GetFocus(object sender, RoutedEventArgs e) {
            if (zoekveld.Text == "Zoekterm...") {
                zoekveld.Text = string.Empty;
            }
        }

        private void zoekterm_LostFocus(object sender, RoutedEventArgs e) {
            if (zoekveld.Text == string.Empty) {
                zoekveld.Text = "Zoekterm...";
            }
        }
    }
}
