using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Queries.Forms;

public class FormSpecification : Specification<Form>
{
    public FormSpecification(FormFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(FormModel.Name)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Name);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Name.Contains(filter.SearchTerm));
        }
    }
}
