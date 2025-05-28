using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Views;

namespace DDD_CQRS.Application.Helper;

public class OrderHelper
{
    private static readonly Dictionary<string, int> ProductSales = new();

    public static void UpdateProductSales(Order order)
    {
        foreach (var item in order.Items)
        {
            if (!ProductSales.TryAdd(item.Dish.Name, item.Quantity))
                ProductSales[item.Dish.Name] += item.Quantity;
        }
    }

    public static void UpdateProductSales(string name, int oldQuantity, int newQuantity)
    {
        if (!ProductSales.TryAdd(name, newQuantity))
        {
            if (oldQuantity < newQuantity)
                ProductSales[name] += newQuantity - oldQuantity;
            else
                ProductSales[name] -= oldQuantity - newQuantity;
        }
    }
    
    public static void UpdateProductSales(string name, int quantity)
    {
        if (!ProductSales.TryAdd(name, quantity))
            ProductSales[name] = quantity;
    }

    public static (string Name, int Quantity) GetMostPopularProduct()
    {
        var mostPopularProduct = ProductSales.MaxBy(p => p.Value);
        
        return (mostPopularProduct.Key, mostPopularProduct.Value);
    }

    public static OrderView Map(Order order) => 
        new(order.Id,  order.Status, order.Items.Select(Map).ToList());
    
    public static OrderItemView Map(OrderItem orderItem) => new(Map(orderItem.Dish), orderItem.Quantity);
    
    public static DishView Map(Dish dish) => new() { Id = dish.Id, Name = dish.Name, Price = dish.Price };
}
