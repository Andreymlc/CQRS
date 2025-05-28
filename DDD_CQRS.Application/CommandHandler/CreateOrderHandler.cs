using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class CreateOrderHandler(IOrderRepository orderRepo) : IRequestHandler<CreateOrder, Order>
{
    public Task<Order> Handle(CreateOrder command, CancellationToken cancellationToken)
    {
        var order = new Order(command.OrderItems);
        
        orderRepo.Add(order);
        orderRepo.SaveChanges(order);
        
        return Task.FromResult(order);
    }
}
