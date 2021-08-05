using System;
using Tusba.Components.Configuration;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Program
	{
		private readonly static InterfaceLogger InteractiveLogger = new ConsoleLogger();
		private readonly static InterfaceConfigurationReader Configuration = new SystemConfiguration();

		public static void Main(string[] args)
		{
			InteractiveLogger.Log(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
			InteractiveLogger.Log(Configuration.Get("logDirectory"));
		}
	}
}
