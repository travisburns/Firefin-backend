using Firefin.Api.DTOs;

namespace Firefin.Api.Services;

public interface IOrderService
{
    Task<ServiceResult<OrderDto>> CreateAsync(CreateOrderDto dto);

    /// <summary>Simulates payment capture, moving a pending order to Paid.</summary>
    Task<ServiceResult<OrderDto>> PayAsync(int orderId);

    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto?> GetByNumberAsync(string orderNumber);
    Task<IReadOnlyList<OrderDto>> GetAllAsync();
}
