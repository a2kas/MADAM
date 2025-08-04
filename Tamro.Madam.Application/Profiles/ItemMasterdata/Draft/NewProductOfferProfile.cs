using AutoMapper;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Draft;

public class NewProductOfferProfile : Profile
{
    public NewProductOfferProfile()
    {
        CreateMap<UpsertNewProductOfferCommand, NewProductOffer>()
            .ForMember(npo => npo.CreatedDate, o => o.Ignore())
            .ForMember(npo => npo.FileReference, o => o.Ignore())
            .ForMember(npo => npo.ItemCategoryManager, o => o.Ignore())
            .ForMember(npo => npo.Items, o => o.Ignore())
            .ForMember(npo => npo.Comments, o => o.Ignore())
            .ForMember(npo => npo.Attachments, o => o.Ignore());

        CreateMap<NewProductOffer, UpsertNewProductOfferResult>()
            .ForMember(r => r.FileReference, o => o.Ignore());
    }
}
