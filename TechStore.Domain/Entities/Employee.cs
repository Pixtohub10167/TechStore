namespace TechStore.Domain.Entities;

public class Employee : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Login { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
