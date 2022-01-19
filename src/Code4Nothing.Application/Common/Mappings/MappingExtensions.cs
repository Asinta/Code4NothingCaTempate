using Code4Nothing.Application.Common.Models;

namespace Code4Nothing.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
    {
        return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable<TDestination> queryable, IConfigurationProvider configuration)
    {
        return queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }

    public static PaginatedList<TDestination> PaginatedListFromEnumerable<TDestination>(this IEnumerable<TDestination> entities, int pageNumber, int pageSize)
    {
        return PaginatedList<TDestination>.Create(entities, pageNumber, pageSize);
    }
}