/*
===============================================================================
 File        : NavigationCoreTests.cs
 Namespace   : AbsenceApp.Tests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-20
 Updated     : 2026-03-20
-------------------------------------------------------------------------------
 Purpose     : Verifies the Core navigation types — PageStatus enum values
               and NavigationItem property initialisation — and exercises
               INavigationMetadataService contract via an in-test stub.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Navigation;

namespace AbsenceApp.Tests;

public class NavigationCoreTests
{
    // =========================================================================
    // PageStatus enum
    // =========================================================================

    [Fact]
    public void PageStatus_HasThreeValues()
    {
        var values = Enum.GetValues<PageStatus>();
        Assert.Equal(3, values.Length);
    }

    [Theory]
    [InlineData(PageStatus.Live)]
    [InlineData(PageStatus.Beta)]
    [InlineData(PageStatus.Planned)]
    public void PageStatus_ValuesAreDefined(PageStatus status)
    {
        Assert.True(Enum.IsDefined(status));
    }

    // =========================================================================
    // NavigationItem
    // =========================================================================

    [Fact]
    public void NavigationItem_DefaultsToLiveStatus()
    {
        var item = new NavigationItem();
        Assert.Equal(PageStatus.Live, item.Status);
    }

    [Fact]
    public void NavigationItem_InitPropertiesRoundtrip()
    {
        var item = new NavigationItem
        {
            Title       = "Dashboard",
            Href        = "/dashboard/overview",
            Icon        = "bi-speedometer2",
            Group       = "Dashboard",
            Status      = PageStatus.Beta,
            Description = "Overview page"
        };

        Assert.Equal("Dashboard",           item.Title);
        Assert.Equal("/dashboard/overview", item.Href);
        Assert.Equal("bi-speedometer2",     item.Icon);
        Assert.Equal("Dashboard",           item.Group);
        Assert.Equal(PageStatus.Beta,       item.Status);
        Assert.Equal("Overview page",       item.Description);
    }

    [Fact]
    public void NavigationItem_NullableDescriptionAllowed()
    {
        var item = new NavigationItem { Title = "T", Href = "/t", Icon = "", Group = "G" };
        Assert.Null(item.Description);
    }

    // =========================================================================
    // INavigationMetadataService — contract via stub
    // =========================================================================

    private sealed class StubNavService : INavigationMetadataService
    {
        private static readonly List<NavigationItem> _data = new()
        {
            new() { Title = "Overview", Href = "/dashboard", Icon = "bi-kanban", Group = "Dashboard", Status = PageStatus.Live },
            new() { Title = "Students", Href = "/students",  Icon = "bi-people",  Group = "People",    Status = PageStatus.Live },
            new() { Title = "Planned",  Href = "/future",    Icon = "bi-clock",   Group = "People",    Status = PageStatus.Planned }
        };

        public IEnumerable<NavigationItem> GetAll()               => _data;
        public IEnumerable<string>         GetGroups()            => _data.Select(i => i.Group).Distinct();
        public IEnumerable<NavigationItem> GetByGroup(string g)   => _data.Where(i => i.Group == g);
    }

    [Fact]
    public void INavigationMetadataService_GetAll_ReturnsAllItems()
    {
        INavigationMetadataService svc = new StubNavService();
        Assert.Equal(3, svc.GetAll().Count());
    }

    [Fact]
    public void INavigationMetadataService_GetGroups_ReturnsDistinct()
    {
        INavigationMetadataService svc    = new StubNavService();
        var                        groups = svc.GetGroups().ToList();
        Assert.Equal(2, groups.Count);
        Assert.Contains("Dashboard", groups);
        Assert.Contains("People",    groups);
    }

    [Fact]
    public void INavigationMetadataService_GetByGroup_FiltersCorrectly()
    {
        INavigationMetadataService svc   = new StubNavService();
        var                        items = svc.GetByGroup("People").ToList();
        Assert.Equal(2, items.Count);
        Assert.All(items, i => Assert.Equal("People", i.Group));
    }

    [Fact]
    public void INavigationMetadataService_GetByGroup_UnknownGroup_ReturnsEmpty()
    {
        INavigationMetadataService svc = new StubNavService();
        Assert.Empty(svc.GetByGroup("Nonexistent"));
    }
}
