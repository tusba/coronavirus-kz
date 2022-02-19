namespace Tusba.Components.Parsers
{
	public class PostStatsParserFactory
	{
		public static PostStatsParser<string> ExportableAsResult(string rawContent)
		{
			return new PostStatsParser<string>(rawContent);
		}

		public static PostStatsParser<bool> ExportableToStorage(string rawContent)
		{
			return new PostStatsParser<bool>(rawContent);
		}
	}
}
