namespace Tusba.Models.Application
{
	public struct Options
	{
		public string? Action { get; set; }
		public string? PostId { get; set; }

		public DateRange Dates { get; private set; }

		public void SetDate() => Dates = new DateRange();

		public void SetDate(DateRange value) => Dates = value;

		public void SetDate(string value) => Dates = new DateRange(value);

		public void SetDate(string value, string boundary) => Dates = new DateRange(value, boundary);
	}
}
