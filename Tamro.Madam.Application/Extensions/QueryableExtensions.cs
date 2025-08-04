using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities;

namespace Tamro.Madam.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec, bool evaluateCriteriaOnly = false) where T : class, IEntity
    {
        return SpecificationEvaluator.Default.GetQuery(query, spec, evaluateCriteriaOnly);
    }

    public static async Task<PaginatedData<TResult>> ProjectToPaginatedDataAsync<T, TResult>(this IQueryable<T> query, ISpecification<T> spec, int pageNumber, int pageSize, IConfigurationProvider configuration, CancellationToken cancellationToken = default) where T : class
    {
        var specificationEvaluator = SpecificationEvaluator.Default;
        var count = await specificationEvaluator.GetQuery(query, spec).CountAsync(cancellationToken);

        var data = await specificationEvaluator.GetQuery(query.AsNoTracking(), spec).Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .ProjectTo<TResult>(configuration)
            .ToListAsync(cancellationToken);

        return new PaginatedData<TResult>(data, count, pageNumber, pageSize);
    }

    public static Task<int> Count<T>(this IQueryable<T> query, ISpecification<T> spec, CancellationToken cancellationToken = default) where T : class, IEntity
    {
        var specificationEvaluator = SpecificationEvaluator.Default;
        return specificationEvaluator.GetQuery(query, spec).CountAsync(cancellationToken);
    }

    public static IQueryable<T> ApplySorting<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, string sortDirection)
    {
        return sortDirection == nameof(SortDirection.Ascending) ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
    }
}
