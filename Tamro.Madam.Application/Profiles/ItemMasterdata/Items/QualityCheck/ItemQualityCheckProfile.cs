using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.QualityCheck;

public class ItemQualityCheckProfile : Profile
{
    public ItemQualityCheckProfile()
    {
        CreateMap<ItemQualityCheck, ItemQualityCheckGridModel>()
            .ForMember(d => d.Id, o => o.MapFrom(src => src.Id))
            .ForMember(d => d.ItemId, o => o.MapFrom(src => src.ItemId))
            .ForMember(d => d.ItemName, o => o.MapFrom(src => src.Item.ItemName))
            .ForMember(d => d.ScanDate, o => o.MapFrom(src => src.RowVer))
            .ForMember(d => d.IssueCount, o => o.MapFrom(src => src.ItemQualityCheckIssues.Count))
            .ForMember(d => d.UnresolvedIssuesCount, o => o.MapFrom(src => src.ItemQualityCheckIssues.Count(x => x.IssueStatus == ItemQualityIssueStatus.New)))
            .ForMember(d => d.UnresolvedSeverities, o => o.MapFrom(src => src.ItemQualityCheckIssues.Where(x => x.IssueStatus == ItemQualityIssueStatus.New).Select(x => x.IssueSeverity).Distinct()))
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.Issues, o => o.MapFrom(src => src.ItemQualityCheckIssues));

        CreateMap<ItemQualityCheckIssue, ItemQualityCheckIssueGridModel>()
            .ForMember(d => d.Id, o => o.MapFrom(src => src.Id))
            .ForMember(d => d.IssueField, o => o.MapFrom(src => src.IssueField))
            .ForMember(d => d.IssueEntity, o => o.MapFrom(src => src.IssueEntity))
            .ForMember(d => d.IssueDescription, o => o.MapFrom(src => src.Description))
            .ForMember(d => d.Severity, o => o.MapFrom(src => src.IssueSeverity))
            .ForMember(d => d.ActualValue, o => o.MapFrom(src => src.ActualValue))
            .ForMember(d => d.ExpectedValue, o => o.MapFrom(src => src.ExpectedValue))
            .ForMember(d => d.Status, o => o.MapFrom(src => src.IssueStatus))
            .ForMember(d => d.RowVer, o => o.MapFrom(src => src.RowVer))
            .ForMember(d => d.Country, o => o.MapFrom(src => src.Country));
    }
}
