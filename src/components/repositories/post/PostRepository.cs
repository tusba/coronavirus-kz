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

		public PostRepository(string? postId) : base()
		{
			this.postId = postId;
			DefaultFileName = SystemConfiguration.Instance.Get("post.file.name.default");
		}
	}
}
