namespace Tamro.Madam.Repository.Entities;

public interface IEntity
{

}

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}
