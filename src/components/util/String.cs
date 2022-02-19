namespace Tusba.Components.Util
{
	public class String
	{
		const char CHAR_BOM = '\uFEFF';

		public static string RemoveBom(string source)
		{
			return source.TrimStart(CHAR_BOM);
		}
	}
}
