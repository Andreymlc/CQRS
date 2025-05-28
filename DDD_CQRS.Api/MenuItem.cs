namespace DDD_CQRS;

public class MenuItem
{
    public required string Description { get; init; }
    public required Action Action { get; init; }
    public Func<bool> IsAvailable { get; init; } = () => true;
}
