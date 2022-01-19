namespace Code4Nothing.Domain.TodoItem.Events;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(Entities.TodoItem item)
        => Item = item;
    public Entities.TodoItem Item { get; }
}