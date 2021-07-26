using System;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	class Program
	{
		private readonly static InterfaceLogger InteractiveLogger = new ConsoleLogger();

		static void Main(string[] args)
		{
			InteractiveLogger.Log(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
		}
	}
}
