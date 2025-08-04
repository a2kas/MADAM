using AutoMapper;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.ById;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.Grid;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountryAndType;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;

namespace Tamro.Madam.RestApi.Profiles.ItemMasterdata.Draft.NewProductOffers;

public class SkuFormProfile : Profile
{
    public SkuFormProfile()
    {
        CreateMap<SkuFormSearchViewModel, SkuFormsGridQuery>()
            .ForMember(cm => cm.Specification, o => o.Ignore())
            .ForMember(cm => cm.ErrorMessage, o => o.Ignore());

        CreateMap<SkuFormModel, SkuFormViewModel>();
        CreateMap<PaginatedData<SkuFormModel>, PaginatedData<SkuFormViewModel>>();

        CreateMap<GetLatestSkuFormViewModel, SkuFormLatestForCountryAndTypeQuery>()
            .ForMember(q => q.ErrorMessage, o => o.Ignore());

        CreateMap<Result<SkuFormModel?>, Result<SkuFormViewModel?>>();

        CreateMap<GetSkuFormByIdViewModel, SkuFormByIdQuery>()
            .ForMember(cm => cm.ErrorMessage, o => o.Ignore());
    }
}
