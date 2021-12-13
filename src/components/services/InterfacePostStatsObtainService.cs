using System.Threading.Tasks;

namespace Tusba.Components.Services.PostStats
{
	public interface InterfacePostStatsObtainService
	{
		Task<bool> Fetch();
	}
}
