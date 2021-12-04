using System;
using System.Threading.Tasks;
using Tusba.Components.Repositories.Post;
using Tusba.Models;

namespace Tusba.Components.Services.PostStats
{
	public class PostStatsPersistService : InterfacePostStatsPersistService
	{
		public Post[] Posts { get; set; }

		public string? Directory { get; set; }

		private InterfacePostRepository repository;

		public PostStatsPersistService()
		{
			Posts = new Post[] {};
			repository = new PostStatsRepository();
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

		private void AdjustRepository(Post post)
		{
			var repo = (PostStatsRepository) repository;

			repo.Type = post.Type;
			repo.Date = post.Date;

			if (!String.IsNullOrEmpty(Directory))
			{
				repo.Directory = Directory;
			}
		}
	}
}
