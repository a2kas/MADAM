using Tamro.Madam.Repository.Entities;

namespace Tamro.Madam.Repository.Context.Madam;

public interface IMadamEntity<T> : IMadamEntity, IEntity<T>
{
}

public interface IMadamEntity : IEntity
{
}
