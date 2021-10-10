namespace Tusba.Enumerations.Application
{
  public enum Action
  {
    // no specific action
    NONE,

    // fetch and store post HTML page
    FETCH_PAGE
  }

  public static class ActionExtensions
  {
    public static Action Resolve(this string s) => s.ToLower() switch
    {
      "fetch" => Action.FETCH_PAGE,
      _ => Action.NONE,
    };
  }
}
