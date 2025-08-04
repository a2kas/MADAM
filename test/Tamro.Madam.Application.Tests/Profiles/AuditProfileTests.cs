using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles;
using Tamro.Madam.Models.Audit;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Application.Tests.Profiles;

[TestFixture]
public class AuditProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(AuditProfile)))));
    }

    [Test]
    public void Map_DbAuditEntry_To_AuditGridModel_MapsCorrectly()
    {
        // Arrange
        var dbAuditEntry = new DbAuditEntry()
        {
            AuditEntryID = 1,
            EntityID = "6",
        };

        // Act
        var result = _mapper.Map<AuditGridModel>(dbAuditEntry);

        // Assert
        result.Id.ShouldBe(1);
        result.EntityId.ShouldBe("6");
    }
}
