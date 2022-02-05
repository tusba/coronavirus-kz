namespace Tusba.Models
{
	public struct PostStats
	{
		public string Region { get; init; }

		public int Quantity { get; init; }

		public PostStats(string region, int quantity)
		{
			Region = region;
			Quantity = quantity;
		}
	}
}
