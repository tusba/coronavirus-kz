using System;
using System.Linq;
using System.Threading.Tasks;

using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Exceptions;
using Tusba.Components.FileSystem;
using Tusba.Components.Http;
using Tusba.Components.Logging;
using Tusba.Components.Parsers;
using Tusba.Components.Repositories.Post;
using Tusba.Components.Services.PostStats;

using Tusba.Enumerations.Application;
using ApplicationAction = Tusba.Enumerations.Application.Action;
using PostType = Tusba.Enumerations.Post.Type;

using ApplicationState = Tusba.Models.Application.State;
using Tusba.Models;

namespace CoronavirusKz
{
	public class Application
	{
		private static readonly InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;
		private static readonly InterfaceLogger InteractiveLogger = ConsoleLogger.Instance;
		private static readonly InterfaceLogger PersistentLogger;
		private static readonly InterfaceLogger ErrorLogger;

		private readonly string? startFromPostId;
		private readonly ApplicationAction? action;
		private readonly DateTime startedAt = DateTime.Now;

		private ApplicationState state = new ApplicationState();

		private PostRepository? postRepo;
		private PostRepository PostRepo
		{
			get
			{
				if (postRepo is null) {
					postRepo = new PostRepository(startFromPostId);
					postRepo.Directory = Configuration.Get("html.data.directory");
				}

				return postRepo;
			}
		}

		/** Constructors & Initializers */

		/**
		 * @throws ApplicationRuntimeException
		 */
		static Application()
		{
			(InterfaceLogger appFileLogger, InterfaceLogger errorFileLogger) = InitializeLoggers();
			PersistentLogger = new DateTimeLogDecorator(appFileLogger);
			ErrorLogger = new DateTimeLogDecorator(errorFileLogger);

			InitializeDataDirectories();
		}

		private Application(string? postId, string? action)
		{
			startFromPostId = postId;

			if (action is not null)
			{
				this.action = action.ResolveApplicationAction();
			}
		}

		private static async Task<Application> ApplicationBuilder(string? postId, string? action)
		{
			Application app = new Application(postId, action);

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
		private static void InitializeDataDirectories()
		{
			string[] dataDirectories = new string[]
			{
				"app.data.directory",
				"html.data.directory",
				"stats.html.data.directory"
			};

			foreach (string dirAlias in dataDirectories)
			{
				string dataDir = Configuration.Get(dirAlias);
				if (!FileStorage.ProvideDirectory(dataDir))
				{
					throw new ApplicationRuntimeException(@$"cannot create directory for data: {dataDir}");
				}
			}
		}

		/** Utility methods */

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task Run()
		{
			switch (action)
			{
				case ApplicationAction.FETCH_PAGE:
					await ActionFetch();
					return;
				case ApplicationAction.EXTRACT_POSTS:
					await ActionExtract();
					return;
				default:
					await ActionFetch();
					await ActionExtract();
					return;
			}
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task ActionFetch()
		{
			string responseBody = await FetchPageContent();
			await PersistentLogger.Log($"Got {responseBody.Length} bytes");

			if (!(await PostRepo.Store(responseBody)))
			{
				throw new ApplicationRuntimeException("cannot store obtained post page content");
			}

			state.PageContent = responseBody;
			await PersistentLogger.Log($"Stored as {PostRepo.FileName}");
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task ActionExtract()
		{
			string pageContent;
			try
			{
				pageContent = state.PageContent ?? await PostRepo.Fetch();
			}
			catch
			{
				throw new ApplicationRuntimeException("cannot fetch obtained post page content");
			}

			// parse post models from raw page content
			Post[] posts = await new PostParser(pageContent).Parse();
			await PersistentLogger.Log($"Total found {posts.Length} posts after parsing");

			// filter post models for saving
			Post[] statsPosts = posts
				.Where(
					post => post.Type == PostType.STATS_DISEASED || post.Type == PostType.STATS_RECOVERED
				)
				.ToArray();
			await PersistentLogger.Log($"Total found {statsPosts.Length} posts containing statistics information");

			// save filtered post models' raw content
			var persistService = new PostStatsPersistService();
			persistService.Directory = Configuration.Get("stats.html.data.directory");
			persistService.Posts = statsPosts;

			if (!(await persistService.Store()))
			{
				throw new ApplicationRuntimeException("failed to store statistics from posts");
			}
			await PersistentLogger.Log($"Statistics stored in {persistService.Directory}");
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task<string> FetchPageContent()
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

		/**
		 * Resolve command line arguments to post ID and/or action to perform
		 */
		private static (string?, string?) ResolveArgs(string[] args)
		{
			int postId;

			switch (args.Length)
			{
				case 0:
					return (null, null);

				case 1:
					return int.TryParse(args[0], out postId)
						? (args[0], null)
						: (null, args[0]);

				default:
					return int.TryParse(args[1], out postId)
						? (args[1], args[0])
						: (args[0], args[1]);
			}
		}

		/** Main methods */

		public static async Task Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += HandleException;

			var (postId, appAction) = ResolveArgs(args);
			Application app = await ApplicationBuilder(postId, appAction);
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
