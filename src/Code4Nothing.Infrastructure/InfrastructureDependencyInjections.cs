using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Code4Nothing.Infrastructure;

public static class InfrastructureDependencyInjections
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Code4NothingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(Code4NothingDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IDomainEventService, DomainEventService>();
    }
}