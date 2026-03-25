using AbsenceApp.Core.Navigation;

namespace AbsenceApp.Core.Interfaces;

public interface INavigationMetadataService
{
    IEnumerable<NavigationItem> GetAll();
    IEnumerable<string> GetGroups();
    IEnumerable<NavigationItem> GetByGroup(string group);
}
