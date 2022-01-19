namespace Code4Nothing.Domain.TodoItem.Events;

public class TodoItemCompletedEvent : DomainEvent
{
    public TodoItemCompletedEvent(Entities.TodoItem item) => Item = item;
    public Entities.TodoItem Item { get; }
}