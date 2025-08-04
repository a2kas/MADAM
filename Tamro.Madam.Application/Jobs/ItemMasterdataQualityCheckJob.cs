using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tamro.DocuQueryService.Client;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.QualityCheck.Upsert;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Jobs;

public class ItemMasterdataQualityCheckJob : IOneTimeJob
{
    private readonly IItemMasterdataQualityCheckService _itemMasterdataQualityCheckService;
    private readonly IQualityCheckAiConsumerService _qualityCheckAiConsumerService;
    private readonly ILogger<ItemMasterdataQualityCheckJob> _logger;
    private readonly IExtractionClient _extractionClient;
    private readonly IMediator _mediator;

    public ItemMasterdataQualityCheckJob()
    {
    }

    public ItemMasterdataQualityCheckJob(IItemMasterdataQualityCheckService itemMasterdataQualityCheckService,
        IQualityCheckAiConsumerService qualityCheckAiConsumerService,
        IExtractionClient extractionClient,
        ILogger<ItemMasterdataQualityCheckJob> logger,
        IMediator mediator)
    {
        _itemMasterdataQualityCheckService = itemMasterdataQualityCheckService;
        _qualityCheckAiConsumerService = qualityCheckAiConsumerService;
        _extractionClient = extractionClient;
        _logger = logger;
        _mediator = mediator;
    }

    public string Name => "Item masterdata quality check";

    public string Description => "Proof of concept job to do AI powered item masterdata quality check";

    public bool Processing { get; set; }

    public async Task<Result<int>> Execute()
    {
        var items = await _itemMasterdataQualityCheckService.GetItems();
        _logger.LogInformation($"{nameof(ItemMasterdataQualityCheckJob)}: Processing {items.Count()} items");

        foreach (var item in items)
        {
            try
            {
                var reference = await _qualityCheckAiConsumerService.ConstructReference(item);
                var response = await _extractionClient.ExtractAsync(
                        file: null,
                        text: reference.Text,
                        schema: _qualityCheckAiConsumerService.ConstructSchema(),
                        prompt: _qualityCheckAiConsumerService.ConstructPrompt()
                    );
                var json = JsonConvert.SerializeObject(response);
                var result = JsonConvert.DeserializeObject<ItemQualityCheckApiResponseModel>(json);

                var qualityCheckResult = _itemMasterdataQualityCheckService.PerformQualityCheck(reference, result);
                if (qualityCheckResult.ItemQualityCheckIssues?.Count > 0)
                {
                    var upsertResult = await _mediator.Send(new UpsertQualityCheckCommand()
                    {
                        Model = qualityCheckResult,
                    });
                    if (!upsertResult.Succeeded)
                    {
                        _logger.LogError($"Failed to upsert item quality check result for item with Baltic Id {item.Id}, reason: {upsertResult.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to perform master data check on item with Baltic Id {item.Id}");
            }
        }

        return Result<int>.Success(items.Count());
    }
}
