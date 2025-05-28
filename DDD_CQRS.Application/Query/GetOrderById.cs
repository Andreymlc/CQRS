using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetOrderById : IRequest<OrderView>
{
    public required Guid Id { get; init; }
}
