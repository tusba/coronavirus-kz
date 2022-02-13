using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Patterns.Visitor.Export
{
	public interface InterfaceExporter<T> : InterfaceVisitor
	{
		Task<T> ExportPostStats(PostStats[] models);
	}
}
