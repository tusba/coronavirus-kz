namespace Tusba.Patterns.ChainOfResponsibility
{
	public interface InterfaceHandler<Q, R>
	{
		R? handle(Q query);
	}
}
