using Xunit;
using Tusba.Models;

namespace test
{
	public class DateRangeTest : BaseTest
	{
		[Theory]
		[InlineData("2021-12-01", "2021-12-17")]
		[InlineData("2021-12-17", "2021-12-01", true)]
		[InlineData("2021-08-16", "2020-10-20", true)]
		[InlineData("2020-02-04", "2020-02-04")]
		public void InstanceTest(string date, string boundary, bool needSwap = false)
		{
			var dateRange = new DateRange(date, boundary);
			Assert.Equal(!needSwap ? date : boundary, dateRange.Date?.ToString(DateRange.Format));
			Assert.Equal(!needSwap ? boundary : date, dateRange.Boundary?.ToString(DateRange.Format));
		}
	}
}
