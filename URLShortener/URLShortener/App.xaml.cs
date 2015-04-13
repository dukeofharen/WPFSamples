using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using URLShortener.Services;

/*
 * URL Shortener made by Duco Winterwerp in 2015
 * Icon by Aha-Soft Team (https://www.iconfinder.com/icons/328008/chain_link_permalink_url_web_web_address_icon)
*/
namespace URLShortener
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static UrlShortenerService[] Services { get; set; }

		public App()
		{
			Bootstrapper bs = new Bootstrapper();
			Services = bs.ImportServices();
		}
	}
}
