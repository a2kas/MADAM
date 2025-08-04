using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public class ItemMasterdataQualityCheckService : IItemMasterdataQualityCheckService
{
    private readonly IItemRepository _itemRepository;
    private readonly IIssueEntityResolver _issueEntityResolver;
    private readonly IIssueSeverityResolver _issueSeverityResolver;
    private readonly IActualValueResolver _actualValueResolver;

    public ItemMasterdataQualityCheckService(IItemRepository itemRepository, IIssueEntityResolver issueEntityResolver, IIssueSeverityResolver issueSeverityResolver, IActualValueResolver actualValueResolver)
    {
        _itemRepository = itemRepository;
        _issueEntityResolver = issueEntityResolver;
        _issueSeverityResolver = issueSeverityResolver;
        _actualValueResolver = actualValueResolver;
    }

    public async Task<IEnumerable<Item>> GetItems()
    {
        var includes = new List<IncludeOperation<Item>>
        {
            new(q => q.Include(i => i.Atc)
                .Include(i => i.Bindings)
                .ThenInclude(x => x.ItemBindingInfo)),
        };

        return await _itemRepository.GetList(x => x.Bindings.Any(x => x.ItemBindingInfo != null), includes, take: 100, track: false);
    }

    public ItemQualityCheck PerformQualityCheck(ItemQualityCheckReferenceModel reference, ItemQualityCheckApiResponseModel response)
    {
        var qualityCheckResult = new ItemQualityCheck
        {
            ItemQualityCheckIssues = new List<ItemQualityCheckIssue>(),
            ItemId = reference.Id
        };

        var extractionsWithIssues = response.ExtractedInformation
            .Where(extraction => extraction.Fields.Any(field => field.IssuesType != default));

        foreach (var extraction in extractionsWithIssues)
        {
            var sectionParts = extraction.Section.Split('_');
            var country = Enum.Parse<BalticCountry>(sectionParts[0]);
            var localCode = sectionParts[1];
            var binding = reference.Bindings.SingleOrDefault(b => b.LocalId == localCode);

            foreach (var field in extraction.Fields.Where(f => f.IssuesType != default))
            {
                var issue = new ItemQualityCheckIssue
                {
                    ItemId = reference.Id,
                    Country = country,
                    IssueEntity = _issueEntityResolver.ResolveIssueEntity(field.Field),
                    IssueField = field.Field,
                    IssueSeverity = _issueSeverityResolver.ResolveIssueSeverity(field.IssuesType),
                    IssueStatus = ItemQualityIssueStatus.New,
                    ActualValue = _actualValueResolver.ResolveActualValue(field.Field, binding, reference),
                    ExpectedValue = field.Value,
                    Description = field.IssuesFlagged
                };

                if (issue.IssueEntity == nameof(ItemBinding))
                {
                    issue.ItemBindingId = binding.Id;
                }

                qualityCheckResult.ItemQualityCheckIssues.Add(issue);
            }
        }

        qualityCheckResult.ItemQualityCheckIssues = qualityCheckResult.ItemQualityCheckIssues
            .GroupBy(issue => new { issue.IssueEntity, issue.IssueField })
            .SelectMany(g =>
            {
                if (g.Key.IssueEntity == nameof(Item) && g.Count() > 1)
                {
                    return g.Take(1);
                }
                else
                {                  
                    return g;
                }
            })
            .ToList();

        return qualityCheckResult;
    }
}
