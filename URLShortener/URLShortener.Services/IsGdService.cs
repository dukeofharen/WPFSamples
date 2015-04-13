using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.Services
{
	public class IsGdService : UrlShortenerService
	{
		private const string RootUrl = "http://is.gd/create.php?format=simple&url={0}";
		private Action<string> downloadCallback;
		private Action onErrorCallback;

		public string Name
		{
			get
			{
				return "Is.gd";
			}
		}

		public void Shorten(string url, Action<string> callback, Action onError)
		{
			this.downloadCallback = callback;
			this.onErrorCallback = onError;
			WebClient client = new WebClient();
			client.DownloadStringAsync(new Uri(string.Format(RootUrl, WebUtility.UrlEncode(url))));
			client.DownloadStringCompleted += this.DownloadStringCompleted;
		}

		private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				this.onErrorCallback();
			}
			else
			{
				this.downloadCallback(e.Result);
			}
		}
	}
}
