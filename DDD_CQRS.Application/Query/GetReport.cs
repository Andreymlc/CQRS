using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetReport : IRequest<IReadOnlyCollection<Report>>
{
    public int QuantityReports { get; set; }
}
