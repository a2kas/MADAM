using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Queries.Forms.Clsf;

public class FormClsfSpecification : Specification<Form>
{
    public FormClsfSpecification(FormClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
