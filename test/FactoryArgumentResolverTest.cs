using Xunit;
using Tusba.Components.Factories.Application;
using Tusba.Models;
using Tusba.Models.Application;

namespace test
{
	public class FactoryArgumentResolverTest : BaseTest
	{
		[Theory]
		[InlineData(null, null, null)]
		[InlineData("12345", null, null, "12345")]
		[InlineData(null, "do-something", null, "do-something")]
		public void InstanceTest(string? postId, string? action, DateRange dates, params string[] args)
		{
			var options = new Options();
			options.PostId = postId;
			options.Action = action;
			options.setDate(dates);

			Assert.Equal(options, new FactoryArgumentResolver().FactoryInstance.Handle(args));
		}
	}
}
