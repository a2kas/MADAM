using AutoMapper;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings;

public class ItemBindingProfile : Profile
{
    public ItemBindingProfile()
    {
        CreateMap<ItemBinding, ItemBindingModel>()
            .ForMember(d => d.Company, o => o.MapFrom(src => Classifiers.Companies.FirstOrDefault(x => x.Value == src.Company) ?? new Company()));

        CreateMap<ItemBindingModel, ItemBinding>()
            .ForMember(d => d.Item, o => o.Ignore())
            .ForMember(d => d.Company, o => o.MapFrom(src => src.Company.Value))
            .ForMember(d => d.ItemAssortmentBindingMaps, o => o.Ignore())
            .ForMember(d => d.LocalId, o => o.MapFrom(src => src.LocalId.Trim()))
            .ForMember(d => d.EditedAt, o => o.Ignore())
            .ForMember(d => d.ItemBindingInfo, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore());

        CreateMap<ItemBindingLanguage, ItemBindingLanguageModel>()
            .ForMember(d => d.Id, o => o.MapFrom(src => src.Id))
            .ForMember(d => d.Language, o => o.MapFrom(src => MapItemBindingLanguageToLanguage(src)));

        CreateMap<ItemBindingLanguageModel, ItemBindingLanguage>()
            .ForMember(d => d.ItemBinding, o => o.Ignore())
            .ForMember(d => d.ItemBindingId, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore());

        CreateMap<ItemBinding, ItemBindingClsfModel>()
            .ForMember(d => d.ItemNo2, o => o.MapFrom(src => src.LocalId))
            .ForMember(d => d.Name, o => o.MapFrom(src => src.Item.ItemName));

        CreateMap<ItemBinding, ItemAssortmentItemModel>()
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.LocalId.Trim()))
            .ForMember(d => d.ItemName, o => o.MapFrom(src => src.Item.ItemName))
            .ForMember(d => d.ItemBindingId, o => o.MapFrom(src => src.Id));
    }

    private static Language MapItemBindingLanguageToLanguage(ItemBindingLanguage src)
    {
        var language = Classifiers.Languages.SingleOrDefault(x => x.Id == src.LanguageId);

        if (language == null)
        {
            return new Language();
        }

        return new Language()
        {
            Code = language.Code,
            Name = language.Name,
            Id = language.Id,
        };
    }
}
