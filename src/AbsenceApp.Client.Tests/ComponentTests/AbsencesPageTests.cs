// ============================================================
// File:    AbsencesPageTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: bUnit component tests for AbsencesComponent.razor.
// ============================================================

namespace AbsenceApp.Client.Tests.ComponentTests;

public sealed class AbsencesPageTests
{
    // --------------------------------------------------------
    // Helpers
    // --------------------------------------------------------
    private static (TestContext Ctx, Mock<IAbsenceService> Svc, IRenderedComponent<AbsencesComponent> Cut)
        Arrange(string studentId, IEnumerable<AbsenceRecord>? absences = null)
    {
        var ctx = new TestContext();
        var svc = new Mock<IAbsenceService>();

        svc.Setup(s => s.GetAbsencesForStudentAsync(studentId))
           .ReturnsAsync(absences ?? new List<AbsenceRecord>());

        svc.Setup(s => s.AddAbsenceAsync(It.IsAny<AbsenceRecord>()))
           .Returns(Task.CompletedTask);

        ctx.Services.AddSingleton(new AbsencesViewModel(svc.Object));

        var cut = ctx.RenderComponent<AbsencesComponent>(
            p => p.Add(c => c.id, studentId));

        return (ctx, svc, cut);
    }

    // --------------------------------------------------------
    // Tests
    // --------------------------------------------------------
    [Fact]
    public void Renders_Heading_Absences()
    {
        var (ctx, _, cut) = Arrange("3");
        Assert.Equal("Absences", cut.Find("h3").TextContent);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnLoadClick_CallsGetAbsencesForStudent_WithId()
    {
        var (ctx, svc, cut) = Arrange("3");

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        svc.Verify(s => s.GetAbsencesForStudentAsync("3"), Times.Once);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnLoadClick_WithAbsences_RendersListItems()
    {
        var records = new List<AbsenceRecord>
        {
            new() { Id = "a1", StudentId = "3", Date = new DateTime(2024, 1, 15), Reason = "Sick" }
        };
        var (ctx, _, cut) = Arrange("3", records);

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var items = cut.FindAll("li");
        Assert.NotEmpty(items);
        Assert.Contains("Sick", items[0].TextContent);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnAddClick_CallsAddAbsenceAsync()
    {
        var (ctx, svc, cut) = Arrange("3");

        // set reason text
        cut.Find("input").Change("Dentist");
        await cut.Find("#add-btn").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        svc.Verify(s => s.AddAbsenceAsync(It.Is<AbsenceRecord>(r =>
            r.StudentId == "3" && r.Reason == "Dentist")), Times.Once);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnAddClick_ClearsReasonInput()
    {
        var (ctx, _, cut) = Arrange("3");

        cut.Find("input").Change("Holiday");
        await cut.Find("#add-btn").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var inputValue = cut.Find("input").GetAttribute("value") ?? string.Empty;
        Assert.Equal(string.Empty, inputValue);
        ctx.Dispose();
    }
}
