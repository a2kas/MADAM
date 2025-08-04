using AutoFixture;
using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.User.UserSettings;
using Tamro.Madam.Models.User.UserSettings;
using Tamro.Madam.Repository.Entities.Users.UserSettings;

namespace Tamro.Madam.Application.Tests.Profiles.User.UserSettings;

[TestFixture]
public class UserSettingsProfileTests
{
    private Fixture _fixture;

    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(UserSettingsProfile)))));
    }

    [Test]
    public void UserSetting_To_UserSettingsModel_MapsCorrectly()
    {
        // Arrange
        var model = _fixture.Create<UserSetting>();

        // Act
        var result = _mapper.Map<UserSettingsModel>(model);

        // Assert
        result.Usability.RowsPerPage.ShouldBe(model.RowsPerPage);
    }

    [Test]
    public void UserSettingsModel_To_UserSetting_MapsCorrectly()
    {
        // Arrange
        var model = _fixture.Create<UserSettingsModel>();

        // Act
        var result = _mapper.Map<UserSetting>(model);

        // Assert
        result.RowsPerPage.ShouldBe(model.Usability.RowsPerPage);
        result.Id.ShouldBeNull();
    }
}
