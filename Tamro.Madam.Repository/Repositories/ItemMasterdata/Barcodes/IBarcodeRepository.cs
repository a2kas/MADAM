using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Barcodes;

public interface IBarcodeRepository
{
    Task<IEnumerable<Barcode>> GetList(Expression<Func<Barcode, bool>> filter, List<IncludeOperation<Barcode>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
}
