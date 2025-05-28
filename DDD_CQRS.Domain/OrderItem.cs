namespace DDD_CQRS.Domain;

public class OrderItem
{
    private OrderItem(Dish dish, int quantity)
    {
        Dish = dish;
        Quantity = quantity;
    }
    
    public Dish Dish { get; private set; }
    public int Quantity { get; private set; }
    

    public static OrderItem Create(Dish dish, int quantity)
    {
        dish = dish ?? throw new ArgumentNullException(nameof(dish));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть положительным");
        
        return new OrderItem(dish, quantity);
    }

    public void ChangeDishesQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity), "Количество должно быть положительным или 0");

        Quantity = newQuantity;
    }

    public decimal TotalPrice => Dish.Price * Quantity;
}
