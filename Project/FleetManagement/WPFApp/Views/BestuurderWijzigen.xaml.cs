using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Interfaces.Dialogs;

namespace WPFApp.Interfaces {
	// Praktisch hetzelfde als toevoegen code behind, maar zonder reset_click handler
	public partial class BestuurderWijzigen : UserControl {
		public BestuurderWijzigen() {
			InitializeComponent();
		}

		private void GaNaarOverzicht_Click(object sender, RoutedEventArgs e) {
			VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
			GaNaarOverzicht.Command.Execute(GaNaarOverzicht.CommandParameter);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
			VoerStartupRoutineUit.Command.Execute("Loaded");
		}

		private void AdresFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new AdresFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}

		private void TankkaartFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new TankkaartFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}

		private void VoertuigFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new VoertuigFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}
	}
}
