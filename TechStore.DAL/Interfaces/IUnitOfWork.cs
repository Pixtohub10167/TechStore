namespace TechStore.DAL.Interfaces;

/// <summary>Координирует несколько репозиториев в рамках одной транзакции.</summary>
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IRepository<Domain.Entities.Product> Products { get; }
    IRepository<Domain.Entities.Customer> Customers { get; }
    Task<int> SaveChangesAsync();
}
