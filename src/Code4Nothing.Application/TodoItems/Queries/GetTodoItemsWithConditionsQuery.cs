using Code4Nothing.Application.Common.Mappings;

namespace Code4Nothing.Application.TodoItems.Queries;

public class GetTodoItemsWithConditionsQuery : IRequest<PaginatedList<TodoItemDto>>
{
    public bool? Done { get; set; }
    public string? Title { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTodoItemsWithConditionQueryHandler : IRequestHandler<GetTodoItemsWithConditionsQuery, PaginatedList<TodoItemDto>>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IMapper _mapper;

    public GetTodoItemsWithConditionQueryHandler(IRepository<TodoItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsWithConditionsQuery request, CancellationToken cancellationToken)
    {
        return await _repository
            .GetAsQueryable(x => (!request.Done.HasValue || x.Done == request.Done)
                                 && (string.IsNullOrEmpty(request.Title) || x.Title!.Trim().ToLower().Contains(request.Title!.ToLower())))
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}