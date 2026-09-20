using TechStore.Domain.Entities;

namespace TechStore.DAL.Interfaces;

/// <summary>Обобщённый контракт доступа к данным (паттерн «Репозиторий»).</summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
