using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

<<<<<<< HEAD
// Zet een boolean om naar een image, voor gebruik in xaml van databank info (checkmark of red cross)
=======
// Zet een boolean om naar een image, wordt gebruikt door DatabankOverzicht voor weergave checkmark / cross afhankelijk van de waarde
>>>>>>> parent of 87a59f3 (Fix requestDTOnaarDomein enum parsing, verplaatsen interface, RRNValideerder soft error, extra check bestuurdermgr, overbodige vpp files weg)
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
