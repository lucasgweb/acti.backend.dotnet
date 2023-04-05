namespace Acti.Domain.Repositories;

public interface IBaseRepository<T>
{
    Task<T> Add(T entity);
    Task<T> Get(int id);
    Task<T> Update(T entity);
    Task Delete(int id);
    Task<List<T>> Get();
}