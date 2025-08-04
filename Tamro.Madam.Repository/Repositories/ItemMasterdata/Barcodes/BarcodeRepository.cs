using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Barcodes;

public class BarcodeRepository : IBarcodeRepository
{
    private readonly IMadamUnitOfWork _uow;

    public BarcodeRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Barcode>> GetList(Expression<Func<Barcode, bool>> filter, List<IncludeOperation<Barcode>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        IQueryable<Barcode> query = _uow.GetRepository<Barcode>()
            .AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        query = query.Where(filter);

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }
}
