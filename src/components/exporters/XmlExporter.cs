using System.Threading.Tasks;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Exporters
{
	public class XmlExporter : InterfaceExporter<string>
	{
		public async Task<string> ExportPostStats(PostStats[] models)
		{
			return await Task.Run(() => "TODO XML");
		}
	}
}
