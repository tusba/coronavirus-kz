using System.Threading.Tasks;

namespace Tusba.Components.Parsers
{
  public class PostParser : InterfacePostParser
  {
    public string RawContent { get; init; }

    public PostParser(string rawContent) => RawContent = rawContent;

    public async Task<string[]> Parse()
    {
      return await Task.Run(() => new string[] {});
    }
  }
}
