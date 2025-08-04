using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Brands;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Brands;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class GetBrandsDeletionOverviewCommandHandler : IRequestHandler<GetBrandsDeletionOverviewCommand, Result<IEnumerable<BrandDeletionOverviewModel>>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public GetBrandsDeletionOverviewCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BrandDeletionOverviewModel>>> Handle(GetBrandsDeletionOverviewCommand request, CancellationToken cancellationToken)
    {
        var brandIds = request.Brands.Select(x => x.Id);
        var items = await _itemRepository.GetList(x => brandIds.Contains(x.BrandId), cancellationToken: cancellationToken);
        var brandDeletionIssues = from brand in request.Brands
                                  join item in items on brand.Id equals item.BrandId into brandItems
                                  from item in brandItems.DefaultIfEmpty()
                                  group item by brand into brandGroup
                                  select new BrandDeletionOverviewModel
                                  {
                                      Brand = brandGroup.Key,
                                      AttachedItems = _mapper.Map<List<ItemModel>>(brandGroup.Where(x => x != null).ToList()),
                                  };
        return Result<IEnumerable<BrandDeletionOverviewModel>>.Success(brandDeletionIssues);
    }
}
