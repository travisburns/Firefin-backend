namespace Firefin.Api.Models;

/// <summary>Lifecycle of a customer order.</summary>
public enum OrderStatus
{
    Pending,
    Paid,
    Fulfilled,
    Cancelled
}
