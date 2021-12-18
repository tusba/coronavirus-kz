using System.Linq;
using System.Reflection;
using Xunit;
using Tusba.Components.Repositories.Post;
using Tusba.Components.Services.PostStats;
using Tusba.Models;
using PostType = Tusba.Enumerations.Post.Type;

namespace test
{
	public class PostStatsObtainServiceTest : BaseTest
	{
		[Theory]
		[InlineData(new PostType[] {}, "2021-12-17", "2021-12-17", new string[] {})]
		[InlineData(new PostType[] { PostType.STATS_PNEUMONIA }, "2021-12-14", "2021-12-20", new string[] {
			"pneumonia/2021-12-16", "pneumonia/2021-12-17", "pneumonia/2021-12-18"
		})]
		[InlineData(
			new PostType[] { PostType.STATS_DISEASED, PostType.STATS_RECOVERED },
			"2021-12-13",
			"2021-12-18",
			new string[] { "diseased/2021-12-13", "diseased/2021-12-17", "recovered/2021-12-16", "recovered/2021-12-17" }
		)]
		[InlineData(
			new PostType[] { PostType.STATS_PNEUMONIA, PostType.STATS_RECOVERED, PostType.STATS_DISEASED },
			"2021-12-16",
			"2021-12-17",
			new string[] {
				"pneumonia/2021-12-16",
				"pneumonia/2021-12-17",
				"recovered/2021-12-16",
				"recovered/2021-12-17",
				"diseased/2021-12-17"
			}
		)]
		public async void FetchTest(PostType[] types, string date, string boundary, string[] postIds)
		{
			var service = new PostStatsObtainService();

			var repo = (PostStatsRepository?) typeof(PostStatsObtainService)
				.GetField("repository", BindingFlags.Instance | BindingFlags.NonPublic)
				?.GetValue(service);
			if (repo is not null)
			{
				repo.FileExtenstion = "html";
			}

			service.Directory = @"test/data/stats";
			service.Types = types;
			service.Dates = new DateRange(date, boundary);

			Assert.True(await service.Fetch());
			Assert.Equal(postIds, service.Posts.Select(post => post.Id).ToArray());
		}
	}
}
