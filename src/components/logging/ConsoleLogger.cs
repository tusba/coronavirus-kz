using System;

namespace Tusba.Components.Logging
{
	public class ConsoleLogger : InterfaceLogger
	{
		public void Log(object target)
		{
			Console.WriteLine(target);
		}
	}
}
