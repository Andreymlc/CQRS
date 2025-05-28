using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetReportHandler(IReportRepository reportRepo) 
    : IRequestHandler<GetReport, IReadOnlyCollection<Report>>
{
    public Task<IReadOnlyCollection<Report>> Handle(GetReport query, CancellationToken cancellationToken) => 
        Task.FromResult(reportRepo.GetLastReport(query.QuantityReports));
}
