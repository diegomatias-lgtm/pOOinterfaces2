using PooInterface.Core.Formatters;
using Xunit;

namespace PooInterface.Tests;

public class FormatterTests
{
    [Fact]
    public void Procedural_TitleCase_Works()
    {
        var input = "hello test";
        var outp = FormatterProcedural.Format(input, FormatterProcedural.Mode.TitleCase);
        Assert.Equal("Hello Test", outp);
    }

    [Fact]
    public void OO_TitleCase_Works()
    {
        var f = new TitleCaseFormatter();
        Assert.Equal("Hello Test", f.Format("hello test"));
    }

    [Fact]
    public void Interface_TitleCase_Works()
    {
        var f = new InterfaceTitleCaseFormatter();
        Assert.Equal("Hello Test", f.Format("hello test"));
    }
}