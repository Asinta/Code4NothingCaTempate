namespace Code4Nothing.Api.Controllers;

[Route("/todo-items")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoItemsController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost]
    public async Task<ApiResponse<TodoItem>> Create([FromBody] CreateTodoItemCommand command)
    {
        return ApiResponse<TodoItem>.Success(await _mediator.Send(command));
    }
    
    [HttpGet]
    public async Task<ApiResponse<PaginatedList<TodoItemDto>>> GetTodoItemsWithCondition([FromQuery] GetTodoItemsWithConditionsQuery query)
    {
        return ApiResponse<PaginatedList<TodoItemDto>>.Success(await _mediator.Send(query));
    }
}