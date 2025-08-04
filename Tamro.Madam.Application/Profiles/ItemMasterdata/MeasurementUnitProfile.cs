using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class MeasurementUnitProfile : Profile
{
    public MeasurementUnitProfile()
    {
        CreateMap<MeasurementUnitModel, MeasurementUnit>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<MeasurementUnit, MeasurementUnitClsfModel>();
    }
}
