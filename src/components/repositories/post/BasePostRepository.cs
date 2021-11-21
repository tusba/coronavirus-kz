using System.IO;
using System.Threading.Tasks;

namespace Tusba.Components.Repositories.Post
{
	public abstract class BasePostRepository : BaseRepository, InterfacePostRepository
	{
		public string? DefaultFileName { get; set; }

		public string? FileExtenstion { get; set; }

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
