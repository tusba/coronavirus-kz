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
using ApplicationOptions = Tusba.Models.Application.Options;
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
		private readonly ApplicationOptions options;
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

		private Application(ApplicationOptions options)
		{
			startFromPostId = options.PostId;
			action = options.Action?.ResolveApplicationAction();
			this.options = options;
		}

		private static async Task<Application> ApplicationBuilder(ApplicationOptions options)
		{
			Application app = new Application(options);

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
				case ApplicationAction.PARSE_STATS:
					await ActionParse();
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
			persistService.SetPosts(statsPosts).Directory = Configuration.Get("stats.html.data.directory");

			if (!(await persistService.Store()))
			{
				throw new ApplicationRuntimeException("failed to store statistics from posts");
			}
			await PersistentLogger.Log($"Statistics stored in {persistService.Directory}");
		}

		/**
		 * @throws ApplicationRuntimeException
		 */
		private async Task ActionParse()
		{
			string s1 = options.Dates.Date?.ToString("yyyy-MM-dd") ?? "null";
			string s2 = options.Dates.Boundary?.ToString("yyyy-MM-dd") ?? "null";
			await InteractiveLogger.Log($"TODO parse html into xml/json: {s1}, {s2}");
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

		private static ApplicationOptions ResolveArgs(string[] args)
		{
			var options = new ApplicationOptions();
			int argCount = args.Length;
			int postId;

			switch (argCount)
			{
				case 0:
					return options;

				case 1:
					if (int.TryParse(args[0], out postId))
					{
						options.PostId = args[0];
					}
					else
					{
						options.Action = args[0];
					}

					return options;
			}

			if (argCount is >= 2 and <=3 && args[0].ResolveApplicationAction() == ApplicationAction.PARSE_STATS)
			{
				options.Action = args[0];
				switch (argCount)
				{
					case 2:
						options.setDate(args[1]);
						break;
					case 3:
						options.setDate(args[1], args[2]);
						break;
					// default:
					// 	options.setDate();
				}

				return options;
			}

			(options.PostId, options.Action) = int.TryParse(args[0], out postId)
				? (args[0], args[1])
				: (args[1], args[0]);

			return options;
		}

		/** Main methods */

		public static async Task Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += HandleException;

			var appOptions = ResolveArgs(args);
			Application app = await ApplicationBuilder(appOptions);
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
