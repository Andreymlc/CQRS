namespace DDD_CQRS.Domain.Views;

public class Report
{
    public required int OrdersQuantity { get; init; }
    public required decimal Income { get; init; }
    public required int NumberCompletedOrders { get; init; }
    public required (string Name, int Quantity) MostPopularProducts { get; init; }

    public required string Reason { get; init; }
    public DateTimeOffset PublishDate { get; } = DateTimeOffset.Now;

    public override string ToString() =>
        $"""
         ++++++++++++++++++++++++++++++ Отчет ++++++++++++++++++++++++++++++
         Дата: {PublishDate:dd.MM.yyyy HH:mm:ss}
         Причина обновления: {Reason}
         Всего заказов: {OrdersQuantity}
         Заказов выполнено: {NumberCompletedOrders}
         Доход: {Income:C}
         
         Самый популярный продукт: {MostPopularProducts.Name, -10} × {MostPopularProducts.Quantity, -3}
         +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         """;
}
