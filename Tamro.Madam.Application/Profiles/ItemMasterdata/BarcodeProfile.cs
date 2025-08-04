using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class BarcodeProfile : Profile
{
    public BarcodeProfile()
    {
        CreateMap<Barcode, BarcodeGridModel>()
            .ForMember(d => d.ItemId, o => o.MapFrom(src => src.Item.Id))
            .ForMember(d => d.ItemName, o => o.MapFrom(src => src.Item.ItemName));

        CreateMap<BarcodeModel, Barcode>()
            .ForMember(d => d.ItemId, o => o.MapFrom(src => src.Item.Id))
            .ForMember(d => d.Item, o => o.Ignore());

        CreateMap<Barcode, BarcodeModel>();
    }
}
