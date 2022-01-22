using System.Reflection;
using Xunit;
using Tusba.Components.Factories.Application;
using Tusba.Models.Application;
using Tusba.Patterns.ChainOfResponsibility.Application;
using ApplicationAction = Tusba.Enumerations.Application.Action;

namespace test
{
	public class ArgumentResolverTest : BaseTest
	{
		[Theory]
		// no arguments
		[InlineData(null, null, null)]
		// 1 argument
		[InlineData("12345", null, null, "12345")]
		[InlineData(null, "do-something", null, "do-something")]
		// 2 arguments
		[InlineData("12345", "do-something", null, "12345", "do-something")]
		[InlineData("12345", "do-something", null, "do-something", "12345")]
		// "parse" action
		[InlineData(null, "parse", new string[] {}, "parse")]
		[InlineData(null, "parse", new string[] { "2022-01-15" }, "parse", "2022-01-15")]
		[InlineData(null, "parse", new string[] { "2022-01-29", "2021-12-31" }, "parse", "2022-01-29", "2021-12-31")]
		public void FactoryInstanceTest(string? postId, string? action, string[]? dates, params string[] args)
		{
			var options = new Options();

			options.PostId = postId;
			options.Action = action;

			if (action == "parse")
			{
				switch (dates?.Length)
				{
					case 0:
						options.SetDate();
						break;
					case 1:
						options.SetDate(dates[0]);
						break;
					case 2:
						options.SetDate(dates[0], dates[1]);
						break;
				}
			}

			Assert.Equal(options, new FactoryArgumentResolver().FactoryInstance.Handle(args));
		}

		[Theory]
		// no arguments
		[InlineData(new string[] {}, null, null, new string[] {})]
		// no specific action
		[InlineData(new string[] { "do-something" }, null, null, new string[] { "do-something" })]
		[InlineData(new string[] { "uno", "dos" }, null, null, new string[] { "uno", "dos" })]
		[InlineData(new string[] { "uno", "fetch", "dos" }, null, 1, new string[] { "uno", "dos" })]
		[InlineData(new string[] { "uno", "dos", "extract", "parse" }, null, 2, new string[] { "uno", "dos", "parse" })]
		// specify action
		[InlineData(new string[] { "fetch" }, ApplicationAction.FETCH_PAGE, 0, new string[] {})]
		[InlineData(new string[] { "extract" }, ApplicationAction.PARSE_STATS, null, new string[] { "extract" })]
		[InlineData(new string[] { "uno", "dos" }, ApplicationAction.EXTRACT_POSTS, null, new string[] { "uno", "dos" })]
		[InlineData(new string[] { "uno", "fetch", "dos" }, ApplicationAction.FETCH_PAGE, 1, new string[] { "uno", "dos" })]
		[InlineData(new string[] { "uno", "fetch", "dos" }, ApplicationAction.EXTRACT_POSTS, null, new string[] { "uno", "fetch", "dos" })]
		[InlineData(new string[] { "uno", "dos", "extract", "parse" }, ApplicationAction.PARSE_STATS, 3, new string[] { "uno", "dos", "extract" })]
		public void ApplicationActionDetectionTest(string[] args, ApplicationAction? action, int? index, string[] parameters)
		{
			object? result = typeof(BaseArgumentResolver)
				.GetMethod("DetectApplicationAction", BindingFlags.NonPublic | BindingFlags.Static)
				?.Invoke(null, new object?[] { args, action });

			Assert.NotNull(result);

			var actionInfo = ((int? index, string[] parameters)) result!;
			Assert.Equal(index, actionInfo.index);
			Assert.Equal(parameters, actionInfo.parameters);
		}
	}
}
