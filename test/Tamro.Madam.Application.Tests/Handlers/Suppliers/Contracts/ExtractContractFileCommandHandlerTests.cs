using System.Text;
using Shouldly;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Gateways.GptService;
using Tamro.Madam.Application.Handlers.Suppliers.Contracts;
using Tamro.Madam.Models.Gateways.GptService;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Tests.Handlers.Suppliers.Contracts;

[TestFixture]
public class ExtractContractFileCommandHandlerTests
{
    private MockRepository _mockRepository;
    private Mock<IContractExtractionsClient> _contractExtractionsClient;
    private ExtractContractFileCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _contractExtractionsClient = _mockRepository.Create<IContractExtractionsClient>();
        _handler = new ExtractContractFileCommandHandler(_contractExtractionsClient.Object);
    }

    [Test]
    public async Task Handle_Should_Return_SuccessResult_When_Extraction_Is_Successful()
    {
        // Arrange
        var contract = new SupplierContractModel();
        var extractedInfo = new AiExtractedSupplierContract
        {
            ExtractedInformation = new ExtractedInformation
            {
                AgreementParties = new AgreementParties
                {
                    SupplierName = new ValueReference() { Value = "Teva", },
                    SupplierRegistrationNumber = new ValueReference() { Value = "454545", },
                },
                AgreementTerm = new AgreementTerm
                {
                    AgreementDateValidFrom = new DateReference { Value = DateTime.UtcNow.AddDays(-10) },
                    AgreementDateValidTo = new DateReference { Value = DateTime.UtcNow.AddDays(20) }
                },
                FinancialProvisions = new FinancialProvisions
                {
                    PaymentTermInDays = new NumberReference { Value = 30 }
                },
                DocumentMetadata = new DocumentMetadata
                {
                    DateOfAgreementDocument = new DateReference{ Value = DateTime.UtcNow }
                }
            }
        };

        var extractedJson = JsonConvert.SerializeObject(extractedInfo);
        _contractExtractionsClient
            .Setup(client => client.ExtractInfoAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Token>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(extractedJson);

        var command = new ExtractContractFileCommand(contract, new MemoryStream(Encoding.UTF8.GetBytes("dummy data")), "contract.pdf");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Contract.AgreementValidFrom.ShouldBe(extractedInfo.ExtractedInformation.AgreementTerm.AgreementDateValidFrom.Value);
        result.Data.Contract.AgreementValidTo.ShouldBe(extractedInfo.ExtractedInformation.AgreementTerm.AgreementDateValidTo.Value);
        result.Data.Contract.PaymentTermInDays.ShouldBe(extractedInfo.ExtractedInformation.FinancialProvisions.PaymentTermInDays.Value);
        result.Data.Contract.AgreementDate.ShouldBe(extractedInfo.ExtractedInformation.DocumentMetadata.DateOfAgreementDocument.Value);
        result.Data.RegistrationNumber.ShouldBe(extractedInfo.ExtractedInformation.AgreementParties.SupplierRegistrationNumber.Value);
        result.Data.SupplierName.ShouldBe(extractedInfo.ExtractedInformation.AgreementParties.SupplierName.Value);
    }
}
