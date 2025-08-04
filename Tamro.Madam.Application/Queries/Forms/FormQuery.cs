using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Forms;

public class FormQuery : FormFilter, IRequest<PaginatedData<FormModel>>
{
    public FormSpecification Specification => new(this);
}
