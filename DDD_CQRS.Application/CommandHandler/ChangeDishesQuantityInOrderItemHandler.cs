using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class ChangeDishesQuantityInOrderItemHandler(IOrderRepository orderRepo)
    : IRequestHandler<ChangeDishesQuantityInOrderItem, Order>
{
    public Task<Order> Handle(ChangeDishesQuantityInOrderItem command, CancellationToken cancellationToken)
    {
        var order = orderRepo
                        .FindById(command.OrderId)
                         ?? throw new NullReferenceException("Заказ не найден");
        
        order.ChangeDishesQuantityInOrderItem(command.NewQuantity, command.DishId);
        orderRepo.SaveChanges(order);
        
        return Task.FromResult(order);
    }
}
