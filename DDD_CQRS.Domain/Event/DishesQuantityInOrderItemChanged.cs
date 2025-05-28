using MediatR;

namespace DDD_CQRS.Domain.Event;

public class DishesQuantityInOrderItemChanged : Event, INotification
{
    public required Guid OrderId { get; init; }
    public required Dish Dish { get; init; }
    public required int OldQuantity { get; init; }
    public required int NewQuantity { get; init; }
}
