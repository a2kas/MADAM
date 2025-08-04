using MediatR;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.MeasurementUnits;

public class MeasurementUnitQuery : MeasurementUnitFilter, IRequest<PaginatedData<MeasurementUnitModel>>
{
    public MeasurementUnitSpecification Specification => new(this);
}
