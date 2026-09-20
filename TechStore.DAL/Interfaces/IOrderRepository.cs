using TechStore.Domain.Entities;

namespace TechStore.DAL.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetWithItemsAsync(int orderId);
    Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
}
