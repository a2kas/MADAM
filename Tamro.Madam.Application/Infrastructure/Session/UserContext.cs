namespace Tamro.Madam.Application.Infrastructure.Session;

public class UserContext : IUserContext
{
    public string DisplayName { get; set; }
    public string[] Permissions { get; set; } = [];
    public string UserName { get; set; }

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission);
    }

    public bool HasPermission(IEnumerable<string> permissions)
    {
        return permissions.Any(permission => Permissions.Contains(permission));
    }
}
