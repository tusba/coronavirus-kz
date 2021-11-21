using Tusba.Components.Configuration;

namespace Tusba.Components.Repositories.Post
{
	public class PostRepository : BasePostRepository
	{
		public override string FileName
		{
			get
			{
				base.FileName = $"{postId ?? DefaultFileName}.{FileExtenstion}";
				return base.FileName;
			}

			set => base.FileName = value;
		}

		protected readonly string? postId;

		public PostRepository(string? postId)
		{
			this.postId = postId;
			Configure();
		}

		protected void Configure()
		{
			InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;

			DefaultFileName = Configuration.Get("post.file.name.default");
			FileExtenstion = Configuration.Get("post.file.extension");
		}
	}
}
