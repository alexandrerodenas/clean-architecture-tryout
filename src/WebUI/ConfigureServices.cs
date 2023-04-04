using WebUI.Filters;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddWebUiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
    }
}
