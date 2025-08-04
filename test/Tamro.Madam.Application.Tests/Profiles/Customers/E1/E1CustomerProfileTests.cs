using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Customers.E1;

namespace Tamro.Madam.Application.Tests.Profiles.Customers.E1;

[TestFixture]
public class E1CustomerProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(E1CustomerProfile)))));
    }

    [Test]
    public void LvWholesaleCustomer_To_WholesaleCustomerClsfModel_MapsCorrectly()
    {
        // Arrange
        var source = new Customer()
        {
            MailingName = "DZ",
        };

        // Act
        var destination = _mapper.Map<WholesaleCustomerClsfModel>(source);

        // Assert
        destination.Name.ShouldBe("DZ");
    }
}