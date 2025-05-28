namespace DDD_CQRS.Domain;

public enum OrderStatus
{
    Created = 0,
    Preparing = 1,
    Completed = 2,
    Canceled = 3
}

public static class OrderStatusExtensions
{
    public static string ToRussianString(this OrderStatus status) =>
        status switch
        {
            OrderStatus.Created => "Создан",
            OrderStatus.Preparing => "Готовится",
            OrderStatus.Completed => "Завершен",
            OrderStatus.Canceled => "Отменен",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
}
