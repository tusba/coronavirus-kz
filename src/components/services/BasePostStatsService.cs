using System;
using Tusba.Components.Repositories.Post;
using Tusba.Models;
using PostType = Tusba.Enumerations.Post.Type;

namespace Tusba.Components.Services.PostStats
{
	public abstract class BasePostStatsService
	{
		public Post[] Posts { get; protected set; }

		public string? Directory { get; set; }

		protected InterfacePostRepository repository;

		public BasePostStatsService()
		{
			Posts = new Post[] {};
			repository = new PostStatsRepository();
		}

		protected void AdjustRepository(PostType type, string date)
		{
			var repo = (PostStatsRepository) repository;

			repo.Type = type;
			repo.Date = date;

			if (!String.IsNullOrEmpty(Directory))
			{
				repo.Directory = Directory;
			}
		}

		protected void AdjustRepository(Post post)
		{
			AdjustRepository(post.Type, post.Date);
		}
	}
}
