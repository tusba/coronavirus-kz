using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Tusba.Components.Parsers
{
	public class PostParser : InterfacePostParser
	{
		public string RawContent { get; init; }

		public PostParser(string rawContent) => RawContent = rawContent;

		public async Task<string[]> Parse()
		{
			return await Task.Run(() => {
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(RawContent);

				return htmlDoc.DocumentNode
					.SelectNodes("//*[@data-post]//*[contains(@class,'tgme_widget_message_text')]")
					.Select(node => node.InnerHtml)
					.ToArray();
			});
		}
	}
}
