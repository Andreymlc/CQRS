using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetOrderByIdHandler(IOrderViewRepository orderRepo) : IRequestHandler<GetOrderById, OrderView>
{
    public Task<OrderView> Handle(GetOrderById query, CancellationToken cancellationToken) =>
        Task.FromResult(orderRepo.FindById(query.Id) ?? throw new NullReferenceException("Заказ не найден"));
}
