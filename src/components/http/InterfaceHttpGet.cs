namespace Tusba.Components.Http
{
	public interface InterfaceHttpGet : InterfaceHttpRequest
	{
		string Get(string? url);
	}
}
