#nullable disable

namespace Tusba.Patterns.Singleton
{
  public interface InterfaceSingleton<T> where T : class
  {
		static T Instance
		{
			get;
		}
  }
}
