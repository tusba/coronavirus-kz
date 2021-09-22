using System;

namespace Tusba.Patterns.Singleton
{
  /**
   * T is an instance type for declaration
   * U is an instance type for instantiation
   */
  public abstract class BaseSingleton<T, U> : InterfaceSingleton<T>
    where T : class
    where U : T
  {
		protected static readonly object sharedLock = new object(); // for thread safety
		protected static T? sharedInstance;

		public static T Instance
		{
			get
			{
				lock (sharedLock)
				{
					if (sharedInstance is null) {
            // true matches a public or non-public default constructor
						sharedInstance = Activator.CreateInstance(typeof(U), true) as T;
					}

					return sharedInstance!; // not null!
				}
			}
		}
  }
}
