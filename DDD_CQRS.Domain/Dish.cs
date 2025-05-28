namespace DDD_CQRS.Domain;

public class Dish : Entity<Guid>
{
    public Dish(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
    
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    public static Dish Create(Guid id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена должна быть положительной");
        
        return new Dish(id, name, price);
    }

}
