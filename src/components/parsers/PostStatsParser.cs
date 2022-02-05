using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Components.Parsers
{
	public class PostStatsParser : InterfacePostStatsParser
	{
		public async Task<PostStats[]> Parse()
		{
			return await Task.Run(() => new PostStats[] {});
		}
	}
}
