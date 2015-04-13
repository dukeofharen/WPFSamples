using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace URLShortener.Services
{
	public interface UrlShortenerService
	{
		string Name { get; }
		void Shorten(string url, Action<string> callback, Action onError);
	}
}
