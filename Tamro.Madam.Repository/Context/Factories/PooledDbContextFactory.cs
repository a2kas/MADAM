using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Repository.Audit;
using TamroUtilities.EFCore;

namespace Tamro.Madam.Repository.Context.Factories;

public class PooledDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : BaseDbContext
{
    private readonly UserAccessor _userAccessor;
    private readonly IServiceProvider _provider;

    public PooledDbContextFactory(UserAccessor userAccessor, IServiceProvider provider)
    {
        _userAccessor = userAccessor;
        _provider = provider;
    }

    public TContext CreateDbContext()
    {
        if (_provider == null)
        {
            throw new InvalidOperationException(
                "IServiceProvider not configured");
        }

        var context = ActivatorUtilities.CreateInstance<TContext>(_provider);
        context.UserAccessor = _userAccessor;

        return context;
    }
}
