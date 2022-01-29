using System.Collections.Generic;
using System.Linq;

namespace Tusba.Components.Util
{
	public class Array
	{
		public static (T Min, T Max)? MinMax<T>(T[] source)
		{
			int size = source.Length;

			if (size == 0)
			{
				return null;
			}

			T min, max;
			min = max = source[0];
			var comparer = Comparer<T>.Default;

			source
				.Skip(1)
				.ToList()
				.ForEach(item => {
					if (comparer.Compare(item, min) < 0)
					{
						min = item;
					}
					else if (comparer.Compare(item, max) > 0)
					{
						max = item;
					}
				});

			return (min, max);
		}
	}
}
