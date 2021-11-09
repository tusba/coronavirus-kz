using System.IO;
using System.Linq;
using Xunit;
using Tusba.Components.Parsers;
using Tusba.Models;

namespace test
{
	public class PostParserTest : BaseTest
	{
		[Fact]
		public async void ParseTest()
		{
			string fileDir = @"data/html";
			string fileName = "post-page.html";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			string fileContent = await File.ReadAllTextAsync(filePath);
			InterfacePostParser postParser = new PostParser(fileContent);

			Assert.Equal(getMockPosts(), await postParser.Parse());
		}

		private Post[] getMockPosts()
		{
			var rawData = new (string, string)[] {
				("5646", "2021-09-22T12:18:09+00:00"),
				("5647", "2021-09-23T02:00:00+00:00"),
				("5648", "2021-09-23T02:15:00+00:00"),
				("5649", "2021-09-23T02:25:00+00:00"),
				("5651", "2021-09-23T03:20:06+00:00"),
				("5656", "2021-09-24T02:00:00+00:00"),
				("5657", "2021-09-24T02:15:00+00:00"),
				("5658", "2021-09-24T02:36:27+00:00"),
				("5659", "2021-09-24T03:04:27+00:00"),
				("5660", "2021-09-24T03:05:06+00:00"),
				("5661", "2021-09-24T03:45:15+00:00"),
				("5664", "2021-09-24T06:56:32+00:00"),
				("5666", "2021-09-24T09:10:14+00:00")
			};

			return rawData
				.Select(postData => new Post(postData.Item1, postData.Item2, $"post#{postData.Item1}"))
				.ToArray();
		}
	}
}
