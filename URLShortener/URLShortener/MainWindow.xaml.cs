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
using URLShortener.Services;

namespace URLShortener
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.InitServices();

			this.shortenedUrlText.Text = "http://duco.cc";
		}

		private void InitServices()
		{
			foreach (UrlShortenerService service in App.Services)
			{
				this.services.Items.Add(service.Name);
			}
			this.services.SelectedItem = this.services.Items[0];
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(this.shortenedUrlText.Text);
		}

		private void copyToClipboard_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Windows.Clipboard.SetText(this.shortenedUrlText.Text);
		}

		private void shortenUrl_Click(object sender, RoutedEventArgs e)
		{
			string longUrl = this.longUrl.Text;
			if (string.IsNullOrEmpty(longUrl))
			{
				this.Error(Application.Current.FindResource("errorFillInUrl").ToString());
				return;
			}

			string serviceText = string.Empty;
			if (this.services.SelectedValue != null)
			{
				serviceText = this.services.SelectedValue.ToString();
			}

			UrlShortenerService service = null;
			if (!string.IsNullOrEmpty(serviceText))
			{
				service = App.Services.Where(s => s.Name == serviceText).FirstOrDefault();
				Action<string> callback = (string shortUrl) =>
				{
					this.shortUrlBlock.Visibility = System.Windows.Visibility.Visible;
					this.shortenedUrlText.Text = shortUrl;
				};
				Action onError = () =>
				{
					this.Error(Application.Current.FindResource("errorShortenUrl").ToString());
				};
				service.Shorten(longUrl, callback, onError);
			}
		}

		private void Error(string message)
		{
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void longUrl_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				shortenUrl_Click(null, null);
			}
		}
	}
}
