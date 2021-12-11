using System;
using System.Globalization;

namespace Tusba.Models.Application
{
	public struct Options
	{
		public string? Action { get; set; }
		public string? PostId { get; set; }

		public DateTime? Date { get; private set; }
		public DateTime? BoundaryDate { get; private set; }

		public Options setDate(string? value, bool isBoundary = false)
		{
			var date = DateTime.Now;
			DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out date);

			if (isBoundary)
			{
				BoundaryDate = date;
			}
			else
			{
				Date = date;
			}

			if (Date is not null && BoundaryDate is not null && Date > BoundaryDate)
			{
				(Date, BoundaryDate) = (BoundaryDate, Date);
			}

			return this;
		}
	}
}
