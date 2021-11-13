using MaterialDesignThemes.Wpf;
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

        // grootte zal afhangen van je scherm
        private void ZetWindowGrootte(double deductiePercentageBreedte, double deductiePercentageHoogte) {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;
            double adjusted_width = width * ((100 - deductiePercentageBreedte) / 100);
            double suggestive_adjusted_height = height * ((100 - deductiePercentageHoogte) / 100);

            this.Width = adjusted_width;
            this.Height = (adjusted_width > suggestive_adjusted_height) && adjusted_width < height ? adjusted_width : suggestive_adjusted_height;
        }
        public ApplicatieOverzicht() {
            InitializeComponent();
            ZetWindowGrootte(25, 10);
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

        // NotificatieModule
        private void OpenDialog(object sender, DependencyPropertyChangedEventArgs e) {
            if (BerichtTextBox.Text.Length > 0) {
                PopupDialogHost.ShowDialog(PopupDialogHost.DialogContent);
            }
        }

        private void PopupDialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs) {
            ((dynamic)this.DataContext).PopupDialoogContent = "";
        }
        // fin
    }
}
