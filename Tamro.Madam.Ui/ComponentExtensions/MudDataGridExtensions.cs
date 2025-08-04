using MudBlazor;

namespace Tamro.Madam.Ui.ComponentExtensions;

public static class MudDataGridExtensions<T>
{
    /// <summary>
    /// Workaround used for DataGrid when default filter is set programatically, but columns are not rendered yet
    /// </summary>
    /// <param name="filterDefinitions">Existing filter definitions</param>
    /// <param name="columns">Currently rendered columns</param>
    public static void ApplyDefaultFilterColumns(ICollection<IFilterDefinition<T>> filterDefinitions, ICollection<Column<T>> columns)
    {
        if (filterDefinitions == null || !filterDefinitions.Any() || columns == null || !columns.Any())
        {
            return;
        }

        UpdateMissingColumnFilterDefinitions(filterDefinitions, columns);
    }

    /// <summary>
    /// Resets grid data. Sets first page, clears all applied filters and removes sort definitions
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public async static Task ResetGridState(MudDataGrid<T> grid)
    {
        grid.CurrentPage = 0;
        await grid.ClearFiltersAsync();

        foreach (var sortDefinition in grid.SortDefinitions)
        {
            await grid.RemoveSortAsync(sortDefinition.Key);
        }
    }

    private static void UpdateMissingColumnFilterDefinitions(ICollection<IFilterDefinition<T>> filterDefinitions, ICollection<Column<T>> columns)
    {
        var missingColumnFilterDefinitions = filterDefinitions.Where(x => x.Column is null);

        foreach (var missingColumnFilterDefinition in missingColumnFilterDefinitions)
        {
            var matchingColumn = columns.SingleOrDefault(x => x.PropertyName == missingColumnFilterDefinition.Title);

            if (matchingColumn != null)
            {
                missingColumnFilterDefinition.Column = matchingColumn;
            }
        }
    }
}
