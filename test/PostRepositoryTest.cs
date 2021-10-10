using System;
using System.IO;
using Xunit;
using Tusba.Components.FileSystem;
using Tusba.Components.Repositories;

namespace test
{
	public class PostRepositoryTest
	{
		// project's root directory
		private readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");

		[Theory]
		[InlineData(null)]
		[InlineData("5251")]
		[InlineData("4140")]
		public async void StoreFetchTest(string? postId)
		{
			string dirName = @"repo-test";
			string dirPath = Path.Combine(baseDir, dirName);

			if (Directory.Exists(dirPath))
			{
				CleanUp(dirPath);
			}

			FileStorage.ProvideDirectory(dirName);
			var repo = new PostRepository(postId);
			repo.Directory = dirName;

			string filePath = repo.FileName;
			Assert.False(File.Exists(filePath));

			string content = $"Post ID is {(postId is null ? "null" : postId)}";
			Assert.True(await repo.Store(content));
			Assert.True(File.Exists(filePath));

			Assert.Equal(content, await repo.Fetch());

			CleanUp(dirPath);
		}

		private void CleanUp(string directoryPath)
		{
			Directory.Delete(directoryPath, true);
		}
	}
}
