using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Patterns.Visitor.Export
{
	public interface InterfaceExporter : InterfaceVisitor
	{
		Task<bool> ExportPostStats(PostStats[] models);
	}
}
