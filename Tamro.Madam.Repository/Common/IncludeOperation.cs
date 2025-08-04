namespace Tamro.Madam.Repository.Common;

public class IncludeOperation<T>
{
    public Func<IQueryable<T>, IQueryable<T>> ApplyInclude { get; }

    public IncludeOperation(Func<IQueryable<T>, IQueryable<T>> applyInclude)
    {
        ApplyInclude = applyInclude;
    }
}
