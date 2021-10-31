using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Threading.Tasks;

namespace WPFApp.Views {
    /// <summary>
    /// Interaction logic for DatabankOverzicht.xaml
    /// </summary>
    public partial class DatabankOverzicht : UserControl {
        public DatabankOverzicht() {
            InitializeComponent();
        }

		private async void Refresh_Click(object sender, RoutedEventArgs e) {
            RefreshShock.Fill = new SolidColorBrush(Colors.LimeGreen);
            await _waiter(0.3);
            RefreshShock.Fill = new SolidColorBrush(Colors.Transparent);
		}

        private Task _waiter(double seconds) {
            return Task.Run(() => { Thread.Sleep((int)(seconds * 1000)); }); 
        }
    }
}
