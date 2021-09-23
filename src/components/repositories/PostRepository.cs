using System.Threading.Tasks;

namespace Tusba.Components.Repositories
{
	public class PostRepository : BaseRepository, InterfacePostRepository
	{
		private readonly string? postId;

		public PostRepository(string? postId)
		{
			this.postId = postId;
		}

		public async Task<string> Fetch()
		{
			return await Task.Run(() => "TODO fetch post" + (postId is null ? "" : $"#{postId}"));
		}

		public async Task<bool> Store(string content)
		{
			return await Task.Run(() => false);
		}
	}
}
