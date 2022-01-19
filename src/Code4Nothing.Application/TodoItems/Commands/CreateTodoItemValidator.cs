namespace Code4Nothing.Application.TodoItems.Commands;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    private readonly IRepository<TodoItem> _repository;

    public CreateTodoItemCommandValidator(IRepository<TodoItem> repository)
    {
        _repository = repository;
        
        // 我们把最大长度限制到10，以便更好地验证这个校验
        // 更多的用法请参考FluentValidation官方文档
        RuleFor(v => v.Title)
            .MaximumLength(50).WithMessage("TodoItem title must not exceed 50 characters.").WithSeverity(Severity.Warning)
            .NotEmpty().WithMessage("Title is required.").WithSeverity(Severity.Error)
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.").WithSeverity(Severity.Error);
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _repository.GetAsQueryable().AllAsync(l => l.Title != title, cancellationToken);
    }
}