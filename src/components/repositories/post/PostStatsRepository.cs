using System;
using System.IO;
using System.Threading.Tasks;
using Tusba.Enumerations.Post;
using PostType = Tusba.Enumerations.Post.Type;

namespace Tusba.Components.Repositories.Post
{
	public class PostStatsRepository : BasePostRepository
	{
		public PostType Type { get; set; }

		public string? Date { get; set; }

		public override string Directory
		{
			get => base.Directory;
			set => base.Directory = @ResolveDirectory(value);
		}

		public override string FileName
		{
			get
			{
				base.FileName = $"{Date ?? DateTime.Now.ToString("yyyy-MM-dd")}.{FileExtenstion}";
				return base.FileName;
			}

			set => base.FileName = value;
		}

		public PostStatsRepository() : base()
		{
		}

		public PostStatsRepository(PostType type, string? date = null) : this()
		{
			Type = type;
			Date = date;
		}

		public override async Task<bool> Store(string content)
		{
			ProvideDirectory(Directory, true);

			return await base.Store(content);
		}

		protected string SubDirectory => Type.AsString();

		private string ResolveDirectory(string directoryValue)
		{
			string dirName = @directoryValue;
			string subDirName = @SubDirectory;

			if (!String.IsNullOrEmpty(subDirName))
			{
				dirName += @$"{Path.DirectorySeparatorChar}{subDirName}";
			}

			return dirName;
		}
	}
}
