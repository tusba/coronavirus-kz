using System.IO;
using System.Threading.Tasks;

namespace Tusba.Components.Repositories
{
	public class PostRepository : BaseRepository, InterfacePostRepository
	{
		private const string DEFAULT_DATA_FILE_NAME = "index.html";
		private readonly string? postId;

		public PostRepository(string? postId)
		{
			this.postId = postId;
			FileName = this.postId is null ? DEFAULT_DATA_FILE_NAME : $"{this.postId}.html";
		}

		public async Task<string> Fetch()
		{
			return await Task.Run(() => "TODO fetch post" + (postId is null ? "" : $"#{postId}"));
		}

		public async Task<bool> Store(string content)
		{
			try {
				await File.WriteAllTextAsync(FileName, content);
				return true;
			} catch {
				return false;
			}
		}
	}
}
