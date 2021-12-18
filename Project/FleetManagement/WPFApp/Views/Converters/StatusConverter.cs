using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

// Zet een boolean om naar een image, voor gebruik in xaml van databank info (checkmark of red cross)
namespace WPFApp.Views {
    public class StatusConverter : IValueConverter {

        private string positieveMarker = "pack://application:,,,/WPFApp;component/_images/checkmark.png";
        private string negatieveMarker = "pack://application:,,,/WPFApp;component/_images/cross.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string pad = (bool)value ? positieveMarker : negatieveMarker ;
            BitmapImage bmp = new BitmapImage(new Uri(pad, UriKind.Absolute));
            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
