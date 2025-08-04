using MediatR;
using Newtonsoft.Json;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Gateways.GptService;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Gateways.GptService;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Handlers.Suppliers.Contracts;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class ExtractContractFileCommandHandler : IRequestHandler<ExtractContractFileCommand, Result<SupplierContractExtractionModel>>
{
    public readonly IContractExtractionsClient _contractExtractionsClient;

    public ExtractContractFileCommandHandler(IContractExtractionsClient contractExtractionsClient)
    {
        _contractExtractionsClient = contractExtractionsClient;
    }

    public async Task<Result<SupplierContractExtractionModel>> Handle(ExtractContractFileCommand command, CancellationToken cancellationToken)
    {
        var extractedContract = await _contractExtractionsClient.ExtractInfoAsync(command.Stream, command.FileName);
        var extractedString = extractedContract.ToString();
        var structuredExtractedContract = JsonConvert.DeserializeObject<AiExtractedSupplierContract>(extractedString);
        var contract = command.Contract;

        var extractedInfo = structuredExtractedContract.ExtractedInformation;

        contract.AgreementValidFrom ??= extractedInfo?.AgreementTerm?.AgreementDateValidFrom?.Value;
        contract.AgreementValidTo ??= extractedInfo?.AgreementTerm?.AgreementDateValidTo?.Value;
        contract.PaymentTermInDays ??= extractedInfo?.FinancialProvisions?.PaymentTermInDays?.Value;
        contract.AgreementDate ??= extractedInfo?.DocumentMetadata?.DateOfAgreementDocument?.Value;

        var result = new SupplierContractExtractionModel();
        result.Contract = contract;
        result.SupplierName = extractedInfo?.AgreementParties?.SupplierName?.Value ?? string.Empty;
        result.RegistrationNumber = extractedInfo?.AgreementParties?.SupplierRegistrationNumber?.Value ?? string.Empty;

        return Result<SupplierContractExtractionModel>.Success(result);
    }
}
