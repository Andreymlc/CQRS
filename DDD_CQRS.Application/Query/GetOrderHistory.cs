using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetOrderHistory : IRequest<IReadOnlyList<OrderView>>
{
    public required int NumberDays { get; init; }
}
