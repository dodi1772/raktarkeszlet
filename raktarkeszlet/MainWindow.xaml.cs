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
using System.IO; // <-- szükséges a fájlműveletekhez

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
				else if (darab <= 0 || darab1 <= 0)
				{
					MessageBox.Show("Hiba: Az egység/darab számának 0-nál nagyobbnak kell lennie.", "Érvénytelen szám", MessageBoxButton.OK, MessageBoxImage.Error);
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
		private void MentesFajlbaClick(object sender, RoutedEventArgs e)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter("raktar.txt", false, Encoding.UTF8))
				{
					foreach (var item in keszletLista.Items)
					{
						sw.WriteLine(item.ToString());
					}
				}
				MessageBox.Show("Mentés sikeres!", "Mentés", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (IOException ex)
			{
				MessageBox.Show("Hiba mentés közben: " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void BetoltesFajlbolClick(object sender, RoutedEventArgs e)
		{
			if (!File.Exists("raktar.txt"))
			{
				MessageBox.Show("A raktar.txt fájl nem található.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				keszletLista.Items.Clear();
				osszDarab = 0;
				foreach (var sor in File.ReadAllLines("raktar.txt", Encoding.UTF8))
				{
					keszletLista.Items.Add(sor);
					string[] parts = sor.Split(' ');
					if (parts.Length >= 6 && int.TryParse(parts[5], out int darab))
					{
						osszDarab += darab;
					}
				}
				osszdarab.Content = osszDarab;
				MessageBox.Show("Betöltés sikeres!", "Betöltés", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (IOException ex)
			{
				MessageBox.Show("Hiba betöltés közben: " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}