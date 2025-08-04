using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemGridModel>()
            .ForMember(d => d.Producer, o => o.MapFrom(src => src.Producer.Name))
            .ForMember(d => d.Brand, o => o.MapFrom(src => src.Brand.Name))
            .ForMember(d => d.Form, o => o.MapFrom(src => src.Form == null ? null : src.Form.Name))
            .ForMember(d => d.AtcName, o => o.MapFrom(src => src.Atc == null ? null : src.Atc.Name))
            .ForMember(d => d.AtcCode, o => o.MapFrom(src => src.Atc == null ? null : src.Atc.Value))
            .ForMember(d => d.SupplierNick, o => o.MapFrom(src => src.SupplierNick == null ? null : src.SupplierNick.Name))
            .ForMember(d => d.MeasurementUnit, o => o.MapFrom(src => src.MeasurementUnit == null ? null : src.MeasurementUnit.Name));

        CreateMap<Item, ItemClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.ItemName));

        CreateMap<Item, ItemModel>();

        CreateMap<ItemModel, Item>()
            .ForMember(d => d.ProducerId, o => o.MapFrom(src => src.Producer == null ? default : src.Producer.Id))
            .ForMember(d => d.BrandId, o => o.MapFrom(src => src.Brand == null ? default : src.Brand.Id))
            .ForMember(d => d.FormId, o => o.MapFrom(src => src.Form == null ? default : src.Form.Id))
            .ForMember(d => d.AtcId, o => o.MapFrom(src => src.Atc == null ? default : src.Atc.Id))
            .ForMember(d => d.SupplierNickId, o => o.MapFrom(src => src.SupplierNick == null ? default : src.SupplierNick.Id))
            .ForMember(d => d.MeasurementUnitId, o => o.MapFrom(src => src.MeasurementUnit == null ? default : src.MeasurementUnit.Id))
            .ForMember(d => d.RequestorId, o => o.MapFrom(src => src.Requestor == null ? default : src.Requestor.Id))
            .ForMember(d => d.Producer, o => o.Ignore())
            .ForMember(d => d.Brand, o => o.Ignore())
            .ForMember(d => d.Form, o => o.Ignore())
            .ForMember(d => d.Atc, o => o.Ignore())
            .ForMember(d => d.SupplierNick, o => o.Ignore())
            .ForMember(d => d.MeasurementUnit, o => o.Ignore())
            .ForMember(d => d.Requestor, o => o.Ignore())
            .ForMember(d => d.Producer, o => o.Ignore())
            .ForMember(d => d.Barcodes, o => o.Ignore())
            .ForMember(d => d.Bindings, o => o.Ignore())
            .ForMember(d => d.EditedAt, o => o.MapFrom(src => DateTime.Now));
    }
}
