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

namespace WPFApp
{
    public partial class ApplicatieOverzicht : Window
    {
        private List<Button> historical = new();

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
        }

        private void ActiveTab_Click(object sender, RoutedEventArgs e) {
            SolidColorBrush active = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF4AC355"));
            SolidColorBrush inactive = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF729D3F"));//FF8BC34A

            foreach (Button historicalBtn in historical.ToList()) {
                historicalBtn.Background = inactive;
                historicalBtn.BorderBrush = inactive;
                historical.Remove(historicalBtn);
            }

            Button btn = sender as Button;
            btn.Background = active;
            btn.BorderBrush = active;

            historical.Add(btn);
        }
    }
}
