using System;
using System.Threading.Tasks;

namespace Tusba.Components.Logging
{
	public class ConsoleLogger : InterfaceLogger
	{
		public async Task Log(object target)
		{
			await Task.Run(() => Console.WriteLine(target));
		}
	}
}
