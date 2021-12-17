using System;
using System.Globalization;

namespace Tusba.Components.Util
{
	public class Date
	{
		public static DateTime? Parse(string value, string format)
		{
			if (DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out DateTime date))
			{
				return date;
			}

			return null;
		}
	}
}
