using AutoMapper;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries._Abstract.SingleEntityGridQueryHandler;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms.Grid;

[RequiresPermission(Permissions.CanViewProductOffers)]
public class SkuFormGridQueryHandler(IMadamUnitOfWork _uow, IMapper _mapper)
    : SingleEntityGridQueryHandler<IMadamEntity, SkuForm, SkuFormsGridQuery, SkuFormModel>(_uow, _mapper)
{
}
