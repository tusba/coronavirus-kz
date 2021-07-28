using System;
using Tusba.Components.Logging;

namespace CoronavirusKz
{
	public class Program
	{
		private readonly static InterfaceLogger InteractiveLogger = new ConsoleLogger();

		public static void Main(string[] args)
		{
			InteractiveLogger.Log(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
		}
	}
}
