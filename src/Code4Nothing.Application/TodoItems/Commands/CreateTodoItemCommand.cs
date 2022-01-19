namespace Code4Nothing.Application.TodoItems.Commands;

public class CreateTodoItemCommand : IRequest<TodoItem>
{
    public string? Title { get; set; }
    public string? Colour { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItem>
{
    private readonly IRepository<TodoItem> _repository;

    public CreateTodoItemCommandHandler(IRepository<TodoItem> repository) => _repository = repository;

    public async Task<TodoItem> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title!,
            Colour = Colour.From(request.Colour ?? string.Empty),
            Done = false
        };

        entity.DomainEvents.Add(new TodoItemCreatedEvent(entity));
        
        await _repository.AddAsync(entity, cancellationToken);
        
        return entity;
    }
}