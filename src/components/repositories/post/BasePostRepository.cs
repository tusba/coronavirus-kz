using System.IO;
using System.Threading.Tasks;
using Tusba.Components.Configuration;

namespace Tusba.Components.Repositories.Post
{
	public abstract class BasePostRepository : BaseRepository, InterfacePostRepository
	{
		public string? DefaultFileName { get; set; }

		public string? FileExtenstion { get; set; }

		protected BasePostRepository()
		{
			FileExtenstion = SystemConfiguration.Instance.Get("post.file.extension");
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
	}
}
