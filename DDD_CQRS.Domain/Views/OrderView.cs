using System.Text;

namespace DDD_CQRS.Domain.Views;

public class OrderView
{
    public OrderView(Guid id, OrderStatus status, List<OrderItemView> dishes)
    {
        Id = id;
        _items = dishes;
        Status = status;
        Cost = CalculateTotalPrice();
    }
    
    public Guid Id { get; private set; }
    public decimal Cost { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    public IReadOnlyList<OrderItemView> Items => _items.AsReadOnly();

    private readonly List<OrderItemView> _items;

    public void AddItem(DishView dish, int quantity)
    {
        _items.Add(new OrderItemView(dish, quantity));
        Cost = CalculateTotalPrice();
    }
    
    public void ChangeDishesQuantityInOrderItem(int newQuantity, Guid dishId)
    {
        var itemForChange = _items
            .FirstOrDefault(item => item.DishView.Id == dishId)
             ?? throw new NullReferenceException("Блюдо не найдено");
        
        itemForChange.ChangeDishesQuantity(newQuantity);
        Cost = CalculateTotalPrice();
    }
    
    public void ChangeStatus(OrderStatus newStatus) => Status = newStatus;

    private decimal CalculateTotalPrice() => _items.Sum(item => item.TotalPrice);
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" ЗАКАЗ №: {Id,-28}");
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" Сумма: {Cost:C}{"",23}");
        sb.AppendLine($" Статус: {Status.ToRussianString(),-25}");
        sb.AppendLine($" Дата создания: {CreatedAt:dd.MM.yyyy HH:mm}{"",12}");
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" Блюда:{"",32}");

        foreach (var item in _items)
            sb.AppendLine($"   • {item.DishView.Name,-25} × {item.Quantity,3} ({item.TotalPrice:C}) ");

        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        return sb.ToString();
    }
}
