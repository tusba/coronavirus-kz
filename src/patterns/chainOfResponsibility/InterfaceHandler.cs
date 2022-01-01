namespace Tusba.Patterns.ChainOfResponsibility
{
	public interface InterfaceHandler<Q, R>
	{
		R? Handle(Q query);
	}
}
