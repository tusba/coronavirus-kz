using System;

namespace Tusba.Components.Logging
{
	class ConsoleLogger : InterfaceLogger
	{
		public void Log(object target)
		{
			Console.WriteLine(target);
		}
	}
}
