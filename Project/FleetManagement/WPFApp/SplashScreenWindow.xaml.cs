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
using System.Windows.Shapes;

namespace WPFApp {
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window {
        //private Size GetDpiSafeResolution() {
        //    PresentationSource _presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
        //    Matrix matix = _presentationSource.CompositionTarget.TransformToDevice;
        //    return new System.Windows.Size(
        //        System.Windows.SystemParameters.PrimaryScreenWidth * matix.M22,
        //        System.Windows.SystemParameters.PrimaryScreenHeight * matix.M11);
        //}

        private void ZetWindowGrootte(double deductiePercentage) {
            //Size s = GetDpiSafeResolution();
            //double width = s.Width;
            //double height = s.Height;
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;
            double adjusted_width = (width * ((100 - deductiePercentage)/100));
            double adjusted_height = (height * ((100 - deductiePercentage) / 100));

            this.Width = adjusted_width;
            this.Height = adjusted_height;
            
            Application.Current.MainWindow.Width = adjusted_width;
            Application.Current.MainWindow.Height = adjusted_height;
            this.UpdateLayout();
        }

        public SplashScreenWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ZetWindowGrootte(30);
        }
    }
}
