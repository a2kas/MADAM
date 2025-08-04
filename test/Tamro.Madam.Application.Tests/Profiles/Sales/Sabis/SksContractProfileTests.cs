using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Sales.Sabis;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Application.Tests.Profiles.Sales.Sabis;

[TestFixture]
public class SksContractProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SksContractProfile)))));
    }

    [Test]
    public void SksContractMapping_To_SksContractGridModel_MapsCorrectly()
    {
        // Arrange
        var source = new SksContractMapping()
        {
            Customer = new()
            {
                MailingName = "DZ",
            },
            AdditionalTaxId = "123",
        };

        // Act
        var destination = _mapper.Map<SksContractGridModel>(source);

        // Assert
        destination.CustomerName.ShouldBe("DZ");
        destination.CompanyId.ShouldBe("123");
    }

    [Test]
    public void SksContractModel_To_SksContractMapping_MapsCorrectly()
    {
        // Arrange
        var source = new SksContractModel()
        {
            Customer = new WholesaleCustomerClsfModel()
            {
                AddressNumber = 36,
            },
            CompanyId = "6",
            EditedAt = new DateTime(2024, 1, 1),
        };

        // Act
        var destination = _mapper.Map<SksContractMapping>(source);

        // Assert
        destination.AddressNumber.ShouldBe(36);
        destination.AdditionalTaxId.ShouldBe("6");
        destination.RowVer.Year.ShouldBe(2024);
        destination.Customer.ShouldBe(null);
    }

    [Test]
    public void SksContractGridModel_ToSksContractModel_MapsCorrectly()
    {
        // Arrange
        var source = new SksContractGridModel()
        {
            AddressNumber = 36,
            CustomerName = "Ye",
        };

        // Act
        var destination = _mapper.Map<SksContractModel>(source);

        // Assert
        destination.Customer.AddressNumber.ShouldBe(36);
        destination.Customer.Name.ShouldBe("Ye");
    }
}