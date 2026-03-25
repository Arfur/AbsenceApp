// ============================================================
// File:    ClassDetailsPageTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: bUnit component tests for ClassDetailsComponent.razor.
//          Component is fully static — no service injection required.
// ============================================================

namespace AbsenceApp.Client.Tests.ComponentTests;

public sealed class ClassDetailsPageTests
{
    [Fact]
    public void WhenId1_RendersClassOne()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ClassDetailsComponent>(
            p => p.Add(c => c.id, "1"));

        Assert.Contains("Class One", cut.Markup);
        Assert.Contains("Year 7",    cut.Markup);

        var hrefs = cut.FindAll("a").Select(a => a.GetAttribute("href") ?? string.Empty).ToList();
        Assert.Contains("/students/1", hrefs);
        Assert.Contains("/students/2", hrefs);
    }

    [Fact]
    public void WhenId2_RendersClassTwo()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ClassDetailsComponent>(
            p => p.Add(c => c.id, "2"));

        Assert.Contains("Class Two", cut.Markup);
        Assert.Contains("Year 8",    cut.Markup);

        var hrefs = cut.FindAll("a").Select(a => a.GetAttribute("href") ?? string.Empty).ToList();
        Assert.Contains("/students/3", hrefs);
    }

    [Fact]
    public void WhenInvalidId_RendersNotFound()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ClassDetailsComponent>(
            p => p.Add(c => c.id, "999"));

        Assert.Contains("Class not found.", cut.Markup);
    }
}
