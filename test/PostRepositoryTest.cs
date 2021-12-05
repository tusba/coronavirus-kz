using System.IO;
using Xunit;
using Tusba.Components.FileSystem;
using Tusba.Components.Repositories.Post;

namespace test
{
	public class PostRepositoryTest : BaseTest
	{
		[Theory]
		[InlineData(null, false)]
		[InlineData("6362", true)]
		[InlineData("5251", false)]
		[InlineData("4140", true)]
		public async void StoreFetchTest(string? postId, bool overwrite)
		{
			string dirName = @"repo-test";

			CleanUp(dirName, true);
			FileStorage.ProvideDirectory(dirName);

			var repo = new PostRepository(postId);
			Assert.True(repo.Overwrite);
			repo.Directory = dirName;
			repo.DefaultFileName = "index";
			repo.FileExtenstion = "html";
			repo.Overwrite = overwrite;

			string filePath = repo.FileName;
			Assert.False(File.Exists(filePath));
			Assert.False(await repo.Exist());

			string content = $"Post ID is {(postId is null ? "null" : postId)}";
			Assert.True(await repo.Store(content));
			Assert.True(File.Exists(filePath));
			Assert.True(await repo.Exist());

			Assert.Equal(content, await repo.Fetch());

			// test overwriting
			string newContent = "Overwritten";
			Assert.True(await repo.Store(newContent));
			Assert.Equal(repo.Overwrite ? newContent : content, await repo.Fetch());

			CleanUp(dirName);
		}
	}
}
