using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Command;

public class CreateOrder : IRequest<Order>
{
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}
