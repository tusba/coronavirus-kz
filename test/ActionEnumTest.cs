using Xunit;
using Tusba.Enumerations.Application;

namespace test
{
  public class ActionEnumTest : BaseTest
  {
    [Theory]
    [InlineData("")]
    [InlineData("action")]
    [InlineData("none")]
    [InlineData("undefined")]
    public void DefaultResolveTest(string action)
    {
      Assert.Equal(Action.NONE, action.ResolveApplicationAction());
    }

    [Theory]
    [InlineData("fetch")]
    [InlineData("FETCH")]
    [InlineData("FeTcH")]
    public void FetchResolveTest(string action)
    {
      Assert.Equal(Action.FETCH_PAGE, action.ResolveApplicationAction());
    }
  }
}
