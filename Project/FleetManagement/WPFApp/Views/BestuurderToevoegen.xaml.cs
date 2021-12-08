﻿using MaterialDesignThemes.Wpf;
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
using WPFApp.Views.Dialogs;

namespace WPFApp.Views {
	/// <summary>
	/// Interaction logic for BestuurderToevoegen.xaml
	/// </summary>
	public partial class BestuurderToevoegen : UserControl {
		public BestuurderToevoegen() {
			InitializeComponent();
		}

		private void Reset_Click(object sender, RoutedEventArgs e) {
			VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
			GaNaarNieuwViewModel.Command.Execute(GaNaarNieuwViewModel.CommandParameter);
		}

		protected void GaNaarOverzicht_Click(object sender, RoutedEventArgs e) {
			VerwijderViewModel.Command.Execute(VerwijderViewModel.CommandParameter);
			GaNaarOverzicht.Command.Execute(GaNaarOverzicht.CommandParameter);
		}

		protected void UserControl_Loaded(object sender, RoutedEventArgs e) {
			VoerStartupRoutineUit.Command.Execute("Loaded");
		}

		protected void AdresFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new AdresFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}

		protected void TankkaartFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new TankkaartFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}

		protected void VoertuigFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			var view = new VoertuigFilterDialog {
				DataContext = this.DataContext,
				Width = 340
			};

			FiltersDialogHost.ShowDialog(view);
		}

	}
}
