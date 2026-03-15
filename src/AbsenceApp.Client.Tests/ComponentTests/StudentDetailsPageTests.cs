// ============================================================
// File:    StudentDetailsPageTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: bUnit component tests for StudentDetailsComponent.razor.
// ============================================================

namespace AbsenceApp.Client.Tests.ComponentTests;

public sealed class StudentDetailsPageTests
{
    // --------------------------------------------------------
    // Helpers
    // --------------------------------------------------------
    private static (TestContext Ctx, Mock<IStudentService> Svc, IRenderedComponent<StudentDetailsComponent> Cut)
        Arrange(string id, Student? student = null)
    {
        var ctx = new TestContext();
        var svc = new Mock<IStudentService>();

        svc.Setup(s => s.GetStudentByIdAsync(id))
           .ReturnsAsync(student);

        ctx.Services.AddSingleton(new StudentDetailsViewModel(svc.Object));

        var cut = ctx.RenderComponent<StudentDetailsComponent>(
            p => p.Add(c => c.id, id));

        return (ctx, svc, cut);
    }

    // --------------------------------------------------------
    // Tests
    // --------------------------------------------------------
    [Fact]
    public void Renders_Heading_StudentDetails()
    {
        var (ctx, _, cut) = Arrange("5");
        Assert.Equal("Student Details", cut.Find("h3").TextContent);
        ctx.Dispose();
    }

    [Fact]
    public void OnParameterSet_CallsLoadAsync_WithId()
    {
        var (ctx, svc, _) = Arrange("5");
        svc.Verify(s => s.GetStudentByIdAsync("5"), Times.Once);
        ctx.Dispose();
    }

    [Fact]
    public void WhenStudentLoaded_RendersNameAndYearGroup()
    {
        var student = new Student
        {
            Id = "5", FirstName = "Charlie", LastName = "Brown", YearGroup = "Year 9"
        };
        var (ctx, _, cut) = Arrange("5", student);

        Assert.Contains("Charlie", cut.Markup);
        Assert.Contains("Brown",   cut.Markup);
        Assert.Contains("Year 9",  cut.Markup);
        ctx.Dispose();
    }

    [Fact]
    public void WhenStudentIsNull_RendersLoadingMessage()
    {
        var (ctx, _, cut) = Arrange("99");
        Assert.Contains("Loading...", cut.Markup);
        ctx.Dispose();
    }

    [Fact]
    public void ViewAbsencesLink_HasCorrectHref()
    {
        var (ctx, _, cut) = Arrange("5");
        var links = cut.FindAll("a");
        Assert.Contains(links, l => (l.GetAttribute("href") ?? string.Empty).Contains("/students/5/absences"));
        ctx.Dispose();
    }
}
