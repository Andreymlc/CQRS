using MediatR;

namespace DDD_CQRS.Domain;

public abstract class Entity<T>
{
    public T Id { get; init; }
    
    private readonly List<INotification> _domainEvents = [];

    public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();
    
    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
