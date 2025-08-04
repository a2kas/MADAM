using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Swashbuckle.AspNetCore.Annotations;

namespace Tamro.Madam.RestApi.Tests.MaintenanceChecks.Controllers;

[TestFixture]
public class NSwagTests
{
    [Test(Description = "NSwag requires operation id to be unique, as we auto-generate operation names from {Controller}_{Action}, the action+controller should be unique")]
    public void InEachController_AllActionMethodNames_AreUnique()
    {
        foreach (var controller in GetAllControllers())
        {
            var controllerMethods = new List<Type>() { controller }.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public));

            foreach (var controllerMethodNameGroup in controllerMethods.GroupBy(x => x.Name))
            {
                controllerMethodNameGroup.Count().ShouldBe(1, $"Duplicate action name was found. Controller: {controller.Name}, action name: {controllerMethodNameGroup.First().Name}");
            }
        }
    }

    [Test(Description = "NSwag requires SwaggerOperation with status code 200 or 204 to be included, just so it knows what return type a method should have")]
    public void AllControllerActionMethods_HasSwaggerResponse_With_200_or_and_204_StatusCode_Attribute()
    {
        foreach (var actionMethod in GetAllActionMethods())
        {
            var attributes = actionMethod.GetCustomAttributes()
                .Where(a => a is SwaggerResponseAttribute &&
                            (a as SwaggerResponseAttribute)?.StatusCode == 200
                            || (a as SwaggerResponseAttribute)?.StatusCode == 204)
                .ToArray();

            attributes.Count(a => a is SwaggerResponseAttribute && (a as SwaggerResponseAttribute)!.StatusCode == 200).ShouldBeLessThanOrEqualTo(1);
            attributes.Count(a => a is SwaggerResponseAttribute && (a as SwaggerResponseAttribute)!.StatusCode == 204).ShouldBeLessThanOrEqualTo(1);
            attributes.Length.ShouldBeOneOf(1, 2);
        }
    }

    private static IEnumerable<MethodInfo> GetAllActionMethods()
    {
        return GetAllControllers()
            .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public));
    }

    private static IEnumerable<Type> GetAllControllers()
    {
        var assembly = Assembly.GetAssembly(typeof(Program));

        return assembly
            .GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t));
    }
}
