using MediatR;

namespace DDD_CQRS.Domain.Event;

public class DishAddedToOrderEvent : Event, INotification
{
    public required Guid OrderId { get; init; }
    public required Dish Dish { get; init; }
    public required int Quantity { get; init; }
}
