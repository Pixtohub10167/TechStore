namespace TechStore.Domain.Entities;

public class Delivery : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string CarrierName { get; set; } = null!;
    public string? TrackNumber { get; set; }
    public DateTime? ShippedAt { get; set; }
}
