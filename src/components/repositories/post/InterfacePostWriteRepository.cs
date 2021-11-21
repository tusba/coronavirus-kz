using System.Threading.Tasks;

namespace Tusba.Components.Repositories.Post
{
	public interface InterfacePostWriteRepository
	{
		Task<bool> Store(string content);
	}
}
