using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Customers.Sks;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Profiles.Customers.Wholesale;

[TestFixture]
public class WholesaleCustomerProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(WholesaleCustomerProfile)))));
    }

    [Test]
    public void LvWholesaleCustomer_To_WholesaleCustomerClsfModel_MapsCorrectly()
    {
        // Arrange
        var source = new LvWholesaleCustomer()
        {
            MailingName = "DZ",
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerClsfModel>(source);

        // Assert
        destination.Name.ShouldBe("DZ");
    }

    [Test]
    public void LtWholesaleCustomer_To_WholesaleCustomerClsfModel_MapsCorrectly()
    {
        // Arrange
        var source = new LtWholesaleCustomer()
        {
            MailingName = "DZ",
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerClsfModel>(source);

        // Assert
        destination.Name.ShouldBe("DZ");
    }

    [Test]
    public void EeWholesaleCustomer_To_WholesaleCustomerClsfModel_MapsCorrectly()
    {
        // Arrange
        var source = new EeWholesaleCustomer()
        {
            MailingName = "DZ",
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerClsfModel>(source);

        // Assert
        destination.Name.ShouldBe("DZ");
    }

    [Test]
    public void LvWholesaleCustomer_To_WholesaleCustomerModel_MapsCorrectly()
    {
        // Arrange
        var source = new LvWholesaleCustomer()
        {
            ElectronicAddress = "dz@dz.lv",
            AddressNumber2 = 1,
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerModel>(source);

        // Assert
        destination.EmailAddress.ShouldBe("dz@dz.lv");
        destination.LegalEntityNumber.ShouldBe(1);
    }

    [Test]
    public void EeWholesaleCustomer_To_WholesaleCustomerModel_MapsCorrectly()
    {
        // Arrange
        var source = new EeWholesaleCustomer()
        {
            ElectronicAddress = "dz@dz.lv",
            AddressNumber2 = 1,
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerModel>(source);

        // Assert
        destination.EmailAddress.ShouldBe("dz@dz.lv");
        destination.LegalEntityNumber.ShouldBe(1);
    }

    [Test]
    public void OrderNotificationEmail_To_WholesaleCustomerModel_MapsCorrectly()
    {
        // Arrange
        var source = new OrderNotificationEmail()
        {
            Email = "dz@dz.lv",
            AddressNumber = "1",
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerModel>(source);

        // Assert
        destination.EmailAddress.ShouldBe("dz@dz.lv");
        destination.LegalEntityNumber.ShouldBe(1);
    }
}