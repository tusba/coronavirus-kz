using System;
using System.IO;
using Xunit;
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
	}
}
