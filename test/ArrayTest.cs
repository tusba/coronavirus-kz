using System;
using Xunit;
using ArrayUtil = Tusba.Components.Util.Array;

namespace test
{
	public class ArrayTest : BaseTest
	{
		[Theory]
		[InlineData(new int[] { }, null, null)]
		[InlineData(new int[] { 27 }, 27, 27)]
		[InlineData(new int[] { -1, 1 }, -1, 1)]
		[InlineData(new int[] { 1, -1 }, -1, 1)]
		[InlineData(new int[] { 8, 46, 1, 29, 2022 }, 1, 2022)]
		public void MinMaxIntTest(int[] source, int? min, int? max)
		{
			MinMaxTest<int>(source, min ?? 0, max ?? 0);
		}

		[Theory]
		[InlineData(new string[] { }, null, null)]
		[InlineData(new string[] { "qwerty" }, "qwerty", "qwerty")]
		[InlineData(new string[] { "a", "z" }, "a", "z")]
		[InlineData(new string[] { "z", "a" }, "a", "z")]
		[InlineData(new string[] { "2022-02-01", "2021-12-31", "2022-01-29" }, "2021-12-31", "2022-02-01")]
		public void MinMaxStringTest(string[] source, string? min, string? max)
		{
			MinMaxTest<string>(source, min ?? String.Empty, max ?? String.Empty);
		}

		private void MinMaxTest<T>(T[] source, T min, T max)
		{
			var tuple = ArrayUtil.MinMax(source);
			if (tuple.HasValue)
			{
				var result = ((T Min, T Max)) tuple;
				Assert.Equal(result.Min, min);
				Assert.Equal(result.Max, max);
			}
			else
			{
				Assert.Null(tuple);
			}
		}
	}
}
