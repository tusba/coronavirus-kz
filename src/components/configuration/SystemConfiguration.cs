using System.Configuration;
using Tusba.Patterns.Singleton;

namespace Tusba.Components.Configuration
{
	public class SystemConfiguration : BaseSingleton<InterfaceConfigurationReader, SystemConfiguration>,
		InterfaceConfigurationReader
	{
		private SystemConfiguration()
		{
		}

		public string Get(string key) => ConfigurationManager.AppSettings.Get(key) ?? "";
	}
}
