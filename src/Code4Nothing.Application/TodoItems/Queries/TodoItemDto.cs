using Code4Nothing.Application.Common.Mappings;

namespace Code4Nothing.Application.TodoItems.Queries;

public record TodoItemDto : IMapFrom<TodoItem>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public bool Done { get; set; }
    public int Priority { get; set; }
    public string? Colour { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TodoItem, TodoItemDto>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority))
            .ForMember(d => d.Colour, opt => opt.MapFrom(s => s.Colour.ToString()));
        
        profile.CreateMap<TodoItem, TodoItemDto>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority))
            .ForMember(d => d.Colour, opt => opt.MapFrom(s => s.Colour.ToString()))
            .ReverseMap();
    }
}