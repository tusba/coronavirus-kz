using System;
using System.IO;
using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Http;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Application
	{
		private readonly InterfaceConfigurationReader Configuration = new SystemConfiguration();
		private readonly InterfaceLogger InteractiveLogger = new ConsoleLogger();
		private readonly InterfaceLogger PersistentLogger;

		private readonly string? startFromPostId;
		private readonly DateTime startedAt = DateTime.Now;

		private Application(string? postId)
		{
			startFromPostId = postId;

			FileLogger appFileLogger = new FileLogger(Configuration.Get("app.log.file"));
			Bootstrap(appFileLogger);

			PersistentLogger = new DateTimeLogDecorator(appFileLogger);
			PersistentLogger.Log(new String('-', 66));
			PersistentLogger.Log("Started" + (startFromPostId is null ? "" : " from post ID=" + startFromPostId));
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
			InterfaceHttpGet indexPage = new WebIndexPage(Configuration.Get("vendor.index.url"));
			// TODO try catch Get()
			string responseBody = indexPage.Get(startFromPostId);
			// TODO store responseBody
			InteractiveLogger.Log("Got " + responseBody.Length + " bytes"); // TODO remove
		}

		public static void Main(string[] args)
		{
			Application app = new Application(args.Length > 0 ? args[0] : null);
			app.Run();

			string finishedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			app.PersistentLogger.Log("Finished");
			app.PersistentLogger.Log("Time elapsed (ms): " + (int) DateTime.Now.Subtract(app.startedAt).TotalMilliseconds);
		}
	}
}
