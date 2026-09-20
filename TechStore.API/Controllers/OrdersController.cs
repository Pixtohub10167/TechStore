using Microsoft.AspNetCore.Mvc;
using TechStore.API.DTO;
using TechStore.BLL.Interfaces;
using TechStore.Domain.Enums;

namespace TechStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        try
        {
            var id = await _orders.CreateOrderAsync(
                dto.CustomerId, dto.AddressId,
                dto.Items.Select(i => (i.ProductId, i.Quantity)));

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orders.GetOrderAsync(id);
        if (order is null) return NotFound();

        return Ok(new OrderResponseDto(order.Id, order.Status.ToString(),
                                       order.TotalAmount, order.CreatedAt));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] OrderStatus status,
                                                  [FromQuery] int? employeeId)
    {
        try
        {
            await _orders.ChangeStatusAsync(id, status, employeeId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }
}
