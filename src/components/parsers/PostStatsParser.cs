using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Components.Parsers
{
	public class PostStatsParser : InterfacePostStatsParser
	{
		public string RawContent { get; init; }

		public PostStatsParser(string rawContent) => RawContent = rawContent;

		public async Task<PostStats[]> Parse()
		{
			return await Task.Run(() => {
				var region = @"(\bгород\s+\S+\b)|(\b[^>]+\s+область\b)";
				var separator = @"\s*\W+\s*\D*";
				var quantity = @"\d+";

				var regExp = new Regex(@$"({region}){separator}({quantity})", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				var match = regExp.Match(RawContent);
				var entries = new List<PostStats>(20);

				while (match.Success)
				{
					var groups = match.Groups;
					if (groups.Count >= 4)
					{
						entries.Add(new PostStats(groups[1].Value, Int32.Parse(groups[4].Value)));
					}
					match = match.NextMatch();
				}

				return entries.ToArray();
			});
		}
	}
}
