using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Views.Dialogs;

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for VoertuigWijzigen.xaml
    /// </summary>
    public partial class VoertuigWijzigen : UserControl
    {
        public VoertuigWijzigen()
        {
            InitializeComponent();
        }
        private void GaNaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
            VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
            GaNaarOverzicht.Command.Execute(GaNaarOverzicht.CommandParameter);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VoerStartupRoutineUit.Command.Execute("Loaded");
        }
        private void BestuurderFilterDialogOpen_Click(object sender, RoutedEventArgs e)
        {
            var view = new BestuurderFilterDialog
            {
                DataContext = this.DataContext,
                Width = 340
            };

            FiltersDialogHost.ShowDialog(view);
        }
    }
}
