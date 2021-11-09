using Tusba.Enumerations.Post;

namespace Tusba.Models
{
  public struct Post
  {
    public string Id { get; init; }

    public string DateTime { get; init; }

    public string Content { get; init; }

    public string Date => DateTime.Substring(0, 10);

		public Type Type => Content.ResolvePostType();

		public Post(string id, string dateTime, string content)
		{
			Id = id;
			DateTime = dateTime;
			Content = content;
		}
  }
}
