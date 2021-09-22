using System;
using System.Threading.Tasks;
using Tusba.Patterns.Singleton;

namespace Tusba.Components.Logging
{
	public class ConsoleLogger : BaseSingleton<InterfaceLogger, ConsoleLogger>,
		InterfaceLogger
	{
		private ConsoleLogger()
		{
		}

		public async Task Log(object target)
		{
			await Task.Run(() => Console.WriteLine(target));
		}
	}
}
