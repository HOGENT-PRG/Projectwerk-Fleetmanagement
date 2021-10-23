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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void ZetWindowGrootte(double deductiePercentage) {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;
            double adjusted_width = (width * ((100 - deductiePercentage) / 100));
            double adjusted_height = (height * ((100 - deductiePercentage) / 100));

            this.Width = adjusted_width;
            this.Height = adjusted_height;
        }
        public MainWindow()
        {
            InitializeComponent();
            ZetWindowGrootte(20);
        }
    }
}
