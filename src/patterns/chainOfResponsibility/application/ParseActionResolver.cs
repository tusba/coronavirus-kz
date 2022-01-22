using System;
using Tusba.Models.Application;
using ApplicationAction = Tusba.Enumerations.Application.Action;

namespace Tusba.Patterns.ChainOfResponsibility.Application
{
	public class ParseActionResolver : BaseArgumentResolver
	{
		public ParseActionResolver(BaseArgumentResolver? next = null) : base(next)
		{
		}

		public override Options Handle(string[] query)
		{
			var (index, parameters) = DetectApplicationAction(query, ApplicationAction.PARSE_STATS);
			if (index is int actionIndex)
			{
				var options = new Options();
				options.Action = query[actionIndex];

				switch (parameters.Length)
				{
					case 0:
						options.SetDate();
						break;
					case 1:
						options.SetDate(parameters[0]);
						break;
					default:
						options.SetDate(parameters[0], parameters[1]);
						break;
				}

				return options;
			}

			return base.Handle(query);
		}
	}
}
