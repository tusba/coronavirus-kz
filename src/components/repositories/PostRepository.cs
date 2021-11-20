using Tusba.Components.Configuration;

namespace Tusba.Components.Repositories
{
	public class PostRepository : BasePostRepository
	{
		public override string FileName
		{
			get => $"{postId ?? DefaultFileName}.{FileExtenstion}";

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
