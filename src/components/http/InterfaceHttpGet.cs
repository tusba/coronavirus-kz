using System;

namespace Tusba.Components.Http
{
	public interface InterfaceHttpGet : IDisposable
	{
		string Get(string? url);
	}
}
