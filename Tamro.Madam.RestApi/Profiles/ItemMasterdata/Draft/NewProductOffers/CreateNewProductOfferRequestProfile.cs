using AutoMapper;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.RestApi.Profiles.ItemMasterdata.Draft.NewProductOffers;

public class CreateNewProductOfferRequestProfile : Profile
{
    public CreateNewProductOfferRequestProfile()
    {
        CreateMap<CreateNewProductOfferViewModel, UpsertNewProductOfferCommand>()
            .ForMember(c => c.Id, o => o.Ignore())
            .ForMember(c => c.Status, o => o.Ignore());

        CreateMap<UpsertNewProductOfferResult, CreateNewProductOfferResultViewModel>();
        CreateMap<Result<UpsertNewProductOfferResult>, Result<CreateNewProductOfferResultViewModel>>();
    }
}
