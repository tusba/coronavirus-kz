using System.IO;
using Xunit;
using Tusba.Components.Parsers;
using Tusba.Models;

namespace test
{
	public class PostStatsParserTest : PostStatsTest
	{
		[Theory]
		[InlineData("diseased/2021-12-11.html", new string[] {
			"город Нур-Султан - 100",
			"город Алматы - 34",
			"город Шымкент - 3",
			"Акмолинская область - 58",
			"Актюбинская область - 7",
			"Алматинская область - 16",
			"Атырауская область - 4",
			"Восточно-Казахстанская область - 21",
			"Жамбылская область - 4",
			"Западно-Казахстанская область - 18",
			"Карагандинская область - 82",
			"Костанайская область - 58",
			"Кызылординская область - 5",
			"Павлодарская область - 73",
			"Северо-Казахстанская область - 77",
			"Туркестанская область - 8"
		})]
		[InlineData("recovered/2021-12-12.html", new string[] {
			"город Нур-Султан - 163",
			"город Алматы - 50",
			"город Шымкент - 15",
			"Акмолинская область - 83",
			"Алматинская область - 31",
			"Атырауская область - 3",
			"Восточно-Казахстанская область - 53",
			"Жамбылская область - 7",
			"Западно-Казахстанская область - 20",
			"Карагандинская область - 115",
			"Костанайская область - 5",
			"Кызылординская область - 14",
			"Мангистауская область - 3",
			"Павлодарская область - 12",
			"Северо-Казахстанская область - 54",
			"Туркестанская область - 3"
		})]
		public async void ParseTest(string fileName, string[] entries)
		{
			string fileDir = @"data/stats";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			string fileContent = await File.ReadAllTextAsync(filePath);

			InterfacePostStatsParser postStatsParser = PostStatsParserFactory.ExportableAsResult(fileContent);
			PostStats[] parsed = await postStatsParser.Parse();

			Assert.Equal(entries.Length, parsed.Length);

			PostStats[] expected = FromStrings(entries);

			Assert.Equal(expected, parsed);
		}
	}
}
