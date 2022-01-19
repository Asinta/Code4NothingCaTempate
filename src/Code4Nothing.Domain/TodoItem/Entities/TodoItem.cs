namespace Code4Nothing.Domain.TodoItem.Entities;

public class TodoItem : AuditableEntity, IEntity<Guid>, IHasDomainEvent, IAggregateRoot
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public PriorityLevel Priority { get; set; }
    public Colour Colour { get; set; } = Colour.White;

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && _done == false)
            {
                DomainEvents.Add(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }
    
    public List<DomainEvent> DomainEvents { get; set; } = new();
}
