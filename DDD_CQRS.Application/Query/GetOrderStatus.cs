using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetOrderStatus : IRequest<OrderStatus>
{
    public required Guid OrderId { get; init; }
}
