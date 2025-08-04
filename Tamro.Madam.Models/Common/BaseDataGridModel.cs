using System.Collections.Concurrent;
using System.Reflection;

namespace Tamro.Madam.Models.Common;
public abstract class BaseDataGridModel<T> : IEquatable<T> where T : BaseDataGridModel<T>
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> KeyPropertyCache = new();

    public override bool Equals(object obj)
    {
        return Equals(obj as T);
    }

    public bool Equals(T other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return GetKeyValues().SequenceEqual(other.GetKeyValues());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var value in GetKeyValues())
        {
            hash.Add(value);
        }
        return hash.ToHashCode();
    }

    public static bool operator ==(BaseDataGridModel<T> a, BaseDataGridModel<T> b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(BaseDataGridModel<T> a, BaseDataGridModel<T> b)
    {
        return !(a == b);
    }

    private static PropertyInfo[] GetKeyProperties()
    {
        return KeyPropertyCache.GetOrAdd(typeof(T), type =>
        {
            var keyProperties = type.GetProperties()
                .Where(p => p.IsDefined(typeof(DataGridIdentifierAttribute)))
                .ToArray();

            if (keyProperties.Length == 0)
            {
                var idProperty = type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (idProperty != null)
                {
                    return [idProperty];
                }
            }

            if (keyProperties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"The type '{type.Name}' must have at least one property marked with the [{nameof(DataGridIdentifierAttribute)}] or a public property named 'Id'."
                );
            }
            return keyProperties;
        });
    }

    private IEnumerable<object> GetKeyValues() => GetKeyProperties().Select(p => p.GetValue(this));
}
