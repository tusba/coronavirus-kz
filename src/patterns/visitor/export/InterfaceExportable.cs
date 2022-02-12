using System.Threading.Tasks;

namespace Tusba.Patterns.Visitor.Export
{
	public interface InterfaceExportable : InterfaceVisitorReceiver
	{
		Task<bool> ReceiveExporter(InterfaceExporter exporter);
	}
}
