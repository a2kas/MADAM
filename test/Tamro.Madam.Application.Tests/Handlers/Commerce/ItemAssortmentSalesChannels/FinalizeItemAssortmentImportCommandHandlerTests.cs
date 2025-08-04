using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

namespace Tamro.Madam.Application.Tests.Handlers.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class FinalizeItemAssortmentImportCommandHandlerTests
{
    private FinalizeItemAssortmentImportCommandHandler _finalizeItemAssortmentImportCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _finalizeItemAssortmentImportCommandHandler = new FinalizeItemAssortmentImportCommandHandler();
    }

    [TestCase(ItemAssortmentImportAction.Extend)]
    [TestCase(ItemAssortmentImportAction.Replace)]
    public async Task Handle_ItemNameIsEmpty_AddsAsNotFound(ItemAssortmentImportAction importAction)
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            ImportAction = importAction,
            ImportedAssortment =
            [
                new ItemAssortmentItemModel()
                {
                    ItemNo = "123W",
                    ItemName = string.Empty,
                },
            ]
        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(0);
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().IsSuccess.ShouldBeFalse();
        result.Data.Overview.First().Comment.ShouldBe("Item was not found");
    }

    [Test]
    public async Task Handle_ImportActionIsReplace_ItemDidntExistBefore_ItemIsAddedToAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Replace,
            ImportedAssortment =
            [
                new ItemAssortmentItemModel()
                {
                    ItemNo = "123W",
                    ItemName = "ItemName",
                    ItemBindingId = 4,
                },
            ]
        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(1);
        result.Data.Assortment.First().ItemName.ShouldBe("ItemName");
        result.Data.Assortment.First().ItemAssortmentSalesChannelId.ShouldBe(5);
        result.Data.Assortment.First().ItemBindingId.ShouldBe(4);
        result.Data.Assortment.First().ItemCode.ShouldBe("123W");
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().ItemNo.ShouldBe("123W");
        result.Data.Overview.First().ItemName.ShouldBe("ItemName");
        result.Data.Overview.First().Comment.ShouldBe("Added to assortment");
    }

    [Test]
    public async Task Handle_ImportActionIsReplace_ItemExistedBefore_ItemRemainsAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Replace,
            ImportedAssortment =
            [
                new ItemAssortmentItemModel()
                {
                    ItemNo = "123W",
                    ItemName = "ItemName",
                    ItemBindingId = 4,
                },
            ],
            ExistingAssortment = 
            [
                new ItemAssortmentGridModel() 
                {
                    ItemCode = "123W",
                    ItemName = "ItemName",
                    ItemAssortmentSalesChannelId = 5,
                    ItemBindingId = 4,
                    Id = 6,
                }
            ],
            
        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(1);
        result.Data.Assortment.First().ItemName.ShouldBe("ItemName");
        result.Data.Assortment.First().ItemAssortmentSalesChannelId.ShouldBe(5);
        result.Data.Assortment.First().ItemBindingId.ShouldBe(4);
        result.Data.Assortment.First().ItemCode.ShouldBe("123W");
        result.Data.Assortment.First().Id.ShouldBe(6);
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().ItemNo.ShouldBe("123W");
        result.Data.Overview.First().ItemName.ShouldBe("ItemName");
        result.Data.Overview.First().Comment.ShouldBe("Remains in assortment");
    }

    [Test]
    public async Task Handle_ImportActionIsReplace_ItemExistedBefore_ButIsRemovedNow_ItemIsRemovedFromAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Replace,
            ExistingAssortment =
            [
                new ItemAssortmentGridModel()
                {
                    ItemCode = "123W",
                    ItemName = "ItemName",
                    ItemAssortmentSalesChannelId = 5,
                    ItemBindingId = 4,
                    Id = 6,
                }
            ],

        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(0);
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().ItemNo.ShouldBe("123W");
        result.Data.Overview.First().ItemName.ShouldBe("ItemName");
        result.Data.Overview.First().Comment.ShouldBe("Removed from assortment");
    }

    [Test]
    public async Task Handle_ImportActionIsExtend_ItemDidntExistBefore_ItemIsAddedToAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Extend,
            ImportedAssortment =
            [
                new ItemAssortmentItemModel()
                {
                    ItemNo = "123W",
                    ItemName = "ItemName",
                    ItemBindingId = 4,
                },
            ]
        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(1);
        result.Data.Assortment.First().ItemName.ShouldBe("ItemName");
        result.Data.Assortment.First().ItemAssortmentSalesChannelId.ShouldBe(5);
        result.Data.Assortment.First().ItemBindingId.ShouldBe(4);
        result.Data.Assortment.First().ItemCode.ShouldBe("123W");
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().ItemNo.ShouldBe("123W");
        result.Data.Overview.First().ItemName.ShouldBe("ItemName");
        result.Data.Overview.First().Comment.ShouldBe("Added to assortment");
    }

    [Test]
    public async Task Handle_ImportActionIsExtend_ItemExistedBefore_ItemRemainsAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Extend,
            ImportedAssortment =
            [
                new ItemAssortmentItemModel()
                {
                    ItemNo = "123W",
                    ItemName = "ItemName",
                    ItemBindingId = 4,
                },
            ],
            ExistingAssortment =
            [
                new ItemAssortmentGridModel()
                {
                    ItemCode = "123W",
                    ItemName = "ItemName",
                    ItemAssortmentSalesChannelId = 5,
                    ItemBindingId = 4,
                    Id = 6,
                }
            ],

        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(1);
        result.Data.Assortment.First().ItemName.ShouldBe("ItemName");
        result.Data.Assortment.First().ItemAssortmentSalesChannelId.ShouldBe(5);
        result.Data.Assortment.First().ItemBindingId.ShouldBe(4);
        result.Data.Assortment.First().ItemCode.ShouldBe("123W");
        result.Data.Assortment.First().Id.ShouldBe(6);
        result.Data.Overview.Count().ShouldBe(1);
        result.Data.Overview.First().ItemNo.ShouldBe("123W");
        result.Data.Overview.First().ItemName.ShouldBe("ItemName");
        result.Data.Overview.First().Comment.ShouldBe("Remains in assortment");
    }

    [Test]
    public async Task Handle_ImportActionIsExtend_ItemExistedBefore_ButIsRemovedNow_ItemRemainsInAssortment()
    {
        // Arrange
        var model = new ItemAssortmentFinalizeImportModel()
        {
            SalesChannelId = 5,
            ImportAction = ItemAssortmentImportAction.Extend,
            ExistingAssortment =
            [
                new ItemAssortmentGridModel()
                {
                    ItemCode = "123W",
                    ItemName = "ItemName",
                    ItemAssortmentSalesChannelId = 5,
                    ItemBindingId = 4,
                    Id = 6,
                }
            ],

        };
        var request = new FinalizeItemAssortmentImportCommand(model);

        // Act
        var result = await _finalizeItemAssortmentImportCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Assortment.Count().ShouldBe(1);
    }
}