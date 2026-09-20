using TechStore.Domain.Entities;
using TechStore.Domain.Enums;

namespace TechStore.BLL.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(int customerId, int addressId, IEnumerable<(int ProductId, int Qty)> items);
    Task ChangeStatusAsync(int orderId, OrderStatus newStatus, int? employeeId);
    Task<Order?> GetOrderAsync(int orderId);
    Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
}
