namespace Tamro.Madam.Application.Infrastructure.Session;

public interface IUserContext
{
    string DisplayName { get; set; }
    string[] Permissions { get; set; }
    string UserName { get; set; }
    bool HasPermission(string permission);
    bool HasPermission(IEnumerable<string> permissions);
}
