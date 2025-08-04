using Shouldly;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Ui.Tests.ComponentExtensions;

[TestFixture]
public class MudDataGridExtensionsTests
{
    [Test]
    public void ApplyDefaultFilterColumns_MatchingColumns_UpdateFilterDefinitions()
    {
        // Arrange
        var filterDefinitions = new List<Mock<IFilterDefinition<int>>>();
        var columns = new List<Column<int>> { new DummyColumn<int>() };

        foreach (var column in columns)
        {
            var mockFilterDefinition = new Mock<IFilterDefinition<int>>();
            mockFilterDefinition.SetupGet(fd => fd.Title).Returns(column.PropertyName);
            filterDefinitions.Add(mockFilterDefinition);
        }

        // Act
        MudDataGridExtensions<int>.ApplyDefaultFilterColumns(filterDefinitions.Select(fd => fd.Object).ToList(), columns);

        // Assert
        filterDefinitions[0].VerifySet(fd => fd.Column = It.IsAny<Column<int>>(), Times.Once);
    }

    [Test]
    public void ApplyDefaultFilterColumns_NoMatchingColumns_ColumnsNotUpdated()
    {
        // Arrange
        var filterDefinitions = new List<Mock<IFilterDefinition<int>>>();
        filterDefinitions.Add(new Mock<IFilterDefinition<int>>());
        var columns = new List<Column<int>> { new DummyColumn<int>(), };

        // Act
        MudDataGridExtensions<int>.ApplyDefaultFilterColumns(filterDefinitions.Select(fd => fd.Object).ToList(), columns);

        // Assert
        filterDefinitions[0].VerifySet(fd => fd.Column = It.IsAny<Column<int>>(), Times.Never);
    }

    [Test]
    public async Task ResetGridState_ResetsPage()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddMudServices();
        var grid = ctx.RenderComponent<MudDataGrid<ItemGridModel>>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
        );

        // Act
        await MudDataGridExtensions<ItemGridModel>.ResetGridState(grid.Instance);

        // Assert
        grid.Instance.CurrentPage.ShouldBe(0);
    }

    [Test]
    public async Task ResetGridState_ClearsFilter()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddMudServices();
        var grid = ctx.RenderComponent<MudDataGrid<ItemGridModel>>(parameters => parameters
            .Add(p => p.FilterDefinitions, new List<IFilterDefinition<ItemGridModel>> { new FilterDefinition<ItemGridModel>
            {
                Title = nameof(ItemGridModel.Active),
                Operator = SearchBoolConstants.Is,
                Value = true,
            } })
        );

        // Act
        await MudDataGridExtensions<ItemGridModel>.ResetGridState(grid.Instance);

        // Assert
        grid.Instance.FilterDefinitions.Count.ShouldBe(0);
    }

    [Test]
    public async Task ResetGridState_ClearsSortDefinitions()
    {
        // Arrange
        var grid = new MudDataGrid<ItemGridModel>();
        grid.SortDefinitions.Add(nameof(ItemGridModel.Active),
            new SortDefinition<ItemGridModel>(
                nameof(ItemGridModel.Active),
                false,
                1,
                (item) => item.Active,
                null
            ));

        // Act
        await MudDataGridExtensions<ItemGridModel>.ResetGridState(grid);

        // Assert
        grid.SortDefinitions.Count.ShouldBe(0);
    }
}

public class DummyColumn<T> : Column<T>
{
    public DummyColumn()
    {
        PropertyName = "Test";
    }

    public override string PropertyName { get; }
    protected override object CellContent(T item)
    {
        throw new NotImplementedException();
    }

    protected override object PropertyFunc(T item)
    {
        throw new NotImplementedException();
    }

    protected override void SetProperty(object item, object value)
    {
        throw new NotImplementedException();
    }
}
