using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetAllOrdersHandler(IOrderViewRepository orderRepo) : IRequestHandler<GetAllOrders, IReadOnlyList<OrderView>>
{
    public Task<IReadOnlyList<OrderView>> Handle(GetAllOrders request, CancellationToken cancellationToken) => 
        Task.FromResult(orderRepo.FindAll());
}
