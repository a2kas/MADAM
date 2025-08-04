using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Barcodes;

public class BarcodeQuery : BarcodeFilter, IRequest<PaginatedData<BarcodeGridModel>>
{
    public BarcodeSpecification Specification => new(this);
}
