using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class RemoveDishFromOrderHandler(IOrderRepository orderRepo) : IRequestHandler<RemoveDishFromOrder, Order>
{
    public Task<Order> Handle(RemoveDishFromOrder command, CancellationToken cancellationToken)
    {
        var order = orderRepo
                        .FindById(command.OrderId)
                    ?? throw new NullReferenceException("Заказ не найден");
        
        order.RemoveItem(command.DishId);
        orderRepo.SaveChanges(order);
        
        return Task.FromResult(order);
    }
}
