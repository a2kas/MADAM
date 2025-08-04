using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Forms.Clsf;

public class FormClsfQuery : FormClsfFilter, IRequest<PaginatedData<FormClsfModel>>
{
    public FormClsfSpecification Specification => new(this);
}
