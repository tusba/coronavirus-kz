namespace Tusba.Enumerations.Post
{
  public enum Type
  {
    // default type
    COMMON,

    // statistics on diseased people
    STATS_DISEASED,

    // statistics on cases of pneumonia
    STATS_PNEUMONIA,

    // statistics on recovered people
    STATS_RECOVERED
  }

  public static class TypeExtensions
  {
    public static Type ResolvePostType(this string s) => s.ToLower() switch
    {
      _ => Type.COMMON,
    };
  }
}
