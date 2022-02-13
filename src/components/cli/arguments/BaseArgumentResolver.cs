using System;
using Tusba.Enumerations.Application;
using Tusba.Models.Application;
using Tusba.Patterns.ChainOfResponsibility;
using ApplicationAction = Tusba.Enumerations.Application.Action;

namespace Tusba.Components.Cli.Arguments
{
	public abstract class BaseArgumentResolver : BaseHandler<string[], Options>, InterfaceArgumentResolver
	{
		protected override Options DefaultResult => new Options();

		public BaseArgumentResolver(BaseArgumentResolver? next = null) : base(next)
		{
		}

		/**
		 * @return index of found action (or null if not found) and array of action's parameters
		 */
		protected static (int? Index, string[] Parameters) DetectApplicationAction(string[] args, ApplicationAction? action = null)
		{
			int actionIndex = Array.FindIndex(args, arg => {
				var resolvedAction = arg.ResolveApplicationAction();

				return action is null
					? resolvedAction != ApplicationAction.NONE
					: resolvedAction == action;
			});

			if (actionIndex == -1)
			{
				return (null, args);
			}

			int argCount = args.Length;
			string[] parameters = new string[argCount - 1];

			for (int i = 0; i < argCount; i++)
			{
				if (i == actionIndex)
				{
					continue;
				}

				parameters[i < actionIndex ? i : i - 1] = args[i];
			}

			return (actionIndex, parameters);
		}

		protected static bool IsPostId(string value) => int.TryParse(value, out int postId);
	}
}
