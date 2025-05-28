namespace DDD_CQRS.Domain.Event;

public abstract class Event
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now;
    public string Description { get; set; } = "Описание не передано";

    public void PrintInfo() => Console.WriteLine($"\nПроизошло событие '{EventId}'\nВремя: {TimeStamp:dd.mm.yyyy HH:MM}\nОписание: {Description}");
}
