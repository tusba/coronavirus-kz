using System;
using System.Linq;
using Tusba.Models;

namespace test
{
	public class PostStatsTest : BaseTest
	{
		protected PostStats[] FromStrings(string[] entries, string separator = " - ")
		{
			return entries
				.Select((string entry) => {
					var parts = entry.Split(separator);
					return new PostStats(parts[0], Int32.Parse(parts[1]));
				})
				.ToArray();
		}
	}
}
