namespace DDD_CQRS.Domain.Views;

public class OrderItemView
{
    public OrderItemView(DishView dishView, int quantity)
    {
        DishView = dishView;
        Quantity = quantity;
    }

    public DishView DishView { get; private set; }
    public int Quantity { get; private set; }
    
    public decimal TotalPrice => DishView.Price * Quantity;
    
    public void ChangeDishesQuantity(int newQuantity) => Quantity = newQuantity;

    public override string ToString() => 
        $"{DishView.Name} x {Quantity} = {TotalPrice:C} ({DishView.Price:C})";
}
