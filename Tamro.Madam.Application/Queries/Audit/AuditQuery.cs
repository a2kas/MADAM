using MediatR;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Audit;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Audit;

public class AuditQuery : AuditFilter, IRequest<Result<PaginatedData<AuditGridModel>>>
{
    public AuditSpecification Specification => new(this);
}
