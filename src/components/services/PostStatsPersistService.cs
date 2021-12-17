using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Components.Services.PostStats
{
	public class PostStatsPersistService : BasePostStatsService, InterfacePostStatsPersistService
	{
		public PostStatsPersistService() : base()
		{
		}

		public PostStatsPersistService SetPosts(Post[] posts)
		{
			Posts = posts;
			return this;
		}

		public async Task<bool> Store()
		{
			foreach (var post in Posts)
			{
				AdjustRepository(post);

				if (!(await repository.Store(post.Content)))
				{
					return false;
				}
			}

			return true;
		}
	}
}
