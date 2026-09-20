using TechStore.Domain.Enums;

namespace TechStore.Domain.Entities;

public class Payment : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
}
