using System.Threading.Tasks;

namespace Tusba.Components.Repositories
{
	public interface InterfacePostReadRepository
	{
		Task<string> Fetch();
	}
}
