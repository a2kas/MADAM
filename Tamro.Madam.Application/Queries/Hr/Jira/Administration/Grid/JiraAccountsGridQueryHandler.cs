using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Repository.UnitOfWork;
using System.Linq.Dynamic.Core;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Models.Hr.Jira.Administration;

namespace Tamro.Madam.Application.Queries.Hr.Jira.Administration.Grid;

[RequiresPermission(Permissions.CanViewJira)]
public class JiraAccountsGridQueryHandler : IRequestHandler<JiraAccountsGridQuery, PaginatedData<JiraAccountModel>>
{
    private readonly IJpgUnitOfWork _uow;
    private readonly IMapper _mapper;

    public JiraAccountsGridQueryHandler(IJpgUnitOfWork uow, IMapper mapper)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<PaginatedData<JiraAccountModel>> Handle(JiraAccountsGridQuery request, CancellationToken cancellationToken)
    {
        return _uow.GetRepository<JiraAccount>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<JiraAccount, JiraAccountModel>(
                request.Specification,
                request.PageNumber,
                request.PageSize,
                _mapper.ConfigurationProvider,
                cancellationToken: cancellationToken
            );
    }
}
