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
        private void ZetWindowGrootte(double deductiePercentage) {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;
            double adjusted_width = (width * ((100 - deductiePercentage)/100));
            double adjusted_height = (height * ((100 - deductiePercentage) / 100));

            this.Width = adjusted_width;
            this.Height = adjusted_height;
        }

        public SplashScreenWindow() {
            InitializeComponent();
            ZetWindowGrootte(35);
        }

        //private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
        //    this.Width = e.NewSize.Width;
        //    this.Height = e.NewSize.Height;

        //    double xChange = 1, yChange = 1;

        //    if (e.PreviousSize.Width != 0)
        //        xChange = (e.NewSize.Width / e.PreviousSize.Width);

        //    if (e.PreviousSize.Height != 0)
        //        yChange = (e.NewSize.Height / e.PreviousSize.Height);

        //    foreach (FrameworkElement fe in this.LogicalChildren) {
        //        /*because I didn't want to resize the grid I'm having inside the canvas in this particular instance. (doing that from xaml) */
        //        if (fe is Grid == false) {
        //            fe.Height = fe.ActualHeight * yChange;
        //            fe.Width = fe.ActualWidth * xChange;

        //            Canvas.SetTop(fe, Canvas.GetTop(fe) * yChange);
        //            Canvas.SetLeft(fe, Canvas.GetLeft(fe) * xChange);

        //        }
        //    }
        //}



    }
}
