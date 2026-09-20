using TechStore.DAL.Context;
using TechStore.DAL.Interfaces;
using TechStore.Domain.Entities;

namespace TechStore.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
        Orders = new OrderRepository(db);
        Products = new GenericRepository<Product>(db);
        Customers = new GenericRepository<Customer>(db);
    }

    public IOrderRepository Orders { get; }
    public IRepository<Product> Products { get; }
    public IRepository<Customer> Customers { get; }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();

    public void Dispose() => _db.Dispose();
}
