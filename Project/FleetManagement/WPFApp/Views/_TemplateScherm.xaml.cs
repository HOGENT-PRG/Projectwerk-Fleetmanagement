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

namespace WPFApp.Views {
	public partial class _TemplateScherm : UserControl {
		public _TemplateScherm() {
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

		// Voorbeeld filter dialog komt weldra
		protected void TankkaartFilterDialogOpen_Click(object sender, RoutedEventArgs e) {
			// TODO Benjamin
			//var view = new TankkaartFilterDialog {
			//	DataContext = this.DataContext,
			//	Width = 340
			//};

			//FiltersDialogHost.ShowDialog(view);
		}

	}
}
