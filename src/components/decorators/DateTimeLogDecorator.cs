using System;
using System.Threading.Tasks;
using Tusba.Components.Logging;

namespace Tusba.Components.Decorators
{
	public class DateTimeLogDecorator : InterfaceLogger
	{
		private string dateTimeFormat = "yyyy.MM.dd HH:mm:ss.fff";
		private string logTemplate = "[%DT%]\t%LOG%";
		private readonly InterfaceLogger logger;

		public DateTimeLogDecorator(InterfaceLogger logger) => this.logger = logger;

		public async Task Log(object target)
		{
			string value = logTemplate.Replace("%LOG%", target.ToString());
			value = value.Replace("%DT%", DateTime.Now.ToString(dateTimeFormat));

			await logger.Log(value);
		}
	}
}
