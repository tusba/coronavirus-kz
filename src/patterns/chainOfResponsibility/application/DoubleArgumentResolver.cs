using Tusba.Models.Application;

namespace Tusba.Patterns.ChainOfResponsibility.Application
{
	public class DoubleArgumentResolver : BaseArgumentResolver
	{
		public DoubleArgumentResolver(BaseArgumentResolver? next = null) : base(next)
		{
		}

		public override Options Handle(string[] query)
		{
			if (query.Length != 2)
			{
				return base.Handle(query);
			}

			var options = new Options();

			(options.PostId, options.Action) = IsPostId(query[0])
				? (query[0], query[1])
				: (query[1], query[0]);

			return options;
		}
	}
}
