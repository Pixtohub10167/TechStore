using Microsoft.EntityFrameworkCore;
using TechStore.DAL.Context;
using TechStore.DAL.Interfaces;
using TechStore.Domain.Entities;

namespace TechStore.DAL.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext db) : base(db) { }

    public async Task<Order?> GetWithItemsAsync(int orderId) =>
        await Set.Include(o => o.Items).ThenInclude(i => i.Product)
                 .Include(o => o.Customer)
                 .FirstOrDefaultAsync(o => o.Id == orderId);

    public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId) =>
        await Set.AsNoTracking()
                 .Where(o => o.CustomerId == customerId)
                 .OrderByDescending(o => o.CreatedAt)
                 .ToListAsync();
}
