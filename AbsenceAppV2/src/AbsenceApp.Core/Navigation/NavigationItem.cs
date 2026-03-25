namespace AbsenceApp.Core.Navigation;

public class NavigationItem
{
    public string     Title       { get; init; } = string.Empty;
    public string     Href        { get; init; } = string.Empty;
    public string     Icon        { get; init; } = string.Empty;
    public string     Group       { get; init; } = string.Empty;
    public PageStatus Status      { get; init; } = PageStatus.Live;
    public string?    Description { get; init; }
}
