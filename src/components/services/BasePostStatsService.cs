using Tusba.Components.Repositories.Post;
using Tusba.Models;

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
	}
}
