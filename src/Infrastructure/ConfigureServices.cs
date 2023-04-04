using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebUI;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    private const string DatabaseName = "InMemoryTestDB";

    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseInMemoryDatabase(DatabaseName)
        );

        services.AddScoped<IApplicationDbContext, DatabaseContext>();
        services.AddScoped<IDbSeeder, DbSeeder>();
    }
}
