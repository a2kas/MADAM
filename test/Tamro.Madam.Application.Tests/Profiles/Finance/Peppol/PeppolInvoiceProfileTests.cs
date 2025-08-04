using System.Reflection;
using AutoFixture;
using AutoMapper;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Profiles.Finance.Peppol;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Finance.Peppol;

namespace Tamro.Madam.Application.Tests.Profiles.Finance.Peppol;

[TestFixture]
public class PeppolInvoiceProfileTests
{
    private Fixture _fixture;

    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(PeppolInvoiceProfile)))));
    }

    [TestCase(123, "123")]
    [TestCase(0, "")]
    public void PeppolInvoice_To_PeppolInvoiceGridModel_InvoiceNumber_MapsCorrectly(int invoiceNumber, string expected)
    {
        // Arrange
        var peppolInvoice = _fixture.Create<PeppolInvoice>();
        peppolInvoice.InvoiceNumber = invoiceNumber;

        // Act
        var result = _mapper.Map<PeppolInvoiceGridModel>(peppolInvoice);

        // Assert
        result.InvoiceNumber.ShouldBe(expected);
    }

    [TestCase("123", "123")]
    [TestCase(null, "")]
    public void PeppolInvoice_To_PeppolInvoiceGridModel_ConsolidationNumber_MapsCorrectly(string? consolidationNumber, string expected)
    {
        // Arrange
        var peppolInvoice = _fixture.Create<PeppolInvoice>();
        peppolInvoice.ConsolidationNumber = consolidationNumber;

        // Act
        var result = _mapper.Map<PeppolInvoiceGridModel>(peppolInvoice);

        // Assert
        result.ConsolidationNumber.ShouldBe(expected);
    }
}
