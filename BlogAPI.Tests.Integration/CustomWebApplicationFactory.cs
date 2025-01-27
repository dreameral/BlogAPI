namespace BlogAPI.Tests.Integration;

using BlogAPI.Helpers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing PostgreSQL DbContext configuration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            
                if (descriptor != null)
                {
                    services.Remove(descriptor);  // Remove PostgreSQL DbContext configuration
                }

                // Add in-memory database for testing
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");  // Use in-memory database
                });
            });

        });
    }
}
