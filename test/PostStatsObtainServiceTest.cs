using System.Linq;
using Xunit;
using Tusba.Components.Services.PostStats;
using Tusba.Models;
using PostType = Tusba.Enumerations.Post.Type;

namespace test
{
	public class PostStatsObtainServiceTest : BaseTest
	{
		[Theory]
		[InlineData(new PostType[] {}, "2021-12-17", "2021-12-17", new string[] {})]
		public async void FetchTest(PostType[] types, string date, string boundary, string[] postIds)
		{
			var service = new PostStatsObtainService();

			service.Types = types;
			service.Dates = new DateRange(date, boundary);

			Assert.True(await service.Fetch());
			Assert.Equal(postIds, service.Posts.Select(post => post.Id).ToArray());
		}
	}
}
