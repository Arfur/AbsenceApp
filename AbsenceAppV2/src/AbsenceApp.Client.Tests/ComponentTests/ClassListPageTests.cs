// ============================================================
// File:    ClassListPageTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: bUnit component tests for ClassListComponent.razor.
//          Component is fully static — no service injection required.
// ============================================================

namespace AbsenceApp.Client.Tests.ComponentTests;

public sealed class ClassListPageTests
{
    [Fact]
    public void Renders_ClassListHeading()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ClassListComponent>();
        Assert.Equal("Class List", cut.Find("h3").TextContent);
    }

    [Fact]
    public void Renders_ClassNavigationLinks()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ClassListComponent>();

        var hrefs = cut.FindAll("a").Select(a => a.GetAttribute("href") ?? string.Empty).ToList();
        Assert.Contains("/classes/1", hrefs);
        Assert.Contains("/classes/2", hrefs);
    }
}
