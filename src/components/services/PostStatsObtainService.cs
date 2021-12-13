using System.Threading.Tasks;

namespace Tusba.Components.Services.PostStats
{
	public class PostStatsObtainService : BasePostStatsService, InterfacePostStatsObtainService
	{
		public PostStatsObtainService() : base()
		{
		}

		public async Task<bool> Fetch()
		{
			return await Task.Run(() => false);
		}
	}
}
