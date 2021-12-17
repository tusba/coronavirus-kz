using System.Threading.Tasks;

namespace Tusba.Components.Http
{
	public interface InterfaceHttpGet : InterfaceHttpRequest
	{
		Task<string> Get(string? url = null);
	}
}
