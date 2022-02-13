using Tusba.Models.Application;
using Tusba.Patterns.ChainOfResponsibility;

namespace Tusba.Components.Cli.Arguments
{
	public interface InterfaceArgumentResolver : InterfaceHandler<string[], Options>
	{
	}
}
