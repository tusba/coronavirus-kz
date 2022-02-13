using System.Threading.Tasks;
using System.Xml.Linq;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Exporters
{
	public class XmlExporter : InterfaceExporter<string>
	{
		private const string ELEMENT_ROOT = "PostStats";
		private const string ELEMENT_NODE = "Entry";
		private const string ELEMENT_REGION = "Region";
		private const string ELEMENT_QUANTITY = "Quantity";

		public async Task<string> ExportPostStats(PostStats[] models)
		{
			return await Task.Run(() => {
				var root = new XElement(ELEMENT_ROOT);

				foreach (var model in models)
				{
					root.Add(new XElement(ELEMENT_NODE,
						new XElement(ELEMENT_REGION, model.Region),
						new XElement(ELEMENT_QUANTITY, model.Quantity)
					));
				}

				return root.ToString(SaveOptions.DisableFormatting);
			});
		}
	}
}
