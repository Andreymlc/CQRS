using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;

namespace DDD_CQRS.Infrastructure.Repository;

public class ReportRepository : IReportRepository
{
    private static readonly List<Report> _report = 
    [new()
    {
        OrdersQuantity = 0,
        NumberCompletedOrders = 0,
        Income = 0,
        MostPopularProducts = ("", 0),
        Reason = "Запуск програмы"
    }];

    public IReadOnlyCollection<Report> GetLastReport(int quantityReports) =>
        _report.TakeLast(quantityReports).ToList();

    public void AddReport(Report report) => _report.Add(report);
}
