using System;
using PostType = Tusba.Enumerations.Post.Type;

namespace Tusba.Components.Repositories.Post
{
	public class PostStatsRepository : BasePostRepository
	{
		public PostType Type { get; set; }

		public string? Date { get; set; }

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

		public PostStatsRepository(PostType type, string date) : this()
		{
			Type = type;
			Date = date;
		}
	}
}
