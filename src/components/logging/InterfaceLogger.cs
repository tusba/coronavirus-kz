using System.Threading.Tasks;

namespace Tusba.Components.Logging
{
	public interface InterfaceLogger
	{
		Task Log(object target);
	}
}
