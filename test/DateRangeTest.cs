using System.Linq;
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

		[Theory]
		[InlineData(null, "2021-12-17", new string[] {})]
		[InlineData("2021-12-17", null, new string[] { "2021-12-17" })]
		[InlineData("2021-12-17", "2021-12-17", new string[] { "2021-12-17" })]
		[InlineData("2021-12-30", "2022-01-02", new string[] { "2021-12-30", "2021-12-31", "2022-01-01", "2022-01-02" })]
		[InlineData("2020-03-02", "2020-02-27", new string[] { "2020-02-27", "2020-02-28", "2020-02-29", "2020-03-01", "2020-03-02" })]
		public void RangeTest(string? date, string? boundary, string[] range)
		{
			var resultRange = new DateRange(date ?? "", boundary ?? "").Range();
			Assert.Equal(range, resultRange.Select(date => date.ToString(DateRange.Format)).ToArray());
		}
	}
}
