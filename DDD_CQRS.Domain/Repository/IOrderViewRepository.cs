using DDD_CQRS.Domain.Views;

namespace DDD_CQRS.Domain.Repository;

public interface IOrderViewRepository
{
    OrderView? FindById(Guid id);

    IReadOnlyList<OrderView> FindAll();

    void Add(OrderView order);
}
