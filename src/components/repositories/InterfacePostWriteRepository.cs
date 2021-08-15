namespace Tusba.Components.Repositories
{
	public interface InterfacePostWriteRepository
	{
		bool Store();
		bool Store(string postId);
	}
}
