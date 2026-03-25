// ============================================================
// File:    StudentsPageTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: bUnit component tests for StudentsComponent.razor.
// ============================================================

namespace AbsenceApp.Client.Tests.ComponentTests;

public sealed class StudentsPageTests
{
    // --------------------------------------------------------
    // Helpers
    // --------------------------------------------------------
    private static (TestContext Ctx, Mock<IStudentFullViewService> Svc, IRenderedComponent<StudentsComponent> Cut)
        Arrange(IEnumerable<StudentFullViewDto>? students = null)
    {
        var ctx = new TestContext();
        var svc = new Mock<IStudentFullViewService>();

        svc.Setup(s => s.GetAllAsync())
           .ReturnsAsync((students ?? new List<StudentFullViewDto>()).ToList().AsReadOnly());

        ctx.Services.AddSingleton(new StudentsViewModel(svc.Object));

        var cut = ctx.RenderComponent<StudentsComponent>();
        return (ctx, svc, cut);
    }

    // --------------------------------------------------------
    // Tests
    // --------------------------------------------------------
    [Fact]
    public void Renders_Heading_Students()
    {
        var (ctx, _, cut) = Arrange();
        Assert.Equal("Students", cut.Find("h3").TextContent);
        ctx.Dispose();
    }

    [Fact]
    public void Renders_LoadButton()
    {
        var (ctx, _, cut) = Arrange();
        Assert.Contains("Load Students", cut.Find("button").TextContent);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnLoadClick_CallsGetAllAsync()
    {
        var (ctx, svc, cut) = Arrange();

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        svc.Verify(s => s.GetAllAsync(), Times.Once);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnLoadClick_WithStudents_RendersNavLinks()
    {
        var students = new List<StudentFullViewDto>
        {
            new() { FirstName = "Alice", LastName = "Smith", YearGroupName = "Year 7" }
        };
        var (ctx, _, cut) = Arrange(students);

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var link = cut.Find("a");
        Assert.Contains("Alice", cut.Markup);
        Assert.Contains("Year 7", cut.Markup);
        ctx.Dispose();
    }
}
