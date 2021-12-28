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

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for AdresWijzigen.xaml
    /// </summary>
    public partial class AdresWijzigen : UserControl
    {
        public AdresWijzigen()
        {
            InitializeComponent();
        }
      
        protected void GaNaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
            VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
            GaNaarOverzicht.Command.Execute(GaNaarOverzicht.CommandParameter);
        }

        protected void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
            GaNaarNieuwViewModel.Command.Execute(GaNaarNieuwViewModel.CommandParameter);
        }
    }
}
