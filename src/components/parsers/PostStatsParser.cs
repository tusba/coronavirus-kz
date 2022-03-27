using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Parsers
{
	public class PostStatsParser<T> : InterfacePostStatsParser, InterfaceExportable<T>
	{
		private const string REGEX_PATTERN_REGION = @"(\bгород\s+\S+\b)|(\b[^>]+\s+область\b)";
		private const string REGEX_PATTERN_SEPARATOR = @"\s*\W+\s*\D*";
		private const string REGEX_PATTERN_QUANTITY = @"\d+";

		private static readonly Regex RegExp = new Regex(
			@$"({REGEX_PATTERN_REGION}){REGEX_PATTERN_SEPARATOR}({REGEX_PATTERN_QUANTITY})",
			RegexOptions.IgnoreCase | RegexOptions.Multiline
		);

		public string RawContent { get; init; }

		public PostStatsParser(string rawContent) => RawContent = rawContent;

		public async Task<PostStats[]> Parse()
		{
			return await Task.Run(() => {
				var match = RegExp.Match(RawContent);
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

		public async Task<T> ReceiveExporter(InterfaceExporter<T> exporter)
		{
			return await exporter.ExportPostStats(await Parse());
		}
	}
}
