using System.Threading.Tasks;
using Tusba.Models;

namespace Tusba.Components.Parsers
{
  public interface InterfacePostStatsParser
  {
    Task<PostStats[]> Parse();
  }
}
