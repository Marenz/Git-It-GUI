﻿using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GitItGUI
{
	public class ClonePage : UserControl, NavigationPage
	{
		public static ClonePage singleton;

		// ui
		Grid grid;
		TextBox destinationTextBox, urlTextBox, usernameTextBox, passwordTextBox;
		Button destinationSelectButton, cloneButton, cancelButton;

		public ClonePage()
		{
			singleton = this;
			AvaloniaXamlLoader.Load(this);

			// load ui
			grid = this.Find<Grid>("grid");
			destinationTextBox = this.Find<TextBox>("destinationTextBox");
			urlTextBox = this.Find<TextBox>("urlTextBox");
			usernameTextBox = this.Find<TextBox>("usernameTextBox");
			passwordTextBox = this.Find<TextBox>("passwordTextBox");
			destinationSelectButton = this.Find<Button>("destinationSelectButton");
			cloneButton = this.Find<Button>("cloneButton");
			cancelButton = this.Find<Button>("cancelButton");

			// bind ui
			destinationSelectButton.Click += DestinationSelectButton_Click;
			cloneButton.Click += CloneButton_Click;
			cancelButton.Click += CancelButton_Click;
		}

		private void CancelButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			MainWindow.LoadPage(PageTypes.Start);
		}

		private void CloneButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			ProcessingPage.singleton.mode = ProcessingPageModes.Clone;
			ProcessingPage.singleton.cloneSucceeded = false;
			ProcessingPage.singleton.cloneURL = urlTextBox.Text;
			ProcessingPage.singleton.clonePath = destinationTextBox.Text;
			ProcessingPage.singleton.cloneUsername = usernameTextBox.Text;
			ProcessingPage.singleton.clonePassword = passwordTextBox.Text;
			MainWindow.LoadPage(PageTypes.Processing);
		}

		private async void DestinationSelectButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			grid.IsVisible = false;
			var dlg = new OpenFolderDialog();
			var path = await dlg.ShowAsync();
			if (string.IsNullOrEmpty(path))
			{
				grid.IsVisible = true;
				return;
			}

			destinationTextBox.Text = path;
			grid.IsVisible = true;
		}

		public void NavigatedFrom()
		{
			
		}

		public void NavigatedTo()
		{
			if (!ProcessingPage.singleton.cloneSucceeded) return;

			destinationTextBox.Text = "";
			urlTextBox.Text = "";
			usernameTextBox.Text = "";
			passwordTextBox.Text = "";
		}
	}
}