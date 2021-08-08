using System;
using System.IO;
using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Application
	{
		private readonly InterfaceConfigurationReader Configuration = new SystemConfiguration();
		private readonly InterfaceLogger InteractiveLogger = new ConsoleLogger();
		private readonly InterfaceLogger PersistentLogger;

		private readonly DateTime startedAt = DateTime.Now;

		private Application()
		{
			FileLogger appFileLogger = new FileLogger(Configuration.Get("app.log.file"));
			Bootstrap(appFileLogger);

			PersistentLogger = new DateTimeLogDecorator(appFileLogger);
			PersistentLogger.Log(new String('-', 66));
			PersistentLogger.Log("Started");
		}

		private void Bootstrap(FileLogger appFileLogger)
		{
			appFileLogger.Directory = Configuration.Get("app.log.directory");

			if (!Directory.Exists(appFileLogger.Directory))
			{
				Directory.CreateDirectory(appFileLogger.Directory);
			}
		}

		private void Run()
		{
			// TODO
		}

		public static void Main(string[] args)
		{
			Application app = new Application();
			app.Run();

			string finishedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			app.PersistentLogger.Log("Finished");
			app.PersistentLogger.Log("Time elapsed (ms): " + (int) DateTime.Now.Subtract(app.startedAt).TotalMilliseconds);
		}
	}
}
