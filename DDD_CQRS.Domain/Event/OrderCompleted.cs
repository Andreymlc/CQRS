using MediatR;

namespace DDD_CQRS.Domain.Event;

public class OrderCompleted : Event, INotification
{
    public required Guid OrderId { get; init; }
}
