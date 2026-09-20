namespace TechStore.API.DTO;

public record OrderItemDto(int ProductId, int Quantity);

public record CreateOrderDto(int CustomerId, int AddressId, List<OrderItemDto> Items);

public record OrderResponseDto(int Id, string Status, decimal TotalAmount, DateTime CreatedAt);
