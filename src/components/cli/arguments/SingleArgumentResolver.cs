using Tusba.Models.Application;

namespace Tusba.Components.Cli.Arguments
{
	public class SingleArgumentResolver : BaseArgumentResolver
	{
		public SingleArgumentResolver(BaseArgumentResolver? next = null) : base(next)
		{
		}

		public override Options Handle(string[] query)
		{
			if (query.Length != 1)
			{
				return base.Handle(query);
			}

			string arg = query[0];
			var options = new Options();

			if (IsPostId(arg))
			{
				options.PostId = arg;
			}
			else
			{
				options.Action = arg;
			}

			return options;
		}
	}
}
