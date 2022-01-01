namespace Tusba.Patterns.FactoryMethod
{
	public interface InterfaceFactoryMethod<T> where T : class
	{
		T FactoryInstance { get; }
	}
}
