using DDD_CQRS.Domain.Event;

namespace DDD_CQRS.Domain;

public class Order : Entity<Guid>
{
    public Order(IEnumerable<OrderItem> dishes)
    {
        Id = Guid.NewGuid();
        _items = dishes.ToList();
        Status = OrderStatus.Created;
        Cost = CalculateTotalPrice();
        AddDomainEvent(new OrderCreated { Order = this, Description = $"Заказ с id '{Id}' создан" });
    }
    
    public Guid Id { get; private set; }
    public decimal Cost { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    private readonly List<OrderItem> _items;


    public void AddItem(Dish dish, int quantity)
    {
        ArgumentNullException.ThrowIfNull(dish);
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть положительным");

        _items.Add(OrderItem.Create(dish, quantity));
        
        AddDomainEvent(new DishAddedToOrderEvent
        {
            OrderId = Id,
            Dish = dish,
            Quantity = quantity,
            Description = $"Блюдо {dish.Name} было добавлено в заказ в количестве {quantity} шт"
        });
        
        Cost = CalculateTotalPrice();
    }

    public void ChangeDishesQuantityInOrderItem(int newQuantity, Guid dishId)
    {
        var itemForChange = _items
            .FirstOrDefault(item => item.Dish.Id == dishId)
             ?? throw new NullReferenceException("Блюдо не найдено");
        
        if (newQuantity <= 0)
            throw new ArgumentException("Количество должно быть больше 0");

        AddDomainEvent(new DishesQuantityInOrderItemChanged
        {
            OrderId = Id,
            Dish = itemForChange.Dish,
            OldQuantity = itemForChange.Quantity,
            NewQuantity = newQuantity,
            Description = $"Количество {itemForChange.Dish.Name} в заказе изменено на {newQuantity}"
        });
        
        Cost = CalculateTotalPrice();
        itemForChange.ChangeDishesQuantity(newQuantity);
    }

    public void RemoveItem(Guid dishId)
    {
        var itemForRemove = _items
            .FirstOrDefault(item => item.Dish.Id == dishId)
             ?? throw new NullReferenceException("Блюдо не найден");

        AddDomainEvent(new DishesQuantityInOrderItemChanged
        {
            OrderId = Id,
            Dish = itemForRemove.Dish,
            NewQuantity = 0,
            OldQuantity = itemForRemove.Quantity,
            Description = $"Блюдо '{itemForRemove.Dish.Name}' было удалено из заказа"
        });
        
        _items.Remove(itemForRemove);
        Cost = CalculateTotalPrice();
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
            throw new InvalidOperationException($"Недопустимый переход статуса:" +
                                                $" {Status.ToRussianString()} -> {newStatus.ToRussianString()}");

        if (newStatus is OrderStatus.Completed)
            AddDomainEvent(new OrderCompleted { OrderId = Id, Description = $"Заказ '{Id}' завершен"});
        
        Status = newStatus;
    }

    private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return current switch
        {
            OrderStatus.Created => next is OrderStatus.Preparing or OrderStatus.Canceled,
            OrderStatus.Preparing => next is OrderStatus.Completed or OrderStatus.Canceled,
            OrderStatus.Completed => next is OrderStatus.Canceled,
            _ => false
        };
    }

    private decimal CalculateTotalPrice() => _items.Sum(item => item.TotalPrice);
}
