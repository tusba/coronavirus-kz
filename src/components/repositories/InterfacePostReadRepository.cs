namespace Tusba.Components.Repositories
{
	public interface InterfacePostReadRepository
	{
		string Fetch();
		string Fetch(string postId);
	}
}
