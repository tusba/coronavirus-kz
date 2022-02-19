using System.IO;
using Xunit;
using Tusba.Components.Exporters;

namespace test
{
	public class XmlExporterTest : PostStatsTest
	{
		[Theory]
		[InlineData("empty.xml", new string[] {})]
		[InlineData("items-1.xml", new string[] { "Qwerty - 100500" })]
		[InlineData("items-2.xml", new string[] { "Qwerty - 100500", "CR - 7" })]
		[InlineData("items-3.xml", new string[] { "Qwerty - 100500", "CR - 7", "zero - 0" })]
		[InlineData("items-10.xml", new string[] {
			"one - 1", "two - 2", "three - 3", "four - 4", "five - 5",
			"six - 6", "seven - 7", "eight - 8", "nine - 9", "ten - 10"
		})]
		public async void ExportPostStatsReturnTest(string fileName, string[] entries)
		{
			string fileDir = @"data/xml";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			string fileContent = await File.ReadAllTextAsync(filePath);
			string xml = await new XmlExporter<string>().ExportPostStats(FromStrings(entries));

			Assert.Equal(fileContent, xml);
		}

		[Theory]
		[InlineData("empty.xml", new string[] {})]
		[InlineData("items-1.xml", new string[] { "Qwerty - 100500" })]
		[InlineData("items-2.xml", new string[] { "Qwerty - 100500", "CR - 7" })]
		[InlineData("items-3.xml", new string[] { "Qwerty - 100500", "CR - 7", "zero - 0" })]
		[InlineData("items-10.xml", new string[] {
			"one - 1", "two - 2", "three - 3", "four - 4", "five - 5",
			"six - 6", "seven - 7", "eight - 8", "nine - 9", "ten - 10"
		})]
		public async void ExportPostStatsStoreTest(string fileName, string[] entries)
		{
			bool stored = await new XmlExporter<bool>().ExportPostStats(FromStrings(entries));
			Assert.False(stored);
		}
	}
}
