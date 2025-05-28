namespace DDD_CQRS.Domain.Views;

public class DishView
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public override string ToString() => $"{Name} ({Price}₽)";
}
