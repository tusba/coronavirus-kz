using System.Configuration;

namespace Tusba.Components.Configuration
{
	public class SystemConfiguration : InterfaceConfigurationReader
	{
		public string Get(string key) => ConfigurationManager.AppSettings.Get(key) ?? "";
	}
}
