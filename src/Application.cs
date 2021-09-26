using System;
using System.Threading.Tasks;
using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Exceptions;
using Tusba.Components.FileSystem;
using Tusba.Components.Http;
using Tusba.Components.Logging;
using Tusba.Components.Repositories;

namespace CoronavirusKz
{
	public class Application
	{
		private static readonly InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;
		private static readonly InterfaceLogger InteractiveLogger = ConsoleLogger.Instance;
		private static readonly InterfaceLogger PersistentLogger;
		private static readonly InterfaceLogger ErrorLogger;

		private readonly string? startFromPostId;
		private readonly DateTime startedAt = DateTime.Now;

		/** Constructors & Initializers */

		/**
		 * @throws ApplicationRuntimeException
		 */
		static Application()
		{
			(InterfaceLogger appFileLogger, InterfaceLogger errorFileLogger) = InitializeLoggers();
			PersistentLogger = new DateTimeLogDecorator(appFileLogger);
			ErrorLogger = new DateTimeLogDecorator(errorFileLogger);

			InitializeDataDirectory();
		}

		private Application(string? postId)
		{
			startFromPostId = postId;
		}

		private static async Task<Application> ApplicationBuilder(string? postId)
		{
			Application app = new Application(postId);

			await PersistentLogger.Log(new String('-', 66));
			await PersistentLogger.Log(
				"Started" + (app.startFromPostId is null ? "" : $" from post ID={app.startFromPostId}")
			);

			return app;
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private static (InterfaceLogger app, InterfaceLogger error) InitializeLoggers()
		{
			FileLogger appFileLogger = new FileLogger(Configuration.Get("app.log.file"));
			FileLogger errorFileLogger = new FileLogger(Configuration.Get("error.log.file"));
			errorFileLogger.Directory = appFileLogger.Directory = Configuration.Get("app.log.directory");

			if (!FileStorage.ProvideDirectory(appFileLogger.Directory))
			{
				throw new ApplicationRuntimeException(@$"cannot create directory for log: {appFileLogger.Directory}");
			}

			return (appFileLogger, errorFileLogger);
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private static void InitializeDataDirectory()
		{
			string dataDir = Configuration.Get("app.data.directory");
			if (!FileStorage.ProvideDirectory(dataDir))
			{
				throw new ApplicationRuntimeException(@$"cannot create directory for data: {dataDir}");
			}
		}

		/** Utility methods */

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task Run()
		{
			await ActionFetch();
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task ActionFetch()
		{
			string responseBody = await FetchIndexPage();
			await PersistentLogger.Log($"Got {responseBody.Length} bytes");

			var postRepo = new PostRepository(startFromPostId);
			postRepo.Directory = Configuration.Get("app.data.directory");
			if (!(await postRepo.Store(responseBody))) {
				throw new ApplicationRuntimeException("cannot store obtained post page");
			}
			await PersistentLogger.Log($"Stored as {postRepo.FileName}");
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task<string> FetchIndexPage()
		{
			using (InterfaceHttpGet indexPage = new WebIndexPage(Configuration.Get("vendor.index.url")))
			{
				try
				{
					return await indexPage.Get(startFromPostId);
				}
				catch (Exception e)
				{
					throw new ApplicationRuntimeException("fetch index page", e);
				}
			}
		}

		/** Main methods */

		public static async Task Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += HandleException;

			Application app = await ApplicationBuilder(args.Length > 0 ? args[0] : null);
			await app.Run();

			string finishedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			await PersistentLogger.Log("Finished");
			await PersistentLogger.Log($"Time elapsed (ms): {(int) DateTime.Now.Subtract(app.startedAt).TotalMilliseconds}");
		}

		private static async void HandleException(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception) args.ExceptionObject;
			Exception? innerException = e.InnerException;

			string message = e.Message + (innerException is null ? "" : $": {innerException.Message}");
			await InteractiveLogger.Log($"Error: {message}");
			await PersistentLogger.Log($"ERROR:\t{message}");
			await ErrorLogger.Log(e + Environment.NewLine);

			Environment.Exit(0);
		}
	}
}
