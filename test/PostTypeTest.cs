using System.IO;
using System.Threading.Tasks;
using Xunit;
using Tusba.Enumerations.Post;

namespace test
{
  public class PostTypeTest : BaseTest
  {
    [Theory]
    [InlineData("common-1.html")]
    [InlineData("common-2.html")]
    [InlineData("empty.txt")]
    [InlineData("status-1.html")]
    public async void CommonResolveTest(string fileName)
    {
			string type = await FileContent(fileName);
      Assert.Equal(Type.COMMON, type.ResolvePostType());
    }

    [Theory]
    [InlineData("stats-diseased-1.html")]
    [InlineData("stats-diseased-2.html")]
    public async void StatsDiseasedResolveTest(string fileName)
    {
			string type = await FileContent(fileName);
      Assert.Equal(Type.STATS_DISEASED, type.ResolvePostType());
    }

    [Theory]
    [InlineData("stats-pneumonia-1.html")]
    [InlineData("stats-pneumonia-2.html")]
    public async void StatsPneumoniaResolveTest(string fileName)
    {
			string type = await FileContent(fileName);
      Assert.Equal(Type.STATS_PNEUMONIA, type.ResolvePostType());
    }

    [Theory]
    [InlineData("stats-recovered-1.html")]
    [InlineData("stats-recovered-2.html")]
    public async void StatsRecoveredResolveTest(string fileName)
    {
			string type = await FileContent(fileName);
      Assert.Equal(Type.STATS_RECOVERED, type.ResolvePostType());
    }

		private async Task<string> FileContent(string fileName)
		{
			string fileDir = @"data/posts";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			return await File.ReadAllTextAsync(filePath);
		}
  }
}
