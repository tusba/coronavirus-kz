using System;
using DateUtil = Tusba.Components.Util.Date;

namespace Tusba.Models
{
	public class DateRange
	{
		public DateTime? Date { get; init; }
		public DateTime? Boundary { get; init; }

		public static string Format { get; set; } = "yyyy-MM-dd";

		public DateRange()
		{
			Date = DateTime.Now;
		}

		public DateRange(string date)
		{
			Date = DateUtil.Parse(date, Format);
		}

		public DateRange(string date, string boundary) : this(date)
		{
			Boundary = DateUtil.Parse(boundary, Format);

			if (Date is not null && Boundary is not null && Date > Boundary)
			{
				(Date, Boundary) = (Boundary, Date);
			}
		}
	}
}
