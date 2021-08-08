using System;
using System.IO;
using Tusba.Components.Configuration;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Application
	{
		private readonly InterfaceConfigurationReader Configuration = new SystemConfiguration();
		private readonly InterfaceLogger InteractiveLogger = new ConsoleLogger();
		private readonly InterfaceLogger PersistentLogger;

		private Application()
		{
			string startedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			PersistentLogger = new FileLogger(Configuration.Get("app.log.file"));
			Bootstrap();
			PersistentLogger.Log(new String('-', 66));
			PersistentLogger.Log("Started at " + startedAt);
		}

		private void Bootstrap()
		{
			FileLogger appFileLogger = (FileLogger) PersistentLogger;
			appFileLogger.Directory = Configuration.Get("app.log.directory");

			if (!Directory.Exists(appFileLogger.Directory))
			{
				Directory.CreateDirectory(appFileLogger.Directory);
			}
		}

		public static void Main(string[] args)
		{
			Application app = new Application();
		}
	}
}
