using System;
using System.IO;
using Xunit;
using Tusba.Components.FileSystem;
using Tusba.Components.Repositories.Post;
using PostType = Tusba.Enumerations.Post.Type;

namespace test
{
	public class PostStatsRepositoryTest : BaseTest
	{
		[Theory]
		[InlineData(PostType.COMMON, @"")]
		[InlineData(PostType.STATS_DISEASED, @"diseased")]
		[InlineData(PostType.STATS_PNEUMONIA, @"pneumonia")]
		[InlineData(PostType.STATS_RECOVERED, @"recovered")]
		public void DirectoryNameTest(PostType type, string subDirectory)
		{
			string baseDirName = @"stats-test";

			var repo = new PostStatsRepository(type);
			repo.Directory = baseDirName;

			string resultDirName = baseDirName;
			if (!String.IsNullOrEmpty(subDirectory))
			{
				resultDirName += $@"{Path.DirectorySeparatorChar}{subDirectory}";
			}

			Assert.EndsWith(@resultDirName, repo.Directory);
		}

		[Theory]
		[InlineData(PostType.COMMON, @"", null, "html")]
		[InlineData(PostType.STATS_PNEUMONIA, @"pneumonia", null, "txt")]
		[InlineData(PostType.COMMON, @"", "2021-07-21", "log")]
		[InlineData(PostType.STATS_PNEUMONIA, @"pneumonia", "2021-07-21", "htm")]
		public void FileNameTest(PostType type, string subDirectory, string? date, string fileExtenstion)
		{
			string baseDirName = @"stats-test";
			string fileName = date ?? DateTime.Now.ToString("yyyy-MM-dd");

			var repo = new PostStatsRepository(type, date);
			repo.FileExtenstion = fileExtenstion;
			repo.Directory = baseDirName;

			string resultFileName = baseDirName;
			if (!String.IsNullOrEmpty(subDirectory))
			{
				resultFileName += $@"{Path.DirectorySeparatorChar}{subDirectory}";
			}
			resultFileName += $@"{Path.DirectorySeparatorChar}{fileName}.{fileExtenstion}";

			Assert.EndsWith(resultFileName, repo.FileName);
		}

		[Theory]
		[InlineData(PostType.COMMON, null, "just a text")]
		[InlineData(PostType.STATS_PNEUMONIA, "2021-07-21", "statistics on cases of pneumonia")]
		public async void StoreFetchTest(PostType type, string? date, string content)
		{
			string dirName = @"stats-test";

			CleanUp(dirName, true);
			FileStorage.ProvideDirectory(dirName);

			var repo = new PostStatsRepository(type, date);
			repo.Directory = dirName;
			repo.FileExtenstion = "html";

			Assert.True(await repo.Store(content));
			Assert.Equal(content, await repo.Fetch());

			CleanUp(dirName);
		}
	}
}
