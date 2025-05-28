using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;

namespace DDD_CQRS.Infrastructure.Repository;

public class OrderViewRepository : IOrderViewRepository
{
    private static readonly Dictionary<Guid, OrderView> _orders = new();
    
    public OrderView FindById(Guid id) => _orders[id];

    public IReadOnlyList<OrderView> FindAll() => _orders.Values.ToList();

    public void Add(OrderView order) => _orders[order.Id] = order;
}
