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
			if (query.Length is < 2 or > 3)
			{
				return base.Handle(query);
			}

			var (index, parameters) = DetectApplicationAction(query, ApplicationAction.PARSE_STATS);
			if (index is int actionIndex)
			{
				var options = new Options();
				options.Action = query[actionIndex];

				if (parameters.Length > 1)
				{
					options.SetDate(parameters[0], parameters[1]);
				}
				else
				{
					options.SetDate(parameters[0]);
				}

				return options;
			}

			return base.Handle(query);
		}
	}
}
