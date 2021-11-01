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
using WPFApp.Views;

namespace WPFApp
{
    public partial class ApplicatieOverzicht : Window
    {
        private List<Button> HistoriekTabs = new();
        private ApplicatieOverzichtViewModel referentieleViewModel = new ApplicatieOverzichtViewModel();

        private void ZetWindowGrootte(double deductiePercentage) {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;
            double adjusted_width = (width * ((100 - deductiePercentage) / 100));
            double adjusted_height = (height * ((100 - deductiePercentage) / 100));

            this.Width = adjusted_width;
            this.Height = adjusted_height;
        }
        public ApplicatieOverzicht()
        {
            InitializeComponent();
            ZetWindowGrootte(30);
            ActiveerTab_click(AdresTab, null);
        }

        private void ActiveerTab_click(object sender, RoutedEventArgs e) {
            SolidColorBrush actief = referentieleViewModel.ActiefTabbladKleur;
            SolidColorBrush inactief = referentieleViewModel.InactiefTabbladKleur;

            foreach (Button historicalBtn in HistoriekTabs.ToList()) {
                historicalBtn.Background = inactief;
                historicalBtn.BorderBrush = inactief;
                HistoriekTabs.Remove(historicalBtn);
            }

            Button btn = sender as Button;
            btn.Background = actief;
            btn.BorderBrush = actief;

            HistoriekTabs.Add(btn);
        }
    }
}
