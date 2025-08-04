using MediatR;
using System.Collections.Concurrent;
using System.Reflection;

namespace Tamro.Madam.Application.Infrastructure.Cache;

public static class MediatRHandlerCache
{
    public static readonly ConcurrentDictionary<Type, Type> HandlerCache = new();

    public static void PopulateHandlerCache(Assembly[] assembliesToScan)
    {
        foreach (var assembly in assembliesToScan)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var interfaceTypes = handlerType.GetInterfaces().Where(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                foreach (var interfaceType in interfaceTypes)
                {
                    var requestType = interfaceType.GetGenericArguments()[0];
                    HandlerCache.TryAdd(requestType, handlerType);
                }
            }
        }
    }
}
