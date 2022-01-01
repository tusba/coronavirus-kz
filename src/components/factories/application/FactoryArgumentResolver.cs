using Tusba.Patterns.ChainOfResponsibility.Application;
using Tusba.Patterns.FactoryMethod;

namespace Tusba.Components.Factories.Application
{
	public class FactoryArgumentResolver : InterfaceFactoryMethod<InterfaceArgumentResolver>
	{
		public InterfaceArgumentResolver FactoryInstance => new SingleArgumentResolver();
	}
}
