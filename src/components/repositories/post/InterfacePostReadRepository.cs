using System.Threading.Tasks;

namespace Tusba.Components.Repositories.Post
{
	public interface InterfacePostReadRepository
	{
		Task<string> Fetch();
	}
}
