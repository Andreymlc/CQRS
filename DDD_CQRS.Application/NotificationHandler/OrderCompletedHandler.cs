using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Event;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class OrderCompletedHandler(IOrderViewRepository orderRepo, IReportRepository reportRepo) : INotificationHandler<OrderCompleted>
{
    public Task Handle(OrderCompleted notification, CancellationToken cancellationToken)
    {
        var viewOrder = orderRepo
            .FindById(notification.OrderId)
             ?? throw new NullReferenceException("Модель чтения заказа не найдена");
        
        viewOrder.ChangeStatus(OrderStatus.Completed);
        
        var lastReport = reportRepo.GetLastReport(1).First();
        var report = new Report
        {
            OrdersQuantity = lastReport.OrdersQuantity,
            Income = lastReport.Income,
            MostPopularProducts = lastReport.MostPopularProducts,
            NumberCompletedOrders = lastReport.NumberCompletedOrders + 1,
            Reason = notification.Description
        };
        
        reportRepo.AddReport(report);
        
        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
