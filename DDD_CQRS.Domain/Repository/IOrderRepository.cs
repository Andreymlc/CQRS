namespace DDD_CQRS.Domain.Repository;

public interface IOrderRepository
{
    Order? FindById(Guid id);

    void Add(Order order);
    
    void SaveChanges(Order order);
}
