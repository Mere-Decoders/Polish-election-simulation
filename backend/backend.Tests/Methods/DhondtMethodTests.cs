using backend.Services.Methods;

namespace backend.Tests.Methods;

// TODO TODODODODODDODOD↓
/// <summary>
///  MARCIN MAKE THE ASSERTS MORE MEANINGFUL
/// </summary>
public class DhondtMethodTests
{
    [Fact]
    public void Case1_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case1);
        Assert.NotNull(result);
    }

    [Fact]
    public void Case2_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case2);
        Assert.NotNull(result);
    }

    [Fact]
    public void Case3_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case3);
        Assert.NotNull(result);
    }

    [Fact]
    public void Case4_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case4);
        Assert.NotNull(result);
    }

    [Fact]
    public void Case5_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case5);
        Assert.NotNull(result);
    }

    [Fact]
    public void Case6_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case6);
        Assert.NotNull(result);
    }
}