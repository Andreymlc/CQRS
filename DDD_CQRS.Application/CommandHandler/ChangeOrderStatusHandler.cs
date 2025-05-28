using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class ChangeOrderStatusHandler(IOrderRepository orderRepo) : IRequestHandler<ChangeOrderStatus, Order>
{
    public Task<Order> Handle(ChangeOrderStatus command, CancellationToken cancellationToken)
    {
        var order = orderRepo
            .FindById(command.OrderId)
             ?? throw new NullReferenceException("Заказ не найден");
        
        order.ChangeStatus(command.OrderStatus);
        orderRepo.SaveChanges(order);
        
        return Task.FromResult(order);
    }
}
