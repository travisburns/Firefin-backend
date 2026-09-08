using Firefin.Api.DTOs;
using Firefin.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Firefin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll()
        => Ok(await _orders.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var order = await _orders.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet("number/{orderNumber}")]
    public async Task<ActionResult<OrderDto>> GetByNumber(string orderNumber)
    {
        var order = await _orders.GetByNumberAsync(orderNumber);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
    {
        var result = await _orders.CreateAsync(dto);
        if (!result.Ok) return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPost("{id:int}/pay")]
    public async Task<ActionResult<OrderDto>> Pay(int id)
    {
        var result = await _orders.PayAsync(id);
        return result.Ok ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}
