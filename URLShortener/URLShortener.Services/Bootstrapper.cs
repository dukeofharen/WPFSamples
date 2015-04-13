using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.Services
{
	public class Bootstrapper
	{
		public UrlShortenerService[] ImportServices()
		{
			Type interfaceType = typeof(UrlShortenerService);
			Type[] types = AppDomain.CurrentDomain.GetAssemblies()
							.SelectMany(s => s.GetTypes())
							.Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
							.ToArray();

			UrlShortenerService[] result = new UrlShortenerService[types.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = (UrlShortenerService)Activator.CreateInstance(types[i]);
			}

			return result;
		}
	}
}
