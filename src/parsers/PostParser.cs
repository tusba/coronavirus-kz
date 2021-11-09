using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Tusba.Models;

namespace Tusba.Components.Parsers
{
	public class PostParser : InterfacePostParser
	{
		public string RawContent { get; init; }

		public PostParser(string rawContent) => RawContent = rawContent;

		public async Task<Post[]> Parse()
		{
			return await Task.Run(() => {
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(RawContent);

				string postAttr = "data-post";
				string dateTimeAttr = "datetime";

				return htmlDoc.DocumentNode
					.SelectNodes($"//*[@{postAttr}]//*[contains(@class, 'tgme_widget_message_text')]")
					.Select(node => {
						var postNode = node;
						while (postNode.GetAttributeValue(postAttr, "").Equals(""))
						{
							postNode = postNode.ParentNode;
						}

						var timeNode = postNode.SelectSingleNode($".//*[@{dateTimeAttr}]");

						string id = postNode
							.GetAttributeValue(postAttr, "")
							.Replace("coronavirus2020_kz/", "");

						string dateTime = timeNode.GetAttributeValue(dateTimeAttr, "");

						string content = node.InnerHtml;

						return new Post(id, dateTime, content);
					})
					.ToArray();
			});
		}
	}
}
