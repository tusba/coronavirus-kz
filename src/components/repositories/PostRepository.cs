using System.IO;
using System.Threading.Tasks;
using Tusba.Components.Configuration;

namespace Tusba.Components.Repositories
{
	public class PostRepository : BaseRepository, InterfacePostRepository
	{
		private readonly string? postId;

		public PostRepository(string? postId)
		{
			this.postId = postId;
			CustomizeFileName();
		}

		public async Task<string> Fetch()
		{
			return await File.ReadAllTextAsync(FileName);
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

		public void ResolveFileName(string defaultFileName, string fileExtension)
		{
			string fileName = this.postId is null ? defaultFileName : this.postId;

			FileName = $"{fileName}.{fileExtension}";
		}

		protected void CustomizeFileName()
		{
			InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;
			ResolveFileName(Configuration.Get("post.file.name.default"), Configuration.Get("post.file.extension"));
		}
	}
}
