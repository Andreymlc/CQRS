using MediatR;

namespace DDD_CQRS.Domain.Event;

public class OrderCreated : Event, INotification
{
    public required Order Order { get; init; }
}
