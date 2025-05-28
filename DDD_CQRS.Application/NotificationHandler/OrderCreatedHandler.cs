using DDD_CQRS.Application.Helper;
using DDD_CQRS.Domain.Event;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class OrderCreatedHandler(IOrderViewRepository orderRepo, IReportRepository reportRepo)
    : INotificationHandler<OrderCreated>
{
    public Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        orderRepo.Add(OrderHelper.Map(notification.Order));
        OrderHelper.UpdateProductSales(notification.Order);
        
        var lastReport = reportRepo.GetLastReport(1).First();
        var report = new Report
        {
            OrdersQuantity = lastReport.OrdersQuantity + 1,
            Income = lastReport.Income + notification.Order.Cost,
            MostPopularProducts = OrderHelper.GetMostPopularProduct(),
            NumberCompletedOrders = lastReport.NumberCompletedOrders,
            Reason = notification.Description
        };
        
        reportRepo.AddReport(report);
        
        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
