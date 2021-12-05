namespace Tusba.Enumerations.Application
{
	public enum Action
	{
		// no specific action
		NONE,

		// fetch and store post HTML page
		FETCH_PAGE,

		// parse post HTML page,
		// get content of all posts,
		// filter out inappropriate posts,
		// store appropriate ones
		EXTRACT_POSTS,

		// parse HTML data from post statistics into a structured format
		PARSE_STATS
	}

	public static class ActionExtensions
	{
		public static Action ResolveApplicationAction(this string s) => s.ToLower() switch
		{
			"fetch" => Action.FETCH_PAGE,
			"extract" => Action.EXTRACT_POSTS,
			"parse" => Action.PARSE_STATS,
			_ => Action.NONE,
		};
	}
}
