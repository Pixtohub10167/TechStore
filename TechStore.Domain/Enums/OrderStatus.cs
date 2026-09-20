namespace TechStore.Domain.Enums;

/// <summary>Жизненный цикл заказа.</summary>
public enum OrderStatus
{
    New = 0,
    Confirmed = 1,
    Paid = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}
