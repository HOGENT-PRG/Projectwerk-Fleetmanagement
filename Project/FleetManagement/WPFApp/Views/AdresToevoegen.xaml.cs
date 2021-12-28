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

namespace WPFApp.Interfaces
{
    /// <summary>
    /// Interaction logic for AdresToevoegen.xaml
    /// </summary>
    public partial class AdresToevoegen : UserControl
    {
        public AdresToevoegen()
        {
            InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
            GaNaarNieuwViewModel.Command.Execute(GaNaarNieuwViewModel.CommandParameter);
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
    }
}
