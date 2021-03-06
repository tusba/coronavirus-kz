using System;
using System.Linq;
using System.Threading.Tasks;

using Tusba.Components.Configuration;
using Tusba.Components.Decorators;
using Tusba.Components.Exceptions;
using Tusba.Components.Factories.Application;
using Tusba.Components.Factories.Post;
using Tusba.Components.FileSystem;
using Tusba.Components.Http;
using Tusba.Components.Logging;
using Tusba.Components.Parsers;
using Tusba.Components.Repositories.Post;
using Tusba.Components.Services.PostStats;
using Tusba.Patterns.Visitor.Export;

using Tusba.Enumerations.Application;
using ApplicationAction = Tusba.Enumerations.Application.Action;
using Tusba.Enumerations.Post;
using PostType = Tusba.Enumerations.Post.Type;

using Tusba.Models;
using ApplicationState = Tusba.Models.Application.State;
using ApplicationOptions = Tusba.Models.Application.Options;

using ArrayUtil = Tusba.Components.Util.Array;

namespace CoronavirusKz
{
	public class Application
	{
		private const string HTML_DATA_DIRECTORY_ALIAS = "html.data.directory";
		private const string STATS_HTML_DATA_DIRECTORY_ALIAS = "stats.html.data.directory";

		private static readonly InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;
		private static readonly InterfaceLogger InteractiveLogger = ConsoleLogger.Instance;
		private static readonly InterfaceLogger PersistentLogger;
		private static readonly InterfaceLogger ErrorLogger;

		private readonly string? startFromPostId;
		private readonly ApplicationAction? action;
		private readonly ApplicationOptions options;
		private readonly DateTime startedAt = DateTime.Now;

		private readonly string postStatsFormat = PostStatsExporter<string>.Format;
		private string StatsExportDataDirectoryAlias => $"{postStatsFormat}.data.directory";


		private ApplicationState state = new ApplicationState();

		private PostRepository? postRepo;
		private PostRepository PostRepo
		{
			get
			{
				if (postRepo is null) {
					postRepo = new PostRepository(startFromPostId)
					{
						Directory = Configuration.Get(HTML_DATA_DIRECTORY_ALIAS)
					};
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
			string logDirectory = Configuration.Get("app.log.directory");

			FileLogger appFileLogger = new FileLogger(Configuration.Get("app.log.file"))
			{
				Directory = logDirectory
			};
			FileLogger errorFileLogger = new FileLogger(Configuration.Get("error.log.file"))
			{
				Directory = logDirectory
			};

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
				HTML_DATA_DIRECTORY_ALIAS,
				STATS_HTML_DATA_DIRECTORY_ALIAS
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
					await ActionParse();
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

			// determine minimum & maximum dates from all statistics posts
			var statsDateRange = ArrayUtil.MinMax(statsPosts.Select(post => post.Date).ToArray());
			if (statsDateRange is not null)
			{
				var dateRange = ((string Min, string Max)) statsDateRange;
				state.Dates = new DateRange(dateRange.Min, dateRange.Max);
			}

			// save filtered post models' raw content
			var persistService = new PostStatsPersistService();
			persistService.SetPosts(statsPosts).Directory = Configuration.Get(STATS_HTML_DATA_DIRECTORY_ALIAS);

			if (!(await persistService.Store()))
			{
				throw new ApplicationRuntimeException("failed to store statistics from posts");
			}
			await PersistentLogger.Log($"Statistics stored in {persistService.Directory}");
		}

		/**
		 * 1. Fetch raw statistics data from previously obtained posts
		 * 2. Parse & store it as structured statistics data
		 *
		 * @throws ApplicationConfigurationException
		 * @throws ApplicationRuntimeException
		 */
		private async Task ActionParse()
		{
			// (1)
			var obtainService = new PostStatsObtainService()
			{
				Directory = Configuration.Get(STATS_HTML_DATA_DIRECTORY_ALIAS),
				Types = new PostType[] { PostType.STATS_DISEASED, PostType.STATS_RECOVERED }
			};

			if ((state.Dates ?? options.Dates) is DateRange dates)
			{
				obtainService.Dates = dates;
			}

			if (!(await obtainService.Fetch()))
			{
				throw new ApplicationRuntimeException($"failed to obtain statistics from posts for {obtainService.Dates}");
			}
			await PersistentLogger.Log($"Statistics obtained from {obtainService.Posts.Length} posts for {obtainService.Dates}");

			// (2)
			InterfaceExporter<string> postStatsExporter = new PostStatsExporter<string>().FactoryInstance;
			string postStatsExportDirectory = Configuration.Get(StatsExportDataDirectoryAlias);

			var postStatsExportRepository = new PostStatsRepository()
			{
				FileExtenstion = postStatsFormat
			};

			foreach (var post in obtainService.Posts)
			{
				postStatsExportRepository.Type = post.Type;
				postStatsExportRepository.Date = post.Date;
				postStatsExportRepository.Directory = postStatsExportDirectory;

				string structuredContent = await PostStatsParserFactory
					.ExportableAsResult(post.Content)
					.ReceiveExporter(postStatsExporter);

				if (!(await postStatsExportRepository.Store(structuredContent)))
				{
					throw new ApplicationRuntimeException($"failed to parse & store statistics from post \"{post.Type.AsString()}\"/{post.Date}");
				}
			}
			await PersistentLogger.Log($"Statistics parsed & stored in {postStatsExportDirectory}");
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
			return new FactoryArgumentResolver().FactoryInstance.Handle(args);
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
