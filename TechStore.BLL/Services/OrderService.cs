using TechStore.BLL.Interfaces;
using TechStore.DAL.Interfaces;
using TechStore.Domain.Entities;
using TechStore.Domain.Enums;

namespace TechStore.BLL.Services;

/// <summary>
/// Бизнес-логика работы с заказами. Слой не знает ни о СУБД, ни о HTTP —
/// он работает только с абстракциями репозиториев.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;

    public OrderService(IUnitOfWork uow) => _uow = uow;

    public async Task<int> CreateOrderAsync(int customerId, int addressId,
                                            IEnumerable<(int ProductId, int Qty)> items)
    {
        var itemList = items.ToList();
        if (itemList.Count == 0)
            throw new ArgumentException("Заказ не может быть пустым.", nameof(items));

        var customer = await _uow.Customers.GetByIdAsync(customerId)
            ?? throw new KeyNotFoundException($"Клиент {customerId} не найден.");

        var order = new Order
        {
            CustomerId = customer.Id,
            AddressId = addressId,
            Status = OrderStatus.New
        };

        foreach (var (productId, qty) in itemList)
        {
            if (qty <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            var product = await _uow.Products.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException($"Товар {productId} не найден.");

            if (product.StockQty < qty)
                throw new InvalidOperationException(
                    $"Недостаточно товара «{product.Name}»: на складе {product.StockQty}, требуется {qty}.");

            product.StockQty -= qty;
            _uow.Products.Update(product);

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = qty,
                UnitPrice = product.Price
            });
        }

        order.RecalculateTotal();
        await _uow.Orders.AddAsync(order);
        await _uow.SaveChangesAsync();
        return order.Id;
    }

    public async Task ChangeStatusAsync(int orderId, OrderStatus newStatus, int? employeeId)
    {
        var order = await _uow.Orders.GetByIdAsync(orderId)
            ?? throw new KeyNotFoundException($"Заказ {orderId} не найден.");

        if (!IsTransitionAllowed(order.Status, newStatus))
            throw new InvalidOperationException(
                $"Переход {order.Status} -> {newStatus} запрещён бизнес-правилами.");

        order.StatusHistory.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            EmployeeId = employeeId,
            OldStatus = order.Status,
            NewStatus = newStatus
        });

        order.Status = newStatus;
        order.EmployeeId ??= employeeId;
        _uow.Orders.Update(order);
        await _uow.SaveChangesAsync();
    }

    public Task<Order?> GetOrderAsync(int orderId) => _uow.Orders.GetWithItemsAsync(orderId);

    public Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId) =>
        _uow.Orders.GetByCustomerAsync(customerId);

    /// <summary>Матрица допустимых переходов статусов заказа.</summary>
    private static bool IsTransitionAllowed(OrderStatus from, OrderStatus to) => from switch
    {
        OrderStatus.New       => to is OrderStatus.Confirmed or OrderStatus.Cancelled,
        OrderStatus.Confirmed => to is OrderStatus.Paid or OrderStatus.Cancelled,
        OrderStatus.Paid      => to is OrderStatus.Shipped or OrderStatus.Cancelled,
        OrderStatus.Shipped   => to is OrderStatus.Delivered,
        _                     => false
    };
}
