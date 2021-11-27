using System;
using System.IO;
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

			set
			{
				string dirName = @value;
				string subDirName = @SubDirectory;

				if (!String.IsNullOrEmpty(subDirName))
				{
					dirName += @$"{Path.DirectorySeparatorChar}{subDirName}";
				}

				base.Directory = @dirName;
			}
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

		protected string SubDirectory => Type switch
		{
			PostType.STATS_DISEASED => "diseased",
			PostType.STATS_PNEUMONIA => "pneumonia",
			PostType.STATS_RECOVERED => "recovered",
			_ => ""
		};
	}
}
