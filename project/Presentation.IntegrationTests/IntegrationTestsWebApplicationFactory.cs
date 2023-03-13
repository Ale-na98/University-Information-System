using DataAccess;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.IntegrationTests
{
    public class IntegrationTestsWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d =>
                {
                    return d.ServiceType == typeof(DbContextOptions<AppDbContext>);
                });
                services.Remove(dbContextDescriptor);

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    options.UseSqlite(connection);
                });

                var appContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
                appContext.Database.Migrate();
            });
        }
    }
}
