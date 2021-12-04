using System.Threading.Tasks;

namespace Tusba.Components.Services.PostStats
{
	public interface InterfacePostStatsPersistService
	{
		Task<bool> Store();
	}
}
