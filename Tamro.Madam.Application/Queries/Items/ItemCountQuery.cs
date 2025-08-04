using MediatR;

namespace Tamro.Madam.Application.Queries.Items;

public class ItemCountQuery : IRequest<int>
{
    public ItemCountSpecification Specification => new();
}
