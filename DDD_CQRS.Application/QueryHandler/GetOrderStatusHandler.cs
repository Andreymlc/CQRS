using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetOrderStatusHandler(IOrderRepository orderRepo) : IRequestHandler<GetOrderStatus, OrderStatus>
{
    public Task<OrderStatus> Handle(GetOrderStatus query, CancellationToken cancellationToken)
    {
        var order = orderRepo
                        .FindById(query.OrderId)
                         ?? throw new NullReferenceException("Заказ не найден");

        return Task.FromResult(order.Status);
    }
}
