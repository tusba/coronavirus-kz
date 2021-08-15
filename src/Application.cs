using System;
using System.IO;
using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Exceptions;
using Tusba.Components.Http;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Application
	{
		private static readonly InterfaceConfigurationReader Configuration = new SystemConfiguration();
		private static readonly InterfaceLogger InteractiveLogger = new ConsoleLogger();
		private static readonly InterfaceLogger PersistentLogger;
		private static readonly InterfaceLogger ErrorLogger;

		private readonly string? startFromPostId;
		private readonly DateTime startedAt = DateTime.Now;

		private Application(string? postId)
		{
			startFromPostId = postId;

			PersistentLogger.Log(new String('-', 66));
			PersistentLogger.Log("Started" + (startFromPostId is null ? "" : " from post ID=" + startFromPostId));
		}

		static Application()
		{
			FileLogger appFileLogger = new FileLogger(Configuration.Get("app.log.file"));
			FileLogger errorFileLogger = new FileLogger(Configuration.Get("error.log.file"));
			errorFileLogger.Directory = appFileLogger.Directory = Configuration.Get("app.log.directory");

			if (!Directory.Exists(appFileLogger.Directory))
			{
				Directory.CreateDirectory(appFileLogger.Directory);
			}

			PersistentLogger = new DateTimeLogDecorator(appFileLogger);
			ErrorLogger = new DateTimeLogDecorator(errorFileLogger);
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private void Run()
		{
			InterfaceHttpGet indexPage = new WebIndexPage(Configuration.Get("vendor.index.url"));
			string responseBody;

			try
			{
				responseBody = indexPage.Get(startFromPostId);
			}
			catch (Exception e)
			{
				throw new ApplicationRuntimeException("fetch index page", e);
			}

			PersistentLogger.Log("Got " + responseBody.Length + " bytes");
		}

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += HandleException;

			Application app = new Application(args.Length > 0 ? args[0] : null);
			app.Run();

			string finishedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			PersistentLogger.Log("Finished");
			PersistentLogger.Log("Time elapsed (ms): " + (int) DateTime.Now.Subtract(app.startedAt).TotalMilliseconds);
		}

		private static void HandleException(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception) args.ExceptionObject;
			Exception? innerException = e.InnerException;

			string message = e.Message + (innerException is null ? "" : ": " + innerException.Message);
			InteractiveLogger.Log("Error: " + message);
			PersistentLogger.Log("ERROR:\t" + message);
			ErrorLogger.Log(e + Environment.NewLine);

			Environment.Exit(0);
		}
	}
}
