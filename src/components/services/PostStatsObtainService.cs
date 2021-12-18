using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tusba.Enumerations.Post;
using Tusba.Models;
using PostType = Tusba.Enumerations.Post.Type;

namespace Tusba.Components.Services.PostStats
{
	public class PostStatsObtainService : BasePostStatsService, InterfacePostStatsObtainService
	{
		public PostType[] Types { get; set; } = new PostType[] {};
		public DateRange Dates { get; set; } = new DateRange();

		public PostStatsObtainService() : base()
		{
		}

		public async Task<bool> Fetch()
		{
			var dates = Dates.Range()
				.Select(date => date.ToString(DateRange.Format))
				.ToArray();
			var posts = new List<Post>(Types.Length * dates.Length);

			foreach (var type in Types)
			{
				foreach (var date in dates)
				{
					AdjustRepository(type, date);
					if (!(await repository.Exist()))
					{
						continue;
					}

					try
					{
						posts.Add(CreatePost(type, date, await repository.Fetch()));
					}
					catch
					{
						return false;
					}
				}
			}

			Posts = posts.ToArray();

			return true;
		}

		private Post CreatePost(PostType type, string date, string content)
		{
			return new Post(
				$"{type.AsString()}/{date}",
				date,
				content
			);
		}
	}
}
