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
    private static (TestContext Ctx, Mock<IStudentService> Svc, IRenderedComponent<StudentsComponent> Cut)
        Arrange(IEnumerable<Student>? students = null)
    {
        var ctx = new TestContext();
        var svc = new Mock<IStudentService>();

        svc.Setup(s => s.GetAllStudentsAsync())
           .ReturnsAsync(students ?? new List<Student>());

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
    public async Task OnLoadClick_CallsGetAllStudentsAsync()
    {
        var (ctx, svc, cut) = Arrange();

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        svc.Verify(s => s.GetAllStudentsAsync(), Times.Once);
        ctx.Dispose();
    }

    [Fact]
    public async Task OnLoadClick_WithStudents_RendersNavLinks()
    {
        var students = new List<Student>
        {
            new() { Id = "1", FirstName = "Alice", LastName = "Smith", YearGroup = "Year 7" }
        };
        var (ctx, _, cut) = Arrange(students);

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var link = cut.Find("a");
        Assert.Contains("/students/1", link.GetAttribute("href"));
        Assert.Contains("Alice", link.TextContent);
        ctx.Dispose();
    }
}
