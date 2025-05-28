using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class AddDishToOrderHandler(IOrderRepository orderRepo) : IRequestHandler<AddDishToOrder, Order>
{
    public Task<Order> Handle(AddDishToOrder command, CancellationToken cancellationToken)
    {
        var order = orderRepo
            .FindById(command.OrderId)
             ?? throw new NullReferenceException("Заказ не найден");

        var dish = Dish.Create(Guid.NewGuid(), command.Name, command.Price);
        
        order.AddItem(dish, command.Quantity);
        orderRepo.SaveChanges(order);
        
        return Task.FromResult(order);
    }
}
