using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace raktarkeszlet
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string elem = "";
		private int osszDarab = 0;
		private int darabszam = 0;
		public MainWindow()
		{
			InitializeComponent();
			termekMezo.Focus();
		}

		private void HozzaadClick(object sender, RoutedEventArgs e)
		{
			if (termekMezo.Text == "" || egysegMezo.Text == "" || darabMezo.Text == "")
			{
				MessageBox.Show("A mezők kitöltése kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				if (!int.TryParse(egysegMezo.Text, out int darab) || !int.TryParse(darabMezo.Text, out int darab1))
				{
					MessageBox.Show("Hiba: Az egység/darab számának és vagy pozitív egésznek kell lennie.", "Érvénytelen szám", MessageBoxButton.OK, MessageBoxImage.Error);
					MezoTorles();
					return;
				}
				else
				{
					darabszam = int.Parse(darabMezo.Text);
					osszDarab += darabszam;
					osszdarab.Content = osszDarab;
					elem = $"{termekMezo.Text} - {egysegMezo.Text} Ft - {darabszam} db";
					keszletLista.Items.Add(elem);
					MezoTorles();
				}
			}
		}
		private void MezoTorles()
		{
			darabMezo.Text = "";
			egysegMezo.Text = "";
			termekMezo.Text = "";
			termekMezo.Focus();
		}

		private void TorlesClick(object sender, RoutedEventArgs e)
		{
			if (keszletLista.SelectedItem != null)
			{
				string[] sor = keszletLista.SelectedItem.ToString().Split(" ");
				osszDarab -= int.Parse(sor[5]);
				osszdarab.Content = osszDarab;
				keszletLista.Items.Remove(keszletLista.SelectedItem);
			}
			else
			{
				MessageBox.Show("Kérlek jelölj ki egy elemet a listából.");
			}
		}
	}
}