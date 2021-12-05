using System.IO;
using System.Threading.Tasks;
using Tusba.Components.Configuration;

namespace Tusba.Components.Repositories.Post
{
	public abstract class BasePostRepository : BaseRepository, InterfacePostRepository
	{
		public string? DefaultFileName { get; set; }

		public string FileExtenstion { get; set; }

		public bool Overwrite { get; set; }

		protected BasePostRepository()
		{
			var configuration = SystemConfiguration.Instance;

			FileExtenstion = configuration.Get("post.file.extension");
			Overwrite = ResolveOverwrite(configuration.Get("post.file.overwrite"));
		}

		public async Task<string> Fetch()
		{
			return await File.ReadAllTextAsync(FileName);
		}

		public virtual async Task<bool> Store(string content)
		{
			if (!Overwrite && await Exist())
			{
				return true;
			}

			try
			{
				await File.WriteAllTextAsync(FileName, content);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> Exist()
		{
			return await Task.Run(() => File.Exists(FileName));
		}

		protected bool ResolveOverwrite(string value) => value.ToLower() switch
		{
			"false" or "0" or "no" => false,
			_ => true
		};
	}
}
