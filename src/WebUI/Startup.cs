using Application;

namespace WebUI;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebUiServices();
        services.AddApplicationServices();
        services.AddInfrastructureServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var databaseSeeder = services.GetRequiredService<IDbSeeder>();
            databaseSeeder.Seed();
        }

    }
}
