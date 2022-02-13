using Tusba.Components.Cli.Arguments;
using Tusba.Patterns.FactoryMethod;

namespace Tusba.Components.Factories.Application
{
	public class FactoryArgumentResolver : InterfaceFactoryMethod<InterfaceArgumentResolver>
	{
		public InterfaceArgumentResolver FactoryInstance =>
			new ParseActionResolver(
				new SingleArgumentResolver(
					new DoubleArgumentResolver()
				)
			);
	}
}
