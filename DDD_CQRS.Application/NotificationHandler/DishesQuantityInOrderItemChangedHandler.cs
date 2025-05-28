using DDD_CQRS.Application.Helper;
using DDD_CQRS.Domain.Event;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class DishesQuantityInOrderItemChangedHandler(IOrderViewRepository orderRepo, IReportRepository reportRepo) 
    : INotificationHandler<DishesQuantityInOrderItemChanged>
{
    public Task Handle(DishesQuantityInOrderItemChanged notification, CancellationToken cancellationToken)
    {
        var orderView = orderRepo
            .FindById(notification.OrderId)
             ?? throw new NullReferenceException("Модель чтения заказа не найдена");
        
        orderView.ChangeDishesQuantityInOrderItem(notification.NewQuantity, notification.Dish.Id);
        OrderHelper.UpdateProductSales(notification.Dish.Name , notification.OldQuantity, notification.NewQuantity);
        
        var allOrders = orderRepo.FindAll();
        var lastReport = reportRepo.GetLastReport(1).First();
        
        var report = new Report
        {
            OrdersQuantity = lastReport.OrdersQuantity,
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
