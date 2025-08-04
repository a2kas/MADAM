using MediatR;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.MeasurementUnits.Clsf;

public class MeasurementUnitClsfQuery : MeasurementUnitClsfFilter, IRequest<PaginatedData<MeasurementUnitClsfModel>>
{
    public MeasurementUnitClsfSpecification Specification => new(this);
}
