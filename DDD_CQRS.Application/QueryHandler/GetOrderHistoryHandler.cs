using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetOrderHistoryHandler(IOrderViewRepository orderRepo) : IRequestHandler<GetOrderHistory, IReadOnlyList<OrderView>>
{
    public Task<IReadOnlyList<OrderView>> Handle(GetOrderHistory query, CancellationToken cancellationToken) => 
        Task.FromResult((IReadOnlyList<OrderView>)orderRepo.FindAll().Take(query.NumberDays));
}
