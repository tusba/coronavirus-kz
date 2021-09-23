using System.Threading.Tasks;

namespace Tusba.Components.Repositories
{
	public interface InterfacePostWriteRepository
	{
		Task<bool> Store(string content);
	}
}
