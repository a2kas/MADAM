using Ardalis.Specification;
using AutoMapper;
using MediatR;
using System.Collections.Concurrent;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Queries._Abstract.SingleEntityGridQueryHandler;
public class SingleEntityGridQueryHandler<BaseEntityType, EntityType, QueryType, ModelType>(
    IUnitOfWork<BaseEntityType> _uow,
    IMapper _mapper)
: IRequestHandler<QueryType, PaginatedData<ModelType>>
where BaseEntityType : class, Repository.Entities.IEntity
where QueryType : PaginationFilter, IRequest<PaginatedData<ModelType>>
where EntityType : class, BaseEntityType
{
    private static ConcurrentDictionary<Type, Type> SpecificationTypeCache = new();

    public Task<PaginatedData<ModelType>> Handle(QueryType request, CancellationToken cancellationToken)
    {
        var specificationType = SpecificationTypeCache.GetOrAdd(typeof(EntityType), (Type entityType) =>
        {
            var assembly = Assembly.GetExecutingAssembly();

            var specificationTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(Specification<>));

            var targetSpecificationType = specificationTypes
                .FirstOrDefault(t => t.BaseType!.GetGenericArguments()[0] == entityType);

            if (targetSpecificationType != null)
            {
                return targetSpecificationType;
            }
            throw new SystemException($"Could not find Specification for {entityType.FullName}");
        });

        var specification = (ISpecification<EntityType>)Activator.CreateInstance(specificationType, request)!;

        return _uow.GetRepository<EntityType>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<EntityType, ModelType>(
                specification,
                request.PageNumber,
                request.PageSize,
                _mapper.ConfigurationProvider,
                cancellationToken: cancellationToken);
    }
}
