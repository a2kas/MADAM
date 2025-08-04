using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms.ById;
public class SkuFormByIdQuery : IRequest<Result<FileWithName?>>, IDefaultErrorMessage
{
    public required int Id { get; set; }
    public string ErrorMessage { get => $"Failed to fetch the last SKU Form by id={Id}"; set { } }
}
