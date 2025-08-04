namespace Tamro.Madam.Application.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiresPermissionAttribute : Attribute
{
    public string Permission { get; }

    public RequiresPermissionAttribute(string permission)
    {
        Permission = permission;
    }
}
