using System.Threading.Tasks;

namespace Tusba.Patterns.Visitor.Export
{
	public interface InterfaceExportable<T> : InterfaceVisitorReceiver
	{
		Task<T> ReceiveExporter(InterfaceExporter<T> exporter);
	}
}
