using System;

namespace Tusba.Enumerations.Post
{
	public enum Type
	{
		// default type
		COMMON,

		// statistics on diseased people
		STATS_DISEASED,

		// statistics on cases of pneumonia
		STATS_PNEUMONIA,

		// statistics on recovered people
		STATS_RECOVERED
	}

	public static class TypeExtensions
	{
		public static string AsString(this Type t) => t switch
		{
			Type.STATS_DISEASED => "diseased",
			Type.STATS_PNEUMONIA => "pneumonia",
			Type.STATS_RECOVERED => "recovered",
			_ => ""
		};

		public static Type ResolvePostType(this string s)
		{
			var matcher = new PostMatcher(s);

			if (matcher.MatchPeopleStats() && matcher.MatchPeopleRecoveredStats())
			{
				return Type.STATS_RECOVERED;
			}

			if (matcher.MatchPeopleStats() && matcher.MatchPeopleDiseasedStats())
			{
				return Type.STATS_DISEASED;
			}

			if (matcher.MatchPeoplePneumoniaStats())
			{
				return Type.STATS_PNEUMONIA;
			}

			return Type.COMMON;
		}

		private struct PostMatcher
		{
			private string Content { get; init; }

			public PostMatcher(string content) => Content = content;

			public bool MatchPeopleStats() => Content.Contains(
				"За прошедшие сутки в Казахстане",
				StringComparison.OrdinalIgnoreCase
			);

			public bool MatchPeopleRecoveredStats() => Content.Contains(
				"выздорове",
				StringComparison.OrdinalIgnoreCase
			);

			public bool MatchPeopleDiseasedStats() => Content.Contains(
				"заболевши",
				StringComparison.OrdinalIgnoreCase
			);

			public bool MatchPeoplePneumoniaStats() => Content.Contains(
				"пневмонией с признаками коронавирусной инфекции",
				StringComparison.OrdinalIgnoreCase
			);
		}
	}
}
