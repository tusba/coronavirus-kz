using System.IO;
using Xunit;
using Tusba.Components.Parsers;

namespace test
{
	public class PostParserTest : BaseTest
	{
		[Theory]
		[InlineData(
			"post-page.html",
			new string[] {
				"post#5646",
				"post#5647",
				"post#5648",
				"post#5649",
				"post#5651",
				"post#5656",
				"post#5657",
				"post#5658",
				"post#5659",
				"post#5660",
				"post#5661",
				"post#5664",
				"post#5666"
			}
		)]
		public async void ParseTest(string fileName, string[] postContent)
		{
			string fileDir = @"data/html";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			string fileContent = await File.ReadAllTextAsync(filePath);
			InterfacePostParser postParser = new PostParser(fileContent);

			Assert.Equal(postContent, await postParser.Parse());
		}
	}
}
