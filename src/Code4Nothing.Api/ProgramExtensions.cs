using Serilog;

namespace Code4Nothing.Api;

public static class ProgramExtensions
{
    private const string AppName = "Code4Nothing.Api";

    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
    }

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool>("UseFileToLog"))
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "Logs/log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 15)
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        builder.Host.UseSerilog();
    }
    
    public static void AddCustomApiVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }

    public static void AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"Code4Nothing - {AppName}", Version = "v1" });
            
            var identityUrlExternal = builder.Configuration.GetValue<string>("IdentityUrlExternal");

            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                        TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "Code4Nothing", AppName }
                        }
                    }
                }
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });

            // c.OperationFilter<AuthorizeCheckOperationFilter>();
        });
    }
    
    public static void AddCustomAuthentication(this WebApplicationBuilder builder)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Audience = "Code4Nothing.Api";
                options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
                options.RequireHttpsMetadata = false;
            });
    }
    
    public static void AddCustomAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "Code4Nothing");
            });
        });
    }
    
    public static void AddCustomHealthChecks(this WebApplicationBuilder builder) =>
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(
                builder.Configuration["ConnectionStrings:DefaultConnection"],
                name: "Code4Nothing-check",
                tags: new[] { "Code4NothingDb" });
    
    public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
    }
    
    public static void AddCustomDatabase(this WebApplicationBuilder builder) =>
        builder.Services.AddDbContext<Code4NothingDbContext>(
            options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
    
    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V1");
            c.OAuthClientId("Code4NothingSwaggerUi");
            c.OAuthAppName("Code4Nothing Swagger UI");
        });
    }
    
    public static void ApplyDatabaseMigration(this WebApplication app)
    {
        // Apply database migration automatically. Note that this approach is not
        // recommended for production scenarios. Consider generating SQL scripts from
        // migrations instead.
        using var scope = app.Services.CreateScope();
        
        var retryPolicy = CreateRetryPolicy(app.Configuration, Log.Logger);
        var context = scope.ServiceProvider.GetRequiredService<Code4NothingDbContext>();

        retryPolicy.Execute(context.Database.Migrate);

        Code4NothingDbContextSeed.SeedSampleDataAsync(context).Wait();
    }

    private static Policy CreateRetryPolicy(IConfiguration configuration, Serilog.ILogger logger)
    {
        // Only use a retry policy if configured to do so.
        // When running in an orchestrator/K8s, it will take care of restarting failed services.
        if (bool.TryParse(configuration["RetryMigrations"], out _))
        {
            return Policy.Handle<Exception>().
                WaitAndRetryForever(
                    _ => TimeSpan.FromSeconds(5),
                    (exception, retry, _) =>
                    {
                        logger.Warning(
                            exception,
                            "Exception {ExceptionType} with message {Message} detected during database migration (retry attempt {retry})",
                            exception.GetType().Name,
                            exception.Message,
                            retry);
                    }
                );
        }

        return Policy.NoOp();
    }
}