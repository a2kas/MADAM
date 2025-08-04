using System.Reflection;
using AutoMapper;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Profiles.Items.Bindings;

[TestFixture]
public class ItemBindingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemBindingProfile)))));
    }

    [Test]
    public void ItemBinding_To_ItemBindingModel_MapsCorrectly()
    {
        // Arrange
        var itemBinding = new ItemBinding()
        {
            Company = "00501",
        };

        // Act
        var itemBindingModel = _mapper.Map<ItemBindingModel>(itemBinding);

        // Assert
        itemBindingModel.Company.ShouldBe(Classifiers.Companies.FirstOrDefault(x => x.Value == "00501"));
    }

    [Test]
    public void ItemBinding_To_ItemBindingModel_NoCompany_MapsToEmptyCompany()
    {
        // Arrange
        var itemBinding = new ItemBinding()
        {
            Company = "XYZ",
        };

        // Act
        var itemBindingModel = _mapper.Map<ItemBindingModel>(itemBinding);

        // Assert
        itemBindingModel.Company.ShouldNotBeNull();
        itemBindingModel.Company.Name.ShouldBeNull();
        itemBindingModel.Company.Value.ShouldBeNull();
    }

    [Test]
    public void ItemBindingModel_ToItemBinding_MapsCorrectly()
    {
        // Arrange
        var itemBindingModel = new ItemBindingModel()
        {
            Company = new Company()
            {
                Value = "883",
            },
            LocalId = " W0004 "
        };

        // Act
        var itemBinding = _mapper.Map<ItemBinding>(itemBindingModel);

        // Assert
        itemBinding.Company.ShouldBe("883");
        itemBinding.LocalId.ShouldBe("W0004");
    }

    [Test]
    public void ItemBindingLanguage_ToItemBindingLanguageModel_MapsCorrectly()
    {
        // Arrange
        var itemBindingLanguage = new ItemBindingLanguage()
        {
            LanguageId = 1,
        };
        var language = Classifiers.Languages.FirstOrDefault(x => x.Id == 1);

        // Act
        var itemBindingLanguageModel = _mapper.Map<ItemBindingLanguageModel>(itemBindingLanguage);

        // Assert
        itemBindingLanguageModel.Language.Code.ShouldBe(language.Code);
        itemBindingLanguageModel.Language.Name.ShouldBe(language.Name);
        itemBindingLanguageModel.Language.Id.ShouldBe(language.Id);
    }

    [Test]
    public void ItemBindingLanguage_To_ItemBindingLanguageModel_NoLanguage_MapsToEmptyCompany()
    {
        // Arrange
        var itemBindingLanguage = new ItemBindingLanguage()
        {
            LanguageId = 832832,
        };

        // Act
        var itemBindingLanguageModel = _mapper.Map<ItemBindingLanguageModel>(itemBindingLanguage);

        // Assert
        itemBindingLanguageModel.Language.ShouldNotBeNull();
        itemBindingLanguageModel.Language.Name.ShouldBeNull();
    }

    [Test]
    public void ItemBinding_To_ItemBindingClsfModel_MapsCorrectly()
    {
        // Arrange
        var itemBinding = new ItemBinding()
        {
            LocalId = "W231",
            Item = new Item()
            {
                ItemName = "Wtest",
            },
        };

        // Act
        var itemBindingClsfModel = _mapper.Map<ItemBindingClsfModel>(itemBinding);

        // Assert
        itemBindingClsfModel.ItemNo2.ShouldBe("W231");
        itemBindingClsfModel.Name.ShouldBe("Wtest");
    }

    [Test]
    public void ItemBinding_To_ItemAssortmentItemModel_MapsCorrectly()
    {
        // Arrange
        var src = new ItemBinding()
        {
            LocalId = "W004",
            Id = 4,
            Item = new Item()
            {
                ItemName = "Ibumetin",
            },
        };

        // Act
        var dest = _mapper.Map<ItemAssortmentItemModel>(src);

        // Assert
        dest.ItemNo.ShouldBe("W004");
        dest.ItemName.ShouldBe("Ibumetin");
        dest.ItemBindingId.ShouldBe(4);
    }
}