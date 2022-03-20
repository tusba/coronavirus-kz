using System.IO;
using Xunit;
using Tusba.Components.Exporters;

namespace test
{
	public class JsonExporterTest : PostStatsTest
	{
		[Theory]
		[InlineData("empty.json", new string[] {})]
		[InlineData("items-1.json", new string[] { "Qwerty - 100500" })]
		[InlineData("items-2.json", new string[] { "Qwerty - 100500", "CR - 7" })]
		[InlineData("items-3.json", new string[] { "Qwerty - 100500", "CR - 7", "zero - 0" })]
		[InlineData("items-10.json", new string[] {
			"one - 1", "two - 2", "three - 3", "four - 4", "five - 5",
			"six - 6", "seven - 7", "eight - 8", "nine - 9", "ten - 10"
		})]
		public async void ExportPostStatsReturnTest(string fileName, string[] entries)
		{
			string fileDir = @"data/json";
			string filePath = Path.Combine(testDir, fileDir, fileName);

			string fileContent = await File.ReadAllTextAsync(filePath);
			string json = await new JsonExporter<string>().ExportPostStats(FromStrings(entries));

			Assert.Equal(fileContent, json);
		}

		[Theory]
		[InlineData("empty.json", new string[] {})]
		[InlineData("items-1.json", new string[] { "Qwerty - 100500" })]
		[InlineData("items-2.json", new string[] { "Qwerty - 100500", "CR - 7" })]
		[InlineData("items-3.json", new string[] { "Qwerty - 100500", "CR - 7", "zero - 0" })]
		[InlineData("items-10.json", new string[] {
			"one - 1", "two - 2", "three - 3", "four - 4", "five - 5",
			"six - 6", "seven - 7", "eight - 8", "nine - 9", "ten - 10"
		})]
		public async void ExportPostStatsStoreTest(string fileName, string[] entries)
		{
			string exportTestDir = @"data/json/export";
			string exportDir = Path.Combine(testDir, exportTestDir);

			CleanUp(exportDir, true);

			var exporter = new JsonExporter<bool>
			{
				Directory = exportDir,
				FileName = fileName
			};

			bool stored = await exporter.ExportPostStats(FromStrings(entries));
			Assert.True(stored);

			string filePath = Path.Combine(exportDir, fileName);
			Assert.True(File.Exists(filePath));

			CleanUp(exportDir);
		}
	}
}
