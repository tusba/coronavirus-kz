using System.Threading.Tasks;

namespace Tusba.Components.Parsers
{
  public interface InterfacePostParser
  {
    Task<string[]> Parse();
  }
}
