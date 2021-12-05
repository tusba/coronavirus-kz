using System.Threading.Tasks;

namespace Tusba.Components.Repositories.Post
{
	public interface InterfacePostExistRepository
	{
		Task<bool> Exist();
	}
}
