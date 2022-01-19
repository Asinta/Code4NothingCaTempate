namespace Code4Nothing.Domain.Common.Interfaces;

public interface IEntity<T>
{
    public T Id { get; set; }
}