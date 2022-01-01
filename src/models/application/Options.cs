namespace Tusba.Models.Application
{
	public struct Options
	{
		public string? Action { get; set; }
		public string? PostId { get; set; }

		public DateRange Dates { get; private set; }

		public void setDate()
		{
			Dates = new DateRange();
		}

		public void setDate(DateRange value)
		{
			Dates = value;
		}

		public void setDate(string value)
		{
			Dates = new DateRange(value);
		}

		public void setDate(string value, string boundary)
		{
			Dates = new DateRange(value, boundary);
		}
	}
}
