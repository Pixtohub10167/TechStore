using Microsoft.EntityFrameworkCore;
using TechStore.DAL.Context;
using TechStore.DAL.Interfaces;
using TechStore.Domain.Entities;

namespace TechStore.DAL.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Db;
    protected readonly DbSet<T> Set;

    public GenericRepository(AppDbContext db)
    {
        Db = db;
        Set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await Set.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await Set.AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity) => await Set.AddAsync(entity);

    public void Update(T entity) => Set.Update(entity);

    public void Delete(T entity) => Set.Remove(entity);
}
