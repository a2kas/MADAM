using MediatR;
using NUnit.Framework;
using Shouldly;
using System.Reflection;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Requestors;
using Tamro.Madam.Application.Infrastructure.Attributes;

namespace Tamro.Madam.Application.Tests.Security;

[TestFixture]
public class AccessRightsTests
{
    [Test]
    public void AllRequestHandlers_ShouldHaveRequirePermissionsAttribute_OrExplicitAttributeThatTellsItDoesNotNeedPermissions()
    {
        // Act
        var allTypes = typeof(GetRequestorClsfHandler).Assembly.GetTypes();

        var handlerTypes = allTypes
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            // only take fully defined types (having no type params),
            // so abstractions like SingleEntityGridQueryHandler are not included
            .Where(t => !t.IsGenericType)
            .ToList();

        // Assert
        foreach (var handlerType in handlerTypes)
        {
            var hasAttribute = handlerType.GetCustomAttributes()
                .Any(attr => attr is RequiresPermissionAttribute || attr is NoPermissionNeededAttribute);

            hasAttribute.ShouldBeTrue($"{handlerType.Name} must have either RequiresPermissionAttribute or NoPermissionAttribute.");
        }
    }
}
