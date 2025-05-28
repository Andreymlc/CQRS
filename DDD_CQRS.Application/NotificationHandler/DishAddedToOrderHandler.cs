using DDD_CQRS.Application.Helper;
using DDD_CQRS.Domain.Event;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class DishAddedToOrderHandler(IOrderViewRepository orderRepo, IReportRepository reportRepo)
    : INotificationHandler<DishAddedToOrderEvent>
{
    public Task Handle(DishAddedToOrderEvent notification, CancellationToken cancellationToken)
    {
        var orderView = orderRepo
            .FindById(notification.OrderId)
             ?? throw new NullReferenceException("Модель чтения заказа не найдена");

        orderView.AddItem(OrderHelper.Map(notification.Dish), notification.Quantity);
        OrderHelper.UpdateProductSales(notification.Dish.Name , notification.Quantity);
        
        var allOrders = orderRepo.FindAll();
        var lastReport = reportRepo.GetLastReport(1).First();
        
        var report = new Report
        {
            OrdersQuantity = allOrders.Count,
            Income = allOrders.Sum(o => o.Cost),
            MostPopularProducts = OrderHelper.GetMostPopularProduct(),
            NumberCompletedOrders = lastReport.NumberCompletedOrders,
            Reason = notification.Description
        };
        
        reportRepo.AddReport(report);

        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
