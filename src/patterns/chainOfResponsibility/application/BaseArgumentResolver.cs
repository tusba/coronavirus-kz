using Tusba.Models.Application;

namespace Tusba.Patterns.ChainOfResponsibility.Application
{
	public abstract class BaseArgumentResolver : BaseHandler<string[], Options>, InterfaceArgumentResolver
	{
		protected override Options DefaultResult => new Options();

		public BaseArgumentResolver(BaseArgumentResolver? next = null) : base(next)
		{}
	}
}
