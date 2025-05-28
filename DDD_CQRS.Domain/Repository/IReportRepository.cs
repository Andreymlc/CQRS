using DDD_CQRS.Domain.Views;

namespace DDD_CQRS.Domain.Repository;

public interface IReportRepository
{
    IReadOnlyCollection<Report> GetLastReport(int quantityReports);
    void AddReport(Report report);
}
