using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Tests.Profiles.Sales.CanceledOrderLines;

[TestFixture]
public class CanceledOrderLineProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(CanceledOrderLineProfile)))));
    }

    [Test]
    public void E1CanceledOrderLine_To_CanceledOrderLineGridModel_MapsCorrectly()
    {
        // Arrange
        var source = new E1CanceledOrderLine()
        {
            ItemNo2 = "WD-40",
        };

        // Act
        var destination = _mapper.Map<CanceledOrderLineGridModel>(source);

        // Assert
        destination.ItemNo.ShouldBe("WD-40");
    }
}