using System.Reflection;
using AutoMapper;
using NUnit.Framework;
using Tamro.Madam.Application.Profiles.ItemMasterdata;

namespace Tamro.Madam.Application.Tests.Profiles;

public class AutoMapperSanityTests
{
    private MapperConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        _configuration = new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(BarcodeProfile))));
    }

    [Test]
    public void AssertConfigurationIsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }
}
