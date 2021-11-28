using System.IO;
using Xunit;
using Tusba.Components.FileSystem;
using Tusba.Components.Repositories.Post;

namespace test
{
	public class PostRepositoryTest : BaseTest
	{
		[Theory]
		[InlineData(null)]
		[InlineData("5251")]
		[InlineData("4140")]
		public async void StoreFetchTest(string? postId)
		{
			string dirName = @"repo-test";

			CleanUp(dirName, true);
			FileStorage.ProvideDirectory(dirName);

			var repo = new PostRepository(postId);
			repo.Directory = dirName;
			repo.DefaultFileName = "index";
			repo.FileExtenstion = "html";

			string filePath = repo.FileName;
			Assert.False(File.Exists(filePath));

			string content = $"Post ID is {(postId is null ? "null" : postId)}";
			Assert.True(await repo.Store(content));
			Assert.True(File.Exists(filePath));

			Assert.Equal(content, await repo.Fetch());

			CleanUp(dirName);
		}
	}
}
